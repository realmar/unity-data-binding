using Mono.Cecil;
using Mono.Cecil.Cil;
using Realmar.DataBindings.Editor.Commands;
using Realmar.DataBindings.Editor.Utils;
using Realmar.DataBindings.Editor.Weaving.Commands;
using System.Collections.Generic;

namespace Realmar.DataBindings.Editor.Emitting.Command
{
	internal class EmitThrowCommand : BaseCommand
	{
		private string _message;
		private MethodReference _exceptionCtor;
		private DataMediator<List<Instruction>> _instructions;

		private EmitThrowCommand()
		{
		}

		public override void Execute()
		{
			// IL_001b: ldstr        "BindingTarget (TODO FULL PATH) is null."
			// IL_0020: newobj instance void Realmar.DataBindings.BindingTargetNullException::.ctor(string)
			// IL_0025: throw

			var instructions = _instructions.Data;
			if (_message != null)
			{
				instructions.Add(Instruction.Create(OpCodes.Ldstr, _message));
			}

			instructions.Add(Instruction.Create(OpCodes.Newobj, _exceptionCtor));
			instructions.Add(Instruction.Create(OpCodes.Throw));

			ExecuteNext();
		}

		internal static ICommand Create(DataMediator<List<Instruction>> instructions, MethodReference exceptionCtor, string message = null)
		{
			return new EmitThrowCommand
			{
				_message = message,
				_instructions = instructions,
				_exceptionCtor = exceptionCtor
			};
		}
	}
}
