using System;
using System.Collections.Generic;

namespace Realmar.DataBindings.Editor.TestFramework.Sandbox
{
	internal interface IBindingSet
	{
		IReadOnlyCollection<IBinding<Attribute>> Bindings { get; }
		void RunBindingInitializer();
	}
}
