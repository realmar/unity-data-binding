using System.Linq;
using Mono.Cecil;
using Realmar.DataBindings.Editor.Cecil;
using Realmar.DataBindings.Editor.Commands;
using Realmar.DataBindings.Editor.Extensions;
using Realmar.DataBindings.Editor.Utils;

namespace Realmar.DataBindings.Editor.Weaving.Commands
{
	internal class WeaveSetHelperRecursiveCommand : BaseCommand
	{
		private WeaveSetHelperRecursiveCommand()
		{
		}

		internal static ICommand Create(MethodDefinition setMethod)
		{
			var command = new WeaveSetHelperRecursiveCommand();
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
				if (typeDefinition == originType)
				{
					continue;
				}

				var setMethods = typeDefinition.Properties.Select(definition => definition.SetMethod).WhereNotNull();
				foreach (var setter in setMethods)
				{
					command.AddChild(WeaveSetHelperCommand.Create(setter));
				}
			}

			return command;
		}
	}
}
