using System.Collections.Generic;
using Realmar.DataBindings.Editor.BCL.System;
using Realmar.DataBindings.Editor.Commands;
using Realmar.DataBindings.Editor.Emitting.Command;
using Realmar.DataBindings.Editor.Extensions;
using Realmar.DataBindings.Editor.Utils;

namespace Realmar.DataBindings.Editor.Weaving.Commands
{
	internal class WeaveBindingDirectCommand : BaseCommand
	{
		private WeaveBindingDirectCommand()
		{
		}

		// TODO - Encapsulation: do we really need all params?
		internal static ICommand Create(WeaveParameters parameters)
		{
			var command = new WeaveBindingDirectCommand();
			command.AddChild(WeaveSetHelpersInTypeCommand.Create(parameters.ToType));
			command.AddChild(WeaveSetHelpersInTypeCommand.Create(parameters.FromProperty.DeclaringType));

			// WEAVE BINDING
			var fromSetMethod = parameters.FromProperty.GetSetMethodOrYeet();
			// TODO - SRP: pull up to caller and let the caller do error handling?
			if (fromSetMethod.IsAbstract == false)
			{
				var hash = GetBindingHashCode(parameters);
				var wovenBindings = ServiceLocator.Current.Resolve<HashSet<int>>("WovenBindings");

				if (wovenBindings.Contains(hash) == false)
				{
					wovenBindings.Add(hash);
					command.AddChild(EmitBindingCommand.Create(parameters));
				}
			}

			return command;
		}

		private static int GetBindingHashCode(WeaveParameters parameters)
			=> HashCode.Combine(parameters.FromProperty, parameters.ToProperty, parameters.BindingTarget);
	}
}
