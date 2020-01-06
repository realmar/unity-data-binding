using Realmar.DataBindings.Editor.BCL.System;

namespace Realmar.DataBindings.Editor.Weaving
{
	internal static class WeaveHelpers
	{
		internal static int GetBindingHashCode(in WeaveParameters parameters)
		{
			return HashCode.Combine(parameters.FromProperty, parameters.ToProperty, parameters.BindingTarget);
		}
	}
}
