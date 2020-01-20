using System;
using System.Runtime.Serialization;
using JetBrains.Annotations;
using Mono.Cecil;

namespace Realmar.DataBindings.Editor.Exceptions
{
	[Serializable]
	internal class MismatchingConverterTypesException : Exception
	{
		public TypeReference ConverterType { get; }
		public MethodDefinition BindingSymbol { get; }

		public MismatchingConverterTypesException(TypeReference converterType, MethodDefinition bindingSymbol)
		{
			ConverterType = converterType;
			BindingSymbol = bindingSymbol;
		}

		protected MismatchingConverterTypesException([NotNull] SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
	}
}
