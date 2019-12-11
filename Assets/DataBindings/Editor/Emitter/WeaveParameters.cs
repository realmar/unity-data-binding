using System.Diagnostics;
using Mono.Cecil;

namespace Realmar.DataBindingsEditor.Emitter
{
	internal class WeaveParameters
	{
		internal PropertyDefinition FromProperty { [DebuggerStepThrough] get; [DebuggerStepThrough] set; }
		internal TypeDefinition ToType { [DebuggerStepThrough] get; [DebuggerStepThrough] set; }
		internal PropertyDefinition ToProperty { [DebuggerStepThrough] get; [DebuggerStepThrough] set; }
		internal IMemberDefinition BindingTarget { [DebuggerStepThrough] get; [DebuggerStepThrough] set; }
		internal bool EmitNullCheck { [DebuggerStepThrough] get; [DebuggerStepThrough] set; }

		[DebuggerStepThrough]
		internal WeaveParameters()
		{
		}

		[DebuggerStepThrough]
		internal WeaveParameters(WeaveParameters parameters)
		{
			FromProperty = parameters.FromProperty;
			ToType = parameters.ToType;
			ToProperty = parameters.ToProperty;
			BindingTarget = parameters.BindingTarget;
			EmitNullCheck = parameters.EmitNullCheck;
		}
	}
}
