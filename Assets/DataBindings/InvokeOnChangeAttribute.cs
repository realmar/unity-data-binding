using System;

namespace Realmar.DataBindings
{
	[Serializable]
	[AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = true)]
	public class InvokeOnChangeAttribute : Attribute
	{
		public string[] TargetMethodNames { get; private set; }

		public InvokeOnChangeAttribute(params string[] targetMethodNames)
		{
			TargetMethodNames = targetMethodNames;
		}
	}
}
