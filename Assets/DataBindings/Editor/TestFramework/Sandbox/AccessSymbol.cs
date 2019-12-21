using System;
using System.Reflection;
using Realmar.DataBindings.Editor.Extensions;

namespace Realmar.DataBindings.Editor.TestFramework.Sandbox
{
	internal class AccessSymbol : MarshalByRefObject, IAccessSymbol
	{
		private readonly MemberInfo _info;
		private readonly object _obj;

		internal AccessSymbol(MemberInfo info, object obj)
		{
			_info = info;
			_obj = obj;
		}

		public string DeclaringTypeFQDN => _obj.GetType().AssemblyQualifiedName;

		public object BindingValue
		{
			get => _info.GetFieldOrPropertyValue(_obj);
			set => _info.SetFieldOrPropertyValue(_obj, value);
		}

		public int GetHashCodeOfObject() => _obj.GetHashCode();

		public object ReflectValue(string name) => _obj.GetType().GetFieldOrPropertyValue(name, _obj);
	}
}
