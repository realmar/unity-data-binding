using Mono.Cecil;
using Mono.Cecil.Cil;
using static Realmar.DataBindings.Editor.Emitting.EmitHelpers;

namespace Realmar.DataBindings.Editor.Emitting
{
	internal class MethodAppender : MethodExtender
	{
		public MethodAppender(MethodDefinition method) : base(method)
		{
		}

		internal override void Emit()
		{
			var lastInstruction = GetSucceedingInstruction();
			var referencingLast = GetInstructionsReferencing(lastInstruction, Method.Body.Instructions);

			base.Emit();

			var firstInjected = Instructions[0];
			foreach (var instruction in referencingLast)
			{
				instruction.Operand = firstInjected;
			}
		}

		protected override Instruction GetSucceedingInstruction()
		{
			var methodBody = Method.Body;
			var methodInstructions = methodBody.Instructions;
			return methodInstructions[methodInstructions.Count - 1];
		}
	}
}
