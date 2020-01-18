using Mono.Cecil;
using Mono.Cecil.Cil;

namespace Realmar.DataBindings.Editor.Emitting
{
	internal class MethodPreppender : MethodExtender
	{
		public MethodPreppender(MethodDefinition method) : base(method)
		{
		}

		protected override Instruction GetSucceedingInstruction()
		{
			return Method.Body.Instructions[0];
		}
	}
}
