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

namespace UnitsUnderTest.Positive_E2E_AbstractTests.OneWay_AbstractToAbstract_MultipleAbstractsInHierarchy
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

namespace UnitsUnderTest.Positive_E2E_AbstractTests.OneWay_AbstractBindingTarget
{
	[Source, CompileTimeType]
	internal abstract class BaseSource
	{
		[BindingTarget, Id(1)] public abstract Target BindingTarget { get; set; }
		[Binding] public string Text { get; set; }
	}

	[Source, RunTimeType]
	internal class Source : BaseSource
	{
		public override Target BindingTarget { get; set; }
	}

	[Target, Id(1)]
	internal class Target
	{
		public string Text { get; set; }
	}
}

namespace UnitsUnderTest.Positive_E2E_AbstractTests.OneWay_PropertyToManyOverrides
{
	[Source, CompileTimeType, RunTimeType]
	internal class BaseSource
	{
		[BindingTarget, Id(1)] public BaseTarget BindingTarget1 { get; set; }
		[BindingTarget, Id(2)] public BaseTarget BindingTarget2 { get; set; }
		[BindingTarget, Id(3)] public BaseTarget BindingTarget3 { get; set; }
		[Binding] public string Text { get; set; }
	}

	internal abstract class BaseTarget
	{
		public abstract string Text { get; set; }
	}

	[Target, Id(1)]
	internal class Target1 : BaseTarget
	{
		public override string Text { get; set; }
	}

	[Target, Id(2)]
	internal class Target2 : BaseTarget
	{
		public override string Text { get; set; }
	}

	[Target, Id(3)]
	internal class Target3 : BaseTarget
	{
		public override string Text { get; set; }
	}
}

namespace UnitsUnderTest.Positive_E2E_AbstractTests.FromTarget_PropertyToManyOverrides
{
	[Source, CompileTimeType, RunTimeType]
	internal class BaseSource
	{
		[BindingTarget, Id(1)] public BaseTarget BindingTarget1 { get; set; }
		[BindingTarget, Id(2)] public BaseTarget BindingTarget2 { get; set; }
		[BindingTarget, Id(3)] public BaseTarget BindingTarget3 { get; set; }

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
	internal class Target1 : BaseTarget
	{
		public override string Text { get; set; }
	}

	[Target, Id(2)]
	internal class Target2 : BaseTarget
	{
		public override string Text { get; set; }
	}

	[Target, Id(3)]
	internal class Target3 : BaseTarget
	{
		public override string Text { get; set; }
	}
}

namespace UnitsUnderTest.Positive_E2E_AbstractTests.TwoWay_PropertyToManyOverrides
{
	[Source, CompileTimeType, RunTimeType]
	internal class BaseSource
	{
		[BindingTarget, Id(1)] public BaseTarget BindingTarget1 { get; set; }
		[BindingTarget, Id(2)] public BaseTarget BindingTarget2 { get; set; }
		[BindingTarget, Id(3)] public BaseTarget BindingTarget3 { get; set; }

		[Binding(BindingType.TwoWay)]
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
	internal class Target1 : BaseTarget
	{
		public override string Text { get; set; }
	}

	[Target, Id(2)]
	internal class Target2 : BaseTarget
	{
		public override string Text { get; set; }
	}

	[Target, Id(3)]
	internal class Target3 : BaseTarget
	{
		public override string Text { get; set; }
	}
}

namespace UnitsUnderTest.Positive_E2E_AbstractTests.OneTime_PropertyToManyOverrides
{
	[Source, CompileTimeType]
	internal abstract class BaseSource
	{
		[BindingTarget, Id(1)] public BaseTarget BindingTarget1 { get; set; }
		[BindingTarget, Id(2)] public BaseTarget BindingTarget2 { get; set; }
		[BindingTarget, Id(3)] public BaseTarget BindingTarget3 { get; set; }

		[Binding(BindingType.OneTime)]
		public string Text { get; set; }

		[BindingInitializer]
		public abstract void InitializeBindings();
	}

	[Source, RunTimeType]
	internal class Source1 : BaseSource
	{
		public override void InitializeBindings()
		{
		}
	}

	[Source, RunTimeType]
	internal class Source2 : BaseSource
	{
		public override void InitializeBindings()
		{
		}
	}

	[Source, RunTimeType]
	internal class Source3 : BaseSource
	{
		public override void InitializeBindings()
		{
		}
	}

	internal abstract class BaseTarget
	{
		public abstract string Text { get; set; }
	}

	[Target, Id(1)]
	internal class Target1 : BaseTarget
	{
		public override string Text { get; set; }
	}

	[Target, Id(2)]
	internal class Target2 : BaseTarget
	{
		public override string Text { get; set; }
	}

	[Target, Id(3)]
	internal class Target3 : BaseTarget
	{
		public override string Text { get; set; }
	}
}

namespace UnitsUnderTest.Positive_E2E_AbstractTests.OneWay_CallToCorrectBindingTargetType
{
	[Source, CompileTimeType, RunTimeType]
	internal class BaseSource
	{
		[BindingTarget, Id(1)] public BaseTarget BindingTarget1 { get; set; }
		[BindingTarget, Id(2)] public BaseTarget BindingTarget2 { get; set; }
		[BindingTarget, Id(3)] public BaseTarget BindingTarget3 { get; set; }

		[Binding]
		public string Text { get; set; }
	}

	internal abstract class BaseTarget
	{
		public abstract string Text { get; set; }
	}

	[Target, Id(1), Id(2), Id(3)]
	internal class Target1 : BaseTarget
	{
		public override string Text { get; set; }
	}

	internal class Target2 : BaseTarget
	{
		public override string Text { get; set; }
	}

	internal class Target3 : BaseTarget
	{
		public override string Text { get; set; }
	}

	internal class Target3_1 : Target3
	{
		private string _text;

		public new string Text
		{
			get => _text;
			set => _text = value;
		}
	}
}
