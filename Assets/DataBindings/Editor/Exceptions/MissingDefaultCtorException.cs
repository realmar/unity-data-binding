using System;
using System.Runtime.Serialization;
using JetBrains.Annotations;

namespace Realmar.DataBindings.Editor.Exceptions
{
	[Serializable]
	internal class MissingDefaultCtorException : Exception
	{
		internal Type Type { get; }

		internal MissingDefaultCtorException(Type type)
		{
			Type = type;
		}

		protected MissingDefaultCtorException([NotNull] SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
	}
}
