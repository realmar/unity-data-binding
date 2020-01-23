using Mono.Cecil;
using Mono.Cecil.Cil;
using Mono.Cecil.Rocks;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using static Realmar.DataBindings.Editor.Emitting.EmitHelpers;
using static Realmar.DataBindings.Editor.Exceptions.YeetHelpers;
using static Realmar.DataBindings.Editor.Shared.SharedHelpers;

namespace Realmar.DataBindings.Editor.Emitting
{
	internal class Emitter
	{
		#region Accessor Definition

		internal PropertyDefinition EmitAccessor(TypeDefinition targetType, TypeDefinition sourceType, bool isInterfaceImpl)
		{
			YeetIfNull(targetType, nameof(targetType));
			YeetIfNull(sourceType, nameof(sourceType));

			var accessor = EmitAccessorPropertyDefinition(targetType, sourceType, isInterfaceImpl);

			if (targetType.IsInterface == false)
			{
				EmitAccessorPropertyBody(accessor);
			}

			return accessor;
		}

		private PropertyDefinition EmitAccessorPropertyDefinition(TypeDefinition targetType, TypeDefinition sourceType, bool isInterfaceImpl)
		{
			var injectedSourceName = GetAccessorPropertyName(sourceType);
			var module = targetType.Module;
			var attributes = MethodAttributes.Public | MethodAttributes.SpecialName;
			var isInterface = targetType.IsInterface;

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

			var accessor = new PropertyDefinition(injectedSourceName, PropertyAttributes.None, sourceType);
			accessor.GetMethod = new MethodDefinition(GetGetterName(injectedSourceName), attributes, sourceType);
			accessor.SetMethod =
				new MethodDefinition(GetSetterName(injectedSourceName), attributes, module.ImportReference(typeof(void)));

			var accessorSetMethod = accessor.SetMethod;
			var accessorGetMethod = accessor.GetMethod;

			accessorSetMethod.Parameters.Add(new ParameterDefinition("value", ParameterAttributes.None, sourceType));

			targetType.Properties.Add(accessor);
			targetType.Methods.Add(accessorGetMethod);
			targetType.Methods.Add(accessorSetMethod);

			EmitCustomAttribute<CompilerGeneratedAttribute>(accessorGetMethod, module);
			EmitCustomAttribute<CompilerGeneratedAttribute>(accessorSetMethod, module);

			return accessor;
		}

		private void EmitAccessorPropertyBody(PropertyDefinition property)
		{
			var type = property.DeclaringType;
			var module = type.Module;
			var backingFieldName = GetBackingFieldName(property.Name);

			YeetIfFieldExists(type, backingFieldName);

			// .field private string '<Text6>k__BackingField'
			var backingField = new FieldDefinition(backingFieldName, FieldAttributes.Private, property.PropertyType);
			type.Fields.Add(backingField);

			EmitCustomAttribute<CompilerGeneratedAttribute>(backingField, module);
			EmitCustomAttribute<DebuggerBrowsableAttribute>(backingField, module, DebuggerBrowsableState.Never);

			EmitBackingFieldGetter(property, backingField);
			EmitBackingFieldSetter(property, backingField);
		}

		private void EmitBackingFieldGetter(PropertyDefinition property, FieldDefinition backingField)
		{
			var methodBody = property.GetMethod.Body;
			var ilProcessor = methodBody.GetILProcessor();

			// IL_0000: ldarg.0      // this
			// IL_0001: ldfld        string Realmar.DataBindings.Example.View2::_text
			// IL_0006: ret

			ilProcessor.Emit(OpCodes.Ldarg_0);
			ilProcessor.Emit(OpCodes.Ldfld, backingField);
			ilProcessor.Emit(OpCodes.Ret);
		}

		private void EmitBackingFieldSetter(PropertyDefinition property, FieldDefinition backingField)
		{
			var methodBody = property.SetMethod.Body;
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

		#endregion

		#region Accessor Initialization

		internal void EmitAccessorInitialization(MethodDefinition accessorSymbol, MethodDefinition bindingInitializer, IMemberDefinition bindingTarget, bool throwOnFailure)
		{
			YeetIfNull(accessorSymbol, nameof(accessorSymbol));
			YeetIfNull(bindingInitializer, nameof(bindingInitializer));
			YeetIfNull(bindingTarget, nameof(bindingTarget));

			// IL_0008: ldarg.0      // this
			// IL_0009: ldfld        class Realmar.DataBindings.Example.ExampleView Realmar.DataBindings.Example.ExampleViewModel::_view
			// IL_000e: ldarg.0      // this
			// IL_000f: stfld        class Realmar.DataBindings.Example.ExampleViewModel Realmar.DataBindings.Example.ExampleView::ViewModel

			var appender = new MethodAppender(bindingInitializer);
			var first = Instruction.Create(OpCodes.Ldarg_0);

			if (throwOnFailure)
			{
				var constructorInfo = typeof(BindingTargetNullException).GetConstructor(new[] { typeof(string) });
				var exceptionCtor = bindingInitializer.Module.ImportReference(constructorInfo);

				EmitJumpToIfNotNull(appender, bindingTarget, first);
				EmitThrow(appender, exceptionCtor, $"BindingTarget is null. Make sure to initialize it in the BindingInitializer method or some time before calling it. BindingInitializer = {bindingInitializer.FullName} BindingTarget = {bindingTarget.FullName}");
			}
			else
			{
				EmitJumpToIfNull(appender, bindingTarget, GetLastInstruction(bindingInitializer));
			}

			appender.AddInstruction(first);
			appender.AddInstruction(GetLoadFromFieldOrCallableInstruction(bindingTarget));
			appender.AddInstruction(Instruction.Create(OpCodes.Ldarg_0));
			appender.AddInstruction(Instruction.Create(GetCallInstruction(accessorSymbol), accessorSymbol));

			appender.Emit();
		}

		#endregion

		#region Set Helper

		internal MethodMemento CreateMethodMemento(MethodDefinition method)
		{
			YeetIfNull(method, nameof(method));
			YeetIfAbstract(method);

			var body = method.Body;
			var memento = new MethodMemento(
				variables: new List<VariableDefinition>(body.Variables),
				exceptionHandlers: new List<ExceptionHandler>(body.ExceptionHandlers),
				instructions: new List<Instruction>(body.Instructions),
				initLocals: body.InitLocals
			);

			return memento;
		}

		internal MethodDefinition EmitSetHelper(string targetSetHelperMethodName, MethodDefinition setMethod)
		{
			return EmitSetHelperMethodDefinition(targetSetHelperMethodName, setMethod);
		}

		internal MethodDefinition EmitSetHelper(string targetSetHelperMethodName, MethodDefinition setMethod, MethodMemento memento)
		{
			if (string.IsNullOrEmpty(targetSetHelperMethodName))
			{
				throw new ArgumentNullException(nameof(targetSetHelperMethodName), "Set helper name cannot be null.");
			}

			YeetIfNull(setMethod, nameof(setMethod));
			YeetIfNull(memento, nameof(memento));
			YeetIfAbstract(setMethod);

			var setHelper = EmitSetHelper(targetSetHelperMethodName, setMethod);
			EmitSetHelperMethodBody(setHelper, memento);

			return setHelper;
		}

		private MethodDefinition EmitSetHelperMethodDefinition(string targetSetHelperMethodName, MethodDefinition setMethod)
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

			return targetSetHelperMethod;
		}

		private void EmitSetHelperMethodBody(MethodDefinition setHelperMethod, MethodMemento memento)
		{
			var targetMethodBody = setHelperMethod.Body;
			var ilProcessor = targetMethodBody.GetILProcessor();

			foreach (var variable in memento.Variables)
			{
				targetMethodBody.Variables.Add(variable);
			}

			foreach (var handler in memento.ExceptionHandlers)
			{
				targetMethodBody.ExceptionHandlers.Add(handler);
			}

			targetMethodBody.InitLocals = memento.InitLocals;

			foreach (var instruction in memento.Instructions)
			{
				ilProcessor.Append(instruction);
			}
		}

		#endregion

		#region Binding

		internal EmitBindingCommand CreateEmitCommand(EmitParameters parameters)
		{
			parameters.Validate();
			return new EmitBindingCommand(fromSetter => EmitBinding(parameters.UsingFromSetter(fromSetter)));
		}

		internal void EmitBinding(in EmitParameters parameters)
		{
			parameters.Validate();

			var appender = new MethodAppender(parameters.FromSetter);

			if (parameters.EmitNullCheck)
			{
				EmitJumpToIfNull(appender, parameters.BindingTarget, GetLastInstruction(parameters.FromSetter));
			}

			// IL_0007: ldarg.0      // this
			// IL_0008: call instance class Realmar.UnityMVVM.Example.ExampleView Realmar.UnityMVVM.Example.ExampleViewModel::get_View()
			// IL_000d: ldarg.0      // this
			// IL_000e: call instance int32 Realmar.UnityMVVM.Example.ExampleViewModel::get_Value3()
			// IL_0013: call instance void Realmar.UnityMVVM.Example.ExampleView::set_Value3(int32)

			appender.AddInstruction(Instruction.Create(OpCodes.Ldarg_0));
			appender.AddInstruction(GetLoadFromFieldOrCallableInstruction(parameters.BindingTarget));
			if (parameters.Converter.ConvertMethod == null)
			{
				appender.AddInstruction(Instruction.Create(OpCodes.Ldarg_0));
				appender.AddInstruction(Instruction.Create(GetCallInstruction(parameters.FromGetter), parameters.FromGetter));
			}
			else
			{
				EmitConversion(appender, parameters);
			}

			appender.AddInstruction(Instruction.Create(GetCallInstruction(parameters.ToSetter), parameters.ToSetter));

			appender.Emit();
		}

		private void EmitConversion(MethodExtender extender, in EmitParameters parameters)
		{
			// IL_000e: ldarg.0      // this
			// IL_000f: ldfld        class ['Assembly-CSharp']Realmar.DataBindings.Converters.StringToIntConverter UnitsUnderTest.Positive_E2E_ConverterTests.OneWay_IntToString.Source::_converter
			// IL_0014: ldarg.0      // this
			// IL_0015: call instance string UnitsUnderTest.Positive_E2E_ConverterTests.OneWay_IntToString.Source::get_Text()
			// IL_001a: callvirt instance int32['Assembly-CSharp'] Realmar.DataBindings.Converters.StringToIntConverter::Convert(string)

			// generic type

			// IL_0007: ldarg.0      // this
			// IL_0008: ldfld class ['Assembly-CSharp'] Realmar.DataBindings.Converters.CastConverter`2<float64, int32> UnitsUnderTest.Positive_E2E_ConverterTests.TwoWay_GenericConverter.Source::s
			// IL_000d: ldarg.0      // this
			// IL_000e: call instance float64 UnitsUnderTest.Positive_E2E_ConverterTests.TwoWay_GenericConverter.Source::get_Text(
			// IL_0013: callvirt instance !1/*int32*/ class ['Assembly-CSharp'] Realmar.DataBindings.Converters.CastConverter`2<float64, int32>::Convert(!0/*float64*/)

			extender.AddInstruction(Instruction.Create(OpCodes.Ldarg_0));
			extender.AddInstruction(Instruction.Create(OpCodes.Ldfld, parameters.Converter.ConverterField));
			extender.AddInstruction(Instruction.Create(OpCodes.Ldarg_0));
			extender.AddInstruction(Instruction.Create(GetCallInstruction(parameters.FromGetter), parameters.FromGetter));
			// TODO: maybe not use callvirt, probably not (safer)
			extender.AddInstruction(Instruction.Create(OpCodes.Callvirt, parameters.Converter.ConvertMethod));
		}

		#endregion

		#region Converter

		internal FieldDefinition EmitConverter(TypeReference converterType, MethodReference ctor, TypeDefinition targetType, string fieldName)
		{
			YeetIfNull(converterType, nameof(converterType));
			YeetIfNull(ctor, nameof(ctor));
			YeetIfNull(targetType, nameof(targetType));
			YeetIfEmptyOrNull(fieldName, nameof(fieldName));

			var field = new FieldDefinition(
				fieldName,
				FieldAttributes.Private | FieldAttributes.InitOnly,
				converterType);
			targetType.Fields.Add(field);

			foreach (var constructor in targetType.GetConstructors())
			{
				var preppender = new MethodPreppender(constructor);

				// IL_0000: ldarg.0      // this
				// IL_0001: newobj instance void ['Assembly-CSharp']Realmar.DataBindings.Converters.StringToIntConverter::.ctor()
				// IL_0006: stfld        class ['Assembly-CSharp']Realmar.DataBindings.Converters.StringToIntConverter UnitsUnderTest.Positive_E2E_ConverterTests.OneWay_IntToString.Source::_converter

				// generic type

				// IL_0007: newobj       instance void class ['Assembly-CSharp']Realmar.DataBindings.Converters.CastConverter`2<float64, int32>::.ctor()

				preppender.AddInstruction(Instruction.Create(OpCodes.Ldarg_0));
				preppender.AddInstruction(Instruction.Create(OpCodes.Newobj, ctor));
				preppender.AddInstruction(Instruction.Create(OpCodes.Stfld, field));

				preppender.Emit();
			}

			return field;
		}

		#endregion

		#region Branching

		private void EmitJumpToIfNull(MethodAppender appender, IMemberDefinition toBeChecked, Instruction jumpTarget)
		{
			EmitNullBranching(appender, toBeChecked, jumpTarget, true);
		}

		private void EmitJumpToIfNotNull(MethodAppender appender, IMemberDefinition toBeChecked, Instruction jumpTarget)
		{
			EmitNullBranching(appender, toBeChecked, jumpTarget, false);
		}

		private void EmitNullBranching(MethodAppender appender, IMemberDefinition toToBeChecked, Instruction skipBranch, bool jumpIfNull)
		{
			appender.AddInstruction(Instruction.Create(OpCodes.Ldarg_0));
			appender.AddInstruction(GetLoadFromFieldOrCallableInstruction(toToBeChecked));
			appender.AddInstruction(Instruction.Create(OpCodes.Ldnull));

			var returnType = GetReturnType(toToBeChecked);
			var op = returnType.GetEqualityOperator();

			OpCode branchingOpCode;
			if (op != null)
			{
				branchingOpCode = jumpIfNull ? OpCodes.Brtrue_S : OpCodes.Brfalse_S;
				var opReference = toToBeChecked.DeclaringType.Module.ImportReference(op);
				appender.AddInstruction(Instruction.Create(OpCodes.Call, opReference));
			}
			else
			{
				branchingOpCode = jumpIfNull ? OpCodes.Brfalse_S : OpCodes.Brtrue_S;
				appender.AddInstruction(Instruction.Create(OpCodes.Cgt_Un));
			}

			appender.AddInstruction(Instruction.Create(branchingOpCode, skipBranch));
		}

		#endregion

		#region Throwing

		private void EmitThrow(MethodAppender appender, MethodReference exceptionCtor, string message)
		{
			// IL_001b: ldstr        "BindingTarget (TODO FULL PATH) is null."
			// IL_0020: newobj instance void Realmar.DataBindings.BindingTargetNullException::.ctor(string)
			// IL_0025: throw

			if (message != null)
			{
				appender.AddInstruction(Instruction.Create(OpCodes.Ldstr, message));
			}

			appender.AddInstruction(Instruction.Create(OpCodes.Newobj, exceptionCtor));
			appender.AddInstruction(Instruction.Create(OpCodes.Throw));
		}

		#endregion

		#region Attributes

		private void EmitCustomAttribute<TAttribute>(
			ICustomAttributeProvider target,
			ModuleDefinition module,
			params object[] ctorArgs)
			where TAttribute : Attribute
		{
			YeetIfNull(target, nameof(target));
			YeetIfNull(module, nameof(module));

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

		#endregion
	}
}
