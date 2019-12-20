using System.Collections.Generic;

namespace Realmar.DataBindings.Editor.TestFramework
{
	public interface IUnitUnderTestSandbox
	{
		IReadOnlyCollection<IBinding> Bindings { get; }
		void InitializeSandbox(string assemblyPath);
		void ChangeNamespace(string @namespace);
		void RunBindingInitializer();
	}
}
