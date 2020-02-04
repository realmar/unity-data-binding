namespace Realmar.DataBindings.Editor.TestFramework.Sandbox.UUT
{
	internal interface IUnitUnderTestSandbox
	{
		IReadOnlyBindingCollection BindingCollection { get; }
		void InitializeSandbox(string assemblyPath);
		void ChangeNamespace(string @namespace);

		/// <summary>
		/// Creates an object which lives the context of the sandbox.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="ctorArgs">The ctor arguments.</param>
		/// <returns></returns>
		T CreateObject<T>(params object[] ctorArgs);
	}
}
