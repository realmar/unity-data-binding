using System.Collections.Generic;

namespace Realmar.DataBindings.Editor.TestFramework
{
	internal interface IBindingSet
	{
		IReadOnlyCollection<IBinding> Bindings { get; }
		void RunBindingInitializer();
	}
}
