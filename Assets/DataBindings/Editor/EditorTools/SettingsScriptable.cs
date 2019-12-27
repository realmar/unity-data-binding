using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace Realmar.DataBindings.Editor.EditorTools
{
	[AssetName("Settings")]
	internal class SettingsScriptable : ScriptableObject
	{
		[SerializeField, HideInInspector]
		private List<string> _assembliesToBeWeaved = new List<string> { "Assembly-CSharp" };

		[SerializeField, HideInInspector]
		private bool _enabled = true;

		public bool Enabled => _enabled;

		internal IEnumerable<Assembly> GetAssemblies()
		{
			return AppDomain.CurrentDomain
				.GetAssemblies()
				.Where(assembly => _assembliesToBeWeaved.Contains(assembly.GetName().Name));
		}
	}
}
