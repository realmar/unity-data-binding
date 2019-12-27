using Mono.Cecil;
using Mono.Cecil.Cil;
using Realmar.DataBindings.Editor.Commands;
using Realmar.DataBindings.Editor.Utils;
using Realmar.DataBindings.Editor.Weaving.Commands;
using System.Linq;
using static Realmar.DataBindings.Editor.Emitting.EmitHelpers;

namespace Realmar.DataBindings.Editor.Emitting.Command
{
	internal class EmitAccessorInitializationCommand : BaseCommand
	{
		private MethodDefinition _bindingInitializer;
		private IMemberDefinition _bindingTarget;
		private DataMediator<MethodDefinition> _accessorSymbol;

		private EmitAccessorInitializationCommand(DataMediator<MethodDefinition> accessorSymbol, MethodDefinition bindingInitializer, IMemberDefinition bindingTarget)
		{
			_bindingInitializer = bindingInitializer;
			_bindingTarget = bindingTarget;
			_accessorSymbol = accessorSymbol;
		}

		public override void Execute()
		{
			var accessorSymbol = _accessorSymbol.Data;
			var methodBody = _bindingInitializer.Body;
			var ilProcessor = methodBody.GetILProcessor();
			var instructions = methodBody.Instructions;
			var lastInstruction = instructions.Last();

			var referencingLast = GetInstructionsReferencing(lastInstruction, instructions);

			// IL_0008: ldarg.0      // this
			// IL_0009: ldfld        class Realmar.DataBindings.Example.ExampleView Realmar.DataBindings.Example.ExampleViewModel::_view
			// IL_000e: ldarg.0      // this
			// IL_000f: stfld        class Realmar.DataBindings.Example.ExampleViewModel Realmar.DataBindings.Example.ExampleView::ViewModel

			var il1 = Instruction.Create(OpCodes.Ldarg_0);
			var il2 = GetLoadFromFieldOrCallableInstruction(_bindingTarget);
			var il3 = Instruction.Create(OpCodes.Ldarg_0);
			var il4 = Instruction.Create(GetCallInstruction(accessorSymbol), accessorSymbol);

			ilProcessor.InsertBefore(lastInstruction, il1);
			ilProcessor.InsertAfter(il1, il2);
			ilProcessor.InsertAfter(il2, il3);
			ilProcessor.InsertAfter(il3, il4);

			foreach (var instruction in referencingLast)
			{
				instruction.Operand = il1;
			}

			ExecuteNext();
		}

		public static ICommand Create(DataMediator<MethodDefinition> accessorSymbol, MethodDefinition bindingInitializer, IMemberDefinition bindingTarget)
		{
			return new EmitAccessorInitializationCommand(accessorSymbol, bindingInitializer, bindingTarget);
		}
	}
}
