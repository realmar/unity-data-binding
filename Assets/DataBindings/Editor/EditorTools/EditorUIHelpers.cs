using UnityEngine;

namespace Realmar.DataBindings.Editor.EditorTools
{
	internal static class EditorUIHelpers
	{
		internal static void DrawLine()
		{
			GUILayout.Box("", GUILayout.ExpandWidth(true), GUILayout.Height(1));
		}
	}
}
