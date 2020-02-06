using Realmar.DataBindings;
using Realmar.DataBindings.Converters;
using Realmar.DataBindings.Editor.TestFramework.Attributes;

namespace UnitsUnderTest.Negative_Weaver_ConverterTests.ConverterType_Interface
{
	internal interface IConverter : IValueConverter<float, int>
	{
	}

	[Source, CompileTimeType, RunTimeType]
	internal class Source
	{
		[BindingTarget, Id(1)] public Target BindingTarget { get; set; }

		[Binding(converter: typeof(IConverter))]
		public string Text { get; set; }
	}

	[Target, Id(1)]
	internal class Target
	{
		public int Text { get; set; }
	}
}

namespace UnitsUnderTest.Negative_Weaver_ConverterTests.ConverterType_Abstract
{
	internal abstract class Converter : IValueConverter<float, int>
	{
		public abstract float Convert(int to);
		public abstract int Convert(float @from);
	}

	[Source, CompileTimeType, RunTimeType]
	internal class Source
	{
		[BindingTarget, Id(1)] public Target BindingTarget { get; set; }

		[Binding(converter: typeof(Converter))]
		public string Text { get; set; }
	}

	[Target, Id(1)]
	internal class Target
	{
		public int Text { get; set; }
	}
}

namespace UnitsUnderTest.Negative_Weaver_ConverterTests.IsNotAConverter
{
	internal class Converter
	{
	}


	[Source, CompileTimeType, RunTimeType]
	internal class Source
	{
		[BindingTarget, Id(1)] public Target BindingTarget { get; set; }

		[Binding(converter: typeof(Converter))]
		public string Text { get; set; }
	}

	[Target, Id(1)]
	internal class Target
	{
		public int Text { get; set; }
	}
}

namespace UnitsUnderTest.Negative_Weaver_ConverterTests.NoDefaultCtor
{
	internal class Converter : IValueConverter<float, int>
	{
		public Converter(int a, int b)
		{
		}

		public float Convert(int to)
		{
			return 0f;
		}

		public int Convert(float @from)
		{
			return 0;
		}
	}


	[Source, CompileTimeType, RunTimeType]
	internal class Source
	{
		[BindingTarget, Id(1)] public Target BindingTarget { get; set; }

		[Binding(converter: typeof(Converter))]
		public string Text { get; set; }
	}

	[Target, Id(1)]
	internal class Target
	{
		public int Text { get; set; }
	}
}

namespace UnitsUnderTest.Negative_Weaver_ConverterTests.MismatchingConverterTypes
{
	internal class Converter : IValueConverter<float, int>
	{
		public float Convert(int to)
		{
			return 0f;
		}

		public int Convert(float @from)
		{
			return 0;
		}
	}


	[Source, CompileTimeType, RunTimeType]
	internal class Source
	{
		[BindingTarget, Id(1)] public Target BindingTarget { get; set; }

		[Binding(converter: typeof(Converter))]
		public string Text { get; set; }
	}

	[Target, Id(1)]
	internal class Target
	{
		public int Text { get; set; }
	}
}

namespace UnitsUnderTest.Negative_Weaver_ConverterTests.OpenGenericConverter
{
	public class Converter<TFrom, TTo> : IValueConverter<TFrom, TTo>
	{
		public TFrom Convert(TTo to)
		{
			throw new System.NotImplementedException();
		}

		public TTo Convert(TFrom from)
		{
			throw new System.NotImplementedException();
		}
	}

	[Source, CompileTimeType, RunTimeType]
	internal class Source
	{
		[BindingTarget, Id(1)] public Target BindingTarget { get; set; }

		[Binding(converter: typeof(Converter<,>))]
		public string Text { get; set; }
	}

	[Target, Id(1)]
	internal class Target
	{
		public int Text { get; set; }
	}
}
