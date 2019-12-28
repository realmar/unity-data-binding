using Realmar.DataBindings.Editor.Extensions;
using System;
using System.Reflection;

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
			get => _info?.GetFieldOrPropertyValue(_obj);
			set => _info?.SetFieldOrPropertyValue(_obj, value);
		}

		public int GetHashCodeOfObject() => _obj?.GetHashCode() ?? 0;

		public object ReflectValue(string name) => _obj?.GetType().GetFieldOrPropertyValue(name, _obj);

		public void SetValue(string name, object value) => _obj?.GetType().SetFieldOrPropertyValue(name, _obj, value);

		public object Invoke(string name, params object[] arguments) => _obj?.GetType().InvokeMethod(name, _obj, arguments);
	}
}
