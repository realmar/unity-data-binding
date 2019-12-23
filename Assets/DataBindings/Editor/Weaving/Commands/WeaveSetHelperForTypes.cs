using DataBindings.Editor.Commands;
using Mono.Cecil;
using Realmar.DataBindings.Editor.Commands;

namespace Realmar.DataBindings.Editor.Weaving.Commands
{
	internal class WeaveSetHelperForTypes : BaseCommand
	{
		private WeaveSetHelperForTypes()
		{
		}

		internal static ICommand Create(params TypeDefinition[] types)
		{
			if (types.Length == 0)
			{
				return TerminalCommand.Create();
			}

			var command = new WeaveSetHelperForTypes();
			foreach (var type in types)
			{
				command.AddChild(WeaveSetHelpersInTypeCommand.Create(type));
			}

			return command;
		}
	}
}
