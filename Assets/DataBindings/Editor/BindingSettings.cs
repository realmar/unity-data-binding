using Realmar.DataBindings;

namespace Realmar.DataBindings.Editor
{
	internal struct BindingSettings
	{
		public BindingType Type { get; set; }
		public int TargetId { get; set; }
		public string TargetPropertyName { get; set; }
		public bool EmitNullCheck { get; set; }
	}
}
