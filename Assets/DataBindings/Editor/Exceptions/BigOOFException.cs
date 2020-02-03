using System;
using System.Runtime.Serialization;
using JetBrains.Annotations;

namespace Realmar.DataBindings.Editor.Exceptions
{
	internal class BigOOFException : Exception
	{
		public override string Message => $"{base.Message}\nThis should not happen. It indicates a logic error in the framework.";

		internal BigOOFException()
		{
		}

		protected BigOOFException([NotNull] SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}

		internal BigOOFException(string message) : base(message)
		{
		}

		internal BigOOFException(string message, Exception innerException) : base(message, innerException)
		{
		}
	}
}
