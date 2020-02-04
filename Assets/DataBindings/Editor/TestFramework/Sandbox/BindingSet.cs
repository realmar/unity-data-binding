using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Realmar.DataBindings.Editor.TestFramework.Sandbox
{
	internal class BindingSet : MarshalByRefObject, IBindingSet
	{
		private readonly MethodInfo _bindingInitializer;

		private readonly object _bindingInitializerObject;

		internal BindingSet(IReadOnlyCollection<IBinding> bindings, MethodInfo bindingInitializer, object bindingInitializerObject)
		{
			// If we don't do this it will throw InvalidCastException when passing it between AppDomains
			// No I don't know why and yes its weird. It looks like a bug in the runtime.
			Bindings = bindings.Select(binding => (IBinding) binding).ToArray();
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
