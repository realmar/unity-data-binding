using Mono.Cecil;
using Mono.Cecil.Cil;
using Mono.Cecil.Rocks;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
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
				new MethodDefinition(GetSetterName(injectedSourceName), attributes, module.ImportReference(module.TypeSystem.Void));

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

			appender.AddInstructions(first);
			appender.AddInstructions(GetLoadFromFieldOrCallableInstruction(bindingTarget));
			appender.AddInstructions(Instruction.Create(OpCodes.Ldarg_0));
			appender.AddInstructions(Instruction.Create(GetCallInstruction(accessorSymbol), accessorSymbol));

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
			YeetIfEmptyOrNull(targetSetHelperMethodName, nameof(targetSetHelperMethodName));
			YeetIfNull(setMethod, nameof(setMethod));
			YeetIfNull(memento, nameof(memento));
			YeetIfAbstract(setMethod);

			var setHelper = EmitSetHelper(targetSetHelperMethodName, setMethod);
			EmitSetHelperMethodBody(setHelper, memento);

			return setHelper;
		}

		private MethodDefinition EmitSetHelperMethodDefinition(string targetSetHelperMethodName, MethodDefinition setMethod)
		{
			var module = setMethod.Module;
			var attributes = setMethod.Attributes & ~MethodAttributes.SpecialName;
			var targetSetHelperMethod = new MethodDefinition(
				targetSetHelperMethodName,
				attributes,
				module.ImportReference(module.TypeSystem.Void));
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

		internal EmitBindingCommand CreateEmitCommand(EmitParameters parameters, IMemberDefinition fromGetter)
		{
			var localFromGetter = fromGetter;
			return new EmitBindingCommand(fromSetter => EmitBinding(parameters.UsingFromSetter(fromSetter), localFromGetter));
		}

		internal EmitBindingCommand CreateEmitCommand(EmitParameters parameters, ushort methodParameterIndex)
		{
			return new EmitBindingCommand(fromSetter => EmitBinding(parameters.UsingFromSetter(fromSetter), methodParameterIndex));
		}

		internal void EmitBinding(in EmitParameters parameters, IMemberDefinition fromGetter)
		{
			var fromGetterLoad = GetLoadFromFieldOrCallableInstruction(fromGetter);
			EmitBinding(
				parameters,
				new[]
				{
					Instruction.Create(OpCodes.Ldarg_0),
					fromGetterLoad
				});
		}

		internal void EmitBinding(in EmitParameters parameters, ushort methodParameterIndex)
		{
			var fromGetterLoad = GetLdArgInstruction(parameters.FromSetter, methodParameterIndex);
			EmitBinding(parameters, new[] { fromGetterLoad });
		}

		private void EmitBinding(in EmitParameters parameters, Instruction[] fromGetterInstructions)
		{
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

			if (parameters.BindingTarget != null)
			{
				appender.AddInstructions(Instruction.Create(OpCodes.Ldarg_0));
				appender.AddInstructions(GetLoadFromFieldOrCallableInstruction(parameters.BindingTarget));
			}

			if (parameters.Converter.ConvertMethod == null)
			{
				appender.AddInstructions(fromGetterInstructions);
			}
			else
			{
				EmitConversion(appender, parameters, fromGetterInstructions);
			}

			appender.AddInstructions(Instruction.Create(GetCallInstruction(parameters.ToSetter), parameters.ToSetter));

			appender.Emit();
		}

		private void EmitConversion(MethodExtender extender, in EmitParameters parameters, Instruction[] fromGetterInstructions)
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

			extender.AddInstructions(Instruction.Create(OpCodes.Ldarg_0));
			extender.AddInstructions(Instruction.Create(OpCodes.Ldfld, parameters.Converter.ConverterField));
			extender.AddInstructions(fromGetterInstructions);
			// TODO: maybe not use callvirt, probably not (safer)
			extender.AddInstructions(Instruction.Create(OpCodes.Callvirt, parameters.Converter.ConvertMethod));
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

				preppender.AddInstructions(Instruction.Create(OpCodes.Ldarg_0));
				preppender.AddInstructions(Instruction.Create(OpCodes.Newobj, ctor));
				preppender.AddInstructions(Instruction.Create(OpCodes.Stfld, field));

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
			appender.AddInstructions(Instruction.Create(OpCodes.Ldarg_0));
			appender.AddInstructions(GetLoadFromFieldOrCallableInstruction(toToBeChecked));
			appender.AddInstructions(Instruction.Create(OpCodes.Ldnull));

			var returnType = GetReturnType(toToBeChecked);
			var op = returnType.GetEqualityOperator();

			OpCode branchingOpCode;
			if (op != null)
			{
				branchingOpCode = jumpIfNull ? OpCodes.Brtrue_S : OpCodes.Brfalse_S;
				var opReference = toToBeChecked.DeclaringType.Module.ImportReference(op);
				appender.AddInstructions(Instruction.Create(OpCodes.Call, opReference));
			}
			else
			{
				branchingOpCode = jumpIfNull ? OpCodes.Brfalse_S : OpCodes.Brtrue_S;
				appender.AddInstructions(Instruction.Create(OpCodes.Cgt_Un));
			}

			appender.AddInstructions(Instruction.Create(branchingOpCode, skipBranch));
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
				appender.AddInstructions(Instruction.Create(OpCodes.Ldstr, message));
			}

			appender.AddInstructions(Instruction.Create(OpCodes.Newobj, exceptionCtor));
			appender.AddInstructions(Instruction.Create(OpCodes.Throw));
		}

		#endregion

		#region Attributes

		private void EmitCustomAttribute<TAttribute>(
			ICustomAttributeProvider target,
			ModuleDefinition module,
			params object[] ctorArgs)
			where TAttribute : Attribute
		{
			// TODO Fix that
			return;

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

		#region Instructions

		private static Instruction GetLoadFromFieldOrCallableInstruction(IMemberDefinition bindingTarget)
		{
			YeetIfNull(bindingTarget, nameof(bindingTarget));

			switch (bindingTarget)
			{
				case FieldDefinition field:
					return Instruction.Create(OpCodes.Ldfld, field);
				case MethodDefinition method:
					return Instruction.Create(GetCallInstruction(method), method);
				default:
					throw new NotSupportedException("BindingTarget can only be field or method");
			}
		}

		private static OpCode GetCallInstruction(MethodDefinition method)
		{
			YeetIfNull(method, nameof(method));

			// TODO: CRITICAL callvirt is not always applicable even is if not virtual.
			// ie. callvirt is used as a fast way to check if reference is null and then throw a nullref
			// there is quite complicated logic in the compiler source to determine if call or callvirt should be emitted
			// https://github.com/dotnet/roslyn/blob/master/src/Compilers/CSharp/Portable/CodeGen/EmitExpression.cs#L1342
			var isVirtual = method.IsVirtual || method.IsAbstract;
			return isVirtual ? OpCodes.Callvirt : OpCodes.Call;
		}

		private static Instruction GetLastInstruction(MethodDefinition method)
		{
			YeetIfNull(method, nameof(method));
			YeetIfAbstract(method);

			var methodBody = method.Body;
			var lastInstruction = methodBody.Instructions.Last();

			return lastInstruction;
		}

		private static Instruction GetLdArgInstruction(IMethodSignature signature, ushort parameterPosition)
		{
			if (signature.Parameters.Count <= parameterPosition)
			{
				throw new ArgumentOutOfRangeException(nameof(parameterPosition), $"{nameof(parameterPosition)} must be lower than {signature.Parameters.Count}, signature: {signature}");
			}

			Instruction result;

			if (parameterPosition == 0)
			{
				result = Instruction.Create(OpCodes.Ldarg_1);
			}
			else if (parameterPosition == 1)
			{
				result = Instruction.Create(OpCodes.Ldarg_2);
			}
			else if (parameterPosition == 2)
			{
				result = Instruction.Create(OpCodes.Ldarg_3);
			}
			else if (parameterPosition <= byte.MaxValue)
			{
				result = Instruction.Create(OpCodes.Ldarg_S, signature.Parameters[parameterPosition]);
			}
			else
			{
				result = Instruction.Create(OpCodes.Ldarg, signature.Parameters[parameterPosition]);
			}

			return result;
		}

		#endregion

		#region Naming

		private static string GetBackingFieldName(string normalName) => $"\u003C{normalName}\u003Ek__BackingField";
		private static string GetGetterName(string normalName) => $"get_{normalName}";
		private static string GetSetterName(string normalName) => $"set_{normalName}";

		#endregion
	}
}
