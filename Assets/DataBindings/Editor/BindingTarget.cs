using Mono.Cecil;

namespace Realmar.DataBindingsEditor
{
	internal struct BindingTarget
	{
		public IMemberDefinition Source { get; set; }
		public int Id { get; set; }
	}
}
