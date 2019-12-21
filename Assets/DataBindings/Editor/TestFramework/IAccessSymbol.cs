namespace Realmar.DataBindings.Editor.TestFramework
{
	public interface IAccessSymbol
	{
		string DeclaringTypeFQDN { get; }
		object BindingValue { get; set; }
		int GetHashCodeOfObject();
		object ReflectValue(string name);
	}
}
