using Realmar.DataBindings;
using Realmar.DataBindings.Editor.TestFramework.Attributes;

namespace UnitsUnderTest.Positive_E2E_InvokeOnChangeTests.NonAbstract_NoBindings
{
	[Source, CompileTimeType, RunTimeType]
	internal class Source
	{
		private string _result;

		[InvokeOnChange(nameof(OnChange))]
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
		private int _result;

		[InvokeOnChange(nameof(OnChange))]
		public string Text { get; set; }

		private void OnChange()
		{
			_result = 42;
		}
	}
}
