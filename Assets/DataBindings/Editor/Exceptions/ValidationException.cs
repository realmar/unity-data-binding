using System;
using System.Runtime.Serialization;
using JetBrains.Annotations;

namespace Realmar.DataBindings.Editor.Exceptions
{
	internal class ValidationException : Exception
	{
		protected ValidationException([NotNull] SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}

		public ValidationException(string message) : base(message)
		{
		}
	}
}
