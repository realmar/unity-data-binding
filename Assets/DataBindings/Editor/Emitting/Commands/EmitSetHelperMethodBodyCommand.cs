using Mono.Cecil;
using Realmar.DataBindings.Editor.Commands;
using Realmar.DataBindings.Editor.Utils;
using Realmar.DataBindings.Editor.Weaving.Commands;

namespace Realmar.DataBindings.Editor.Emitting.Command
{
	internal class EmitSetHelperMethodBodyCommand : BaseCommand
	{
		private DataMediator<MethodDefinition> _method;
		private MethodDefinition _setMethod;

		private EmitSetHelperMethodBodyCommand(DataMediator<MethodDefinition> method, MethodDefinition setMethod)
		{
			_method = method;
			_setMethod = setMethod;
		}

		public override void Execute()
		{
			var setMethodBody = _setMethod.Body;
			var targetMethodBody = _method.Data.Body;
			var ilProcessor = targetMethodBody.GetILProcessor();

			foreach (var variable in setMethodBody.Variables)
			{
				targetMethodBody.Variables.Add(variable);
			}

			foreach (var handler in setMethodBody.ExceptionHandlers)
			{
				targetMethodBody.ExceptionHandlers.Add(handler);
			}

			targetMethodBody.InitLocals = setMethodBody.InitLocals;

			foreach (var instruction in setMethodBody.Instructions)
			{
				ilProcessor.Append(instruction);
			}

			ExecuteNext();
		}

		internal static ICommand Create(DataMediator<MethodDefinition> data, MethodDefinition setMethod)
		{
			return new EmitSetHelperMethodBodyCommand(data, setMethod);
		}
	}
}
