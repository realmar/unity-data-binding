using Mono.Cecil;
using Mono.Cecil.Cil;
using System.Collections.Generic;

namespace Realmar.DataBindings.Editor.Emitting
{
	internal abstract class MethodExtender
	{
		protected List<Instruction> Instructions { get; } = new List<Instruction>();
		protected MethodDefinition Method { get; }

		protected MethodExtender(MethodDefinition method)
		{
			Method = method;
		}

		internal void AddInstruction(Instruction instruction)
		{
			Instructions.Add(instruction);
		}

		internal virtual void Emit()
		{
			var methodBody = Method.Body;
			var ilProcessor = methodBody.GetILProcessor();
			var lastInstruction = GetSucceedingInstruction();

			foreach (var instruction in Instructions)
			{
				ilProcessor.InsertBefore(lastInstruction, instruction);
			}
		}

		protected abstract Instruction GetSucceedingInstruction();
	}
}
