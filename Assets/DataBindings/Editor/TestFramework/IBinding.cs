namespace Realmar.DataBindings.Editor.TestFramework
{
	public interface IBinding
	{
		BindingAttribute BindingAttribute { get; }

		IAccessSymbol Target { get; }
		IAccessSymbol Source { get; }
	}
}
