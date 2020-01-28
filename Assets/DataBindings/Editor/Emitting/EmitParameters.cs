using Mono.Cecil;

namespace Realmar.DataBindings.Editor.Emitting
{
	internal readonly struct EmitParameters
	{
		internal IMemberDefinition BindingTarget { get; }
		internal MethodDefinition FromSetter { get; }
		internal MethodDefinition ToSetter { get; }
		internal bool EmitNullCheck { get; }
		internal Converter Converter { get; }

		internal EmitParameters(IMemberDefinition bindingTarget, MethodDefinition fromSetter, MethodDefinition setter, bool emitNullCheck, Converter converter)
		{
			BindingTarget = bindingTarget;
			FromSetter = fromSetter;
			ToSetter = setter;
			EmitNullCheck = emitNullCheck;
			Converter = converter;
		}

		internal EmitParameters UsingFromSetter(MethodDefinition fromSetter)
		{
			return new EmitParameters(
				BindingTarget,
				fromSetter,
				ToSetter,
				EmitNullCheck,
				Converter);
		}
	}
}
