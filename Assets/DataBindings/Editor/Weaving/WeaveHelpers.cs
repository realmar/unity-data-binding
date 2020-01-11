using Mono.Cecil;
using Realmar.DataBindings.Editor.BCL.System;

namespace Realmar.DataBindings.Editor.Weaving
{
	internal static class WeaveHelpers
	{
		internal static int GetBindingHashCode(in WeaveParameters parameters)
		{
			return HashCode.Combine(parameters.FromProperty, parameters.ToProperty, parameters.BindingTarget);
		}

		internal static int GetSetHelperHashCode(PropertyDefinition from, PropertyDefinition to)
		{
			return HashCode.Combine(from, to);
		}
	}
}
