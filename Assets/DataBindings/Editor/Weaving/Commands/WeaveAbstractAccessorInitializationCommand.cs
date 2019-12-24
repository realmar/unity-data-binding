using Mono.Cecil;
using Realmar.DataBindings.Editor.Cecil;
using Realmar.DataBindings.Editor.Commands;
using Realmar.DataBindings.Editor.Emitting.Command;
using Realmar.DataBindings.Editor.Extensions;
using Realmar.DataBindings.Editor.Utils;

namespace Realmar.DataBindings.Editor.Weaving.Commands
{
	internal class WeaveAbstractAccessorInitializationCommand : BaseCommand
	{
		private WeaveAbstractAccessorInitializationCommand()
		{
		}

		internal static ICommand Create(DataMediator<MethodDefinition> accessorSymbolMediator, MethodDefinition bindingInitializer, IMemberDefinition bindingTarget)
		{
			var command = new WeaveAbstractAccessorInitializationCommand();
			var originType = bindingInitializer.DeclaringType;

			WeaveInType(command, originType, accessorSymbolMediator, bindingInitializer.Name, bindingTarget);

			return command;
		}

		private static void WeaveInType(WeaveAbstractAccessorInitializationCommand command, TypeDefinition derivedType, DataMediator<MethodDefinition> accessorSymbolMediator, string bindingInitializerName, IMemberDefinition bindingTarget)
		{
			var derivativeResolver = ServiceLocator.Current.Resolve<DerivativeResolver>();
			var initializer = derivedType.GetMethod(bindingInitializerName);
			var found = false;

			if (initializer != null)
			{
				if (initializer.IsAbstract == false)
				{
					command.AddChild(EmitAccessorInitializationCommand.Create(accessorSymbolMediator, initializer, bindingTarget));
					found = true;
				}
				else
				{
					var newDerivedTypes = derivativeResolver.GetDirectlyDerivedTypes(derivedType);
					foreach (var newDerivedType in newDerivedTypes)
					{
						WeaveInType(command, newDerivedType, accessorSymbolMediator, bindingInitializerName, bindingTarget);
					}
				}
			}
			else
			{
				var newDerivedTypes = derivativeResolver.GetDirectlyDerivedTypes(derivedType);
				foreach (var newDerivedType in newDerivedTypes)
				{
					WeaveInType(command, newDerivedType, accessorSymbolMediator, bindingInitializerName, bindingTarget);
				}
			}

			// if (found == false)
			// {
			// 	// TODO - ABSTRACT - Exception
			// 	throw new MissingSymbolException(
			// 		$"Could not find overriding non-abstract binding target for {bindingTarget.FullName}");
			// }
		}
	}
}
