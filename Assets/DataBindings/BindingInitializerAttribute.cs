using System;

namespace Realmar.DataBindings
{
	[Serializable]
	[AttributeUsage(AttributeTargets.Method | AttributeTargets.Constructor, AllowMultiple = false, Inherited = false)]
	public class BindingInitializerAttribute : Attribute
	{
	}
}
