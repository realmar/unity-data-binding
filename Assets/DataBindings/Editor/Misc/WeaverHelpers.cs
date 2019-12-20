using Mono.Cecil;
using Realmar.DataBindings.Editor.Exceptions;
using Realmar.DataBindings.Editor.Extensions;
using System;
using System.Linq;
using System.Text;
using static Realmar.DataBindings.Editor.YeetHelpers;

namespace Realmar.DataBindings.Editor
{
	internal static class WeaverHelpers
	{
		private static readonly StringBuilder _sb = new StringBuilder();

		internal static PropertyDefinition GetTargetProperty(
			PropertyDefinition sourceProperty,
			TypeDefinition targetType,
			string targetPropertyName)
		{
			var saneTargetPropertyName = targetPropertyName ?? sourceProperty.Name;

			var targetProperty = targetType.GetProperty(saneTargetPropertyName);
			if (targetProperty == null)
			{
				throw new MissingTargetPropertyException(targetType.FullName, saneTargetPropertyName);
			}

			return targetProperty;
		}

		internal static MethodDefinition GetSetHelperMethod(PropertyDefinition property, TypeDefinition type)
		{
			var targetSetHelperMethodName = GetTargetSetHelperMethodName(property.SetMethod);
			var targetSetHelperMethod = type.GetMethod(targetSetHelperMethodName);
			if (targetSetHelperMethod == null)
			{
				throw new MissingGetterException(property.FullName);
			}

			return targetSetHelperMethod;
		}

		internal static string GetTargetSetHelperMethodName(MethodDefinition setMethod)
		{
			return _sb.Clear().Append(setMethod.Name).Append("WeaveBinding").ToString();
		}

		internal static MethodDefinition GetBindingInitializer(TypeDefinition type)
		{
			var initializer = type.Methods.FirstOrDefault(method =>
				method.GetCustomAttribute<BindingInitializerAttribute>() != null);

			YeetIfNoBindingInitializer(initializer, type);

			return initializer;
		}

		internal static TypeDefinition GetReturnType(IMemberDefinition bindingTarget)
		{
			switch (bindingTarget)
			{
				case FieldDefinition field:
					return field.FieldType.Resolve();
				case MethodDefinition method:
					return method.ReturnType.Resolve();
				case PropertyDefinition property:
					return property.PropertyType.Resolve();
				case EventDefinition @event:
					return @event.EventType.Resolve();
				default:
					throw new ArgumentException("Only fields, properties, events or methods can have return types.");
			}
		}
	}
}
