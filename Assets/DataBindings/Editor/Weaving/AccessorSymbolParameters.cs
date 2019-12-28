using Mono.Cecil;

namespace Realmar.DataBindings.Editor.Weaving
{
	internal class AccessorSymbolParameters
	{
		internal TypeDefinition SourceType { get; set; }
		internal TypeDefinition TargetType { get; set; }
		internal IMemberDefinition BindingTarget { get; set; }
		internal MethodDefinition BindingInitializer { get; set; }
		internal BindingInitializerSettings Settings { get; set; }
	}
}
