using System;
using System.Reflection;
using Realmar.DataBindings.Editor.Utils;
using Mono.Cecil;

namespace Realmar.DataBindings.Editor.Cecil
{
	internal class CachedReflectionImporter : DefaultReflectionImporter
	{
		private readonly Cache _cache = new Cache();

		internal CachedReflectionImporter(ModuleDefinition module) : base(module)
		{
		}

		protected override IMetadataScope ImportScope(Type type)
		{
			return _cache.Get(type, base.ImportScope);
		}

		public override AssemblyNameReference ImportReference(AssemblyName name)
		{
			return _cache.Get(name, base.ImportReference);
		}

		public override TypeReference ImportReference(Type type, IGenericParameterProvider context)
		{
			return _cache.Get(type, context, base.ImportReference);
		}

		public override FieldReference ImportReference(FieldInfo field, IGenericParameterProvider context)
		{
			return _cache.Get(field, context, base.ImportReference);
		}

		public override MethodReference ImportReference(MethodBase method, IGenericParameterProvider context)
		{
			return _cache.Get(method, context, base.ImportReference);
		}
	}
}
