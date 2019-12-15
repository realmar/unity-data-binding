using System;

namespace Realmar.DataBindings.Editor.TestFramework
{
	[Serializable]
	public struct BindingSet
	{
		internal int BindingTargetId { get; set; }
		internal int SourceId { get; set; }
		internal int TargetId { get; set; }
	}
}
