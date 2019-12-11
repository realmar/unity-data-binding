using System;
using System.Collections.Generic;
using Realmar.DataBindings.Editor.BCL.System;

namespace Realmar.DataBindings.Editor.Misc
{
	internal class Cache
	{
		private readonly Dictionary<int, object> _cache = new Dictionary<int, object>();

		internal TResult Get<TArg, TResult>(TArg arg, Func<TArg, TResult> provider)
		{
			if (arg == null)
			{
				return provider.Invoke(arg);
			}

			var hashCode = arg.GetHashCode();
			if (_cache.TryGetValue(hashCode, out var result) == false)
			{
				result = provider.Invoke(arg);
				_cache[hashCode] = result;
			}

			return (TResult) result;
		}

		internal TResult Get<TArg1, TArg2, TResult>(TArg1 arg1, TArg2 arg2, Func<TArg1, TArg2, TResult> provider)
		{
			var hashCode = HashCode.Combine(arg1?.GetHashCode(), arg2?.GetHashCode());
			if (_cache.TryGetValue(hashCode, out var result) == false)
			{
				result = provider.Invoke(arg1, arg2);
				_cache[hashCode] = result;
			}

			return (TResult) result;
		}
	}
}
