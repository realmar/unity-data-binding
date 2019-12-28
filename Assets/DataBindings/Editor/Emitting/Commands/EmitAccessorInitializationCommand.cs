using Mono.Cecil;
using Mono.Cecil.Cil;
using Realmar.DataBindings.Editor.Commands;
using Realmar.DataBindings.Editor.Utils;
using Realmar.DataBindings.Editor.Weaving.Commands;
using System.Collections.Generic;
using System.Linq;
using static Realmar.DataBindings.Editor.Emitting.EmitHelpers;

namespace Realmar.DataBindings.Editor.Emitting.Command
{
	// TODO refactor this emitting code, lots of method arguments and parameter objects, very confusing
	internal class EmitAccessorInitializationCommand : BaseCommand
	{
		private DataMediator<List<Instruction>> _instructions;
		private MethodDefinition _bindingInitializer;
		private IMemberDefinition _bindingTarget;
		private DataMediator<MethodDefinition> _accessorSymbol;
		private DataMediator<Instruction> _skipBranch;
		private bool _throwOnFailure;

		public override void Execute()
		{
			// IL_0008: ldarg.0      // this
			// IL_0009: ldfld        class Realmar.DataBindings.Example.ExampleView Realmar.DataBindings.Example.ExampleViewModel::_view
			// IL_000e: ldarg.0      // this
			// IL_000f: stfld        class Realmar.DataBindings.Example.ExampleViewModel Realmar.DataBindings.Example.ExampleView::ViewModel

			var methodBody = _bindingInitializer.Body;
			var lastInstruction = methodBody.Instructions.Last();
			var referencingLast = GetInstructionsReferencing(lastInstruction, methodBody.Instructions);

			var accessorSymbol = _accessorSymbol.Data;
			var first = Instruction.Create(OpCodes.Ldarg_0);

			if (_throwOnFailure)
			{
				_skipBranch.Data = first;
			}
			else
			{
				_skipBranch.Data = lastInstruction;
			}

			ExecuteNext();

			_instructions.Data.Add(first);
			_instructions.Data.Add(GetLoadFromFieldOrCallableInstruction(_bindingTarget));
			_instructions.Data.Add(Instruction.Create(OpCodes.Ldarg_0));
			_instructions.Data.Add(Instruction.Create(GetCallInstruction(accessorSymbol), accessorSymbol));

			AppendInstructionsToMethod(_bindingInitializer, _instructions.Data);

			foreach (var instruction in referencingLast)
			{
				instruction.Operand = _instructions.Data[0];
			}
		}

		public static ICommand Create(DataMediator<MethodDefinition> accessorSymbol, MethodDefinition bindingInitializer, IMemberDefinition bindingTarget, bool throwOnFailure)
		{
			var instructions = new DataMediator<List<Instruction>> { Data = new List<Instruction>() };
			var skipBranch = new DataMediator<Instruction>();

			var command = new EmitAccessorInitializationCommand
			{
				_instructions = instructions,
				_bindingTarget = bindingTarget,
				_accessorSymbol = accessorSymbol,
				_skipBranch = skipBranch,
				_bindingInitializer = bindingInitializer,
				_throwOnFailure = throwOnFailure
			};

			var constructorInfo = typeof(BindingTargetNullException).GetConstructor(new[] { typeof(string) });
			var exceptionCtor = bindingInitializer.Module.ImportReference(constructorInfo);


			if (throwOnFailure)
			{
				command.AddChild(EmitJumpToIfNotNullCommand.Create(instructions, bindingTarget, skipBranch));
				command.AddChild(EmitThrowCommand.Create(instructions, exceptionCtor, $"BindingTarget is null. Make sure to initialize it in the BindingInitializer method or some time before calling it. BindingInitializer = {bindingInitializer.FullName} BindingTarget = {bindingTarget.FullName}"));
			}
			else
			{
				command.AddChild(EmitJumpToIfNullCommand.Create(instructions, bindingTarget, skipBranch));
			}

			return command;
		}
	}
}
