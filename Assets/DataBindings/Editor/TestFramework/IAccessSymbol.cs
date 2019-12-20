namespace Realmar.DataBindings.Editor.TestFramework
{
	public interface IAccessSymbol
	{
		object BindingValue { get; set; }
		int GetHashCodeOfObject();
		object ReflectValue(string name);
	}
}
