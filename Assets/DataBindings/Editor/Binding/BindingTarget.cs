using System.Diagnostics;
using Mono.Cecil;

namespace Realmar.DataBindings.Editor.Binding
{
	[DebuggerStepThrough]
	internal struct BindingTarget
	{
		public IMemberDefinition Source { get; set; }
		public int Id { get; set; }
	}
}
