using System;

namespace Realmar.DataBindings.Editor.Extensions
{
	internal static class AppDomainExtensions
	{
		internal static T CreateInstanceFromDeclaringAssemblyOfTypeAndUnwrap<T>(this AppDomain domain)
			where T : MarshalByRefObject
		{
			var type = typeof(T);
			return (T) domain.CreateInstanceAndUnwrap(type.Assembly.FullName, type.FullName);
		}
	}
}
