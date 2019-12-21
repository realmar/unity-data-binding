namespace Realmar.DataBindings.Editor.TestFramework.Sandbox
{
	internal interface IBinding
	{
		BindingAttribute BindingAttribute { get; }

		IAccessSymbol Target { get; }
		IAccessSymbol Source { get; }
	}
}
