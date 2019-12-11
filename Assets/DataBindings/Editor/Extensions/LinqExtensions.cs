using System.Collections.Generic;
using System.Linq;

namespace Realmar.DataBindings.Editor.Extensions
{
	internal static class LinqExtensions
	{
		internal static IEnumerable<T> WhereNotNull<T>(this IEnumerable<T> self) where T : class =>
			self.Where(arg => arg != null);
	}
}
