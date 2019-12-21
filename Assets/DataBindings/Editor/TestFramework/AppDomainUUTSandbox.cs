using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Realmar.DataBindings.Editor.TestFramework
{
	internal class AppDomainUUTSandbox : MarshalByRefObject, IUnitUnderTestSandbox
	{
		private Assembly _context;

		public IReadOnlyCollection<IBindingSet> BindingSets { get; private set; }

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
			BindingSets = bindingFactory.CreateBindings();
		}
	}
}
