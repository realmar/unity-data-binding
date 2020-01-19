using Mono.Cecil;

namespace Realmar.DataBindings.Editor.Binding
{
	internal readonly struct BindingSettings
	{
		public BindingType Type { get; }
		public int TargetId { get; }
		public string TargetPropertyName { get; }
		public NullCheckBehavior NullCheckBehavior { get; }
		public TypeDefinition Converter { get; }

		public BindingSettings(BindingType type, int targetId, string targetPropertyName, NullCheckBehavior nullCheckBehavior, TypeDefinition converter)
		{
			Type = type;
			TargetId = targetId;
			TargetPropertyName = targetPropertyName;
			NullCheckBehavior = nullCheckBehavior;
			Converter = converter;
		}
	}
}
