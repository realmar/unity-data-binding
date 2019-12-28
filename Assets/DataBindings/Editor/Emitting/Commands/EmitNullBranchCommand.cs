using Mono.Cecil;
using Mono.Cecil.Cil;
using Realmar.DataBindings.Editor.Utils;
using Realmar.DataBindings.Editor.Weaving.Commands;
using System.Collections.Generic;
using Realmar.DataBindings.Editor.Commands;
using Realmar.DataBindings.Editor.Shared.Extensions;
using static Realmar.DataBindings.Editor.Emitting.EmitHelpers;
using static Realmar.DataBindings.Editor.Shared.SharedHelpers;

namespace Realmar.DataBindings.Editor.Emitting.Command
{
	internal class EmitNullBranchCommand : BaseCommand
	{
		private DataMediator<List<Instruction>> _branchingInstructions;
		private IMemberDefinition _toToBeChecked;
		private DataMediator<Instruction> _skipBranch;
		private TypeDefinition _returnType;
		private OpCode _branchingOpCode;

		private EmitNullBranchCommand()
		{
		}

		public override void Execute()
		{
			var instructions = _branchingInstructions.Data;

			instructions.Add(Instruction.Create(OpCodes.Ldarg_0));
			instructions.Add(GetLoadFromFieldOrCallableInstruction(_toToBeChecked));
			instructions.Add(Instruction.Create(OpCodes.Ldnull));

			var op = _returnType.GetEqualityOperator();

			if (op != null)
			{
				var opReference = _toToBeChecked.DeclaringType.Module.ImportReference(op);
				instructions.Add(Instruction.Create(OpCodes.Call, opReference));
			}
			else
			{
				instructions.Add(Instruction.Create(OpCodes.Cgt_Un));
			}

			instructions.Add(Instruction.Create(_branchingOpCode, _skipBranch.Data));

			ExecuteNext();
		}

		internal static ICommand Create(DataMediator<List<Instruction>> branchingInstructions, IMemberDefinition toBeChecked, DataMediator<Instruction> skipBranch, OpCode branchingOpCode)
		{
			return new EmitNullBranchCommand
			{
				_branchingInstructions = branchingInstructions,
				_toToBeChecked = toBeChecked,
				_skipBranch = skipBranch,
				_branchingOpCode = branchingOpCode,
				_returnType = GetReturnType(toBeChecked)
			};
		}
	}
}
