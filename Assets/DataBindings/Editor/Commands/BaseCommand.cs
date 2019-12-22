using System.Collections.Generic;
using Realmar.DataBindings.Editor.Commands;

namespace Realmar.DataBindings.Editor.Weaving.Commands
{
	internal class BaseCommand : ICommand
	{
		private readonly List<ICommand> _childern = new List<ICommand>();

		public virtual void Execute() => ExecuteNext();

		protected void AddChild(ICommand command) => _childern.Add(command);

		protected void ExecuteNext()
		{
			foreach (var command in _childern)
			{
				command.Execute();
			}
		}
	}
}
