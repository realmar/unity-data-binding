using System;
using System.Globalization;
using Realmar.DataBindings.Editor.Shared.Extensions;

namespace Realmar.DataBindings.Editor.TestFramework.Sandbox
{
	internal static class SandboxHelpers
	{
		internal static T CreateInstance<T>(params object[] ctorArgs)
		{
			return (T) CreateInstance(typeof(T), ctorArgs);
		}

		internal static object CreateInstance(Type type, params object[] ctorArgs)
		{
			if (type.IsInterface)
			{
				throw new ArgumentException($"Type cannot be an interface {type.FullName}", nameof(type));
			}

			if (type.IsAbstract)
			{
				throw new ArgumentException($"Type cannot be abstract {type.FullName}", nameof(type));
			}

			return Activator.CreateInstance(type, ReflectionExtensions.ALL, null, ctorArgs, CultureInfo.CurrentCulture);
		}
	}
}
