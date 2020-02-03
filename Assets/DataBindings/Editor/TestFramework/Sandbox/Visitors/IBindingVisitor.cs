namespace Realmar.DataBindings.Editor.TestFramework.Sandbox.Visitors
{
	internal interface IBindingVisitor
	{
		void Visit(IPropertyBinding binding);
		void Visit(IToMethodBinding binding);
	}
}
