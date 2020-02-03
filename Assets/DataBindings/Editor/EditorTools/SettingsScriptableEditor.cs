using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using Realmar.DataBindings.Editor.Shared.Extensions;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace Realmar.DataBindings.Editor.EditorTools
{
	[CustomEditor(typeof(SettingsScriptable))]
	public class SettingsScriptableEditor : UnityEditor.Editor
	{
		private const string AssembliesToBeWeavedPropertyName = "_assembliesToBeWeaved";
		private const string EnabledPropertyName = "_enabled";

		private static readonly Regex[] Excluded =
		{
			new Regex(@"^Unity(Engine|Editor)?", RegexOptions.Compiled),
			new Regex(@"^ExCSS\.Unity", RegexOptions.Compiled),
			new Regex(@"^SyntaxTree\.", RegexOptions.Compiled),
			new Regex(@"^Microsoft\.", RegexOptions.Compiled),
			new Regex(@"^System(,|\.)?", RegexOptions.Compiled),
			new Regex(@"^Mono\.", RegexOptions.Compiled),
			new Regex(@"^VisualStudio\.", RegexOptions.Compiled),
			new Regex(@"^nunit\.", RegexOptions.Compiled),
			new Regex(@"^mscorlib", RegexOptions.Compiled),
			new Regex(@"^netstandard", RegexOptions.Compiled),
		};

		private ReorderableList _assemblies;
		private List<Assembly> _allAssemblies;

		private SerializedProperty _assembliesProperty;
		private SerializedProperty _enabledProperty;
		private List<string> _assembliesToBeWeaved;

		private void OnEnable()
		{
			_assembliesToBeWeaved = (List<string>) target.GetType().GetFieldOrPropertyValue(AssembliesToBeWeavedPropertyName, target);

			_assembliesProperty = serializedObject.FindProperty(AssembliesToBeWeavedPropertyName);
			_enabledProperty = serializedObject.FindProperty(EnabledPropertyName);

			_allAssemblies = AppDomain.CurrentDomain.GetAssemblies().Where(p => !p.IsDynamic).ToList();

			_assemblies = new ReorderableList(serializedObject, _assembliesProperty, true, false, false, true)
			{
				drawHeaderCallback = rect => EditorGUI.LabelField(rect, "Scheduled to be weaved", EditorStyles.boldLabel),
				drawElementCallback = (rect, index, active, focused) => EditorGUI.LabelField(rect, _assembliesProperty.GetArrayElementAtIndex(index).stringValue)
			};
		}

		public override void OnInspectorGUI()
		{
			serializedObject.Update();

			EditorGUI.BeginChangeCheck();

			EditorGUILayout.PropertyField(_enabledProperty);

			EditorGUILayout.Space();
			EditorUIHelpers.DrawLine();
			EditorGUILayout.Space();

			_assemblies.DoLayoutList();

			EditorGUILayout.LabelField("Available Assemblies", EditorStyles.boldLabel);

			foreach (var loadedAssembly in _allAssemblies.Where(FilterAssemblies))
			{
				var assemblyName = loadedAssembly.GetName().Name;

				EditorGUILayout.BeginHorizontal();
				EditorGUILayout.LabelField(assemblyName);
				if (GUILayout.Button("Add"))
				{
					_assembliesToBeWeaved.Add(assemblyName);
				}

				EditorGUILayout.EndHorizontal();
			}

			serializedObject.ApplyModifiedProperties();
			if (EditorGUI.EndChangeCheck())
			{
				EditorUtility.SetDirty(serializedObject.targetObject);
			}
		}

		private bool FilterAssemblies(Assembly assembly)
		{
			var assemblyName = assembly.GetName().Name;

			if (Excluded.Any(regex => regex.IsMatch(assemblyName)))
			{
				return false;
			}

			return _assembliesToBeWeaved.Any(x => x == assemblyName) == false;
		}
	}
}
