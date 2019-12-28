using Mono.Cecil;
using Mono.Cecil.Cil;
using Realmar.DataBindings.Editor.Commands;
using Realmar.DataBindings.Editor.Utils;
using Realmar.DataBindings.Editor.Weaving.Commands;
using System.Collections.Generic;

namespace Realmar.DataBindings.Editor.Emitting.Command
{
	internal class EmitJumpToIfNotNullCommand : BaseCommand
	{
		private EmitJumpToIfNotNullCommand()
		{
		}

		internal static ICommand Create(DataMediator<List<Instruction>> branchingInstructions, IMemberDefinition toBeChecked, DataMediator<Instruction> jumpTarget)
		{
			return EmitNullBranchCommand.Create(branchingInstructions, toBeChecked, jumpTarget, OpCodes.Brtrue_S);
		}
	}
}
