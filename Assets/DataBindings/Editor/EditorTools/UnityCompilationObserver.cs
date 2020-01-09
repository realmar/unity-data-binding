using Realmar.DataBindings.Editor.Binding;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEditor;
using UnityEditor.Compilation;

namespace Realmar.DataBindings.Editor.EditorTools
{
	[InitializeOnLoad]
	internal static class UnityCompilationObserver
	{
		private static readonly HashSet<string> _compiledAssemblies = new HashSet<string>();

		static UnityCompilationObserver()
		{
			CompilationPipeline.assemblyCompilationStarted += OnCompilationStarted;
			AssemblyReloadEvents.beforeAssemblyReload += WeaveAssemblies;
			AssemblyReloadEvents.afterAssemblyReload += OnAfterAssemblyReload;
		}

		private static void OnAfterAssemblyReload()
		{
			_compiledAssemblies.Clear();
		}

		private static void OnCompilationStarted(string assemblyPath)
		{
			if (ShouldBeWeaved(assemblyPath))
			{
				_compiledAssemblies.Add(assemblyPath);
			}
		}

		private static bool ShouldBeWeaved(string assemblyPath)
		{
			var settings = ScriptableManager.Get<SettingsScriptable>();
			return settings.Enabled && settings.ShouldAssemblyBeWeaved(assemblyPath);
		}

		private static void WeaveAssemblies()
		{
			foreach (var assemblyPath in _compiledAssemblies)
			{
				var settings = ScriptableManager.Get<SettingsScriptable>();
				if (settings.Enabled && settings.ShouldAssemblyBeWeaved(assemblyPath))
				{
					WeaveAssembly(assemblyPath);
				}
			}
		}

		private static void WeaveAssembly(string assemblyPath)
		{
			UnityEngine.Debug.Log("Start weaving assemblies");

			var stopwatch = Stopwatch.StartNew();
			using (var facade = new BindingFacade())
			{
				facade.CreateBindingsInAssembly(assemblyPath);
			}

			stopwatch.Stop();

			AssetDatabase.Refresh();

			UnityEngine.Debug.Log($"Finished weaving assemblies in {stopwatch.Elapsed.TotalSeconds} seconds");
		}
	}
}
