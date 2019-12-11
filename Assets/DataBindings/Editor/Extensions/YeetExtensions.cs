using Mono.Cecil;
using Realmar.DataBindingsEditor.Exceptions;

namespace Realmar.DataBindingsEditor.Extensions
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
