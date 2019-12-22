using Mono.Cecil;
using Realmar.DataBindings.Editor.Cecil;
using Realmar.DataBindings.Editor.Commands;
using Realmar.DataBindings.Editor.Extensions;
using Realmar.DataBindings.Editor.Utils;
using System;
using System.Collections.Generic;

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
			var derivedTypes = derivativeResolver.GetDirectlyDerivedTypes(fromProperty.DeclaringType);
			var allTypes = new Stack<TypeDefinition>();

			allTypes.Push(fromProperty.DeclaringType);
			allTypes.AddRange(derivedTypes);

			while (allTypes.Count > 0)
			{
				command.AddChild(WeaveBindingDirectCommand.Create(new WeaveParameters(parameters)
				{
					FromProperty = fromProperty
				}));

				if (fromProperty.GetSetMethodOrYeet().IsAbstract == false)
				{
					foundNonAbstract = true;
				}

				var nextType = allTypes.Pop();
				var propertyName = fromProperty.Name;
				fromProperty = nextType.GetProperty(propertyName);

				allTypes.AddRange(derivativeResolver.GetDirectlyDerivedTypes(fromProperty.DeclaringType));
			}

			if (foundNonAbstract == false)
			{
				throw new Exception("TODO Did not found any non abstract symbols in derivatives.");
			}

			return command;
		}
	}
}
