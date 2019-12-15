namespace Realmar.DataBindings.Editor.TestFramework
{
	public interface IUnitUnderTestSandbox
	{
		void InitializeSandbox(string assemblyPath);
		void ChangeNamespace(string @namespace);
		void RunBindingInitializer();
		IBinding[] GetBindings();
	}
}
