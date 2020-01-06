using System.Collections.Generic;

namespace Realmar.DataBindings.Editor.TestFramework.Sandbox
{
	internal interface IBindingCollection
	{
		IReadOnlyCollection<IBindingSet> BindingSets { get; }
		IReadOnlyCollection<IUUTObject> GetSymbols(string className);
		IUUTObject GetSymbol(string className);
	}
}
