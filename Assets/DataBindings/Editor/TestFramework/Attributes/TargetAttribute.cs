using System;

namespace Realmar.DataBindings.Editor.TestFramework.Attributes
{
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface, AllowMultiple = true, Inherited = false)]
	public class TargetAttribute : Attribute
	{
		public int Id { get; set; } = 0;
	}
}
