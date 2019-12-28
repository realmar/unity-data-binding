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

		internal static ICommand Create(AccessorSymbolParameters parameters)
		{
			var command = new WeaveTargetToSourceAccessorCommand();
			var derivativeResolver = ServiceLocator.Current.Resolve<DerivativeResolver>();
			var accessorSymbol = GetAccessorProperty(parameters.SourceType, parameters.TargetType);
			var accessorSymbolMediator = new DataMediator<MethodDefinition>();

			if (accessorSymbol == null)
			{
				// WEAVE ACCESSOR METHOD
				command.AddChild(EmitAccessorCommand.Create(accessorSymbolMediator, parameters.TargetType, parameters.SourceType, false));
				if (parameters.TargetType.IsInterface)
				{
					var list = derivativeResolver.GetDirectlyDerivedTypes(parameters.TargetType);
					foreach (var derivedType in list)
					{
						if (GetAccessorProperty(parameters.SourceType, derivedType) == null)
						{
							command.AddChild(EmitAccessorCommand.Create(derivedType, parameters.SourceType, true));
						}
					}
				}
			}
			else
			{
				accessorSymbolMediator.Data = accessorSymbol.SetMethod;
			}

			if (parameters.BindingInitializer.IsAbstract)
			{
				command.AddChild(WeaveAbstractAccessorInitializationCommand.Create(accessorSymbolMediator, parameters));
			}
			else
			{
				command.AddChild(EmitAccessorInitializationCommand.Create(accessorSymbolMediator, parameters.BindingInitializer, parameters.BindingTarget, parameters.Settings.ThrowOnFailure));
			}

			return command;
		}
	}
}
