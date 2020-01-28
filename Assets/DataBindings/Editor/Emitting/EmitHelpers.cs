using Mono.Cecil.Cil;
using Mono.Collections.Generic;
using System.Collections.Generic;
using static Realmar.DataBindings.Editor.Exceptions.YeetHelpers;

namespace Realmar.DataBindings.Editor.Emitting
{
	internal static class EmitHelpers
	{
		internal static List<Instruction> GetInstructionsReferencing(Instruction searchInstruction, Collection<Instruction> instructions)
		{
			YeetIfNull(searchInstruction, nameof(searchInstruction));
			YeetIfNull(instructions, nameof(instructions));

			var found = new List<Instruction>();

			foreach (var instruction in instructions)
			{
				if (ReferenceEquals(instruction.Operand, searchInstruction))
				{
					found.Add(instruction);
				}
			}

			return found;
		}
	}
}
