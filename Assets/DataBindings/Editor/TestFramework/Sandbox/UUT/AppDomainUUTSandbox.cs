using System;
using System.IO;
using System.Linq;
using System.Reflection;
using Realmar.DataBindings.Editor.TestFramework.Sandbox.Factories;
using static Realmar.DataBindings.Editor.TestFramework.Sandbox.SandboxHelpers;

namespace Realmar.DataBindings.Editor.TestFramework.Sandbox.UUT
{
	internal class AppDomainUUTSandbox : MarshalByRefObject, IUnitUnderTestSandbox
	{
		private Assembly _context;

		public IReadOnlyBindingCollection BindingCollection { get; private set; }

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

			var bindingFactory = new BindingFactoryFacade(types);
			BindingCollection = bindingFactory.CreateBindings();
		}

		public T CreateObject<T>(params object[] ctorArgs)
		{
			if (typeof(MarshalByRefObject).IsAssignableFrom(typeof(T)) == false)
			{
				throw new ArgumentException(
					$"{typeof(T).Name} does not inherit from {nameof(MarshalByRefObject)} which is nonsensical, " +
					$"because the type will be serialized instead of passed by ref across the appdomain (ie. you will loose the sandbox context)");
			}

			return CreateInstance<T>(ctorArgs);
		}
	}
}
