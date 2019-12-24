using System.Collections.Generic;
using System.Diagnostics;
using Realmar.DataBindings.Editor.Commands;

namespace Realmar.DataBindings.Editor.Weaving.Commands
{
	internal class BaseCommand : ICommand
	{
		private readonly List<ICommand> _childern = new List<ICommand>();

		[DebuggerStepThrough]
		public virtual void Execute() => ExecuteNext();

		[DebuggerStepThrough]
		protected void AddChild(ICommand command) => _childern.Add(command);

		[DebuggerStepThrough]
		protected void ExecuteNext()
		{
			foreach (var command in _childern)
			{
				command.Execute();
			}
		}
	}
}
