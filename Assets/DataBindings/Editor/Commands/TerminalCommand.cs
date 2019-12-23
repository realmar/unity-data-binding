using Realmar.DataBindings.Editor.Commands;
using Realmar.DataBindings.Editor.Weaving.Commands;

namespace DataBindings.Editor.Commands
{
	internal class TerminalCommand : BaseCommand
	{
		private static readonly TerminalCommand _instance = new TerminalCommand();

		private TerminalCommand()
		{
		}

		internal static ICommand Create() => _instance;
	}
}
