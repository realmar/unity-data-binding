using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Realmar.DataBindings.Editor.EditorTools
{
	internal static class ScriptableManager
	{
		private static readonly string[] BasePathSegments = { "Assets", "Editor Default Resources", "Realmar.DataBindings" };
		private static readonly Dictionary<Type, Object> _managers = new Dictionary<Type, Object>();

		private static string BasePath { get; } = string.Join("/", BasePathSegments);

		internal static T Get<T>()
			where T : ScriptableObject
		{
			var asset = LoadAsset<T>();
			if (asset == null)
			{
				asset = CreateNew<T>();
			}

			return (T) asset;
		}

		private static Object LoadAsset<T>() where T : Object
		{
			var type = typeof(T);
			Object asset;

			if (_managers.ContainsKey(type))
			{
				asset = _managers[type];
			}
			else
			{
				asset = EditorGUIUtility.Load(GetFilePath<T>());
				if (asset != null)
				{
					_managers[type] = asset;
				}
			}

			return asset;
		}

		private static T CreateNew<T>() where T : ScriptableObject
		{
			EnsureBasePath();

			var path = GetFilePath<T>();
			var asset = ScriptableObject.CreateInstance<T>();

			AssetDatabase.CreateAsset(asset, path);
			AssetDatabase.Refresh();
			AssetDatabase.SaveAssets();

			_managers[typeof(T)] = asset;

			return asset;
		}

		private static string GetFileName<T>()
		{
			var nameAttribute = typeof(T).GetCustomAttribute<AssetNameAttribute>();
			if (nameAttribute == null || string.IsNullOrEmpty(nameAttribute.Name))
			{
				return $"{typeof(T).Name}.asset";
			}
			else
			{
				return $"{nameAttribute.Name}.asset";
			}
		}

		private static string GetFilePath<T>()
		{
			return $"{BasePath}/{GetFileName<T>()}";
		}

		private static void EnsureBasePath()
		{
			var path = BasePathSegments[0];
			for (var i = 1; i < BasePathSegments.Length; i++)
			{
				var segment = BasePathSegments[i];
				var newPath = $"{path}/{segment}";

				if (AssetDatabase.IsValidFolder(newPath) == false)
				{
					AssetDatabase.CreateFolder(path, segment);
				}

				path = newPath;
			}
		}
	}
}
