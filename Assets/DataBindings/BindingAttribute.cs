using System;
using System.Runtime.Serialization;

namespace Realmar.DataBindings
{
	[Serializable]
	[AttributeUsage(AttributeTargets.Property, AllowMultiple = true, Inherited = false)]
	public class BindingAttribute : Attribute, ISerializable
	{
		public BindingType BindingType { get; private set; }
		public int TargetId { get; private set; }
		public string TargetPropertyName { get; private set; }
		public bool EmitNullCheck { get; private set; }

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

		protected BindingAttribute(SerializationInfo info, StreamingContext context)
		{
			BindingType = (BindingType) info.GetValue(nameof(BindingType), typeof(BindingType));
			TargetId = (int) info.GetValue(nameof(TargetId), typeof(int));
			TargetPropertyName = (string) info.GetValue(nameof(TargetPropertyName), typeof(string));
			EmitNullCheck = (bool) info.GetValue(nameof(EmitNullCheck), typeof(bool));
		}

		public void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			info.AddValue(nameof(BindingType), BindingType);
			info.AddValue(nameof(TargetId), TargetId);
			info.AddValue(nameof(TargetPropertyName), TargetPropertyName);
			info.AddValue(nameof(EmitNullCheck), EmitNullCheck);
		}
	}
}
