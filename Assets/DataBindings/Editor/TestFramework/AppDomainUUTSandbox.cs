using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

namespace Realmar.DataBindings.Editor.TestFramework
{
	internal class AppDomainUUTSandbox : MarshalByRefObject, IUnitUnderTestSandbox
	{
		private readonly Regex _namespaceRegex = new Regex(@"", RegexOptions.Compiled);

		private Assembly _context;
		private BindingCollection _bindingCollection;

		public IReadOnlyCollection<IBinding> Bindings => _bindingCollection?.Bindings;

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
				.Where(type => type.Namespace != null && type.Namespace == @namespace)
				.ToArray();

			var bindingFactory = new BindingFactory(types);
			_bindingCollection = bindingFactory.CreateBindings();
		}

		public void RunBindingInitializer()
		{
			_bindingCollection.BindingInitializer
				?.Invoke(_bindingCollection.BindingInitializerObject, Array.Empty<object>());
		}
	}
}
