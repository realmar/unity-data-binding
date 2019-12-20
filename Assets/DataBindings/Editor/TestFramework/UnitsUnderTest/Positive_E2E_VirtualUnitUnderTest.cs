using Realmar.DataBindings;
using Realmar.DataBindings.Editor.TestFramework.Attributes;

namespace UnitsUnderTest.Positive_E2E_VirtualTests.OneWay_VirtualToProperty
{
	[Source, CompileTimeType, RunTimeType]
	internal class Source
	{
		[BindingTarget, Id(1)] public Target BindingTarget { get; set; }
		[Binding] public virtual string Text { get; set; }
	}

	[Target, Id(1)]
	internal class Target
	{
		public string Text { get; set; }
	}
}

namespace UnitsUnderTest.Positive_E2E_VirtualTests.OneWay_VirtualToVirtual
{
	[Source, CompileTimeType, RunTimeType]
	internal class Source
	{
		[BindingTarget, Id(1)] public Target BindingTarget { get; set; }
		[Binding] public virtual string Text { get; set; }
	}

	[Target, Id(1)]
	internal class Target
	{
		public virtual string Text { get; set; }
	}
}

namespace UnitsUnderTest.Positive_E2E_VirtualTests.OneWay_VirtualToOverride
{
	[Source, CompileTimeType, RunTimeType]
	internal class Source
	{
		[BindingTarget, Id(1)] public BaseTarget BindingTarget { get; set; }
		[Binding] public virtual string Text { get; set; }
	}


	internal class BaseTarget
	{
		public virtual string Text { get; set; }
	}

	[Target, Id(1)]
	internal class Target : BaseTarget
	{
		public override string Text { get; set; }
	}
}
