namespace Realmar.DataBindings.Editor.TestFramework
{
	public interface IBinding
	{
		BindingAttribute BindingAttribute { get; }
		void SetSourceProperty(object value);
		void SetTargetProperty(object value);
		object GetSourceProperty();
		object GetTargetProperty();
	}
}
