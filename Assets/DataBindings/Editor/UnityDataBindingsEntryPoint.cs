using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using DataBindings.Editor.Misc;
using Mono.Cecil;
using Mono.Cecil.Rocks;
using Realmar.DataBindings;
using Realmar.DataBindingsEditor.Binder;
using Realmar.DataBindingsEditor.Exceptions;
using Realmar.DataBindingsEditor.Extensions;
using UnityEditor;

namespace Realmar.DataBindingsEditor
{
	[InitializeOnLoad]
	internal class UnityDataBindingsEntryPoint
	{
		private readonly AttributeResolver _attributeResolver;
		private readonly OneWayBinder _oneWayBinder;
		private readonly FromTargetBinder _fromTargetBinder;
		private readonly TwoWayBinder _twoWayBinder;
		private readonly Dictionary<BindingType, IBinder> _binders;

		public UnityDataBindingsEntryPoint()
		{
			var derivativeResolver = new DerivativeResolver();
			_attributeResolver = new AttributeResolver();
			_oneWayBinder = new OneWayBinder(derivativeResolver);
			_fromTargetBinder = new FromTargetBinder(derivativeResolver);
			_twoWayBinder = new TwoWayBinder(_oneWayBinder, _fromTargetBinder);

			_binders = new Dictionary<BindingType, IBinder>
			{
				[BindingType.OneWay] = _oneWayBinder,
				[BindingType.OneWayFromTarget] = _fromTargetBinder,
				[BindingType.TwoWay] = _twoWayBinder
			};
		}

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

		[MenuItem("_______/___")]
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

			using (var assemblyResolver = new UnityAssemblyResolver())
			{
				var metadataResolver = new CachedMetadataResolver(assemblyResolver);
				using (var module = ModuleDefinition.ReadModule(assemblyPath, new ReaderParameters
				{
					ReflectionImporterProvider = new ReflectionImporterProvider(),
					AssemblyResolver = assemblyResolver,
					MetadataResolver = metadataResolver,
					ReadSymbols = true,
					ReadWrite = true,

					// TODO PROPER SOLUTION REQUIRED
					ThrowIfSymbolsAreNotMatching = false
				}))
				{
					foreach (var type in module.GetAllTypes())
					{
						foreach (var property in type.Properties)
						{
							BindProperty(property);
						}
					}

					module.Write();
				}
			}

			AssetDatabase.Refresh();

			stopwatch.Stop();
			UnityEngine.Debug.Log($"Finished weaving assemblies in {stopwatch.Elapsed.TotalSeconds} seconds");
		}

		private void BindProperty(PropertyDefinition propertyDefinition)
		{
			foreach (var bindingAttribute in propertyDefinition.GetCustomAttributes<BindingAttribute>())
			{
				var declaringType = propertyDefinition.DeclaringType;
				var settings = GetBindingSettings(bindingAttribute);
				var allBindingTargets = GetBindingTargets(declaringType);
				var bindingTargets = FilterBindingsTargets(propertyDefinition, settings, allBindingTargets);

				if (bindingTargets.Length == 0)
				{
					throw new MissingBindingTargetException(declaringType.Name);
				}

				_binders[settings.Type].Bind(propertyDefinition, settings, bindingTargets);
			}
		}

		private BindingTarget[] GetBindingTargets(TypeDefinition originType)
		{
			var targets = new List<BindingTarget>();

			foreach (var type in originType.EnumerateBaseClasses())
			{
				var bindingTargets =
					_attributeResolver.GetCustomAttributesOfSymbolsInType<BindingTargetAttribute>(type);
				for (var i = 0; i < bindingTargets.Count; i++)
				{
					var target = bindingTargets[i];
					var source = (IMemberDefinition) target.Source;
					if (source is PropertyDefinition propertyDefinition)
					{
						source = propertyDefinition.GetMethod;
					}

					targets.Add(new BindingTarget
					{
						Source = source,
						Id = (int) target.Attribute.ConstructorArguments[0].Value
					});
				}
			}

			return targets.ToArray();
		}

		private static BindingSettings GetBindingSettings(CustomAttribute attribute)
		{
			var ctorArgs = attribute.ConstructorArguments;
			return new BindingSettings
			{
				Type = (BindingType) ctorArgs[0].Value,
				TargetId = (int) ctorArgs[1].Value,
				TargetPropertyName = (string) ctorArgs[2].Value,
				EmitNullCheck = (bool) ctorArgs[3].Value
			};
		}

		private static BindingTarget[] FilterBindingsTargets(
			PropertyDefinition sourceProperty,
			BindingSettings settings,
			BindingTarget[] targets)
		{
			var bindingTargets =
				targets.Where(bindingTarget => bindingTarget.Id == settings.TargetId).ToArray();
			if (bindingTargets.Length == 0)
			{
				throw new MissingSymbolException(
					$"Cannot find BindingTarget with ID {settings.TargetId} for property {sourceProperty.FullName}");
			}

			return bindingTargets;
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
