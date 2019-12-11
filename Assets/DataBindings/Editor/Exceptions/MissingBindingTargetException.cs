using System;

namespace Realmar.DataBindingsEditor.Exceptions
{
	public class MissingBindingTargetException : Exception
	{
		public string TypeName { get; }

		public MissingBindingTargetException(string typeName)
		{
			TypeName = typeName;
		}

		public override string ToString()
		{
			return $"{TypeName} is missing a BindingTarget" + "\n" + base.ToString();
		}
	}
}
