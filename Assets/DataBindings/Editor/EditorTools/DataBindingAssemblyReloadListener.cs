using Realmar.DataBindings.Editor.Binding;
using System.Diagnostics;
using System.Linq;
using UnityEditor;

namespace Realmar.DataBindings.Editor.EditorTools
{
	[InitializeOnLoad]
	internal static class DataBindingAssemblyReloadListener
	{
		static DataBindingAssemblyReloadListener()
		{
			AssemblyReloadEvents.beforeAssemblyReload += () =>
			{
				if (ScriptableManager.Get<SettingsScriptable>().Enabled)
				{
					WeaveAssemblies();
				}
			};
		}

		private static void WeaveAssemblies()
		{
			UnityEngine.Debug.Log("Start weaving assemblies");

			// TODO Proper solution
			// TODO Detected changes
			var assemblies = ScriptableManager.Get<SettingsScriptable>().GetAssemblies();
			var pathList = assemblies.Select(assembly => assembly.Location);

			var facade = new BindingFacade();

			var stopwatch = Stopwatch.StartNew();
			facade.WeaveAssembly(pathList.First());
			stopwatch.Stop();

			AssetDatabase.Refresh();

			UnityEngine.Debug.Log($"Finished weaving assemblies in {stopwatch.Elapsed.TotalSeconds} seconds");
		}
	}
}
