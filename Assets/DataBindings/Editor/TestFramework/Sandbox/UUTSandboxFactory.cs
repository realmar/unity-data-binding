using System;
using System.Collections.Generic;
using System.Reflection;
using Realmar.DataBindings.Editor.Shared.Extensions;

namespace Realmar.DataBindings.Editor.TestFramework.Sandbox
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
