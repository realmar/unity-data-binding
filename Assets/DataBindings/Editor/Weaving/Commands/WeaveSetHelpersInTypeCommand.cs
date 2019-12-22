using Mono.Cecil;
using Realmar.DataBindings.Editor.Cecil;
using Realmar.DataBindings.Editor.Commands;
using Realmar.DataBindings.Editor.Extensions;
using Realmar.DataBindings.Editor.Utils;
using System.Collections.Generic;
using System.Linq;

namespace Realmar.DataBindings.Editor.Weaving.Commands
{
	internal class WeaveSetHelpersInTypeCommand : BaseCommand
	{
		private WeaveSetHelpersInTypeCommand()
		{
		}

		internal static ICommand Create(TypeDefinition type)
		{
			var command = new WeaveSetHelpersInTypeCommand();
			var setMethods = type.Properties.Select(definition => definition.SetMethod).WhereNotNull();
			var wovenSetHelpers = ServiceLocator.Current.Resolve<HashSet<MethodDefinition>>("WovenSetHelpers");

			foreach (var setMethod in setMethods)
			{
				if (wovenSetHelpers.Contains(setMethod) == false)
				{
					if (setMethod.IsVirtual || setMethod.IsAbstract)
					{
						WeaveSetHelperRecursive(command, setMethod);
					}
					else
					{
						command.AddChild(WeaveSetHelperCommand.Create(setMethod));
					}
				}
			}

			return command;
		}

		private static void WeaveSetHelperRecursive(WeaveSetHelpersInTypeCommand command, MethodDefinition setMethod)
		{
			var derivativeResolver = ServiceLocator.Current.Resolve<DerivativeResolver>();
			var originType = setMethod.DeclaringType;
			var baseMethods = setMethod.GetBaseMethods();
			var derivedTypes = derivativeResolver.GetDerivedTypes(originType);

			foreach (var method in baseMethods)
			{
				command.AddChild(WeaveSetHelperCommand.Create(method));
			}

			foreach (var typeDefinition in derivedTypes)
			{
				var properties = typeDefinition.Properties;
				foreach (var property in properties)
				{
					var method = property.SetMethod;

					if (method != null)
					{
						command.AddChild(WeaveSetHelperCommand.Create(method));
					}
				}
			}
		}
	}
}
