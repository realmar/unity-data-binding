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

namespace UnitsUnderTest.Positive_E2E_InvokeOnChangeTests.NonAbstract_WithBindings
{
	[Source, CompileTimeType, RunTimeType]
	internal class Source
	{
		[Result, Id(1)] private string _result;

		[BindingTarget, Id(1)]
		public Target BT { get; set; }

		[InvokeOnChange(nameof(OnChange)), Id(1)]
		[Binding]
		public string Text { get; set; }

		private void OnChange(string result)
		{
			_result = result;
		}
	}

	[Target, Id(1)]
	internal class Target
	{
		public string Text { get; set; }
	}
}

namespace UnitsUnderTest.Positive_E2E_InvokeOnChangeTests.Virtual_NoBindings
{
	[Source, CompileTimeType, RunTimeType]
	internal class Source
	{
		[Result, Id(1)] private string _result;

		[InvokeOnChange(nameof(OnChange)), Id(1)]
		public string Text { get; set; }

		public virtual void OnChange(string result)
		{
			_result = result;
		}
	}
}

namespace UnitsUnderTest.Positive_E2E_InvokeOnChangeTests.Override_NoBindings
{
	[Source, CompileTimeType]
	internal class Source
	{
		[Result(Expected = "42"), Id(1)] private string _result;

		[InvokeOnChange(nameof(OnChange)), Id(1)]
		public string Text { get; set; }

		public virtual void OnChange(string result)
		{
			_result = result;
		}
	}

	[Source, RunTimeType]
	internal class DerivedSource : Source
	{
		public override void OnChange(string result)
		{
			base.OnChange("42");
		}
	}
}

namespace UnitsUnderTest.Positive_E2E_InvokeOnChangeTests.Abstract_NoBindings
{
	[Source, CompileTimeType]
	internal abstract class Source
	{
		[Result, Id(1)] protected string Result;

		[InvokeOnChange(nameof(OnChange)), Id(1)]
		public string Text { get; set; }

		public abstract void OnChange(string result);
	}

	[Source, RunTimeType]
	internal class DerivedSource : Source
	{
		public override void OnChange(string result)
		{
			Result = result;
		}
	}
}

namespace UnitsUnderTest.Positive_E2E_InvokeOnChangeTests.Interface_NoBindings
{
	[Source, CompileTimeType]
	internal interface ISource
	{
		[Result, Id(1)] string Result { get; set; }

		[InvokeOnChange(nameof(OnChange)), Id(1)]
		string Text { get; set; }

		void OnChange(string result);
	}

	[Source, RunTimeType]
	internal class DerivedSource : ISource
	{
		public string Result { get; set; }
		public string Text { get; set; }

		public void OnChange(string result)
		{
			Result = result;
		}
	}
}

namespace UnitsUnderTest.Positive_E2E_InvokeOnChangeTests.InterfaceAbstract_NoBindings
{
	[Source, CompileTimeType]
	internal interface ISource
	{
		[Result, Id(1)] string Result { get; set; }

		[InvokeOnChange(nameof(OnChange)), Id(1)]
		string Text { get; set; }

		void OnChange(string result);
	}

	internal abstract class AbstractSource : ISource
	{
		public string Result { get; set; }
		public string Text { get; set; }
		public abstract void OnChange(string result);
	}

	[Source, RunTimeType]
	internal class DerivedSource : AbstractSource
	{
		public override void OnChange(string result)
		{
			Result = result;
		}
	}
}

namespace UnitsUnderTest.Positive_E2E_InvokeOnChangeTests.InterfaceInterfaceAbstract_NoBindings
{
	[Source, CompileTimeType]
	internal interface ISource
	{
		[Result, Id(1)] string Result { get; set; }

		[InvokeOnChange(nameof(OnChange)), Id(1)]
		string Text { get; set; }

		void OnChange(string result);
	}

	internal interface ISource2 : ISource
	{
		string Example { get; set; }
	}

	internal abstract class AbstractSource : ISource2
	{
		public string Result { get; set; }
		public string Text { get; set; }
		public abstract string Example { get; set; }
		public abstract void OnChange(string result);
	}

	[Source, RunTimeType]
	internal class DerivedSource : AbstractSource
	{
		public override string Example { get; set; }

		public override void OnChange(string result)
		{
			Result = result;
		}
	}
}
