using Mono.Cecil;

namespace Realmar.DataBindings.Editor
{
	public class ReflectionImporterProvider : IReflectionImporterProvider
	{
		public IReflectionImporter GetReflectionImporter(ModuleDefinition module)
		{
			return new CachedReflectionImporter(module);
		}
	}
}
