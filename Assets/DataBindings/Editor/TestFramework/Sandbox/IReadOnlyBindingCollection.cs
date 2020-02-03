using System.Collections.Generic;
using Realmar.DataBindings.Editor.TestFramework.Sandbox.UUT;

namespace Realmar.DataBindings.Editor.TestFramework.Sandbox
{
	internal interface IReadOnlyBindingCollection
	{
		IReadOnlyCollection<IBindingSet> BindingSets { get; }

		void RunAllBindingInitializers();
		IReadOnlyCollection<IUUTObject> GetSymbols();
		IReadOnlyCollection<IUUTObject> GetSymbols(string className);
		IUUTObject GetSymbol(string className);
		IUUTObject CreateSymbol(string className);
	}

	internal interface IBindingCollection : IReadOnlyBindingCollection
	{
		void AddBindingSet(IBindingSet set, IReadOnlyCollection<object> objs);
	}
}
