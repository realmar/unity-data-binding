using Mono.Cecil;

namespace Realmar.DataBindings.Editor.Weaving
{
	internal static class WeaveHelpers
	{
		internal static string FormatSetterName(MethodDefinition method)
		{
			var name = method.Name;
			if (name.Length <= 4)
			{
				return name;
			}

			return name.Substring(4);
		}
	}
}
