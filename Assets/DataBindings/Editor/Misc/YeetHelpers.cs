using System;
using System.Diagnostics;
using System.Linq;
using Mono.Cecil;
using Realmar.DataBindings.Editor.Exceptions;
using Realmar.DataBindings.Editor.Extensions;

namespace Realmar.DataBindings.Editor
{
	internal static class YeetHelpers
	{
		internal static void YeetIfInaccessible(IMemberDefinition member, TypeDefinition type)
		{
			if (member.IsAccessibleFrom(type) == false)
			{
				throw new ArgumentException($"{member.FullName} is not accessible from {type.FullName}");
			}
		}

		internal static void YeetIfNoBindingInitializer(MethodDefinition initializer, TypeDefinition type)
		{
			if (initializer == null)
			{
				throw new MissingSymbolException(
					$"Cannot find binding initializer in type {type.FullName}. Annotate one method with the [BindingInitializer] attribute.");
			}
		}

		internal static void YeetIfFieldExists(TypeDefinition type, string fieldName)
		{
			if (type.Fields.Any(field => field.Name == fieldName))
			{
				throw new ArgumentException($"Field {fieldName} already exists in the type {type.FullName}");
			}
		}
	}
}
