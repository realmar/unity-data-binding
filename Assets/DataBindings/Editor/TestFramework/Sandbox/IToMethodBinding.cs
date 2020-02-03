using Realmar.DataBindings.Editor.TestFramework.Sandbox.UUT;

namespace Realmar.DataBindings.Editor.TestFramework.Sandbox
{
	internal interface IToMethodBinding : IBinding<InvokeOnChangeAttribute>
	{
		IUUTBindingObject Source { get; }
	}
}
