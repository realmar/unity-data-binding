using System;
using System.Runtime.Serialization;
using JetBrains.Annotations;
using Mono.Cecil;

namespace Realmar.DataBindings.Editor.Exceptions
{
	[Serializable]
	internal class AbstractConverterException : Exception
	{
		internal TypeReference ConverterType { get; }
		internal MethodDefinition BindingSymbol { get; }

		internal AbstractConverterException(TypeReference converterType, MethodDefinition bindingSymbol)
		{
			ConverterType = converterType;
			BindingSymbol = bindingSymbol;
		}

		protected AbstractConverterException([NotNull] SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
	}
}
