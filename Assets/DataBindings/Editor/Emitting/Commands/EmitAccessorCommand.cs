using Mono.Cecil;
using Realmar.DataBindings.Editor.Commands;
using Realmar.DataBindings.Editor.Utils;
using Realmar.DataBindings.Editor.Weaving.Commands;
using System.Runtime.CompilerServices;
using static Realmar.DataBindings.Editor.Emitting.EmitHelpers;
using static Realmar.DataBindings.Editor.Weaving.WeaverHelpers;

namespace Realmar.DataBindings.Editor.Emitting.Command
{
	internal class EmitAccessorCommand : BaseCommand
	{
		private DataMediator<PropertyDefinition> _accessor;
		private DataMediator<MethodDefinition> _accessorMethod;
		private TypeDefinition _targetType;
		private TypeDefinition _sourceType;
		private string _injectedSourceName;
		private bool _isInterfaceImpl;

		private EmitAccessorCommand(DataMediator<PropertyDefinition> accessor, DataMediator<MethodDefinition> accessorMethod, TypeDefinition targetType, TypeDefinition sourceType, bool isInterfaceImpl)
		{
			_accessor = accessor;
			_accessorMethod = accessorMethod;
			_targetType = targetType;
			_sourceType = sourceType;
			_injectedSourceName = GetAccessorPropertyName(sourceType);
			_isInterfaceImpl = isInterfaceImpl;
		}

		public override void Execute()
		{
			var module = _targetType.Module;
			var attributes = MethodAttributes.Public | MethodAttributes.SpecialName;
			var isInterface = _targetType.IsInterface;

			if (isInterface)
			{
				attributes |= MethodAttributes.Abstract;
			}
			else if (_isInterfaceImpl)
			{
				attributes |= MethodAttributes.Final;
			}

			if (isInterface || _isInterfaceImpl)
			{
				attributes |= MethodAttributes.Virtual;
				attributes |= MethodAttributes.NewSlot;
				attributes |= MethodAttributes.HideBySig;
			}

			var accessor = new PropertyDefinition(_injectedSourceName, PropertyAttributes.None, _sourceType);
			accessor.GetMethod = new MethodDefinition(GetGetterName(_injectedSourceName), attributes, _sourceType);
			accessor.SetMethod =
				new MethodDefinition(GetSetterName(_injectedSourceName), attributes, module.ImportReference(typeof(void)));

			var accessorSetMethod = accessor.SetMethod;
			var accessorGetMethod = accessor.GetMethod;

			accessorSetMethod.Parameters.Add(new ParameterDefinition("value", ParameterAttributes.None, _sourceType));

			_targetType.Properties.Add(accessor);
			_targetType.Methods.Add(accessorGetMethod);
			_targetType.Methods.Add(accessorSetMethod);

			EmitCustomAttribute<CompilerGeneratedAttribute>(accessorGetMethod, module);
			EmitCustomAttribute<CompilerGeneratedAttribute>(accessorSetMethod, module);

			_accessor.Data = accessor;
			if (_accessorMethod != null)
			{
				_accessorMethod.Data = accessorSetMethod;
			}

			ExecuteNext();
		}

		internal static ICommand Create(TypeDefinition targetType, TypeDefinition sourceType, bool isInterfaceImpl)
		{
			return Create(null, targetType, sourceType, isInterfaceImpl);
		}

		internal static ICommand Create(DataMediator<MethodDefinition> accessorMethod, TypeDefinition targetType, TypeDefinition sourceType, bool isInterfaceImpl)
		{
			var accessorMediator = new DataMediator<PropertyDefinition>();
			var command = new EmitAccessorCommand(accessorMediator, accessorMethod, targetType, sourceType, isInterfaceImpl);

			if (targetType.IsInterface == false)
			{
				command.AddChild(EmitAutoPropertyGetSetCommand.Create(accessorMediator));
			}

			return command;
		}
	}
}
