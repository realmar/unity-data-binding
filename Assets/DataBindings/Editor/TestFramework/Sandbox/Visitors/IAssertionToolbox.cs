namespace Realmar.DataBindings.Editor.TestFramework.Sandbox.Visitors
{
	internal interface IAssertionToolbox
	{
		IBindingSet BindingSet { get; }
		object RunDefaultAssertions(IPropertyBinding binding);
		object RunDefaultAssertions(IToMethodBinding binding);
	}
}
