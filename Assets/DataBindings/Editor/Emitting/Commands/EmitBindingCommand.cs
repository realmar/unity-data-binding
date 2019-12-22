using System.Collections.Generic;
using System.Linq;
using Mono.Cecil;
using Mono.Cecil.Cil;
using Realmar.DataBindings.Editor.Commands;
using Realmar.DataBindings.Editor.Extensions;
using Realmar.DataBindings.Editor.Weaving;
using Realmar.DataBindings.Editor.Weaving.Commands;
using static Realmar.DataBindings.Editor.Weaving.WeaverHelpers;
using static Realmar.DataBindings.Editor.Emitting.EmitHelpers;

namespace Realmar.DataBindings.Editor.Emitting.Command
{
	internal class EmitBindingCommand : BaseCommand
	{
		private WeaveParameters _parameters;

		private EmitBindingCommand(WeaveParameters parameters)
		{
			_parameters = parameters;
		}

		public override void Execute()
		{
			var bindingTarget = _parameters.BindingTarget;
			var fromGetMethod = _parameters.FromProperty.GetGetMethodOrYeet();
			var fromSetMethod = _parameters.FromProperty.GetSetMethodOrYeet();
			var toSetMethod = GetSetHelperMethod(_parameters.ToProperty, _parameters.ToType);
			var emitNullCheck = _parameters.EmitNullCheck;

			var methodBody = fromSetMethod.Body;
			var ilProcessor = methodBody.GetILProcessor();
			var instructions = methodBody.Instructions;
			var lastInstruction = instructions.Last();

			var referencingLast = GetInstructionsReferencing(lastInstruction, instructions);

			// IL_0007: ldarg.0      // this
			// IL_0008: call instance class Realmar.UnityMVVM.Example.ExampleView Realmar.UnityMVVM.Example.ExampleViewModel::get_View()
			// IL_000d: ldarg.0      // this
			// IL_000e: call instance int32 Realmar.UnityMVVM.Example.ExampleViewModel::get_Value3()
			// IL_0013: call instance void Realmar.UnityMVVM.Example.ExampleView::set_Value3(int32)

			var il1 = ilProcessor.Create(OpCodes.Ldarg_0);
			var il2 = GetLoadFromFieldOrCallableInstruction(bindingTarget, ilProcessor);
			var il3 = ilProcessor.Create(OpCodes.Ldarg_0);
			var il4 = ilProcessor.Create(GetCallInstruction(fromGetMethod), fromGetMethod);
			var il5 = ilProcessor.Create(GetCallInstruction(toSetMethod), toSetMethod);

			Instruction firstInjected;

			if (emitNullCheck)
			{
				var nullCheckInstructions = EmitNullCheckInstructions(bindingTarget, ilProcessor, lastInstruction);
				var last = nullCheckInstructions[0];
				firstInjected = last;
				ilProcessor.InsertBefore(lastInstruction, last);
				for (var i = 1; i < nullCheckInstructions.Count; i++)
				{
					var inst = nullCheckInstructions[i];
					ilProcessor.InsertAfter(last, inst);
					last = inst;
				}

				ilProcessor.InsertAfter(last, il1);
			}
			else
			{
				firstInjected = il1;
				ilProcessor.InsertBefore(lastInstruction, il1);
			}

			ilProcessor.InsertAfter(il1, il2);
			ilProcessor.InsertAfter(il2, il3);
			ilProcessor.InsertAfter(il3, il4);
			ilProcessor.InsertAfter(il4, il5);

			foreach (var instruction in referencingLast)
			{
				instruction.Operand = firstInjected;
			}
		}

		private List<Instruction> EmitNullCheckInstructions(
			IMemberDefinition toBeChecked,
			ILProcessor ilProcessor,
			Instruction isNullBranch)
		{
			var instructions = new List<Instruction>();

			instructions.Add(ilProcessor.Create(OpCodes.Ldarg_0));
			instructions.Add(GetLoadFromFieldOrCallableInstruction(toBeChecked, ilProcessor));
			instructions.Add(ilProcessor.Create(OpCodes.Ldnull));

			var returnType = GetReturnType(toBeChecked);
			var op = returnType.GetInEqualityOperator();
			var opReference = toBeChecked.DeclaringType.Module.ImportReference(op);

			if (op != null)
			{
				instructions.Add(ilProcessor.Create(OpCodes.Call, opReference));
			}
			else
			{
				instructions.Add(ilProcessor.Create(OpCodes.Cgt_Un));
			}

			instructions.Add(ilProcessor.Create(OpCodes.Brfalse_S, isNullBranch));

			return instructions;
		}

		internal static ICommand Create(WeaveParameters parameters)
		{
			return new EmitBindingCommand(parameters);
		}
	}
}
