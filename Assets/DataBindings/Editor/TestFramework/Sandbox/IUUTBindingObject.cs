namespace Realmar.DataBindings.Editor.TestFramework.Sandbox
{
	internal interface IUUTBindingObject : IUUTObject
	{
		string DeclaringTypeFQDN { get; }
		object BindingValue { get; set; }
	}
}
