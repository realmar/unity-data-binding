using Realmar.DataBindings.Editor.Commands;
using Realmar.DataBindings.Editor.Extensions;
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

			command.AddChild(WeaveSetHelperForTypes.Create(parameters.ToType, parameters.FromProperty.DeclaringType));
			if (parameters.FromProperty.GetSetMethodOrYeet().IsVirtual == false)
			{
				command.AddChild(WeaveNonAbstractBindingCommand.Create(parameters));
			}
			else
			{
				command.AddChild(WeaveBindingInHierarchyCommand.Create(parameters));
			}

			return command;
		}
	}
}
