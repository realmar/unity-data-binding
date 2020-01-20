using Realmar.DataBindings;
using Realmar.DataBindings.Converters;
using Realmar.DataBindings.Editor.TestFramework.Attributes;

namespace UnitsUnderTest.Positive_E2E_ConverterTests.OneWay_IntToString
{
	[Source, CompileTimeType, RunTimeType]
	internal class Source
	{
		[BindingTarget, Id(1)] public Target BindingTarget { get; set; }

		[Binding(converter: typeof(StringToIntConverter))]
		public string Text { get; set; }
	}

	[Target, Id(1)]
	internal class Target
	{
		public int Text { get; set; }
	}
}

namespace UnitsUnderTest.Positive_E2E_ConverterTests.TwoWay_IntToString
{
	[Source, CompileTimeType, RunTimeType]
	internal class Source
	{
		[BindingTarget, Id(1)] public Target BindingTarget { get; set; }

		[Binding(BindingType.TwoWay, converter: typeof(StringToIntConverter))]
		public string Text { get; set; }

		[BindingInitializer]
		public void InitializeBindings()
		{
		}
	}

	[Target, Id(1)]
	internal class Target
	{
		public int Text { get; set; }
	}
}

namespace UnitsUnderTest.Positive_E2E_ConverterTests.TwoWay_GenericConverter
{
	[Source, CompileTimeType, RunTimeType]
	internal class Source
	{
		[BindingTarget, Id(1)] public Target BindingTarget { get; set; }

		[Binding(BindingType.TwoWay, converter: typeof(CastConverter<double, int>))]
		public double Text { get; set; }

		[BindingInitializer]
		public void InitializeBindings()
		{
		}
	}

	[Target, Id(1)]
	internal class Target
	{
		public int Text { get; set; }
	}
}
