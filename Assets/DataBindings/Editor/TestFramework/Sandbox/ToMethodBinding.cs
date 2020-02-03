using System;
using Realmar.DataBindings.Editor.TestFramework.Sandbox.UUT;
using Realmar.DataBindings.Editor.TestFramework.Sandbox.Visitors;

namespace Realmar.DataBindings.Editor.TestFramework.Sandbox
{
	internal class ToMethodBinding : MarshalByRefObject, IToMethodBinding
	{
		public InvokeOnChangeAttribute BindingAttribute { get; }
		public IUUTBindingObject Source { get; }

		public void Accept(IBindingVisitor visitor) => visitor.Visit(this);
	}
}
