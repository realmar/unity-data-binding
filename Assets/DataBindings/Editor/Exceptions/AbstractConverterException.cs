using System;
using System.Runtime.Serialization;
using JetBrains.Annotations;
using Mono.Cecil;

namespace Realmar.DataBindings.Editor.Exceptions
{
	[Serializable]
	internal class AbstractConverterException : Exception
	{
		internal TypeDefinition ConverterType { get; }
		internal MethodDefinition BindingSymbol { get; }

		internal AbstractConverterException(TypeDefinition converterType, MethodDefinition bindingSymbol)
		{
			ConverterType = converterType;
			BindingSymbol = bindingSymbol;
		}

		protected AbstractConverterException([NotNull] SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
	}
}
