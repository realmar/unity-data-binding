using System;
using System.Reflection;
using Realmar.DataBindings.Editor.Shared.Extensions;

namespace Realmar.DataBindings.Editor.TestFramework.Sandbox.UUT
{
	internal class UUTBindingObject : UUTObject, IUUTBindingObject
	{
		protected MemberInfo Info;

		internal UUTBindingObject(MemberInfo info, object obj) : base(obj)
		{
			Info = info;
		}

		public string DeclaringTypeFQDN => Obj.GetType().AssemblyQualifiedName;

		public object BindingValue
		{
			get => Info?.GetFieldOrPropertyValue(Obj);
			set => Info?.SetFieldOrPropertyValue(Obj, value);
		}

		public Type BindingValueType => Info?.GetFieldOrPropertyType();
	}
}
