using Mono.Cecil;
using Mono.Cecil.Rocks;
using Realmar.DataBindings.Editor.Cecil;
using Realmar.DataBindings.Editor.Emitting;
using Realmar.DataBindings.Editor.Exceptions;
using Realmar.DataBindings.Editor.IoC;
using Realmar.DataBindings.Editor.Weaving;
using System;
using System.Collections.Generic;
using System.Linq;
using static Realmar.DataBindings.Editor.Binding.BindingHelpers;
using static UnityEngine.Debug;

namespace Realmar.DataBindings.Editor.Binding
{
	internal class BindingFacade : IDisposable
	{
		internal struct Options
		{
			internal bool WeaveDebugSymbols { get; set; }
		}

		private readonly UnityAssemblyResolver _assemblyResolver;
		private readonly CachedMetadataResolver _metadataResolver;
		private readonly AttributeResolver _attributeResolver;

		private readonly Dictionary<BindingType, IBinder> _binders;
		private readonly Options _options;

		public BindingFacade() : this(new Options
		{
			WeaveDebugSymbols = true
		})
		{
		}

		public BindingFacade(Options options)
		{
			_assemblyResolver = new UnityAssemblyResolver();
			_metadataResolver = new CachedMetadataResolver(_assemblyResolver);

			ConfigureServiceLocator();

			var oneWayBinder = new OneWayBinder();
			var fromTargetBinder = new FromTargetBinder();
			var oneTimeBinder = new OneTimeBinder();
			var twoWayBinder = new TwoWayBinder(oneWayBinder, fromTargetBinder);

			_options = options;
			_attributeResolver = new AttributeResolver();
			_binders = new Dictionary<BindingType, IBinder>
			{
				[BindingType.OneTime] = oneTimeBinder,
				[BindingType.OneWay] = oneWayBinder,
				[BindingType.OneWayFromTarget] = fromTargetBinder,
				[BindingType.TwoWay] = twoWayBinder
			};
		}

		public void Dispose()
		{
			_assemblyResolver.Dispose();
		}

		// TODO support multiple assemblies
		public void CreateBindingsInAssembly(string assemblyPath, string outputPath = null)
		{
			var assemblyResolver = ServiceLocator.Current.Resolve<IAssemblyResolver>();
			var metadataResolver = ServiceLocator.Current.Resolve<IMetadataResolver>();

			using (var module = ModuleDefinition.ReadModule(assemblyPath, new ReaderParameters
			{
				ReflectionImporterProvider = ServiceLocator.Current.Resolve<IReflectionImporterProvider>(),
				AssemblyResolver = assemblyResolver,
				MetadataResolver = metadataResolver,

				// PDB, MDB
				ReadSymbols = _options.WeaveDebugSymbols,
				ReadWrite = _options.WeaveDebugSymbols,

				// TODO PROPER SOLUTION REQUIRED
				ThrowIfSymbolsAreNotMatching = false
			}))
			{
				foreach (var type in module.GetAllTypes())
				{
					foreach (var property in type.Properties)
					{
						BindProperty(property);
					}
				}

				if (outputPath != null)
				{
					module.Write(outputPath);
				}
				else
				{
					module.Write();
				}
			}
		}

		private void BindProperty(PropertyDefinition sourceProperty)
		{
			foreach (var bindingAttribute in sourceProperty.GetCustomAttributes<BindingAttribute>())
			{
				var declaringType = sourceProperty.DeclaringType;
				var settings = GetBindingSettings(bindingAttribute);
				var allBindingTargets = GetBindingTargets(declaringType);
				var bindingTargets = FilterBindingsTargets(settings, allBindingTargets);

				if (bindingTargets.Length == 0)
				{
					throw new MissingBindingTargetException(sourceProperty.FullName, settings.TargetId);
				}


				if (_binders.TryGetValue(settings.Type, out var binder))
				{
					binder.Bind(sourceProperty, settings, bindingTargets);
				}
				else
				{
					LogError($"Cannot find a binder for {settings.Type} binding. Property = {sourceProperty.FullName}");
				}
			}
		}

		private BindingTarget[] GetBindingTargets(TypeDefinition originType)
		{
			var targets = new List<BindingTarget>();

			foreach (var type in originType.EnumerateBaseClasses())
			{
				var bindingTargets =
					_attributeResolver.GetCustomAttributesOfSymbolsInType<BindingTargetAttribute>(type);
				foreach (var target in bindingTargets)
				{
					var source = (IMemberDefinition) target.Source;
					if (source is PropertyDefinition propertyDefinition)
					{
						source = propertyDefinition.GetMethod;
					}

					var ctorArgs = target.Attribute.ConstructorArguments;
					targets.Add(new BindingTarget
					{
						Source = source,
						Id = (int) ctorArgs[0].Value
					});
				}
			}

			return targets.ToArray();
		}

		private BindingTarget[] FilterBindingsTargets(BindingSettings settings, BindingTarget[] targets)
		{
			return targets.Where(bindingTarget => bindingTarget.Id == settings.TargetId).ToArray();
		}

		private void ConfigureServiceLocator()
		{
			ServiceLocator.Reset();
			var locator = ServiceLocator.Current;

			locator.RegisterType<Weaver>(ServiceLifetime.Singleton);
			locator.RegisterType<Emitter>();
			locator.RegisterType<DerivativeResolver>(ServiceLifetime.Singleton);
			locator.RegisterType<IReflectionImporterProvider, ReflectionImporterProvider>(ServiceLifetime.Singleton);
			locator.RegisterType<IAssemblyResolver>(_assemblyResolver);
			locator.RegisterType<IMetadataResolver>(_metadataResolver);
		}
	}
}
