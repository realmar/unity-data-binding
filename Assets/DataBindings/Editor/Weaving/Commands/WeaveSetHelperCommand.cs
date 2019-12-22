using Mono.Cecil;
using Realmar.DataBindings.Editor.Commands;
using Realmar.DataBindings.Editor.Emitting.Command;
using Realmar.DataBindings.Editor.Extensions;
using Realmar.DataBindings.Editor.Utils;
using System.Collections.Generic;
using static Realmar.DataBindings.Editor.Weaving.WeaverHelpers;

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
			var type = setMethod.DeclaringType;
			var targetSetHelperMethodName = GetTargetSetHelperMethodName(setMethod);
			var targetSetHelperMethod = type.GetMethod(targetSetHelperMethodName);

			if (targetSetHelperMethod == null)
			{
				command.AddChild(EmitSetHelperCommand.Create(targetSetHelperMethodName, setMethod));
				ServiceLocator.Current.Resolve<HashSet<MethodDefinition>>("WovenSetHelpers").Add(setMethod);
			}

			return command;
		}
	}
}
