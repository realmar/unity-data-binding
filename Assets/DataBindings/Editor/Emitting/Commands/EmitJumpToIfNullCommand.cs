using Mono.Cecil;
using Mono.Cecil.Cil;
using Realmar.DataBindings.Editor.Commands;
using Realmar.DataBindings.Editor.Utils;
using System.Collections.Generic;
using Realmar.DataBindings.Editor.Weaving.Commands;


namespace Realmar.DataBindings.Editor.Emitting.Command
{
	internal class EmitJumpToIfNullCommand : BaseCommand
	{
		private EmitJumpToIfNullCommand()
		{
		}

		internal static ICommand Create(DataMediator<List<Instruction>> branchingInstructions, IMemberDefinition toBeChecked, DataMediator<Instruction> jumpTarget)
		{
			return EmitNullBranchCommand.Create(branchingInstructions, toBeChecked, jumpTarget, OpCodes.Brfalse_S);
		}
	}
}
