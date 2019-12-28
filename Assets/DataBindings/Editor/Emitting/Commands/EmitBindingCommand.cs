using Mono.Cecil.Cil;
using Realmar.DataBindings.Editor.Commands;
using Realmar.DataBindings.Editor.Shared.Extensions;
using Realmar.DataBindings.Editor.Utils;
using Realmar.DataBindings.Editor.Weaving;
using Realmar.DataBindings.Editor.Weaving.Commands;
using System.Collections.Generic;
using System.Linq;
using Realmar.DataBindings.Editor.Exceptions;
using static Realmar.DataBindings.Editor.Emitting.EmitHelpers;
using static Realmar.DataBindings.Editor.Shared.SharedHelpers;

namespace Realmar.DataBindings.Editor.Emitting.Command
{
	internal class EmitBindingCommand : BaseCommand
	{
		private WeaveParameters _parameters;
		private DataMediator<List<Instruction>> _instructions;
		private DataMediator<Instruction> _skipBranch;

		private EmitBindingCommand()
		{
		}

		public override void Execute()
		{
			var bindingTarget = _parameters.BindingTarget;
			var fromGetMethod = _parameters.FromProperty.GetGetMethodOrYeet();
			var fromSetMethod = _parameters.FromProperty.GetSetMethodOrYeet();
			var toSetMethod = GetSetHelperMethod(_parameters.ToProperty, _parameters.ToType);
			var methodBody = fromSetMethod.Body;
			var lastInstruction = methodBody.Instructions.Last();
			var referencingLast = GetInstructionsReferencing(lastInstruction, methodBody.Instructions);

			_skipBranch.Data = lastInstruction;
			ExecuteNext();

			// IL_0007: ldarg.0      // this
			// IL_0008: call instance class Realmar.UnityMVVM.Example.ExampleView Realmar.UnityMVVM.Example.ExampleViewModel::get_View()
			// IL_000d: ldarg.0      // this
			// IL_000e: call instance int32 Realmar.UnityMVVM.Example.ExampleViewModel::get_Value3()
			// IL_0013: call instance void Realmar.UnityMVVM.Example.ExampleView::set_Value3(int32)

			_instructions.Data.Add(Instruction.Create(OpCodes.Ldarg_0));
			_instructions.Data.Add(GetLoadFromFieldOrCallableInstruction(bindingTarget));
			_instructions.Data.Add(Instruction.Create(OpCodes.Ldarg_0));
			_instructions.Data.Add(Instruction.Create(GetCallInstruction(fromGetMethod), fromGetMethod));
			_instructions.Data.Add(Instruction.Create(GetCallInstruction(toSetMethod), toSetMethod));

			AppendInstructionsToMethod(fromSetMethod, _instructions.Data);

			foreach (var instruction in referencingLast)
			{
				instruction.Operand = _instructions.Data;
			}
		}

		internal static ICommand Create(WeaveParameters parameters)
		{
			var skipBranch = new DataMediator<Instruction>();
			var instructions = new DataMediator<List<Instruction>> { Data = new List<Instruction>() };

			var command = new EmitBindingCommand
			{
				_parameters = parameters,
				_instructions = instructions,
				_skipBranch = skipBranch
			};

			if (parameters.EmitNullCheck)
			{
				command.AddChild(EmitJumpToIfNullCommand.Create(instructions, parameters.BindingTarget, skipBranch));
			}

			return command;
		}
	}
}
