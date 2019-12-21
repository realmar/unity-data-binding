using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using UnityEditor;

namespace Realmar.DataBindings.Editor
{
	[InitializeOnLoad]
	internal class UnityDataBindingsEntryPoint
	{
		static UnityDataBindingsEntryPoint()
		{
			AssemblyReloadEvents.beforeAssemblyReload += () =>
			{
				if (_requestedWeaving)
				{
					_requestedWeaving = false;
					WeaveAssemblies();
				}
			};
		}

		private static bool _requestedWeaving;

		[MenuItem("DataBindings/Weave")]
		private static void ManuallyExecuteWeaving()
		{
			_requestedWeaving = true;
			DirtyAllScripts();
		}

		private static void WeaveAssemblies()
		{
			var app = new UnityDataBindingsEntryPoint();
			app.Main();
		}

		private void Main()
		{
			var stopwatch = Stopwatch.StartNew();
			UnityEngine.Debug.Log("Start weaving assemblies");

			// TODO Proper solution
			// TODO Detected changes
			// TODO Select assemblies to be weaved
			var assemblyPath = AppDomain.CurrentDomain.GetAssemblies()
				.Where(p => !p.IsDynamic).First(assembly => assembly.FullName.Contains("Assembly-CSharp")).Location;

			var facade = new BindingFacade();
			facade.WeaveAssembly(assemblyPath);

			AssetDatabase.Refresh();

			stopwatch.Stop();
			UnityEngine.Debug.Log($"Finished weaving assemblies in {stopwatch.Elapsed.TotalSeconds} seconds");
		}


		private static void DirtyAllScripts()
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
				var staticBindingFlags = BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic;
				var dirtyAllScriptsMethod =
					compilationInterface.GetMethod(methodName, staticBindingFlags);
				return dirtyAllScriptsMethod.Invoke(null, null);
			}

			return null;
		}
	}
}
