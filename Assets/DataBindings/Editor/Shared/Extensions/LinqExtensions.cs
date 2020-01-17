using System.Collections.Generic;
using System.Linq;

namespace Realmar.DataBindings.Editor.Shared.Extensions
{
	internal static class LinqExtensions
	{
		internal static IEnumerable<T> WhereNotNull<T>(this IEnumerable<T> self) where T : class
		{
			return self.Where(arg => arg != null);
		}

		internal static IEnumerable<T> Yield<T>(this T self)
		{
			yield return self;
		}
	}
}
