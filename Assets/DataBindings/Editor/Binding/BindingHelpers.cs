using Mono.Cecil;
using Realmar.DataBindings.Editor.Cecil;
using Realmar.DataBindings.Editor.Exceptions;
using Realmar.DataBindings.Editor.Weaving;
using System.Linq;
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


		internal static BindingSettings GetBindingSettings(CustomAttribute attribute)
		{
			var ctorArgs = attribute.ConstructorArguments;
			return new BindingSettings
			(
				type: (BindingType) ctorArgs[0].Value,
				targetId: (int) ctorArgs[1].Value,
				targetPropertyName: (string) ctorArgs[2].Value,
				emitNullCheck: (bool) ctorArgs[3].Value
			);
		}

		private static BindingInitializerSettings GetBindingInitializerSettings(CustomAttribute attribute)
		{
			var ctorArgs = attribute.ConstructorArguments;
			return new BindingInitializerSettings
			(
				throwOnFailure: (bool) ctorArgs[0].Value
			);
		}
	}
}
