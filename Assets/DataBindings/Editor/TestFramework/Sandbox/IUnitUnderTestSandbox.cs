using System.Collections.Generic;

namespace Realmar.DataBindings.Editor.TestFramework.Sandbox
{
	internal interface IUnitUnderTestSandbox
	{
		IBindingCollection BindingCollection { get; }
		void InitializeSandbox(string assemblyPath);
		void ChangeNamespace(string @namespace);
	}
}
