using System.Collections.Generic;

namespace Realmar.DataBindingsEditor.Extensions
{
	public static class DataExtensions
	{
		public static void AddRange<T>(this HashSet<T> set, List<T> data)
		{
			AddRange(set, (IReadOnlyList<T>) data);
		}

		public static void AddRange<T>(this HashSet<T> set, IReadOnlyList<T> data)
		{
			for (var i = data.Count - 1; i >= 0; i--)
			{
				set.Add(data[i]);
			}
		}

		public static void AddRange<T>(this HashSet<T> set, IEnumerable<T> data)
		{
			foreach (var item in data)
			{
				set.Add(item);
			}
		}

		public static HashSet<T> ToHashSet<T>(this IEnumerable<T> data)
		{
			return new HashSet<T>(data);
		}
	}
}
