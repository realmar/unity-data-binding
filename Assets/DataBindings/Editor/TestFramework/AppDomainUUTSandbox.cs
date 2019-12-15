using System;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Realmar.DataBindings.Editor.TestFramework
{
	internal class AppDomainUUTSandbox : MarshalByRefObject, IUnitUnderTestSandbox
	{
		private Assembly _context;
		private BindingCollection _bindingCollection;

		public void InitializeSandbox(string assemblyPath)
		{
			if (_context != null)
			{
				throw new InvalidOperationException("Sandbox has already been initialized.");
			}

			_context = AppDomain.CurrentDomain.Load(File.ReadAllBytes(assemblyPath));
		}

		public void ChangeNamespace(string @namespace)
		{
			if (_context == null)
			{
				throw new InvalidOperationException("Sandbox has not been initialized yet.");
			}

			var types = _context.GetTypes()
				.Where(type => type.Namespace != null && type.Namespace.StartsWith(@namespace))
				.ToArray();

			var bindingFactory = new BindingFactory(types);
			_bindingCollection = bindingFactory.CreateBindings();
		}

		public void RunBindingInitializer()
		{
			_bindingCollection.BindingInitializer
				?.Invoke(_bindingCollection.BindingInitializerObject, Array.Empty<object>());
		}

		public IBinding[] GetBindings()
		{
			return _bindingCollection.Bindings;
		}
	}
}
