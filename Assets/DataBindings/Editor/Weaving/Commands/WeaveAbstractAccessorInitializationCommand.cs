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

		private struct WeaveInTypeParameters
		{
			internal TypeDefinition DerivedType;
			internal DataMediator<MethodDefinition> AccessorSymbolMediator;
			internal string BindingInitializerName;
			internal IMemberDefinition BindingTarget;
		}

		internal static ICommand Create(DataMediator<MethodDefinition> accessorSymbolMediator, MethodDefinition bindingInitializer, IMemberDefinition bindingTarget)
		{
			var command = new WeaveAbstractAccessorInitializationCommand();
			var originType = bindingInitializer.DeclaringType;

			var parameters = new WeaveInTypeParameters
			{
				DerivedType = originType,
				AccessorSymbolMediator = accessorSymbolMediator,
				BindingInitializerName = bindingInitializer.Name,
				BindingTarget = bindingTarget
			};
			var found = WeaveInType(command, parameters);

			if (found == false)
			{
				throw new MissingNonAbstractBindingInitializer(bindingInitializer.FullName);
			}

			return command;
		}

		private static bool WeaveInType(WeaveAbstractAccessorInitializationCommand command, WeaveInTypeParameters parameters)
		{
			var initializer = parameters.DerivedType.GetMethod(parameters.BindingInitializerName);
			var found = false;

			if (initializer != null)
			{
				if (initializer.IsAbstract == false)
				{
					command.AddChild(EmitAccessorInitializationCommand.Create(parameters.AccessorSymbolMediator, initializer, parameters.BindingTarget));
					found = true;
				}
				else
				{
					found = WeaveInDerivedTypes(command, parameters);
				}
			}
			else
			{
				found = WeaveInDerivedTypes(command, parameters);
			}

			return found;
		}

		private static bool WeaveInDerivedTypes(WeaveAbstractAccessorInitializationCommand command, WeaveInTypeParameters parameters)
		{
			var derivativeResolver = ServiceLocator.Current.Resolve<DerivativeResolver>();
			var newDerivedTypes = derivativeResolver.GetDirectlyDerivedTypes(parameters.DerivedType);
			var found = false;

			foreach (var newDerivedType in newDerivedTypes)
			{
				var newParameters = parameters;
				newParameters.DerivedType = newDerivedType;

				found |= WeaveInType(command, newParameters);
			}

			return found;
		}
	}
}
