using Realmar.DataBindings.Editor.TestFramework.Sandbox.UUT;

namespace Realmar.DataBindings.Editor.TestFramework.Sandbox
{
	internal interface IPropertyBinding : IBinding<BindingAttribute>
	{
		IUUTBindingObject Target { get; }
		IUUTBindingObject Source { get; }
	}
}
