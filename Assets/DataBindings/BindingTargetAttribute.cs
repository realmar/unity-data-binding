using System;

namespace Realmar.DataBindings
{
	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = true, Inherited = true)]
	public class BindingTargetAttribute : Attribute
	{
		public int Id { get; }

		public BindingTargetAttribute(int id = 0)
		{
			Id = id;
		}
	}
}
