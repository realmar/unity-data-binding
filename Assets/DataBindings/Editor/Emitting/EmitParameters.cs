using Mono.Cecil;

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
	}
}
