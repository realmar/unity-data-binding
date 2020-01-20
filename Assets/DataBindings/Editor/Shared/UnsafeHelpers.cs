using Realmar.DataBindings.Editor.Utils;
using System;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;

namespace Realmar.DataBindings.Editor.Shared
{
	internal static class UnsafeHelpers
	{
		private static readonly Cache UnmanagedTypes = new Cache();
		private static readonly Random Random = new Random();

		internal static object GetRandomObjectOfType(Type type)
		{
			if (type == typeof(string))
			{
				return Random.Next().ToString();
			}
			else if (type.IsUnmanaged())
			{
				return GetRandomUnmanagedObjectOfType(type);
			}
			else
			{
				throw new ArgumentException($"Type must be a string or unmanaged {type.FullName}", nameof(type));
			}
		}

		private static unsafe object GetRandomUnmanagedObjectOfType(Type type)
		{
			var size = Marshal.SizeOf(type);
			var data = stackalloc byte[size];

			for (int i = size - 1; i >= 0; i--)
			{
				*(data + i) = (byte) Random.Next();
			}

			return Marshal.PtrToStructure((IntPtr) data, type);
		}

		internal static bool IsUnmanaged(this Type t)
		{
			return UnmanagedTypes.Get(t, type =>
			{
				bool result;

				if (type.IsPrimitive || type.IsPointer || type.IsEnum)
				{
					result = true;
				}
				else if (type.IsGenericType || !type.IsValueType)
				{
					result = false;
				}
				else
				{
					result = type
						.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
						.All(x => x.FieldType.IsUnmanaged());
				}

				return result;
			});
		}
	}
}
