using System;
using System.Collections.Generic;

namespace Realmar.DataBindings.Editor.TestFramework.Sandbox.Factories
{
	internal class BindingFactoryFacade
	{
		private readonly IReadOnlyCollection<IBindingFactory> _factories;

		internal BindingFactoryFacade(IReadOnlyCollection<Type> types)
		{
			_factories = new IBindingFactory[]
			{
				new PropertyBindingFactory(types),
				new ToMethodBindingFactory(types)
			};
		}

		internal IReadOnlyBindingCollection CreateBindings()
		{
			var collection = new BindingCollection();
			foreach (var factory in _factories)
			{
				factory.CreateBindings(collection);
			}

			return collection;
		}
	}
}
