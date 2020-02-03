using Realmar.DataBindings.Editor.TestFramework.Sandbox.Visitors;

namespace Realmar.DataBindings.Editor.TestFramework.Sandbox
{
	internal interface IBinding<out TAttribute>
	{
		TAttribute BindingAttribute { get; }
		void Accept(IBindingVisitor visitor);
	}
}
