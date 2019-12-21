using Mono.Cecil;

namespace Realmar.DataBindings.Editor.Cecil
{
	internal class ReflectionImporterProvider : IReflectionImporterProvider
	{
		public IReflectionImporter GetReflectionImporter(ModuleDefinition module)
		{
			return new CachedReflectionImporter(module);
		}
	}
}
