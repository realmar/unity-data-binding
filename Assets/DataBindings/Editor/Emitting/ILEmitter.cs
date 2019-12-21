using System;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using Boo.Lang;
using Mono.Cecil;
using Mono.Cecil.Cil;
using Mono.Collections.Generic;
using Realmar.DataBindings.Editor.Extensions;
using static Realmar.DataBindings.Editor.Weaving.WeaverHelpers;
using static Realmar.DataBindings.Editor.Exceptions.YeetHelpers;

namespace Realmar.DataBindings.Editor.Emitting
{
	internal class ILEmitter
	{
		internal void EmitBinding(
			IMemberDefinition bindingTarget,
			MethodDefinition fromGetMethod,
			MethodDefinition fromSetMethod,
			MethodDefinition toSetMethod,
			bool emitNullCheck)
		{
			var methodBody = fromSetMethod.Body;
			var ilProcessor = methodBody.GetILProcessor();
			var instructions = methodBody.Instructions;
			var lastInstruction = instructions.Last();

			var referencingLast = GetInstructionsReferencing(lastInstruction, instructions);

			// IL_0007: ldarg.0      // this
			// IL_0008: call instance class Realmar.UnityMVVM.Example.ExampleView Realmar.UnityMVVM.Example.ExampleViewModel::get_View()
			// IL_000d: ldarg.0      // this
			// IL_000e: call instance int32 Realmar.UnityMVVM.Example.ExampleViewModel::get_Value3()
			// IL_0013: call instance void Realmar.UnityMVVM.Example.ExampleView::set_Value3(int32)

			var il1 = ilProcessor.Create(OpCodes.Ldarg_0);
			var il2 = GetLoadFromFieldOrCallableInstruction(bindingTarget, ilProcessor);
			var il3 = ilProcessor.Create(OpCodes.Ldarg_0);
			var il4 = ilProcessor.Create(GetCallInstruction(fromGetMethod), fromGetMethod);
			var il5 = ilProcessor.Create(GetCallInstruction(toSetMethod), toSetMethod);

			Instruction firstInjected;

			if (emitNullCheck)
			{
				var nullCheckInstructions = EmitNullCheckInstructions(bindingTarget, ilProcessor, lastInstruction);
				var last = nullCheckInstructions[0];
				firstInjected = last;
				ilProcessor.InsertBefore(lastInstruction, last);
				for (var i = 1; i < nullCheckInstructions.Count; i++)
				{
					var inst = nullCheckInstructions[i];
					ilProcessor.InsertAfter(last, inst);
					last = inst;
				}

				ilProcessor.InsertAfter(last, il1);
			}
			else
			{
				firstInjected = il1;
				ilProcessor.InsertBefore(lastInstruction, il1);
			}

			ilProcessor.InsertAfter(il1, il2);
			ilProcessor.InsertAfter(il2, il3);
			ilProcessor.InsertAfter(il3, il4);
			ilProcessor.InsertAfter(il4, il5);

			foreach (var instruction in referencingLast)
			{
				instruction.Operand = firstInjected;
			}
		}

		internal void EmitSetHelperMethod(
			string targetSetHelperMethodName,
			MethodDefinition setMethod)
		{
			var attributes = setMethod.Attributes & ~MethodAttributes.SpecialName;
			var targetSetHelperMethod = new MethodDefinition(
				targetSetHelperMethodName,
				attributes,
				setMethod.Module.ImportReference(typeof(void)));
			targetSetHelperMethod.Parameters.Add(
				new ParameterDefinition(
					"value",
					ParameterAttributes.None,
					setMethod.Parameters[0].ParameterType));

			setMethod.DeclaringType.Methods.Add(targetSetHelperMethod);

			if (setMethod.IsAbstract == false)
			{
				EmitTargetSetHelper(targetSetHelperMethod, setMethod);
			}
		}

		internal void EmitAccessorInitialization(
			MethodDefinition bindingInitializer,
			IMemberDefinition bindingTarget,
			IMemberDefinition accessorSymbol)
		{
			var methodBody = bindingInitializer.Body;
			var ilProcessor = methodBody.GetILProcessor();
			var instructions = methodBody.Instructions;
			var lastInstruction = instructions.Last();

			var referencingLast = GetInstructionsReferencing(lastInstruction, instructions);

			// IL_0008: ldarg.0      // this
			// IL_0009: ldfld        class Realmar.DataBindings.Example.ExampleView Realmar.DataBindings.Example.ExampleViewModel::_view
			// IL_000e: ldarg.0      // this
			// IL_000f: stfld        class Realmar.DataBindings.Example.ExampleViewModel Realmar.DataBindings.Example.ExampleView::ViewModel

			var il1 = ilProcessor.Create(OpCodes.Ldarg_0);
			var il2 = GetLoadFromFieldOrCallableInstruction(bindingTarget, ilProcessor);
			var il3 = ilProcessor.Create(OpCodes.Ldarg_0);
			var il4 = GetLoadFromFieldOrCallableInstruction(accessorSymbol, ilProcessor);

			ilProcessor.InsertBefore(lastInstruction, il1);
			ilProcessor.InsertAfter(il1, il2);
			ilProcessor.InsertAfter(il2, il3);
			ilProcessor.InsertAfter(il3, il4);

			foreach (var instruction in referencingLast)
			{
				instruction.Operand = il1;
			}
		}

		internal PropertyDefinition EmitAccessor(
			TypeDefinition type,
			string name,
			TypeDefinition sourceType,
			bool isInterfaceImpl)
		{
			var module = type.Module;
			var attributes = MethodAttributes.Public | MethodAttributes.SpecialName;
			var isInterface = type.IsInterface;

			if (isInterface)
			{
				attributes |= MethodAttributes.Abstract;
			}
			else if (isInterfaceImpl)
			{
				attributes |= MethodAttributes.Final;
			}

			if (isInterface || isInterfaceImpl)
			{
				attributes |= MethodAttributes.Virtual;
				attributes |= MethodAttributes.NewSlot;
				attributes |= MethodAttributes.HideBySig;
			}

			var accessor = new PropertyDefinition(name, PropertyAttributes.None, sourceType);
			accessor.GetMethod = new MethodDefinition(GetGetterName(name), attributes, sourceType);
			accessor.SetMethod =
				new MethodDefinition(GetSetterName(name), attributes, module.ImportReference(typeof(void)));
			accessor.SetMethod.Parameters.Add(new ParameterDefinition("value", ParameterAttributes.None, sourceType));

			EmitCustomAttribute<CompilerGeneratedAttribute>(accessor.GetMethod, module);
			EmitCustomAttribute<CompilerGeneratedAttribute>(accessor.SetMethod, module);

			type.Properties.Add(accessor);
			type.Methods.Add(accessor.GetMethod);
			type.Methods.Add(accessor.SetMethod);

			if (isInterface == false)
			{
				EmitAutoPropertyGetSet(accessor);
			}

			return accessor;
		}

		private void EmitAutoPropertyGetSet(PropertyDefinition property)
		{
			var type = property.DeclaringType;
			var module = type.Module;
			var backingFieldName = GetBackingFieldName(property.Name);

			YeetIfFieldExists(type, backingFieldName);

			// .field private string '<Text6>k__BackingField'
			// .custom instance void[mscorlib] System.Runtime.CompilerServices.CompilerGeneratedAttribute::.ctor()
			//   = (01 00 00 00 )
			// .custom instance void[mscorlib] System.Diagnostics.DebuggerBrowsableAttribute::.ctor(valuetype[mscorlib] System.Diagnostics.DebuggerBrowsableState)
			//   = (01 00 00 00 00 00 00 00 ) // ........
			//   // int32(0) // 0x00000000

			var backingField = new FieldDefinition(backingFieldName, FieldAttributes.Private, property.PropertyType);
			type.Fields.Add(backingField);

			EmitCustomAttribute<CompilerGeneratedAttribute>(backingField, module);
			EmitCustomAttribute<DebuggerBrowsableAttribute>(backingField, module, DebuggerBrowsableState.Never);

			EmitBackingFieldGetter(property.GetMethod, backingField);
			EmitBackingFieldSetter(property.SetMethod, backingField);
		}

		private void EmitBackingFieldSetter(MethodDefinition getter, FieldDefinition backingField)
		{
			var methodBody = getter.Body;
			var ilProcessor = methodBody.GetILProcessor();

			// IL_0000: ldarg.0      // this
			// IL_0001: ldarg.1      // 'value'
			// IL_0002: stfld        string Realmar.DataBindings.Example.Abstracts.Abstract2_2::'<Text4>k__BackingField'
			// IL_0007: ret

			ilProcessor.Emit(OpCodes.Ldarg_0);
			ilProcessor.Emit(OpCodes.Ldarg_1);
			ilProcessor.Emit(OpCodes.Stfld, backingField);
			ilProcessor.Emit(OpCodes.Ret);
		}

		private void EmitBackingFieldGetter(MethodDefinition getter, FieldDefinition backingField)
		{
			var methodBody = getter.Body;
			var ilProcessor = methodBody.GetILProcessor();

			// IL_0000: ldarg.0      // this
			// IL_0001: ldfld        string Realmar.DataBindings.Example.View2::_text
			// IL_0006: ret

			ilProcessor.Emit(OpCodes.Ldarg_0);
			ilProcessor.Emit(OpCodes.Ldfld, backingField);
			ilProcessor.Emit(OpCodes.Ret);
		}

		private void EmitTargetSetHelper(MethodDefinition method, MethodDefinition setMethod)
		{
			var setMethodBody = setMethod.Body;
			var targetMethodBody = method.Body;
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
		}

		private List<Instruction> EmitNullCheckInstructions(
			IMemberDefinition toBeChecked,
			ILProcessor ilProcessor,
			Instruction isNullBranch)
		{
			var instructions = new List<Instruction>();

			instructions.Add(ilProcessor.Create(OpCodes.Ldarg_0));
			instructions.Add(GetLoadFromFieldOrCallableInstruction(toBeChecked, ilProcessor));
			instructions.Add(ilProcessor.Create(OpCodes.Ldnull));

			var returnType = GetReturnType(toBeChecked);
			var op = returnType.GetInEqualityOperator();
			var opReference = toBeChecked.DeclaringType.Module.ImportReference(op);

			if (op != null)
			{
				instructions.Add(ilProcessor.Create(OpCodes.Call, opReference));
			}
			else
			{
				instructions.Add(ilProcessor.Create(OpCodes.Cgt_Un));
			}

			instructions.Add(ilProcessor.Create(OpCodes.Brfalse_S, isNullBranch));

			return instructions;
		}

		private void EmitCustomAttribute<TAttribute>(
			ICustomAttributeProvider target,
			ModuleDefinition module,
			params object[] ctorArgs)
			where TAttribute : Attribute

		{
			var attributeType = typeof(TAttribute);
			var args = ctorArgs.Select(arg => (Type: arg.GetType(), Value: arg)).ToArray();
			var argTypes = args.Select(tuple => tuple.Type).ToArray();

			var attributeConstructor =
				module.ImportReference(attributeType.GetConstructor(argTypes));
			var attribute = new CustomAttribute(attributeConstructor);
			target.CustomAttributes.Add(attribute);

			foreach (var (type, value) in args)
			{
				attribute.ConstructorArguments.Add(
					new CustomAttributeArgument(module.ImportReference(type).Resolve(), value));
			}
		}

		private Instruction GetLoadFromFieldOrCallableInstruction(
			IMemberDefinition bindingTarget,
			ILProcessor ilProcessor)
		{
			switch (bindingTarget)
			{
				case FieldDefinition field:
					return ilProcessor.Create(OpCodes.Ldfld, field);
				case MethodDefinition method:
					return ilProcessor.Create(GetCallInstruction(method), method);
				default:
					throw new NotSupportedException("BindingTarget can only be field, property or method");
			}
		}

		private OpCode GetCallInstruction(MethodDefinition method)
		{
			var isVirtual = method.IsVirtual || method.IsAbstract;
			return isVirtual ? OpCodes.Callvirt : OpCodes.Call;
		}

		private List<Instruction> GetInstructionsReferencing(
			Instruction searchInstruction,
			Collection<Instruction> instructions)
		{
			var found = new List<Instruction>();

			foreach (var instruction in instructions)
			{
				if (ReferenceEquals(instruction.Operand, searchInstruction))
				{
					found.Add(instruction);
				}
			}

			return found;
		}

		private string GetBackingFieldName(string normalName) => $"\u003C{normalName}\u003Ek__BackingField";
		private string GetGetterName(string normalName) => $"get_{normalName}";
		private string GetSetterName(string normalName) => $"set_{normalName}";
	}
}
