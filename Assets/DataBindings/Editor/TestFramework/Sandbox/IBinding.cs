namespace Realmar.DataBindings.Editor.TestFramework.Sandbox
{
	internal interface IBinding
	{
		BindingAttribute BindingAttribute { get; }

		IUUTBindingObject Target { get; }
		IUUTBindingObject Source { get; }
	}
}
