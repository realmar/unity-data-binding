using Mono.Cecil;

namespace Realmar.DataBindings.Editor.Weaving
{
	internal readonly struct WeaveMethodParameters
	{
		internal MethodDefinition FromGetter { get; }
		internal MethodDefinition FromSetter { get; }
		internal MethodDefinition ToSetter { get; }
		internal IMemberDefinition BindingTarget { get; }
		internal bool EmitNullCheck { get; }
		internal TypeReference Converter { get; }

		public WeaveMethodParameters(MethodDefinition fromGetter, MethodDefinition fromSetter, MethodDefinition toSetter, IMemberDefinition bindingTarget, bool emitNullCheck, TypeReference converter)
		{
			FromGetter = fromGetter;
			FromSetter = fromSetter;
			ToSetter = toSetter;
			BindingTarget = bindingTarget;
			EmitNullCheck = emitNullCheck;
			Converter = converter;
		}

		internal WeaveMethodParameters UsingToSetter(MethodDefinition toSetter)
		{
			return new WeaveMethodParameters(FromGetter, FromSetter, toSetter, BindingTarget, EmitNullCheck, Converter);
		}

		internal WeaveMethodParameters UsingFromSetter(MethodDefinition fromSetter)
		{
			return new WeaveMethodParameters(FromGetter, fromSetter, ToSetter, BindingTarget, EmitNullCheck, Converter);
		}
	}
}
