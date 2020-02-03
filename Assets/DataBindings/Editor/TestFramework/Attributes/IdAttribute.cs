using System;

namespace Realmar.DataBindings.Editor.TestFramework.Attributes
{
	[AttributeUsage(
		AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Class | AttributeTargets.Interface,
		AllowMultiple = true,
		Inherited = false)]
	public class IdAttribute : Attribute, IEquatable<IdAttribute>
	{
		public int Id { get; set; }

		public IdAttribute(int id)
		{
			Id = id;
		}

		public bool Equals(IdAttribute other)
		{
			if (ReferenceEquals(null, other)) return false;
			if (ReferenceEquals(this, other)) return true;
			return Id == other.Id;
		}

		public override bool Equals(object obj)
		{
			if (ReferenceEquals(null, obj)) return false;
			if (ReferenceEquals(this, obj)) return true;
			if (obj.GetType() != GetType()) return false;
			return Equals((IdAttribute) obj);
		}

		public override int GetHashCode()
		{
			unchecked
			{
				return (base.GetHashCode() * 397) ^ Id;
			}
		}
	}
}
