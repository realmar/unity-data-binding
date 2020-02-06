using System;
using Realmar.DataBindings.Editor.TestFramework.Attributes;
using Realmar.DataBindings.Editor.TestFramework.Sandbox.UUT;
using Realmar.DataBindings.Editor.TestFramework.Sandbox.Visitors;

namespace Realmar.DataBindings.Editor.TestFramework.Sandbox
{
	internal class ToMethodBinding : MarshalByRefObject, IToMethodBinding
	{
		public ToMethodBinding(InvokeOnChangeAttribute bindingAttribute, IUUTBindingObject source, IUUTBindingObject resultObject, ResultAttribute resultAttribute)
		{
			BindingAttribute = bindingAttribute;
			Source = source;
			ResultObject = resultObject;
			ResultAttribute = resultAttribute;
		}

		public InvokeOnChangeAttribute BindingAttribute { get; }
		public IUUTBindingObject Source { get; }
		public IUUTBindingObject ResultObject { get; }
		public ResultAttribute ResultAttribute { get; }

		public void Accept(IBindingVisitor visitor) => visitor.Visit(this);
	}
}
