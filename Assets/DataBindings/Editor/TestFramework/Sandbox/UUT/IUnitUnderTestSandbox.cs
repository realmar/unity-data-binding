namespace Realmar.DataBindings.Editor.TestFramework.Sandbox.UUT
{
	internal interface IUnitUnderTestSandbox
	{
		IReadOnlyBindingCollection BindingCollection { get; }
		void InitializeSandbox(string assemblyPath);
		void ChangeNamespace(string @namespace);
	}
}
