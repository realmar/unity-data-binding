using System;
using System.Runtime.Serialization;
using JetBrains.Annotations;

namespace Realmar.DataBindings.Editor.Exceptions
{
	internal class FatalException : Exception
	{
		internal FatalException()
		{
		}

		protected FatalException([NotNull] SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}

		internal FatalException(string message) : base(message)
		{
		}

		internal FatalException(string message, Exception innerException) : base(message, innerException)
		{
		}
	}
}
