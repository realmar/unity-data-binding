using System;
using System.Collections.Generic;
using System.Reflection;

namespace Realmar.DataBindings.Editor.TestFramework
{
	internal class BindingSet : MarshalByRefObject, IBindingSet
	{
		private MethodInfo _bindingInitializer;

		private object _bindingInitializerObject;

		public BindingSet(IBinding[] bindings, MethodInfo bindingInitializer, object bindingInitializerObject)
		{
			Bindings = bindings;
			_bindingInitializer = bindingInitializer;
			_bindingInitializerObject = bindingInitializerObject;
		}

		public IReadOnlyCollection<IBinding> Bindings { get; }

		public void RunBindingInitializer()
		{
			_bindingInitializer?.Invoke(_bindingInitializerObject, Array.Empty<object>());
		}
	}
}
