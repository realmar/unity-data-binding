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

			var allSetters = baseMethods
				.Concat(
					derivedTypes
						.Where(definition => definition != originType)
						.Where(definition => definition.Module.Assembly.IsSame(originType.Module.Assembly))
						.SelectMany(definition => definition.Properties
							.Select(propertyDefinition => propertyDefinition.SetMethod)
							.WhereNotNull()))
				.ToArray();

			foreach (var methodDefinition in allSetters)
			{
				command.AddChild(WeaveSetHelperCommand.Create(methodDefinition));
			}

			return command;
		}
	}
}
