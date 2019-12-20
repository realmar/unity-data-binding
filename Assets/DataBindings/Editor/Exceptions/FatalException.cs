using System;
using System.Runtime.Serialization;
using JetBrains.Annotations;

namespace Realmar.DataBindings.Editor.Exceptions
{
	internal class FatalException : Exception
	{
		public FatalException()
		{
		}

		protected FatalException([NotNull] SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}

		public FatalException(string message) : base(message)
		{
		}

		public FatalException(string message, Exception innerException) : base(message, innerException)
		{
		}
	}
}
