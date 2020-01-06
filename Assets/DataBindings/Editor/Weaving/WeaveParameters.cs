using Mono.Cecil;

namespace Realmar.DataBindings.Editor.Weaving
{
	internal readonly struct WeaveParameters
	{
		internal PropertyDefinition FromProperty { get; }

		internal TypeDefinition ToType { get; }
		internal PropertyDefinition ToProperty { get; }
		internal IMemberDefinition BindingTarget { get; }
		internal bool EmitNullCheck { get; }

		public WeaveParameters(PropertyDefinition fromProperty, TypeDefinition toType, PropertyDefinition toProperty, IMemberDefinition bindingTarget, bool emitNullCheck)
		{
			FromProperty = fromProperty;
			ToType = toType;
			ToProperty = toProperty;
			BindingTarget = bindingTarget;
			EmitNullCheck = emitNullCheck;
		}
	}
}
