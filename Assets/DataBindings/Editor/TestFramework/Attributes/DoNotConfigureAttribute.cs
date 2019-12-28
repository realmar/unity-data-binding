using System;

namespace Realmar.DataBindings.Editor.TestFramework.Attributes
{
	[AttributeUsage(AttributeTargets.All, Inherited = false, AllowMultiple = false)]
	public class DoNotConfigureAttribute : Attribute
	{
	}
}
