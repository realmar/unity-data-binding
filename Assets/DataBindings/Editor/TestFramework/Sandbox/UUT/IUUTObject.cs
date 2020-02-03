namespace Realmar.DataBindings.Editor.TestFramework.Sandbox.UUT
{
	internal interface IUUTObject
	{
		int GetHashCodeOfSymbol();
		object GetValue(string name);
		void SetValue(string name, object value);
		object Invoke(string name, params object[] arguments);
	}
}
