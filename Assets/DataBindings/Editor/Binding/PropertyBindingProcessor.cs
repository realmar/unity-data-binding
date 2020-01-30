using Mono.Cecil;
using Realmar.DataBindings.Editor.Cecil;
using Realmar.DataBindings.Editor.Exceptions;
using System.Collections.Generic;
using System.Linq;
using static UnityEngine.Debug;

namespace Realmar.DataBindings.Editor.Binding
{
	internal class PropertyBindingProcessor : IPropertyProcessor
	{
		private readonly AttributeResolver _attributeResolver;
		private readonly Dictionary<BindingType, IBinder> _binders;

		public PropertyBindingProcessor(Dictionary<BindingType, IBinder> binders)
		{
			_attributeResolver = new AttributeResolver();
			_binders = binders;
		}

		public void Process(PropertyDefinition sourceProperty)
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

		private static BindingSettings GetBindingSettings(CustomAttribute attribute)
		{
			var ctorArgs = attribute.ConstructorArguments;

			return new BindingSettings
			(
				type: (BindingType) ctorArgs[0].Value,
				targetId: (int) ctorArgs[1].Value,
				targetPropertyName: (string) ctorArgs[2].Value,
				nullCheckBehavior: (NullCheckBehavior) ctorArgs[3].Value,
				dataSource: (DataSource) ctorArgs[4].Value,
				converter: (TypeReference) ctorArgs[5].Value
			);
		}
	}
}
