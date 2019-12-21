using System.Collections.Generic;

namespace Realmar.DataBindings.Editor.TestFramework
{
	internal interface IUnitUnderTestSandbox
	{
		IReadOnlyCollection<IBindingSet> BindingSets { get; }
		void InitializeSandbox(string assemblyPath);
		void ChangeNamespace(string @namespace);
	}
}
