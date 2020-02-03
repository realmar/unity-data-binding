using System;
using System.Collections.Generic;

namespace Realmar.DataBindings.Editor.TestFramework.Sandbox.Factories
{
	internal class ToMethodBindingFactory : IBindingFactory
	{
		private readonly IReadOnlyCollection<Type> _types;

		internal ToMethodBindingFactory(IReadOnlyCollection<Type> types)
		{
			_types = types;
		}

		public void CreateBindings(IBindingCollection collection)
		{
		}
	}
}
