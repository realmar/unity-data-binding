using Mono.Cecil;
using Realmar.DataBindings.Editor.Commands;
using Realmar.DataBindings.Editor.Utils;
using Realmar.DataBindings.Editor.Weaving.Commands;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using static Realmar.DataBindings.Editor.Emitting.EmitHelpers;
using static Realmar.DataBindings.Editor.Exceptions.YeetHelpers;

namespace Realmar.DataBindings.Editor.Emitting.Command
{
	internal class EmitAutoPropertyGetSetCommand : BaseCommand
	{
		private DataMediator<PropertyDefinition> _property;
		private DataMediator<FieldDefinition> _backingField;

		private EmitAutoPropertyGetSetCommand(DataMediator<PropertyDefinition> property, DataMediator<FieldDefinition> backingField)
		{
			_property = property;
			_backingField = backingField;
		}

		public override void Execute()
		{
			var type = _property.Data.DeclaringType;
			var module = type.Module;
			var backingFieldName = GetBackingFieldName(_property.Data.Name);

			YeetIfFieldExists(type, backingFieldName);

			// .field private string '<Text6>k__BackingField'
			var backingField = new FieldDefinition(backingFieldName, FieldAttributes.Private, _property.Data.PropertyType);
			type.Fields.Add(backingField);

			EmitCustomAttribute<CompilerGeneratedAttribute>(backingField, module);
			EmitCustomAttribute<DebuggerBrowsableAttribute>(backingField, module, DebuggerBrowsableState.Never);
			_backingField.Data = backingField;

			ExecuteNext();
		}

		internal static ICommand Create(DataMediator<PropertyDefinition> property)
		{
			var backingFieldMediator = new DataMediator<FieldDefinition>();
			var command = new EmitAutoPropertyGetSetCommand(property, backingFieldMediator);
			command.AddChild(EmitBackingFieldGetterCommand.Create(property, backingFieldMediator));
			command.AddChild(EmitBackingFieldSetterCommand.Create(property, backingFieldMediator));

			return command;
		}
	}
}
