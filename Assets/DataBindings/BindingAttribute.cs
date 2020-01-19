using System;

namespace Realmar.DataBindings
{
	[Serializable]
	[AttributeUsage(AttributeTargets.Property, AllowMultiple = true, Inherited = false)]
	public class BindingAttribute : Attribute
	{
		public BindingType BindingType { get; private set; }
		public int TargetId { get; private set; }
		public string TargetPropertyName { get; private set; }
		public NullCheckBehavior NullCheckBehavior { get; private set; }
		public Type Converter { get; private set; }

		public BindingAttribute(
			BindingType bindingType = BindingType.OneWay,
			int targetId = 0,
			string targetPropertyName = null,
			NullCheckBehavior nullCheckBehavior = NullCheckBehavior.Auto,
			Type converter = null)
		{
			BindingType = bindingType;
			TargetId = targetId;
			TargetPropertyName = targetPropertyName;
			NullCheckBehavior = nullCheckBehavior;
			Converter = converter;
		}
	}
}
