using Mono.Cecil;
using Mono.Cecil.Cil;
using System.Collections.Generic;
using static Realmar.DataBindings.Editor.Emitting.EmitHelpers;

namespace Realmar.DataBindings.Editor.Emitting
{
	internal class MethodAppender
	{
		private readonly List<Instruction> _instructions = new List<Instruction>();
		private readonly MethodDefinition _method;

		public MethodAppender(MethodDefinition method)
		{
			_method = method;
		}

		internal void AddInstruction(Instruction instruction)
		{
			_instructions.Add(instruction);
		}

		internal void Emit()
		{
			var methodBody = _method.Body;
			var ilProcessor = methodBody.GetILProcessor();
			var methodInstructions = methodBody.Instructions;
			var lastInstruction = methodInstructions[methodInstructions.Count - 1];
			var referencingLast = GetInstructionsReferencing(lastInstruction, methodInstructions);
			var firstInjected = _instructions[0];

			foreach (var instruction in _instructions)
			{
				ilProcessor.InsertBefore(lastInstruction, instruction);
			}

			foreach (var instruction in referencingLast)
			{
				instruction.Operand = firstInjected;
			}
		}
	}
}
