using Mono.Cecil;
using Realmar.DataBindings.Editor.Exceptions;
using Realmar.DataBindings.Editor.Weaving;
using static Realmar.DataBindings.Editor.Shared.SharedHelpers;

namespace Realmar.DataBindings.Editor.Emitting
{
	internal readonly struct EmitParameters
	{
		internal IMemberDefinition BindingTarget { get; }
		internal MethodDefinition FromGetter { get; }
		internal MethodDefinition FromSetter { get; }
		internal MethodDefinition ToSetter { get; }
		internal bool EmitNullCheck { get; }

		internal EmitParameters(IMemberDefinition bindingTarget, MethodDefinition fromGetter, MethodDefinition fromSetter, MethodDefinition setter, bool emitNullCheck)
		{
			BindingTarget = bindingTarget;
			FromGetter = fromGetter;
			FromSetter = fromSetter;
			ToSetter = setter;
			EmitNullCheck = emitNullCheck;
		}

		internal static EmitParameters FromWeaveParameters(in WeaveParameters parameters)
		{
			var bindingTarget = parameters.BindingTarget;
			var fromGetter = parameters.FromProperty.GetGetMethodOrYeet();
			var fromSetter = parameters.FromProperty.GetSetMethodOrYeet();
			var toSetter = GetSetHelperMethod(parameters.ToProperty, parameters.ToType);

			return new EmitParameters(bindingTarget, fromGetter, fromSetter, toSetter, parameters.EmitNullCheck);
		}
	}
}
