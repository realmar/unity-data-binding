using Mono.Cecil;

namespace Realmar.DataBindings.Editor.Binding
{
	internal struct BindingTarget
	{
		public IMemberDefinition Source { get; set; }
		public int Id { get; set; }
	}
}
