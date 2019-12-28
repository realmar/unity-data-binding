using System.Collections.Generic;

namespace Realmar.DataBindings.Editor.Shared.Extensions
{
	internal static class DataExtensions
	{
		internal static void AddRange<T>(this HashSet<T> set, List<T> data)
		{
			AddRange(set, (IReadOnlyList<T>) data);
		}

		internal static void AddRange<T>(this HashSet<T> set, IReadOnlyList<T> data)
		{
			for (var i = data.Count - 1; i >= 0; i--)
			{
				set.Add(data[i]);
			}
		}

		internal static void AddRange<T>(this HashSet<T> set, IEnumerable<T> data)
		{
			foreach (var item in data)
			{
				set.Add(item);
			}
		}

		internal static HashSet<T> ToHashSet<T>(this IEnumerable<T> data)
		{
			return new HashSet<T>(data);
		}

		internal static void AddRange<T>(this Stack<T> stack, IEnumerable<T> data)
		{
			foreach (var d in data)
			{
				stack.Push(d);
			}
		}
	}
}
