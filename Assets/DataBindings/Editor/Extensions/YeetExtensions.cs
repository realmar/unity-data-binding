using Mono.Cecil;
using Realmar.DataBindings.Editor.Exceptions;

namespace Realmar.DataBindings.Editor.Extensions
{
	internal static class YeetExtensions
	{
		internal static MethodDefinition GetSetMethodOrYeet(this PropertyDefinition property)
		{
			var setMethod = property.SetMethod;
			if (setMethod == null)
			{
				throw new MissingSymbolException($"{property.FullName} does not have a setter.");
			}

			return setMethod;
		}
	}
}
