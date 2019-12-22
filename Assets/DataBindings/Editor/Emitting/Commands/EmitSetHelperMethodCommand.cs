using Mono.Cecil;
using Realmar.DataBindings.Editor.Commands;
using Realmar.DataBindings.Editor.Utils;
using Realmar.DataBindings.Editor.Weaving.Commands;

namespace Realmar.DataBindings.Editor.Emitting.Command
{
	internal class EmitSetHelperMethodCommand : BaseCommand
	{
		private DataMediator<MethodDefinition> _setHelperMethod;
		private string _targetSetHelperMethodName;
		private MethodDefinition _setMethod;

		private EmitSetHelperMethodCommand(DataMediator<MethodDefinition> setHelperMethod, string targetSetHelperMethodName, MethodDefinition method)
		{
			_setHelperMethod = setHelperMethod;
			_targetSetHelperMethodName = targetSetHelperMethodName;
			_setMethod = method;
		}

		public override void Execute()
		{
			var attributes = _setMethod.Attributes & ~MethodAttributes.SpecialName;
			var targetSetHelperMethod = new MethodDefinition(
				_targetSetHelperMethodName,
				attributes,
				_setMethod.Module.ImportReference(typeof(void)));
			targetSetHelperMethod.Parameters.Add(
				new ParameterDefinition(
					"value",
					ParameterAttributes.None,
					_setMethod.Parameters[0].ParameterType));

			_setMethod.DeclaringType.Methods.Add(targetSetHelperMethod);
			_setHelperMethod.Data = targetSetHelperMethod;

			ExecuteNext();
		}

		internal static ICommand Create(DataMediator<MethodDefinition> setHelperMethod, string targetSetHelperMethodName, MethodDefinition setMethod)
		{
			return new EmitSetHelperMethodCommand(setHelperMethod, targetSetHelperMethodName, setMethod);
		}
	}
}
