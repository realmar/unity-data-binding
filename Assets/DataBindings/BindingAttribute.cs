using System;

namespace Realmar.DataBindings
{
	[AttributeUsage(AttributeTargets.Property, AllowMultiple = true, Inherited = false)]
	public class BindingAttribute : Attribute
	{
		public BindingType BindingType { get; }
		public int TargetId { get; }
		public string TargetPropertyName { get; }
		public bool EmitNullCheck { get; }

		public BindingAttribute(
			BindingType bindingType = BindingType.OneWay,
			int targetId = 0,
			string targetPropertyName = null,
			bool emitNullCheck = false)
		{
			BindingType = bindingType;
			TargetId = targetId;
			TargetPropertyName = targetPropertyName;
			EmitNullCheck = emitNullCheck;
		}
	}
}
