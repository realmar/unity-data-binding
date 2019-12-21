using Realmar.DataBindings.Editor.Utils;
using Mono.Cecil;

namespace Realmar.DataBindings.Editor.Cecil
{
	internal class CachedMetadataResolver : MetadataResolver
	{
		private readonly Cache _cache = new Cache();

		internal CachedMetadataResolver(IAssemblyResolver assemblyResolver) : base(assemblyResolver)
		{
		}

		public override TypeDefinition Resolve(TypeReference type)
		{
			return _cache.Get(type, base.Resolve);
		}

		public override FieldDefinition Resolve(FieldReference field)
		{
			return _cache.Get(field, base.Resolve);
		}

		public override MethodDefinition Resolve(MethodReference method)
		{
			return _cache.Get(method, base.Resolve);
		}
	}
}
