using Mono.Cecil;

namespace Realmar.DataBindings.Editor.Emitting
{
	internal readonly struct Converter
	{
		internal FieldDefinition ConverterField { get; }
		internal MethodReference ConvertMethod { get; }

		public Converter(FieldDefinition converterField, MethodReference convertMethod)
		{
			ConverterField = converterField;
			ConvertMethod = convertMethod;
		}
	}
}
