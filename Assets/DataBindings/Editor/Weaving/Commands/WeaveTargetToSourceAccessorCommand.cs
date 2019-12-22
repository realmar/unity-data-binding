using Mono.Cecil;
using Realmar.DataBindings.Editor.Cecil;
using Realmar.DataBindings.Editor.Commands;
using Realmar.DataBindings.Editor.Emitting.Command;
using Realmar.DataBindings.Editor.Exceptions;
using Realmar.DataBindings.Editor.Extensions;
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
			var derivativeResolver = ServiceLocator.Current.Resolve<DerivativeResolver>();
			var command = new WeaveTargetToSourceAccessorCommand();
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
			}
			else
			{
				command.AddChild(EmitAccessorInitializationCommand.Create(bindingInitializer, bindingTarget, accessorSymbolMediator));
			}

			return command;
		}
	}
}
