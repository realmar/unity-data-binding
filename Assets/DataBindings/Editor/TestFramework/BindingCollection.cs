using System.Reflection;

namespace Realmar.DataBindings.Editor.TestFramework
{
	internal class BindingCollection
	{
		public IBinding[] Bindings { get; }
		public MethodInfo BindingInitializer { get; }
		public object BindingInitializerObject { get; }

		public BindingCollection(IBinding[] bindings, MethodInfo bindingInitializer, object bindingInitializerObject)
		{
			Bindings = bindings;
			BindingInitializer = bindingInitializer;
			BindingInitializerObject = bindingInitializerObject;
		}
	}
}
