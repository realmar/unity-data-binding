using System;
using System.Collections.Generic;
using System.Linq;
using Realmar.DataBindings.Editor.Shared.Extensions;
using static Realmar.DataBindings.Editor.TestFramework.Sandbox.SandboxHelpers;

namespace Realmar.DataBindings.Editor.TestFramework.Sandbox
{
	internal class BindingCollection : MarshalByRefObject, IBindingCollection
	{
		private readonly IReadOnlyCollection<object> _allObjects;
		private readonly Dictionary<string, Type> _types;

		public BindingCollection(IReadOnlyCollection<IBindingSet> bindingSets, IReadOnlyCollection<object> allObjects)
		{
			_allObjects = allObjects;
			BindingSets = bindingSets;

			_types = _allObjects
				.Select(o => o.GetType())
				.SelectMany(type => type.GetBaseTypes())
				.Distinct()
				.ToDictionary(type => type.Name, type => type);
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

		public IUUTObject CreateSymbol(string className)
		{
			if (_types.TryGetValue(className, out var type) == false)
			{
				throw new ArgumentException($"Type {className} cannot be found");
			}

			var obj = CreateInstance(type);
			return new UUTObject(obj);
		}
	}
}
