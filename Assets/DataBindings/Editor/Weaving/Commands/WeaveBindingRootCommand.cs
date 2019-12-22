using System;
using Realmar.DataBindings.Editor.Commands;
using Realmar.DataBindings.Editor.Extensions;
using static Realmar.DataBindings.Editor.Weaving.WeaverHelpers;
using static Realmar.DataBindings.Editor.Exceptions.YeetHelpers;

namespace Realmar.DataBindings.Editor.Weaving.Commands
{
	internal class WeaveBindingRootCommand : BaseCommand
	{
		private WeaveBindingRootCommand()
		{
		}

		internal static ICommand Create(WeaveParameters parameters)
		{
			var command = new WeaveBindingRootCommand();

			var fromPropertyDeclaringType = parameters.FromProperty.DeclaringType;
			YeetIfInaccessible(parameters.ToProperty.SetMethod, fromPropertyDeclaringType);
			YeetIfInaccessible(parameters.BindingTarget, fromPropertyDeclaringType);

			if (parameters.FromProperty.GetSetMethodOrYeet().IsVirtual == false)
			{
				command.AddChild(WeaveBindingDirectCommand.Create(parameters));
			}
			else
			{
				command.AddChild(WeaveBindingInHierarchyCommand.Create(parameters));
			}

			return command;
		}
	}
}
