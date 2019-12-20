using Mono.Cecil;
using Realmar.DataBindings.Editor.Exceptions;

namespace Realmar.DataBindings.Editor.Extensions
{
	internal static class YeetExtensions
	{
		internal static MethodDefinition GetGetMethodOrYeet(this PropertyDefinition property)
		{
			var getMethod = property.GetMethod;
			if (getMethod == null)
			{
				throw new MissingGetterException(property.FullName);
			}

			return getMethod;
		}

		internal static MethodDefinition GetSetMethodOrYeet(this PropertyDefinition property)
		{
			var setMethod = property.SetMethod;
			if (setMethod == null)
			{
				throw new MissingSetterException(property.FullName);
			}

			return setMethod;
		}
	}
}
