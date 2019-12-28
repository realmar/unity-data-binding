using Mono.Cecil;
using Realmar.DataBindings.Editor.Cecil;
using Realmar.DataBindings.Editor.Commands;
using Realmar.DataBindings.Editor.Emitting.Command;
using Realmar.DataBindings.Editor.IoC;
using Realmar.DataBindings.Editor.Shared.Extensions;
using Realmar.DataBindings.Editor.Utils;
using static Realmar.DataBindings.Editor.Shared.SharedHelpers;

namespace Realmar.DataBindings.Editor.Weaving.Commands
{
	internal class WeaveSetHelperCommand : BaseCommand
	{
		private WeaveSetHelperCommand()
		{
		}

		internal static ICommand Create(MethodDefinition setMethod)
		{
			var command = new WeaveSetHelperCommand();
			var wovenSetHelpers = ServiceLocator.Current.GetWovenSetHelpers();
			var type = setMethod.DeclaringType;
			var targetSetHelperMethodName = GetTargetSetHelperMethodName(setMethod);
			var targetSetHelperMethod = type.GetMethod(targetSetHelperMethodName);

			if (targetSetHelperMethod == null && wovenSetHelpers.Contains(setMethod) == false)
			{
				command.AddChild(EmitSetHelperCommand.Create(targetSetHelperMethodName, setMethod));
				wovenSetHelpers.Add(setMethod);
			}

			return command;
		}
	}
}
