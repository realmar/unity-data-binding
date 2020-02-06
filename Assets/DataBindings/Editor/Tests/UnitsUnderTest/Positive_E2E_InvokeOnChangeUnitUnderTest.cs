using Realmar.DataBindings;
using Realmar.DataBindings.Editor.TestFramework.Attributes;

namespace UnitsUnderTest.Positive_E2E_InvokeOnChangeTests.NonAbstract_NoBindings
{
	[Source, CompileTimeType, RunTimeType]
	internal class Source
	{
		[Result, Id(1)] private string _result;

		[InvokeOnChange(nameof(OnChange)), Id(1)]
		public string Text { get; set; }

		private void OnChange(string result)
		{
			_result = result;
		}
	}
}

namespace UnitsUnderTest.Positive_E2E_InvokeOnChangeTests.NonAbstract_NoBindings_ParameterlessMethod
{
	[Source, CompileTimeType, RunTimeType]
	internal class Source
	{
		[Result(Expected = 42), Id(1)] private int _result;

		[InvokeOnChange(nameof(OnChange)), Id(1)]
		public string Text { get; set; }

		private void OnChange()
		{
			_result = 42;
		}
	}
}
