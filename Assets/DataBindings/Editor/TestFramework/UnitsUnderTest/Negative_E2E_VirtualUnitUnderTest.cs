using Realmar.DataBindings;
using Realmar.DataBindings.Editor.TestFramework.Attributes;

namespace UnitsUnderTest.Negative_E2E_VirtualTests.TwoWay_OverrideBindingInitializer_NoBaseCall
{
	[Source, CompileTimeType]
	internal class BaseSource
	{
		[BindingTarget, Id(1)] public Target BindingTarget { get; set; }
		[Binding(BindingType.TwoWay)] public virtual string Text { get; set; }

		[BindingInitializer]
		protected virtual void InitializeBindings()
		{
		}
	}

	[Source, RunTimeType]
	internal class Source : BaseSource
	{
		public override string Text { get; set; }

		protected override void InitializeBindings()
		{
		}
	}

	[Target, Id(1)]
	internal class Target
	{
		public string Text { get; set; }
	}
}
