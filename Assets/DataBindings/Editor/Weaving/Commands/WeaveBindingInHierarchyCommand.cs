using Realmar.DataBindings.Editor.Cecil;
using Realmar.DataBindings.Editor.Commands;
using Realmar.DataBindings.Editor.Exceptions;
using Realmar.DataBindings.Editor.Extensions;
using Realmar.DataBindings.Editor.Utils;

namespace Realmar.DataBindings.Editor.Weaving.Commands
{
	internal class WeaveBindingInHierarchyCommand : BaseCommand
	{
		private WeaveBindingInHierarchyCommand()
		{
		}

		internal static ICommand Create(WeaveParameters parameters)
		{
			var derivativeResolver = ServiceLocator.Current.Resolve<DerivativeResolver>();
			var command = new WeaveBindingInHierarchyCommand();

			var foundNonAbstract = false;
			var fromProperty = parameters.FromProperty;
			var fromPropertyName = fromProperty.Name;
			var derivedTypes = derivativeResolver.GetDerivedTypes(fromProperty.DeclaringType);

			foreach (var typeDefinition in derivedTypes)
			{
				var property = typeDefinition.GetProperty(fromPropertyName);
				if (property != null && property.GetSetMethodOrYeet().IsAbstract == false)
				{
					command.AddChild(WeaveNonAbstractBindingCommand.Create(new WeaveParameters(parameters)
					{
						FromProperty = property
					}));

					foundNonAbstract = true;
				}
			}

			if (foundNonAbstract == false)
			{
				throw new MissingNonAbstractSymbolException(fromProperty.FullName);
			}

			return command;
		}
	}
}
