using Realmar.DataBindings.Editor.Shared.Extensions;
using UnityEditor;

namespace Realmar.DataBindings.Editor.EditorTools
{
	internal static class InternalUnityApi
	{
		public static void DirtyAllScripts()
		{
			CallEditorCompilationInterface("DirtyAllScripts");
			AssetDatabase.Refresh();
		}

		private static object CallEditorCompilationInterface(string methodName, params object[] args)
		{
			var compilationInterface = typeof(UnityEditor.Editor).Assembly
				.GetType("UnityEditor.Scripting.ScriptCompilation.EditorCompilationInterface");

			if (compilationInterface != null)
			{
				var staticBindingFlags = ReflectionExtensions.ALL;
				var dirtyAllScriptsMethod =
					compilationInterface.GetMethod(methodName, staticBindingFlags);
				return dirtyAllScriptsMethod.Invoke(null, null);
			}

			return null;
		}
	}
}
