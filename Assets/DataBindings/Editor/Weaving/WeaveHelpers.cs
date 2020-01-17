using Mono.Cecil;

namespace Realmar.DataBindings.Editor.Weaving
{
	internal static class WeaveHelpers
	{
		internal static string FormatSetterName(MethodDefinition method)
		{
			var name = method.Name;
			return name.Replace("set_", "");
		}
	}
}
