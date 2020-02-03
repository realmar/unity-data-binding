using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Realmar.DataBindings.Editor.EditorTools;
using Realmar.DataBindings.Editor.TestFramework.Facades;
using UnityEditor;
using UnityEngine;

namespace Realmar.DataBindings.Editor.TestFramework.EditorTools
{
	internal static class TestFrameworkEditorTools
	{
		[MenuItem(MenuConstants.PREFIX + "/Compile and Weave TestCase"), UsedImplicitly]
		private static void WeaveAssemblyForSingleTest()
		{
			var popup = ScriptableObject.CreateInstance<CompileTestPopUp>();
			popup.Setup((type, testName) =>
			{
				using (var facade = new WeaverTestFacade())
				{
					var path = facade.CompileAndWeave(type, testName);
					Debug.Log(path);
					EditorUtility.DisplayDialog("Weaved", path, "Ok");
				}
			});
			popup.ShowUtility();
		}

		private class CompileTestPopUp : EditorWindow
		{
			private List<Type> _types;
			private List<string> _testNames;

			private int _type;
			private int _test;

			private Action<Type, string> _callback;

			public void Setup(Action<Type, string> callback)
			{
				_callback = callback;
				_types = AppDomain.CurrentDomain
					.GetAssemblies()
					.SelectMany(assembly => assembly.GetTypes())
					.Where(type => type.Namespace?.StartsWith("Realmar.DataBindings.Editor.Tests") ?? false)
					.ToList();

				ChangeTestNames();
			}

			private void OnGUI()
			{
				var previousType = _type;
				_type = EditorGUILayout.Popup(_type, _types.Select(type => type.Name).ToArray());
				if (previousType != _type)
				{
					ChangeTestNames();
				}

				_test = EditorGUILayout.Popup(_test, _testNames.ToArray());

				if (GUILayout.Button("Confirm"))
				{
					_callback?.Invoke(_types[_type], _testNames[_test]);
				}

				if (GUILayout.Button("Close"))
				{
					Close();
				}
			}

			private void ChangeTestNames()
			{
				var type = _types[_type];
				_testNames = type.GetMethods()
					.Select(info => info.Name)
					.ToList();
				_test = 0;
			}
		}
	}
}
