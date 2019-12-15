using System;

namespace Realmar.DataBindings.Editor.TestFramework.Attributes
{
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface, AllowMultiple = false, Inherited = false)]
	public class CompileTimeTypeAttribute : Attribute
	{
	}
}
