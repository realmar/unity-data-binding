using Realmar.DataBindings;
using Realmar.DataBindings.Editor.TestFramework.Attributes;

namespace UnitsUnderTest.Positive_E2E_AbstractTests.OneWay_AbstractToProperty
{
	[Source, CompileTimeType]
	internal abstract class BaseSource
	{
		[BindingTarget, Id(1)] public Target BindingTarget { get; set; }
		[Binding] public abstract string Text { get; set; }
	}

	[Source, RunTimeType]
	internal class Source : BaseSource
	{
		public override string Text { get; set; }
	}

	[Target, Id(1)]
	internal class Target
	{
		public string Text { get; set; }
	}
}

namespace UnitsUnderTest.Positive_E2E_AbstractTests.OneWay_PropertyToAbstract
{
	[Source, CompileTimeType, RunTimeType]
	internal class BaseSource
	{
		[BindingTarget, Id(1)] public BaseTarget BindingTarget { get; set; }
		[Binding] public string Text { get; set; }
	}

	internal abstract class BaseTarget
	{
		public abstract string Text { get; set; }
	}

	[Target, Id(1)]
	internal class Target : BaseTarget
	{
		public override string Text { get; set; }
	}
}

namespace UnitsUnderTest.Positive_E2E_AbstractTests.OneWay_AbstractToAbstract
{
	[Source, CompileTimeType]
	internal abstract class BaseSource
	{
		[BindingTarget, Id(1)] public BaseTarget BindingTarget { get; set; }
		[Binding] public abstract string Text { get; set; }
	}

	[Source, RunTimeType]
	internal class Source : BaseSource
	{
		public override string Text { get; set; }
	}

	internal abstract class BaseTarget
	{
		public abstract string Text { get; set; }
	}

	[Target, Id(1)]
	internal class Target : BaseTarget
	{
		public override string Text { get; set; }
	}
}

namespace UnitsUnderTest.Positive_E2E_AbstractTests.FromTarget_AbstractToProperty
{
	[Source, CompileTimeType]
	internal abstract class BaseSource
	{
		[BindingTarget, Id(1)] public Target BindingTarget { get; set; }

		[Binding(BindingType.OneWayFromTarget)]
		public abstract string Text { get; set; }

		[BindingInitializer]
		public void InitializeBindings()
		{
		}
	}

	[Source, RunTimeType]
	internal class Source : BaseSource
	{
		public override string Text { get; set; }
	}

	[Target, Id(1)]
	internal class Target
	{
		public string Text { get; set; }
	}
}

namespace UnitsUnderTest.Positive_E2E_AbstractTests.FromTarget_PropertyToAbstract
{
	[Source, CompileTimeType, RunTimeType]
	internal class BaseSource
	{
		[BindingTarget, Id(1)] public BaseTarget BindingTarget { get; set; }

		[Binding(BindingType.OneWayFromTarget)]
		public string Text { get; set; }

		[BindingInitializer]
		public void InitializeBindings()
		{
		}
	}

	internal abstract class BaseTarget
	{
		public abstract string Text { get; set; }
	}

	[Target, Id(1)]
	internal class Target : BaseTarget
	{
		public override string Text { get; set; }
	}
}

namespace UnitsUnderTest.Positive_E2E_AbstractTests.FromTarget_AbstractToAbstract
{
	[Source, CompileTimeType]
	internal abstract class BaseSource
	{
		[BindingTarget, Id(1)] public BaseTarget BindingTarget { get; set; }

		[Binding(BindingType.OneWayFromTarget)]
		public abstract string Text { get; set; }

		[BindingInitializer]
		public void InitializeBindings()
		{
		}
	}

	[Source, RunTimeType]
	internal class Source : BaseSource
	{
		public override string Text { get; set; }
	}

	internal abstract class BaseTarget
	{
		public abstract string Text { get; set; }
	}

	[Target, Id(1)]
	internal class Target : BaseTarget
	{
		public override string Text { get; set; }
	}
}

namespace UnitsUnderTest.Positive_E2E_AbstractTests.FromTarget_AbstractToAbstract_AbstractBindingInitializer
{
	[Source, CompileTimeType]
	internal abstract class BaseSource
	{
		[BindingTarget, Id(1)] public BaseTarget BindingTarget { get; set; }

		[Binding(BindingType.OneWayFromTarget)]
		public abstract string Text { get; set; }

		[BindingInitializer]
		public abstract void InitializeBindings();
	}

	[Source, RunTimeType]
	internal class Source : BaseSource
	{
		public override string Text { get; set; }

		public override void InitializeBindings()
		{
		}
	}

	internal abstract class BaseTarget
	{
		public abstract string Text { get; set; }
	}

	[Target, Id(1)]
	internal class Target : BaseTarget
	{
		public override string Text { get; set; }
	}
}

namespace UnitsUnderTest.Positive_E2E_AbstractTests.TwoWay_AbstractToProperty
{
	[Source, CompileTimeType]
	internal abstract class BaseSource
	{
		[BindingTarget, Id(1)] public Target BindingTarget { get; set; }
		[Binding(BindingType.TwoWay)] public abstract string Text { get; set; }

		[BindingInitializer]
		public void InitializeBindings()
		{
		}
	}

	[Source, RunTimeType]
	internal class Source : BaseSource
	{
		public override string Text { get; set; }
	}

	[Target, Id(1)]
	internal class Target
	{
		public string Text { get; set; }
	}
}

namespace UnitsUnderTest.Positive_E2E_AbstractTests.TwoWay_PropertyToAbstract
{
	[Source, CompileTimeType, RunTimeType]
	internal class BaseSource
	{
		[BindingTarget, Id(1)] public BaseTarget BindingTarget { get; set; }
		[Binding(BindingType.TwoWay)] public string Text { get; set; }

		[BindingInitializer]
		public void InitializeBindings()
		{
		}
	}

	internal abstract class BaseTarget
	{
		public abstract string Text { get; set; }
	}

	[Target, Id(1)]
	internal class Target : BaseTarget
	{
		public override string Text { get; set; }
	}
}

namespace UnitsUnderTest.Positive_E2E_AbstractTests.TwoWay_AbstractToAbstract
{
	[Source, CompileTimeType]
	internal abstract class BaseSource
	{
		[BindingTarget, Id(1)] public BaseTarget BindingTarget { get; set; }
		[Binding(BindingType.TwoWay)] public abstract string Text { get; set; }

		[BindingInitializer]
		public void InitializeBindings()
		{
		}
	}

	[Source, RunTimeType]
	internal class Source : BaseSource
	{
		public override string Text { get; set; }
	}

	internal abstract class BaseTarget
	{
		public abstract string Text { get; set; }
	}

	[Target, Id(1)]
	internal class Target : BaseTarget
	{
		public override string Text { get; set; }
	}
}

namespace UnitsUnderTest.Positive_E2E_AbstractTests.OneWay_AbstractToAbstract_MultipleOverrides
{
	[Source, CompileTimeType]
	internal abstract class BaseSource
	{
		[BindingTarget, Id(1)] public BaseTarget BindingTarget { get; set; }
		[Binding] public abstract string Text { get; set; }
	}

	internal class Source : BaseSource
	{
		public override string Text { get; set; }
	}

	[Source, RunTimeType]
	internal class DerivedSource : Source
	{
		public override string Text { get; set; }
	}

	internal abstract class BaseTarget
	{
		public abstract string Text { get; set; }
	}

	internal class Target : BaseTarget
	{
		public override string Text { get; set; }
	}

	[Target, Id(1)]
	internal class DerivedTarget : Target
	{
		public override string Text { get; set; }
	}
}

namespace UnitsUnderTest.Positive_E2E_AbstractTests.FromTarget_AbstractToAbstract_MultipleOverrides
{
	[Source, CompileTimeType]
	internal abstract class BaseSource
	{
		[BindingTarget, Id(1)] public BaseTarget BindingTarget { get; set; }

		[Binding(BindingType.OneWayFromTarget)]
		public abstract string Text { get; set; }

		[BindingInitializer]
		public void InitializeBindings()
		{
		}
	}

	internal class Source : BaseSource
	{
		public override string Text { get; set; }
	}

	[Source, RunTimeType]
	internal class DerivedSource : Source
	{
		public override string Text { get; set; }
	}

	internal abstract class BaseTarget
	{
		public abstract string Text { get; set; }
	}

	internal class Target : BaseTarget
	{
		public override string Text { get; set; }
	}

	[Target, Id(1)]
	internal class DerivedTarget : Target
	{
		public override string Text { get; set; }
	}
}

namespace UnitsUnderTest.Positive_E2E_AbstractTests.TwoWay_AbstractToAbstract_MultipleOverrides
{
	[Source, CompileTimeType]
	internal abstract class BaseSource
	{
		[BindingTarget, Id(1)] public BaseTarget BindingTarget { get; set; }
		[Binding(BindingType.TwoWay)] public abstract string Text { get; set; }

		[BindingInitializer]
		public void InitializeBindings()
		{
		}
	}

	internal class Source : BaseSource
	{
		public override string Text { get; set; }
	}

	[Source, RunTimeType]
	internal class DerivedSource : Source
	{
		public override string Text { get; set; }
	}

	internal abstract class BaseTarget
	{
		public abstract string Text { get; set; }
	}

	internal class Target : BaseTarget
	{
		public override string Text { get; set; }
	}

	[Target, Id(1)]
	internal class DerivedTarget : Target
	{
		public override string Text { get; set; }
	}
}

namespace UnitsUnderTest.Positive_E2E_AbstractTests.TwoWay_AbstractToAbstract_AbstractBindingInitializer_MultipleOverrides
{
	[Source, CompileTimeType]
	internal abstract class BaseSource
	{
		[BindingTarget, Id(1)] public BaseTarget BindingTarget { get; set; }
		[Binding(BindingType.TwoWay)] public abstract string Text { get; set; }

		[BindingInitializer]
		public abstract void InitializeBindings();
	}

	internal class Source : BaseSource
	{
		public override string Text { get; set; }

		public override void InitializeBindings()
		{
		}
	}

	[Source, RunTimeType]
	internal class DerivedSource : Source
	{
		public override string Text { get; set; }

		public override void InitializeBindings()
		{
			base.InitializeBindings();
		}
	}

	internal abstract class BaseTarget
	{
		public abstract string Text { get; set; }
	}

	internal class Target : BaseTarget
	{
		public override string Text { get; set; }
	}

	[Target, Id(1)]
	internal class DerivedTarget : Target
	{
		public override string Text { get; set; }
	}
}

namespace UnitsUnderTest.Positive_E2E_AbstractTests.OneWay_AbstractToAbstract_MultipleAbstracts
{
	[Source, CompileTimeType]
	internal abstract class BaseSource
	{
		[BindingTarget, Id(1)] public BaseTarget BindingTarget { get; set; }
		[Binding] public abstract string Text { get; set; }
	}

	internal abstract class Source : BaseSource
	{
	}

	[Source, RunTimeType]
	internal class DerivedSource : Source
	{
		public override string Text { get; set; }
	}

	internal abstract class BaseTarget
	{
		public abstract string Text { get; set; }
	}

	internal abstract class Target : BaseTarget
	{
	}

	[Target, Id(1)]
	internal class DerivedTarget : Target
	{
		public override string Text { get; set; }
	}
}
