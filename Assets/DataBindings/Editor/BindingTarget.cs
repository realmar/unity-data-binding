using Mono.Cecil;

namespace Realmar.DataBindings.Editor
{
	internal struct BindingTarget
	{
		public IMemberDefinition Source { get; set; }
		public int Id { get; set; }
	}
}
