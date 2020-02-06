using System;
using System.Runtime.Serialization;
using JetBrains.Annotations;
using Mono.Cecil;

namespace Realmar.DataBindings.Editor.Exceptions
{
	[Serializable]
	internal class OpenGenericConverterNotSupported : Exception
	{
		internal TypeReference ConverterType { get; }
		internal MethodDefinition BindingSymbol { get; }

		public OpenGenericConverterNotSupported(TypeReference converterType, MethodDefinition bindingSymbol)
		{
			ConverterType = converterType;
			BindingSymbol = bindingSymbol;
		}

		protected OpenGenericConverterNotSupported([NotNull] SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
	}
}
