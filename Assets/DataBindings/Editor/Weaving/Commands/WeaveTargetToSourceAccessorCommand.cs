using Mono.Cecil;
using Realmar.DataBindings.Editor.Cecil;
using Realmar.DataBindings.Editor.Commands;
using Realmar.DataBindings.Editor.Emitting.Command;
using Realmar.DataBindings.Editor.Utils;
using static Realmar.DataBindings.Editor.Weaving.WeaverHelpers;

namespace Realmar.DataBindings.Editor.Weaving.Commands
{
	internal class WeaveTargetToSourceAccessorCommand : BaseCommand
	{
		private WeaveTargetToSourceAccessorCommand()
		{
		}

		internal static ICommand Create(
			TypeDefinition sourceType,
			TypeDefinition targetType,
			IMemberDefinition bindingTarget,
			MethodDefinition bindingInitializer)
		{
			var command = new WeaveTargetToSourceAccessorCommand();
			var derivativeResolver = ServiceLocator.Current.Resolve<DerivativeResolver>();
			var targetToSourceSymbol = GetAccessorProperty(sourceType, targetType);
			var accessorSymbolMediator = new DataMediator<IMemberDefinition>();

			if (targetToSourceSymbol == null)
			{
				// WEAVE ACCESSOR METHOD
				command.AddChild(EmitAccessorCommand.Create(accessorSymbolMediator, targetType, sourceType, false));
				if (targetType.IsInterface)
				{
					var list = derivativeResolver.GetDirectlyDerivedTypes(targetType);
					foreach (var subject in list)
					{
						command.AddChild(EmitAccessorCommand.Create(subject, sourceType, true));
					}
				}
			}
			else
			{
				accessorSymbolMediator.Data = targetToSourceSymbol.SetMethod;
			}

			if (bindingInitializer.IsAbstract)
			{
				command.AddChild(WeaveAbstractAccessorInitializationCommand.Create(accessorSymbolMediator, bindingInitializer, bindingTarget));
			}
			else
			{
				command.AddChild(EmitAccessorInitializationCommand.Create(bindingInitializer, bindingTarget, accessorSymbolMediator));
			}

			return command;
		}
	}
}
