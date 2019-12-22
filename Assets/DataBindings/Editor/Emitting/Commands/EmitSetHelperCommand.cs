using Mono.Cecil;
using Realmar.DataBindings.Editor.Commands;
using Realmar.DataBindings.Editor.Utils;
using Realmar.DataBindings.Editor.Weaving.Commands;

namespace Realmar.DataBindings.Editor.Emitting.Command
{
	internal class EmitSetHelperCommand : BaseCommand
	{
		private EmitSetHelperCommand()
		{
		}

		public static ICommand Create(string targetSetHelperMethodName, MethodDefinition setMethod)
		{
			var command = new EmitSetHelperCommand();
			var mediator = new DataMediator<MethodDefinition>();

			command.AddChild(EmitSetHelperMethodCommand.Create(mediator, targetSetHelperMethodName, setMethod));
			if (setMethod.IsAbstract == false)
			{
				command.AddChild(EmitSetHelperMethodBodyCommand.Create(mediator, setMethod));
			}

			return command;
		}
	}
}
