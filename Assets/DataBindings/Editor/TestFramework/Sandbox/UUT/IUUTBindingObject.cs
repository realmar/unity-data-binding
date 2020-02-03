using System;

namespace Realmar.DataBindings.Editor.TestFramework.Sandbox.UUT
{
	internal interface IUUTBindingObject : IUUTObject
	{
		string DeclaringTypeFQDN { get; }
		object BindingValue { get; set; }
		Type BindingValueType { get; }
	}
}
