using System.Collections.Generic;

namespace Realmar.DataBindings.Editor.TestFramework.Sandbox
{
	internal interface IBindingSet
	{
		IReadOnlyCollection<IBinding> Bindings { get; }
		void RunBindingInitializer();
	}
}
