using Realmar.DataBindings;
using Realmar.DataBindings.Editor.TestFramework.Attributes;

namespace UnitsUnderTest.Negative_E2E_NonVirtualTests.FromTarget_BindingInitializerNotCalled
{
	[Source, CompileTimeType, RunTimeType]
	internal class Source
	{
		[BindingTarget, Id(1), DoNotConfigure] public Target BindingTarget { get; set; }

		[Binding(BindingType.OneWayFromTarget, nullCheckBehavior: NullCheckBehavior.EnableNullCheck)]
		public string Text { get; set; }

		[BindingInitializer]
		public void InitializeBindings()
		{
		}
	}

	[Target, Id(1)]
	internal class Target
	{
		public string Text { get; set; }
	}
}

namespace UnitsUnderTest.Negative_E2E_NonVirtualTests.OneTime_Throw_TargetNull
{
	[Source, CompileTimeType, RunTimeType]
	internal class Source
	{
		[BindingTarget, Id(1), DoNotConfigure] public Target BindingTarget { get; set; }

		[Binding(BindingType.OneTime, nullCheckBehavior: NullCheckBehavior.DisableNullCheck)]
		public string Text { get; set; }

		[BindingInitializer]
		public void InitializeBindings()
		{
		}
	}

	[Target, Id(1)]
	internal class Target
	{
		public string Text { get; set; }
	}
}
