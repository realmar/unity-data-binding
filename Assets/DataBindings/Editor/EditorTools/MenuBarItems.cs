using JetBrains.Annotations;
using UnityEditor;

namespace Realmar.DataBindings.Editor.EditorTools
{
	internal class MenuBarItems
	{
		[MenuItem(MenuConstants.PREFIX + "/Settings", priority = -99), UsedImplicitly]
		private static void OpenSettings()
		{
			var settings = ScriptableManager.Get<SettingsScriptable>();

			EditorUtility.FocusProjectWindow();
			Selection.activeObject = settings;
		}

		[MenuItem(MenuConstants.PREFIX + "/Emit Bindings"), UsedImplicitly]
		private static void WeaveAssemblies()
		{
			if (ScriptableManager.Get<SettingsScriptable>().Enabled == false)
			{
				EditorUtility.DisplayDialog("DataBindings are Disabled", "DataBindings are currently disabled. Please enable them in the settings: DataBindings -> Settings -> Enabled", "Ok");
			}
			else
			{
				InternalUnityApi.DirtyAllScripts();
			}
		}
	}
}
