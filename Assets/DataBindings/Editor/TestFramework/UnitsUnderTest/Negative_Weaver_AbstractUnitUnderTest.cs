using Realmar.DataBindings;

namespace UnitsUnderTest.Negative_Weaver_AbstractTests.OneWay_NoNonAbstractSource
{
	internal abstract class AbstractSource
	{
		[BindingTarget] public Target BindingTarget { get; set; }
		[Binding] public abstract string Text { get; set; }
	}

	internal class Target
	{
		public string Text { get; set; }
	}
}

namespace UnitsUnderTest.Negative_Weaver_AbstractTests.FromTarget_NoNonAbstractTarget
{
	internal class AbstractSource
	{
		[BindingTarget] public AbstractTarget BindingTarget { get; set; }

		[Binding(BindingType.OneWayFromTarget)]
		public string Text { get; set; }

		[BindingInitializer]
		public void InitializeBindings()
		{
		}
	}

	internal abstract class AbstractTarget
	{
		public abstract string Text { get; set; }
	}
}

namespace UnitsUnderTest.Negative_Weaver_AbstractTests.OneWay_NoNonAbstractSourceAndTarget
{
	internal abstract class AbstractSource
	{
		[BindingTarget] public AbstractTarget BindingTarget { get; set; }
		[Binding] public abstract string Text { get; set; }
	}

	internal abstract class AbstractTarget
	{
		public abstract string Text { get; set; }
	}
}

namespace UnitsUnderTest.Negative_Weaver_AbstractTests.TwoWay_NoNonAbstractSourceAndTarget
{
	internal abstract class AbstractSource
	{
		[BindingTarget] public AbstractTarget BindingTarget { get; set; }

		[Binding(BindingType.TwoWay)]
		public abstract string Text { get; set; }

		[BindingInitializer]
		public void InitializeBindings()
		{
		}
	}

	internal abstract class AbstractTarget
	{
		public abstract string Text { get; set; }
	}
}

namespace UnitsUnderTest.Negative_Weaver_AbstractTests.TwoWay_NoNonAbstractTarget
{
	internal class AbstractSource
	{
		[BindingTarget] public AbstractTarget BindingTarget { get; set; }

		[Binding(BindingType.TwoWay)]
		public string Text { get; set; }

		[BindingInitializer]
		public void InitializeBindings()
		{
		}
	}

	internal abstract class AbstractTarget
	{
		public abstract string Text { get; set; }
	}
}

namespace UnitsUnderTest.Negative_Weaver_AbstractTests.TwoWay_NoNonAbstractBindingInitializer
{
	internal abstract class AbstractSource
	{
		[BindingTarget] public Target BindingTarget { get; set; }

		[Binding(BindingType.TwoWay)]
		public string Text { get; set; }

		[BindingInitializer]
		public abstract void InitializeBindings();
	}

	internal class Target
	{
		public string Text { get; set; }
	}
}
