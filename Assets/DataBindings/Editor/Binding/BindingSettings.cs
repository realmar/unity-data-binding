using Mono.Cecil;

namespace Realmar.DataBindings.Editor.Binding
{
	internal readonly struct BindingSettings
	{
		public BindingType Type { get; }
		public int TargetId { get; }
		public string TargetPropertyName { get; }
		public bool EmitNullCheck { get; }
		public TypeDefinition Converter { get; }

		public BindingSettings(BindingType type, int targetId, string targetPropertyName, bool emitNullCheck, TypeDefinition converter)
		{
			Type = type;
			TargetId = targetId;
			TargetPropertyName = targetPropertyName;
			EmitNullCheck = emitNullCheck;
			Converter = converter;
		}
	}
}
