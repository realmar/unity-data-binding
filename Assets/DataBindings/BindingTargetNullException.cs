using System;
using System.Runtime.Serialization;
using JetBrains.Annotations;

namespace Realmar.DataBindings
{
	[Serializable]
	public class BindingTargetNullException : NullReferenceException
	{
		public BindingTargetNullException()
		{
		}

		public BindingTargetNullException(string message) : base(message)
		{
		}

		protected BindingTargetNullException([NotNull] SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
	}
}
