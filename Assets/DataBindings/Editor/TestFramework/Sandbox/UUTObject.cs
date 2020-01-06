using Realmar.DataBindings.Editor.Shared.Extensions;
using System;

namespace Realmar.DataBindings.Editor.TestFramework.Sandbox
{
	internal class UUTObject : MarshalByRefObject, IUUTObject
	{
		protected object Obj { get; }

		internal UUTObject(object obj)
		{
			Obj = obj;
		}

		public int GetHashCodeOfSymbol() => Obj?.GetHashCode() ?? 0;
		public object GetValue(string name) => Obj?.GetType().GetFieldOrPropertyValue(name, Obj);
		public void SetValue(string name, object value) => Obj?.GetType().SetFieldOrPropertyValue(name, Obj, value);
		public object Invoke(string name, params object[] arguments) => Obj?.GetType().InvokeMethod(name, Obj, arguments);
	}
}
