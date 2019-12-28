using Realmar.DataBindings.Editor.Binding;
using System.Diagnostics;
using UnityEditor;
using UnityEditor.Compilation;

namespace Realmar.DataBindings.Editor.EditorTools
{
	[InitializeOnLoad]
	internal static class UnityCompilationObserver
	{
		static UnityCompilationObserver()
		{
			CompilationPipeline.assemblyCompilationFinished += OnCompilationFinished;
		}

		private static void OnCompilationFinished(string assemblyPath, CompilerMessage[] _)
		{
			var settings = ScriptableManager.Get<SettingsScriptable>();
			if (settings.Enabled && settings.ShouldAssemblyBeWeaved(assemblyPath))
			{
				WeaveAssembly(assemblyPath);
			}
		}

		private static void WeaveAssembly(string assemblyPath)
		{
			UnityEngine.Debug.Log("Start weaving assemblies");

			var facade = new BindingFacade();

			var stopwatch = Stopwatch.StartNew();
			facade.WeaveAssembly(assemblyPath);
			stopwatch.Stop();

			AssetDatabase.Refresh();

			UnityEngine.Debug.Log($"Finished weaving assemblies in {stopwatch.Elapsed.TotalSeconds} seconds");
		}
	}
}
