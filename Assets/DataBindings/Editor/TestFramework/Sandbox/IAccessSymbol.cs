namespace Realmar.DataBindings.Editor.TestFramework.Sandbox
{
	internal interface IAccessSymbol
	{
		string DeclaringTypeFQDN { get; }
		object BindingValue { get; set; }
		int GetHashCodeOfObject();
		object ReflectValue(string name);
	}
}
