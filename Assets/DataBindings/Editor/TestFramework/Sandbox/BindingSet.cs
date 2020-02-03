using System;
using System.Collections.Generic;
using System.Reflection;

namespace Realmar.DataBindings.Editor.TestFramework.Sandbox
{
	internal class BindingSet : MarshalByRefObject, IBindingSet
	{
		private readonly MethodInfo _bindingInitializer;

		private readonly object _bindingInitializerObject;

		internal BindingSet(IReadOnlyCollection<IBinding<Attribute>> bindings, MethodInfo bindingInitializer, object bindingInitializerObject)
		{
			Bindings = bindings;
			_bindingInitializer = bindingInitializer;
			_bindingInitializerObject = bindingInitializerObject;
		}

		public IReadOnlyCollection<IBinding<Attribute>> Bindings { get; }

		public void RunBindingInitializer()
		{
			_bindingInitializer?.Invoke(_bindingInitializerObject, Array.Empty<object>());
		}
	}
}
