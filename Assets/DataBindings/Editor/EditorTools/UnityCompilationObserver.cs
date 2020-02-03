using System.Diagnostics;
using Realmar.DataBindings.Editor.Binding;
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
			try
			{
				EditorApplication.LockReloadAssemblies();

				UnityEngine.Debug.Log("Start weaving assemblies");

				var stopwatch = Stopwatch.StartNew();
				using (var facade = new BindingFacade())
				{
					facade.CreateBindingsInAssembly(assemblyPath);
				}

				stopwatch.Stop();

				UnityEngine.Debug.Log($"Finished weaving assemblies in {stopwatch.Elapsed.TotalSeconds} seconds");
			}
			finally
			{
				EditorApplication.UnlockReloadAssemblies();
			}
		}
	}
}
