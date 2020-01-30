using System.Diagnostics;
using Mono.Cecil;

namespace Realmar.DataBindings.Editor.Exceptions
{
	internal static class YeetExtensions
	{
		[DebuggerStepThrough]
		internal static MethodDefinition GetGetMethodOrYeet(this PropertyDefinition property)
		{
			var getMethod = property.GetMethod;
			if (getMethod == null)
			{
				throw new MissingGetterException(property.FullName);
			}

			return getMethod;
		}

		[DebuggerStepThrough]
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
