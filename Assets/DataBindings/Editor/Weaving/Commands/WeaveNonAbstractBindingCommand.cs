using Realmar.DataBindings.Editor.BCL.System;
using Realmar.DataBindings.Editor.Commands;
using Realmar.DataBindings.Editor.Emitting.Command;
using Realmar.DataBindings.Editor.Utils;
using System.Collections.Generic;

namespace Realmar.DataBindings.Editor.Weaving.Commands
{
	internal class WeaveNonAbstractBindingCommand : BaseCommand
	{
		private WeaveNonAbstractBindingCommand()
		{
		}

		internal static ICommand Create(WeaveParameters parameters)
		{
			var command = new WeaveNonAbstractBindingCommand();
			var hash = GetBindingHashCode(parameters);
			var wovenBindings = ServiceLocator.Current.Resolve<HashSet<int>>("WovenBindings");

			if (wovenBindings.Contains(hash) == false)
			{
				wovenBindings.Add(hash);
				command.AddChild(EmitBindingCommand.Create(parameters));
			}

			return command;
		}

		private static int GetBindingHashCode(WeaveParameters parameters)
			=> HashCode.Combine(parameters.FromProperty, parameters.ToProperty, parameters.BindingTarget);
	}
}