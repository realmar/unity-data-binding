using System;
using System.Runtime.Serialization;
using JetBrains.Annotations;

namespace Realmar.DataBindings.Editor.Exceptions
{
	internal class BigOOFException : Exception
	{
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
