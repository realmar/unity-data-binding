using Mono.Cecil;
using Realmar.DataBindings.Editor.Cecil;
using Realmar.DataBindings.Editor.Commands;
using Realmar.DataBindings.Editor.Emitting.Command;
using Realmar.DataBindings.Editor.Exceptions;
using Realmar.DataBindings.Editor.Extensions;
using Realmar.DataBindings.Editor.Utils;

namespace Realmar.DataBindings.Editor.Weaving.Commands
{
	internal class WeaveAbstractAccessorInitializationCommand : BaseCommand
	{
		private WeaveAbstractAccessorInitializationCommand()
		{
		}

		internal static ICommand Create(DataMediator<IMemberDefinition> accessorSymbolMediator, MethodDefinition bindingInitializer, IMemberDefinition bindingTarget)
		{
			var command = new WeaveAbstractAccessorInitializationCommand();
			var derivativeResolver = ServiceLocator.Current.Resolve<DerivativeResolver>();
			var originType = bindingInitializer.DeclaringType;
			var derivedTypes = derivativeResolver.GetDirectlyDerivedTypes(originType);
			var found = false;

			for (var i = derivedTypes.Count - 1; i >= 0; i--)
			{
				var derivedType = derivedTypes[i];
				var initializer = derivedType.GetMethod(bindingInitializer.Name);
				if (initializer != null)
				{
					command.AddChild(EmitAccessorInitializationCommand.Create(initializer, bindingTarget, accessorSymbolMediator));
					found = true;
				}
			}

			if (found == false)
			{
				// TODO - ABSTRACT - Exception
				throw new MissingSymbolException(
					$"Could not find overriding non-abstract binding target for {bindingTarget.FullName}");
			}

			return command;
		}
	}
}
