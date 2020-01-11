using System.Collections.Generic;
using static Realmar.DataBindings.Editor.Exceptions.YeetHelpers;

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
			YeetIfNull(data, nameof(data));

			foreach (var d in data)
			{
				set.Add(d);
			}
		}

		internal static void AddRange<T>(this HashSet<T> set, IEnumerable<T> data)
		{
			YeetIfNull(data, nameof(data));

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
			YeetIfNull(data, nameof(data));

			foreach (var d in data)
			{
				stack.Push(d);
			}
		}

		internal static void Deconstruct<TKey, TValue>(this KeyValuePair<TKey, TValue> kvp, out TKey key, out TValue value)
		{
			key = kvp.Key;
			value = kvp.Value;
		}
	}
}
