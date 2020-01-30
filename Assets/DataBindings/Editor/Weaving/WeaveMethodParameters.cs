using System.Diagnostics;
using Mono.Cecil;

namespace Realmar.DataBindings.Editor.Weaving
{
	[DebuggerStepThrough]
	internal readonly struct WeaveMethodParameters
	{
		internal MethodDefinition FromSetter { get; }
		internal MethodDefinition ToSetter { get; }
		internal IMemberDefinition BindingTarget { get; }
		internal bool EmitNullCheck { get; }
		internal TypeReference Converter { get; }

		public WeaveMethodParameters(MethodDefinition fromSetter, MethodDefinition toSetter, IMemberDefinition bindingTarget, bool emitNullCheck, TypeReference converter)
		{
			FromSetter = fromSetter;
			ToSetter = toSetter;
			BindingTarget = bindingTarget;
			EmitNullCheck = emitNullCheck;
			Converter = converter;
		}

		internal WeaveMethodParameters UsingToSetter(MethodDefinition toSetter)
		{
			return new WeaveMethodParameters(FromSetter, toSetter, BindingTarget, EmitNullCheck, Converter);
		}

		internal WeaveMethodParameters UsingFromSetter(MethodDefinition fromSetter)
		{
			return new WeaveMethodParameters(fromSetter, ToSetter, BindingTarget, EmitNullCheck, Converter);
		}
	}
}
