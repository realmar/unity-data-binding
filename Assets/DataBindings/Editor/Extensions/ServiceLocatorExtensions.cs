using System.Collections.Generic;
using Mono.Cecil;
using Realmar.DataBindings.Editor.Utils;

namespace Realmar.DataBindings.Editor.Extensions
{
	internal static class ServiceLocatorExtensions
	{
		internal static void RegisterWovenSetHelpers(this ServiceLocator locator) => locator.RegisterType<HashSet<MethodDefinition>>(ServiceLifetime.Singleton, "WovenSetHelpers");
		internal static HashSet<MethodDefinition> GetWovenSetHelpers(this ServiceLocator locator) => locator.Resolve<HashSet<MethodDefinition>>("WovenSetHelpers");

		internal static void RegisterWovenBindings(this ServiceLocator locator) => locator.RegisterType<HashSet<int>>(ServiceLifetime.Singleton, "WovenBindings");
		internal static HashSet<int> GetWovenBindings(this ServiceLocator locator) => locator.Resolve<HashSet<int>>("WovenBindings");
	}
}
