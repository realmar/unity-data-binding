using System;

namespace Realmar.DataBindings
{
	[AttributeUsage(AttributeTargets.Method | AttributeTargets.Constructor, AllowMultiple = false, Inherited = false)]
	public class BindingInitializerAttribute : Attribute
	{
	}
}
