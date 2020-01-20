using System.Text.RegularExpressions;
using Mono.Cecil;

namespace Realmar.DataBindings.Editor.Weaving
{
	internal static class WeaveHelpers
	{
		private readonly static Regex _nonAlphaNumeric = new Regex("[^a-zA-Z0-9 -]", RegexOptions.Compiled);

		internal static string FormatSetterName(MethodDefinition method)
		{
			var name = method.Name;
			return name.Replace("set_", "");
		}

		internal static string SanitizeName(string name)
		{
			return _nonAlphaNumeric.Replace(name, "");
		}
	}
}
