using System;

namespace Realmar.DataBindings.Editor.TestFramework.Attributes
{
	[Serializable]
	[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
	public class ResultAttribute : Attribute
	{
		public object Expected { get; set; }
	}
}
