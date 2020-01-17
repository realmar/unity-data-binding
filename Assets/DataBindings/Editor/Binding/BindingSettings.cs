using Realmar.DataBindings;

namespace Realmar.DataBindings.Editor.Binding
{
	internal readonly struct BindingSettings
	{
		public BindingType Type { get; }
		public int TargetId { get; }
		public string TargetPropertyName { get; }
		public bool EmitNullCheck { get; }

		public BindingSettings(BindingType type, int targetId, string targetPropertyName, bool emitNullCheck)
		{
			Type = type;
			TargetId = targetId;
			TargetPropertyName = targetPropertyName;
			EmitNullCheck = emitNullCheck;
		}
	}
}
