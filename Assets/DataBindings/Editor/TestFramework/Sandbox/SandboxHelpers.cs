using System;

namespace Realmar.DataBindings.Editor.TestFramework.Sandbox
{
	internal static class SandboxHelpers
	{
		internal static object CreateInstance(Type type)
		{
			if (type.IsInterface)
			{
				throw new ArgumentException($"Type cannot be an interface {type.FullName}", nameof(type));
			}

			if (type.IsAbstract)
			{
				throw new ArgumentException($"Type cannot be abstract {type.FullName}", nameof(type));
			}

			if (type.GetConstructor(Type.EmptyTypes) == null)
			{
				throw new ArgumentException($"Type must have a default constructor {type.FullName}", nameof(type));
			}

			return Activator.CreateInstance(type);
		}
	}
}
