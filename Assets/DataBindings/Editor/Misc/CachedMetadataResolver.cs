using Realmar.DataBindings.Editor.Misc;
using Mono.Cecil;

namespace Realmar.DataBindings.Editor
{
	public class CachedMetadataResolver : MetadataResolver
	{
		private readonly Cache _cache = new Cache();

		public CachedMetadataResolver(IAssemblyResolver assemblyResolver) : base(assemblyResolver)
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
