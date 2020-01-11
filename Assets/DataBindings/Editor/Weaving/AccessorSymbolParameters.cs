using Mono.Cecil;

namespace Realmar.DataBindings.Editor.Weaving
{
	internal readonly struct AccessorSymbolParameters
	{
		internal TypeDefinition SourceType { get; }
		internal TypeDefinition TargetType { get; }
		internal IMemberDefinition BindingTarget { get; }
		internal MethodDefinition BindingInitializer { get; }
		internal BindingInitializerSettings Settings { get; }

		public AccessorSymbolParameters(TypeDefinition sourceType, TypeDefinition targetType, IMemberDefinition bindingTarget, MethodDefinition bindingInitializer, BindingInitializerSettings settings)
		{
			SourceType = sourceType;
			TargetType = targetType;
			BindingTarget = bindingTarget;
			BindingInitializer = bindingInitializer;
			Settings = settings;
		}
	}
}
