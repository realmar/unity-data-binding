using System;
using System.Runtime.Serialization;
using JetBrains.Annotations;
using Mono.Cecil;

namespace Realmar.DataBindings.Editor.Exceptions
{
	[Serializable]
	internal class NotAConverterException : Exception
	{
		public TypeReference ConverterType { get; }
		public MethodDefinition BindingSymbol { get; }

		public NotAConverterException(TypeReference converterType, MethodDefinition bindingSymbol)
		{
			ConverterType = converterType;
			BindingSymbol = bindingSymbol;
		}

		protected NotAConverterException([NotNull] SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
	}
}
