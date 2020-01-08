using System;
using System.Collections.Generic;
using System.Linq;

namespace Realmar.DataBindings.Editor.TestFramework.Sandbox
{
	internal class BindingCollection : MarshalByRefObject, IBindingCollection
	{
		private readonly IReadOnlyCollection<object> _allObjects;

		public BindingCollection(IReadOnlyCollection<IBindingSet> bindingSets, IReadOnlyCollection<object> allObjects)
		{
			_allObjects = allObjects;
			BindingSets = bindingSets;
		}

		public IReadOnlyCollection<IBindingSet> BindingSets { get; }

		public void RunAllBindingInitializers()
		{
			foreach (var bindingSet in BindingSets)
			{
				bindingSet.RunBindingInitializer();
			}
		}

		public IReadOnlyCollection<IUUTObject> GetSymbols()
		{
			return _allObjects.Select(o => new UUTObject(o)).ToArray();
		}

		public IReadOnlyCollection<IUUTObject> GetSymbols(string className)
		{
			return _allObjects
				.Where(o => o.GetType().Name == className)
				.Select(o => new UUTObject(o))
				.ToArray();
		}

		public IUUTObject GetSymbol(string className)
		{
			return GetSymbols(className).FirstOrDefault();
		}
	}
}
