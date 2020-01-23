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

namespace UnitsUnderTest.Negative_E2E_NonVirtualTests.OneTime_Throw_TargetNull_Using_Cgt_Un
{
	[Source, CompileTimeType, RunTimeType]
	internal class Source
	{
		[BindingTarget, Id(1), DoNotConfigure] public Target BindingTarget { get; set; }

		[Binding(BindingType.OneTime)]
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

namespace UnitsUnderTest.Negative_E2E_NonVirtualTests.OneWay_Throw_TargetNull_Using_Cgt_Un
{
	[Source, CompileTimeType, RunTimeType]
	internal class Source
	{
		[BindingTarget, Id(1), DoNotConfigure] public Target BindingTarget { get; set; }

		[Binding(BindingType.OneWay, nullCheckBehavior: NullCheckBehavior.DisableNullCheck)]
		public string Text { get; set; }
	}

	[Target, Id(1)]
	internal class Target
	{
		public string Text { get; set; }
	}
}


namespace UnitsUnderTest.Negative_E2E_NonVirtualTests.OneWay_Throw_TargetNull_Using_op_Equality
{
	[Source, CompileTimeType, RunTimeType]
	internal class Source
	{
		[BindingTarget, Id(1), DoNotConfigure] public Target BindingTarget { get; set; }

		[Binding(BindingType.OneWay, nullCheckBehavior: NullCheckBehavior.DisableNullCheck)]
		public string Text { get; set; }
	}

	[Target, Id(1)]
	internal class Target
	{
		public string Text { get; set; }

		public static bool operator ==(Target a, Target b)
		{
			return ReferenceEquals(a, b);
		}

		public static bool operator !=(Target a, Target b)
		{
			return !(a == b);
		}
	}
}

namespace UnitsUnderTest.Negative_E2E_NonVirtualTests.FromTarget_Throw_TargetNull_Using_op_Equality
{
	[Source, CompileTimeType, RunTimeType]
	internal class Source
	{
		[BindingTarget, Id(1), DoNotConfigure] public Target BindingTarget { get; set; }

		[Binding(BindingType.OneWayFromTarget, nullCheckBehavior: NullCheckBehavior.DisableNullCheck)]
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

		public static bool operator ==(Target a, Target b)
		{
			return ReferenceEquals(a, b);
		}

		public static bool operator !=(Target a, Target b)
		{
			return !(a == b);
		}
	}
}

namespace UnitsUnderTest.Negative_E2E_NonVirtualTests.FromTarget_Throw_TargetNull_Using_Cgt_Un
{
	[Source, CompileTimeType, RunTimeType]
	internal class Source
	{
		[BindingTarget, Id(1), DoNotConfigure] public Target BindingTarget { get; set; }

		[Binding(BindingType.OneWayFromTarget, nullCheckBehavior: NullCheckBehavior.DisableNullCheck)]
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
