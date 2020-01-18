using Mono.Cecil;
using Realmar.DataBindings.Editor.Cecil;
using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

namespace Realmar.DataBindings.Editor.Exceptions
{
	internal static class YeetHelpers
	{
		[DebuggerHidden]
		internal static void YeetIfInaccessible(IMemberDefinition member, TypeDefinition type)
		{
			if (member.IsAccessibleFrom(type) == false)
			{
				throw new ArgumentException($"{member.FullName} is not accessible from {type.FullName}");
			}
		}

		[DebuggerHidden]
		internal static void YeetIfNoBindingInitializer(MethodDefinition initializer, TypeDefinition type)
		{
			if (initializer == null)
			{
				throw new MissingBindingInitializerException(type.FullName);
			}
		}

		[DebuggerHidden]
		internal static void YeetIfFieldExists(TypeDefinition type, string fieldName)
		{
			if (type.Fields.Any(field => field.Name == fieldName))
			{
				throw new ArgumentException($"Field {fieldName} already exists in the type {type.FullName}");
			}
		}

		[DebuggerHidden]
		internal static void YeetIfNull(object obj, string name)
		{
			if (obj == null)
			{
				throw new ArgumentNullException(name);
			}
		}

		[DebuggerHidden]
		internal static void YeetIfNotPropertyOrField(MemberInfo info)
		{
			if (info is FieldInfo == false && info is PropertyInfo == false)
			{
				throw new ArgumentException("Symbol is not a property nor a field.", nameof(info));
			}
		}

		[DebuggerHidden]
		internal static void YeetIfAbstract(MethodDefinition method)
		{
			if (method.IsAbstract)
			{
				throw new ArgumentException($"Method cannot be abstract {method.FullName}", nameof(method));
			}
		}

		[DebuggerHidden]
		internal static void YeetIfNotInterface(TypeDefinition type)
		{
			if (type.IsInterface == false)
			{
				throw new ArgumentException($"Type is not an interface {type.FullName}", nameof(type));
			}
		}
	}
}
