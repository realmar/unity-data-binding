using System;
using System.Collections.Generic;
using System.Linq;
using Mono.Cecil;
using Realmar.DataBindings.Editor.IoC;

namespace Realmar.DataBindings.Editor.Cecil
{
	internal class UnityAssemblyResolver : BaseAssemblyResolver
	{
		private bool _isDisposed;
		private readonly IDictionary<string, string> _appDomainAssemblyLocations = new Dictionary<string, string>();

		private readonly IDictionary<string, AssemblyDefinition> _assemblyDefinitionCache =
			new Dictionary<string, AssemblyDefinition>();

		internal UnityAssemblyResolver()
		{
			var assemblies = AppDomain.CurrentDomain.GetAssemblies().Where(p => !p.IsDynamic);

			foreach (var assembly in assemblies)
			{
				_appDomainAssemblyLocations[assembly.FullName] = assembly.Location;
				AddSearchDirectory(System.IO.Path.GetDirectoryName(assembly.Location));
			}
		}

		protected override void Dispose(bool disposing)
		{
			if (_isDisposed) return;

			foreach (var assemblyDefinition in _assemblyDefinitionCache.Values)
			{
				assemblyDefinition.Dispose();
			}

			base.Dispose(disposing);

			_isDisposed = true;
		}

		public override AssemblyDefinition Resolve(AssemblyNameReference name)
		{
			var assemblyDef = FindAssemblyDefinition(name.FullName, null);

			if (assemblyDef == null)
			{
				assemblyDef = base.Resolve(name);
				_assemblyDefinitionCache[name.FullName] = assemblyDef;
			}

			return assemblyDef;
		}

		public override AssemblyDefinition Resolve(AssemblyNameReference name, ReaderParameters parameters)
		{
			var assemblyDef = FindAssemblyDefinition(name.FullName, parameters);

			if (assemblyDef == null)
			{
				assemblyDef = base.Resolve(name, parameters);
				_assemblyDefinitionCache[name.FullName] = assemblyDef;
			}

			return assemblyDef;
		}

		private AssemblyDefinition FindAssemblyDefinition(string fullName, ReaderParameters parameters)
		{
			parameters = parameters ?? new ReaderParameters();
			parameters.InMemory = true;
			parameters.ReadSymbols = false;
			parameters.ReadWrite = false;
			parameters.ReflectionImporterProvider = ServiceLocator.Current.Resolve<IReflectionImporterProvider>();
			parameters.AssemblyResolver = ServiceLocator.Current.Resolve<IAssemblyResolver>();
			parameters.MetadataResolver = ServiceLocator.Current.Resolve<IMetadataResolver>();

			if (_assemblyDefinitionCache.TryGetValue(fullName, out var assemblyDefinition) == false)
			{
				if (_appDomainAssemblyLocations.TryGetValue(fullName, out var location))
				{
					assemblyDefinition = parameters != null
						? AssemblyDefinition.ReadAssembly(location, parameters)
						: AssemblyDefinition.ReadAssembly(location);

					_assemblyDefinitionCache[fullName] = assemblyDefinition;
				}
			}

			return assemblyDefinition;
		}
	}
}
