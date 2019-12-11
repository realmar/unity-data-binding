using Mono.Cecil;

namespace Realmar.DataBindingsEditor
{
	public class ReflectionImporterProvider : IReflectionImporterProvider
	{
		public IReflectionImporter GetReflectionImporter(ModuleDefinition module)
		{
			return new CachedReflectionImporter(module);
		}
	}
}
