using System;
using System.Collections.Generic;
using System.Reflection;
using Realmar.DataBindings.Editor.Shared.Extensions;
using Realmar.DataBindings.Editor.TestFramework.Sandbox.UUT;

namespace Realmar.DataBindings.Editor.TestFramework.Sandbox.Factories
{
	internal class UUTSandboxFactory : IDisposable
	{
		private readonly Random _random = new Random();
		private readonly List<AppDomain> _sandboxDomains = new List<AppDomain>();

		internal IUnitUnderTestSandbox CreateSandbox()
		{
			var uutDomain = AppDomain.CreateDomain($"UUT_{_random.Next()}");
			_sandboxDomains.Add(uutDomain);

			uutDomain.Load(Assembly.GetExecutingAssembly().GetName());

			return uutDomain.CreateInstanceFromDeclaringAssemblyOfTypeAndUnwrap<AppDomainUUTSandbox>();
		}

		public void Dispose()
		{
			foreach (var domain in _sandboxDomains)
			{
				AppDomain.Unload(domain);
			}
		}
	}
}
