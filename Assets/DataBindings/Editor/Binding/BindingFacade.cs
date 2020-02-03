using System;
using System.Collections.Generic;
using System.Linq;
using Mono.Cecil;
using Mono.Cecil.Rocks;
using Realmar.DataBindings.Editor.Cecil;
using Realmar.DataBindings.Editor.Emitting;
using Realmar.DataBindings.Editor.IoC;
using Realmar.DataBindings.Editor.Weaving;

namespace Realmar.DataBindings.Editor.Binding
{
	internal class BindingFacade : IDisposable
	{
		internal struct Options
		{
			internal bool WeaveDebugSymbols { get; set; }

			// TODO this is actually a big hack
			// the issue is that when doing ImportReference Cecil will add bogus references: 1 reference to the assembly itself and some to mscorlib
			// when actually the assembly already has a reference to netstandard or a reference to a newer version of mscorlib. The issue is definitely deeper,
			// I currently dont understand why Cecil is doing this. However, by definition unity already needs to add references to any assembly required,
			// so we can just ignore all references added by Cecil. (Even with ADFs)
			internal bool RestoreOriginalReferences { get; set; }
		}

		private readonly IPropertyProcessor[] _propertyProcessors;
		private readonly UnityAssemblyResolver _assemblyResolver;
		private readonly CachedMetadataResolver _metadataResolver;

		private readonly Options _options;

		public BindingFacade() : this(new Options
		{
			WeaveDebugSymbols = false,
			RestoreOriginalReferences = true
		})
		{
		}

		public BindingFacade(Options options)
		{
			options.WeaveDebugSymbols = false;
			_options = options;
			_assemblyResolver = new UnityAssemblyResolver();
			_metadataResolver = new CachedMetadataResolver(_assemblyResolver);

			ConfigureServiceLocator();

			var oneWayBinder = new OneWayBinder();
			var fromTargetBinder = new FromTargetBinder();
			var oneTimeBinder = new OneTimeBinder();
			var twoWayBinder = new TwoWayBinder(oneWayBinder, fromTargetBinder);
			var binders = new Dictionary<BindingType, IBinder>
			{
				[BindingType.OneTime] = oneTimeBinder,
				[BindingType.OneWay] = oneWayBinder,
				[BindingType.OneWayFromTarget] = fromTargetBinder,
				[BindingType.TwoWay] = twoWayBinder
			};

			_propertyProcessors = new IPropertyProcessor[]
			{
				new PropertyBindingProcessor(binders),
				new InvokeMethodOnChangeProcessor()
			};
		}

		public void Dispose()
		{
			_assemblyResolver?.Dispose();
		}

		// TODO support multiple assemblies
		public void CreateBindingsInAssembly(string assemblyPath, string outputPath = null)
		{
			using (var assembly = AssemblyDefinition.ReadAssembly(assemblyPath, CreateReaderParameters()))
			{
				// this is a big hack, see options
				var originalReferences = MementifyReferences(assembly);
				ProcessAssembly(assembly);
				// this is a big hack, see options
				RestoreReferences(assembly, originalReferences);
				WriteAssembly(assembly, outputPath);
			}
		}

		private ReaderParameters CreateReaderParameters()
		{
			var assemblyResolver = ServiceLocator.Current.Resolve<IAssemblyResolver>();
			var metadataResolver = ServiceLocator.Current.Resolve<IMetadataResolver>();

			return new ReaderParameters
			{
				ReflectionImporterProvider = ServiceLocator.Current.Resolve<IReflectionImporterProvider>(),
				AssemblyResolver = assemblyResolver,
				MetadataResolver = metadataResolver,

				// PDB, MDB
				ReadSymbols = _options.WeaveDebugSymbols,
				ReadWrite = true,

				ThrowIfSymbolsAreNotMatching = true
			};
		}

		private void ProcessAssembly(AssemblyDefinition assembly)
		{
			foreach (var type in assembly.Modules.SelectMany(definition => definition.GetAllTypes()))
			{
				foreach (var property in type.Properties)
				{
					foreach (var processor in _propertyProcessors)
					{
						processor.Process(property);
					}
				}
			}
		}

		private Dictionary<ModuleDefinition, List<AssemblyNameReference>> MementifyReferences(AssemblyDefinition assembly)
		{
			Dictionary<ModuleDefinition, List<AssemblyNameReference>> originalReferences = null;
			if (_options.RestoreOriginalReferences)
			{
				originalReferences = assembly.Modules.ToDictionary(definition => definition, definition => definition.AssemblyReferences.ToList());
			}

			return originalReferences;
		}

		private void RestoreReferences(AssemblyDefinition assembly, Dictionary<ModuleDefinition, List<AssemblyNameReference>> originalReferences)
		{
			if (_options.RestoreOriginalReferences)
			{
				foreach (var module in assembly.Modules)
				{
					if (originalReferences.TryGetValue(module, out var references))
					{
						// TODO massive hack, also clean code pls
						var originals = new List<AssemblyNameReference>();
						foreach (var reference in references)
						{
							var temp = module.AssemblyReferences.FirstOrDefault(r => r.FullName == reference.FullName);
							if (temp != null)
							{
								originals.Add(temp);
							}
						}

						module.AssemblyReferences.Clear();
						originals.ForEach(module.AssemblyReferences.Add);
					}
				}
			}
		}

		private void WriteAssembly(AssemblyDefinition assembly, string outputPath = null)
		{
			var writeParameters = new WriterParameters
			{
				WriteSymbols = _options.WeaveDebugSymbols,
			};

			if (outputPath != null)
			{
				assembly.Write(outputPath, writeParameters);
			}
			else
			{
				assembly.Write(writeParameters);
			}
		}

		private void ConfigureServiceLocator()
		{
			ServiceLocator.Reset();
			var locator = ServiceLocator.Current;

			locator.RegisterType<Weaver>(ServiceLifetime.Singleton);
			locator.RegisterType<Emitter>();
			locator.RegisterType<DerivativeResolver>(ServiceLifetime.Singleton);
			locator.RegisterType<IReflectionImporterProvider, ReflectionImporterProvider>(ServiceLifetime.Singleton);
			locator.RegisterType<IAssemblyResolver>(_assemblyResolver);
			locator.RegisterType<IMetadataResolver>(_metadataResolver);
		}
	}
}
