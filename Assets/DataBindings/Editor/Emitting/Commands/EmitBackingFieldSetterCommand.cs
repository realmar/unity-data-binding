using Mono.Cecil;
using Mono.Cecil.Cil;
using Realmar.DataBindings.Editor.Commands;
using Realmar.DataBindings.Editor.Utils;
using Realmar.DataBindings.Editor.Weaving.Commands;

namespace Realmar.DataBindings.Editor.Emitting.Command
{
	internal class EmitBackingFieldSetterCommand : BaseCommand
	{
		private DataMediator<PropertyDefinition> _property;
		private DataMediator<FieldDefinition> _backingField;

		private EmitBackingFieldSetterCommand(DataMediator<PropertyDefinition> property, DataMediator<FieldDefinition> backingField)
		{
			_property = property;
			_backingField = backingField;
		}

		public override void Execute()
		{
			var methodBody = _property.Data.SetMethod.Body;
			var ilProcessor = methodBody.GetILProcessor();

			// IL_0000: ldarg.0      // this
			// IL_0001: ldarg.1      // 'value'
			// IL_0002: stfld        string Realmar.DataBindings.Example.Abstracts.Abstract2_2::'<Text4>k__BackingField'
			// IL_0007: ret

			ilProcessor.Emit(OpCodes.Ldarg_0);
			ilProcessor.Emit(OpCodes.Ldarg_1);
			ilProcessor.Emit(OpCodes.Stfld, _backingField.Data);
			ilProcessor.Emit(OpCodes.Ret);

			ExecuteNext();
		}

		internal static ICommand Create(DataMediator<PropertyDefinition> property, DataMediator<FieldDefinition> backingField)
		{
			return new EmitBackingFieldSetterCommand(property, backingField);
		}
	}
}