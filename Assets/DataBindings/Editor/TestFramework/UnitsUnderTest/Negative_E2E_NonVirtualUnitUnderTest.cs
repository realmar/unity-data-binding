using Realmar.DataBindings;
using Realmar.DataBindings.Editor.TestFramework.Attributes;

namespace UnitsUnderTest.Negative_E2E_NonVirtualTests.FromTarget_BindingInitializerNotCalled
{
	[Source, CompileTimeType, RunTimeType]
	internal class Source
	{
		[BindingTarget, Id(1), DoNotConfigure] public Target BindingTarget { get; set; }

		[Binding(BindingType.OneWayFromTarget, emitNullCheck: true)]
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
