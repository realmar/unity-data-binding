using Mono.Cecil;
using Mono.Cecil.Cil;
using Realmar.DataBindings.Editor.Commands;
using Realmar.DataBindings.Editor.Utils;
using Realmar.DataBindings.Editor.Weaving.Commands;

namespace Realmar.DataBindings.Editor.Emitting.Command
{
	internal class EmitBackingFieldGetterCommand : BaseCommand
	{
		private DataMediator<PropertyDefinition> _property;
		private DataMediator<FieldDefinition> _backingField;

		private EmitBackingFieldGetterCommand(DataMediator<PropertyDefinition> property, DataMediator<FieldDefinition> backingField)
		{
			_property = property;
			_backingField = backingField;
		}

		public override void Execute()
		{
			var methodBody = _property.Data.GetMethod.Body;
			var ilProcessor = methodBody.GetILProcessor();

			// IL_0000: ldarg.0      // this
			// IL_0001: ldfld        string Realmar.DataBindings.Example.View2::_text
			// IL_0006: ret

			ilProcessor.Emit(OpCodes.Ldarg_0);
			ilProcessor.Emit(OpCodes.Ldfld, _backingField.Data);
			ilProcessor.Emit(OpCodes.Ret);

			ExecuteNext();
		}

		internal static ICommand Create(DataMediator<PropertyDefinition> property, DataMediator<FieldDefinition> backingField)
		{
			return new EmitBackingFieldGetterCommand(property, backingField);
		}
	}
}
