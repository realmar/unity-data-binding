using Mono.Cecil;
using Mono.Cecil.Rocks;
using Realmar.DataBindings.Editor.Cecil;
using Realmar.DataBindings.Editor.Exceptions;
using Realmar.DataBindings.Editor.Extensions;
using Realmar.DataBindings.Editor.Utils.Cecil;
using System.Collections.Generic;
using System.Linq;
using Realmar.DataBindings.Editor.Utils;

namespace Realmar.DataBindings.Editor.Binding
{
	internal class BindingFacade
	{
		internal struct Options
		{
			internal bool WeaveDebugSymbols { get; set; }
		}

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
			var oneWayBinder = new OneWayBinder();
			var fromTargetBinder = new FromTargetBinder();
			var twoWayBinder = new TwoWayBinder(oneWayBinder, fromTargetBinder);

			_options = options;
			_attributeResolver = new AttributeResolver();
			_binders = new Dictionary<BindingType, IBinder>
			{
				[BindingType.OneWay] = oneWayBinder,
				[BindingType.OneWayFromTarget] = fromTargetBinder,
				[BindingType.TwoWay] = twoWayBinder
			};
		}

		// TODO consistent naming: weave vs bind vs emit
		// TODO support multiple assemblies
		public void WeaveAssembly(string assemblyPath, string outputPath = null)
		{
			ConfigureServiceLocator();

			using (var assemblyResolver = new UnityAssemblyResolver())
			{
				var metadataResolver = new CachedMetadataResolver(assemblyResolver);
				using (var module = ModuleDefinition.ReadModule(assemblyPath, new ReaderParameters
				{
					ReflectionImporterProvider = new ReflectionImporterProvider(),
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

				_binders[settings.Type].Bind(sourceProperty, settings, bindingTargets);
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

					targets.Add(new BindingTarget
					{
						Source = source,
						Id = (int) target.Attribute.ConstructorArguments[0].Value
					});
				}
			}

			return targets.ToArray();
		}

		private static BindingSettings GetBindingSettings(CustomAttribute attribute)
		{
			var ctorArgs = attribute.ConstructorArguments;
			return new BindingSettings
			{
				Type = (BindingType) ctorArgs[0].Value,
				TargetId = (int) ctorArgs[1].Value,
				TargetPropertyName = (string) ctorArgs[2].Value,
				EmitNullCheck = (bool) ctorArgs[3].Value
			};
		}

		private static BindingTarget[] FilterBindingsTargets(BindingSettings settings, BindingTarget[] targets)
		{
			return targets.Where(bindingTarget => bindingTarget.Id == settings.TargetId).ToArray();
		}

		private static void ConfigureServiceLocator()
		{
			ServiceLocator.Reset();
			var locator = ServiceLocator.Current;

			locator.RegisterType<DerivativeResolver>(ServiceLifetime.Singleton);
			locator.RegisterWovenSetHelpers();
			locator.RegisterWovenBindings();
		}
	}
}