using JetBrains.Annotations;
using System;
using System.Runtime.Serialization;

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
