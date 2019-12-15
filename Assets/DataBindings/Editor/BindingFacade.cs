using System.Collections.Generic;
using System.Linq;
using Mono.Cecil;
using Mono.Cecil.Rocks;
using Realmar.DataBindings.Editor.Binder;
using Realmar.DataBindings.Editor.Exceptions;
using Realmar.DataBindings.Editor.Extensions;
using Realmar.DataBindings.Editor.Misc;

namespace Realmar.DataBindings.Editor
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
			var derivativeResolver = new DerivativeResolver();
			var oneWayBinder = new OneWayBinder(derivativeResolver);
			var fromTargetBinder = new FromTargetBinder(derivativeResolver);
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
		// TODO add overload for Stream
		public void WeaveAssembly(string assemblyPath, string outputPath = null)
		{
			// TODO what if the assembly which needs to be weaved is not the one provided here? --> weave beyond assembly boundaries!!
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

		private void BindProperty(PropertyDefinition propertyDefinition)
		{
			foreach (var bindingAttribute in propertyDefinition.GetCustomAttributes<BindingAttribute>())
			{
				var declaringType = propertyDefinition.DeclaringType;
				var settings = GetBindingSettings(bindingAttribute);
				var allBindingTargets = GetBindingTargets(declaringType);
				var bindingTargets = FilterBindingsTargets(propertyDefinition, settings, allBindingTargets);

				if (bindingTargets.Length == 0)
				{
					throw new MissingBindingTargetException(declaringType.Name);
				}

				_binders[settings.Type].Bind(propertyDefinition, settings, bindingTargets);
			}
		}

		private BindingTarget[] GetBindingTargets(TypeDefinition originType)
		{
			var targets = new List<BindingTarget>();

			foreach (var type in originType.EnumerateBaseClasses())
			{
				var bindingTargets =
					_attributeResolver.GetCustomAttributesOfSymbolsInType<BindingTargetAttribute>(type);
				for (var i = 0; i < bindingTargets.Count; i++)
				{
					var target = bindingTargets[i];
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

		private static BindingTarget[] FilterBindingsTargets(
			PropertyDefinition sourceProperty,
			BindingSettings settings,
			BindingTarget[] targets)
		{
			var bindingTargets =
				targets.Where(bindingTarget => bindingTarget.Id == settings.TargetId).ToArray();
			if (bindingTargets.Length == 0)
			{
				throw new MissingSymbolException(
					$"Cannot find BindingTarget with ID {settings.TargetId} for property {sourceProperty.FullName}");
			}

			return bindingTargets;
		}
	}
}
