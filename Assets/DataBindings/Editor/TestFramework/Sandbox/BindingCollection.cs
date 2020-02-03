using System;
using System.Collections.Generic;
using System.Linq;
using Realmar.DataBindings.Editor.Shared.Extensions;
using Realmar.DataBindings.Editor.TestFramework.Sandbox.UUT;
using static Realmar.DataBindings.Editor.TestFramework.Sandbox.SandboxHelpers;

namespace Realmar.DataBindings.Editor.TestFramework.Sandbox
{
	internal class BindingCollection : MarshalByRefObject, IBindingCollection
	{
		private readonly List<object> _allObjects;
		private readonly List<IBindingSet> _bindingSets;
		private readonly Lazy<Dictionary<string, Type>> _types;

		public BindingCollection()
		{
			_allObjects = new List<object>();
			_bindingSets = new List<IBindingSet>();
			_types = new Lazy<Dictionary<string, Type>>(BuildTypeMapping);
		}

		public IReadOnlyCollection<IBindingSet> BindingSets => _bindingSets;

		public void AddBindingSet(IBindingSet set, IReadOnlyCollection<object> objs)
		{
			_bindingSets.Add(set);
			_allObjects.AddRange(objs);
		}

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

		public IUUTObject CreateSymbol(string className)
		{
			if (_types.Value.TryGetValue(className, out var type) == false)
			{
				throw new ArgumentException($"Type {className} cannot be found");
			}

			var obj = CreateInstance(type);
			return new UUTObject(obj);
		}

		private Dictionary<string, Type> BuildTypeMapping()
		{
			return _allObjects
				.Select(o => o.GetType())
				.SelectMany(type => type.GetBaseTypes())
				.Distinct()
				.ToDictionary(type => type.Name, type => type);
		}
	}
}
