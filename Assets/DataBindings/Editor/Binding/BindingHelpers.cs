using System;
using System.Linq;
using Mono.Cecil;
using Realmar.DataBindings.Editor.Cecil;
using Realmar.DataBindings.Editor.Exceptions;
using Realmar.DataBindings.Editor.Weaving;
using static Realmar.DataBindings.Editor.Exceptions.YeetHelpers;

namespace Realmar.DataBindings.Editor.Binding
{
	internal static class BindingHelpers
	{
		internal static PropertyDefinition GetTargetProperty(
			PropertyDefinition sourceProperty,
			TypeDefinition targetType,
			string targetPropertyName)
		{
			var saneTargetPropertyName = targetPropertyName ?? sourceProperty.Name;

			var targetProperty = targetType.GetPropertiesInBaseHierarchy(saneTargetPropertyName).FirstOrDefault();
			if (targetProperty == null)
			{
				throw new MissingTargetPropertyException(targetType.FullName, saneTargetPropertyName);
			}

			return targetProperty;
		}


		internal static (MethodDefinition, BindingInitializerSettings) GetBindingInitializer(TypeDefinition type)
		{
			MethodDefinition initializer = null;
			BindingInitializerSettings settings = default;

			foreach (var method in type.Methods)
			{
				var attribute = method.GetCustomAttribute<BindingInitializerAttribute>();
				if (attribute != null)
				{
					initializer = method;
					settings = GetBindingInitializerSettings(attribute);

					break;
				}
			}

			YeetIfNoBindingInitializer(initializer, type);

			return (initializer, settings);
		}

		private static BindingInitializerSettings GetBindingInitializerSettings(CustomAttribute attribute)
		{
			var ctorArgs = attribute.ConstructorArguments;
			return new BindingInitializerSettings
			(
				throwOnFailure: (bool) ctorArgs[0].Value
			);
		}

		internal static bool ResolveNullCheckBehavior(NullCheckBehavior behavior, bool autoBehavior)
		{
			if (behavior == NullCheckBehavior.Auto)
			{
				return autoBehavior;
			}
			else
			{
				return NullCheckBehaviorToBool(behavior);
			}
		}

		internal static bool NullCheckBehaviorToBool(NullCheckBehavior behavior)
		{
			switch (behavior)
			{
				case NullCheckBehavior.EnableNullCheck:
					return true;
				case NullCheckBehavior.DisableNullCheck:
					return false;
				default:
					throw new ArgumentOutOfRangeException(nameof(behavior), behavior, null);
			}
		}

		internal static void DispatchUsingDataSource(Weaver weaver, in DataSourceConfiguration configuration, in WeaveMethodParameters parameters)
		{
			var dataSource = configuration.DataSource;
			switch (dataSource)
			{
				case DataSource.Value:
					weaver.Weave(parameters, configuration.MethodParameter.Value);
					break;
				case DataSource.Getter:
					weaver.Weave(parameters, configuration.FromGetter.Value);
					break;
				default:
					throw new ArgumentOutOfRangeException(nameof(dataSource), dataSource, null);
			}
		}
	}
}
