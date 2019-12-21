using Realmar.DataBindings;
using Realmar.DataBindings.Editor.TestFramework.Attributes;

namespace UnitsUnderTest.Positive_E2E_InterfaceTests.OneWay_InterfaceToProperty
{
	[Source, CompileTimeType]
	internal interface ISource
	{
		[BindingTarget, Id(1)] Target BindingTarget { get; set; }
		[Binding] string Text { get; set; }
	}

	[Source, RunTimeType]
	internal class Source : ISource
	{
		public Target BindingTarget { get; set; }
		public string Text { get; set; }
	}

	[Target, Id(1)]
	internal class Target
	{
		public string Text { get; set; }
	}
}

namespace UnitsUnderTest.Positive_E2E_InterfaceTests.OneWay_PropertyToInterface
{
	[Source, CompileTimeType, RunTimeType]
	internal class Source
	{
		[BindingTarget, Id(1)] public ITarget BindingTarget { get; set; }
		[Binding] public string Text { get; set; }
	}

	internal interface ITarget
	{
		string Text { get; set; }
	}

	[Target, Id(1)]
	internal class Target : ITarget
	{
		public string Text { get; set; }
	}
}

namespace UnitsUnderTest.Positive_E2E_InterfaceTests.OneWay_InterfaceToInterface
{
	[Source, CompileTimeType]
	internal interface ISource
	{
		[BindingTarget, Id(1)] ITarget BindingTarget { get; set; }
		[Binding] string Text { get; set; }
	}

	[Source, RunTimeType]
	internal class Source : ISource
	{
		public ITarget BindingTarget { get; set; }
		public string Text { get; set; }
	}

	internal interface ITarget
	{
		string Text { get; set; }
	}

	[Target, Id(1)]
	internal class Target : ITarget
	{
		public string Text { get; set; }
	}
}

namespace UnitsUnderTest.Positive_E2E_InterfaceTests.FromTarget_InterfaceToProperty
{
	[Source, CompileTimeType]
	internal interface ISource
	{
		[BindingTarget, Id(1)] Target BindingTarget { get; set; }

		[Binding(BindingType.OneWayFromTarget)]
		string Text { get; set; }

		[BindingInitializer]
		void InitializeBindings();
	}

	[Source, RunTimeType]
	internal class Source : ISource
	{
		public Target BindingTarget { get; set; }
		public string Text { get; set; }

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

namespace UnitsUnderTest.Positive_E2E_InterfaceTests.FromTarget_PropertyToInterface
{
	[Source, CompileTimeType, RunTimeType]
	internal class Source
	{
		[BindingTarget, Id(1)] public ITarget BindingTarget { get; set; }

		[Binding(BindingType.OneWayFromTarget)]
		public string Text { get; set; }

		[BindingInitializer]
		public void InitializeBindings()
		{
		}
	}

	internal interface ITarget
	{
		string Text { get; set; }
	}

	[Target, Id(1)]
	internal class Target : ITarget
	{
		public string Text { get; set; }
	}
}

namespace UnitsUnderTest.Positive_E2E_InterfaceTests.FromTarget_InterfaceToInterface
{
	[Source, CompileTimeType]
	internal interface ISource
	{
		[BindingTarget, Id(1)] ITarget BindingTarget { get; set; }

		[Binding(BindingType.OneWayFromTarget)]
		string Text { get; set; }

		[BindingInitializer]
		void InitializeBindings();
	}

	[Source, RunTimeType]
	internal class Source : ISource
	{
		public ITarget BindingTarget { get; set; }
		public string Text { get; set; }

		public void InitializeBindings()
		{
		}
	}

	internal interface ITarget
	{
		string Text { get; set; }
	}

	[Target, Id(1)]
	internal class Target : ITarget
	{
		public string Text { get; set; }
	}
}

namespace UnitsUnderTest.Positive_E2E_InterfaceTests.TwoWay_InterfaceToProperty
{
	[Source, CompileTimeType]
	internal interface ISource
	{
		[BindingTarget, Id(1)] Target BindingTarget { get; set; }

		[Binding(BindingType.TwoWay)]
		string Text { get; set; }

		[BindingInitializer]
		void InitializeBindings();
	}

	[Source, RunTimeType]
	internal class Source : ISource
	{
		public Target BindingTarget { get; set; }
		public string Text { get; set; }

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

namespace UnitsUnderTest.Positive_E2E_InterfaceTests.TwoWay_PropertyToInterface
{
	[Source, CompileTimeType, RunTimeType]
	internal class Source
	{
		[BindingTarget, Id(1)] public ITarget BindingTarget { get; set; }

		[Binding(BindingType.TwoWay)]
		public string Text { get; set; }

		[BindingInitializer]
		public void InitializeBindings()
		{
		}
	}

	internal interface ITarget
	{
		string Text { get; set; }
	}

	[Target, Id(1)]
	internal class Target : ITarget
	{
		public string Text { get; set; }
	}
}

namespace UnitsUnderTest.Positive_E2E_InterfaceTests.TwoWay_InterfaceToInterface
{
	[Source, CompileTimeType]
	internal interface ISource
	{
		[BindingTarget, Id(1)] ITarget BindingTarget { get; set; }

		[Binding(BindingType.TwoWay)]
		string Text { get; set; }

		[BindingInitializer]
		void InitializeBindings();
	}

	[Source, RunTimeType]
	internal class Source : ISource
	{
		public ITarget BindingTarget { get; set; }
		public string Text { get; set; }

		public void InitializeBindings()
		{
		}
	}

	internal interface ITarget
	{
		string Text { get; set; }
	}

	[Target, Id(1)]
	internal class Target : ITarget
	{
		public string Text { get; set; }
	}
}

/* namespace UnitsUnderTest.Positive_E2E_InterfaceTests.TwoWay_InterfaceToInterface_MultipleInterfaceInheritance
{
	[Source, CompileTimeType]
	internal interface ISource
	{
		[BindingTarget, Id(1)] ITarget1 BindingTarget1 { get; set; }
		[BindingTarget(id: 2), Id(2)] ITarget2 BindingTarget2 { get; set; }

		[Binding(BindingType.TwoWay)]
		string Text1 { get; set; }

		[Binding(BindingType.TwoWay, targetId: 2)]
		string Text2 { get; set; }

		[BindingInitializer]
		void InitializeBindings();
	}

	[Source, RunTimeType]
	internal class Source : ISource
	{
		public ITarget1 BindingTarget1 { get; set; }
		public ITarget2 BindingTarget2 { get; set; }
		public string Text1 { get; set; }
		public string Text2 { get; set; }

		public void InitializeBindings()
		{
		}
	}

	internal interface ITarget1
	{
		string Text1 { get; set; }
	}

	internal interface ITarget2
	{
		string Text2 { get; set; }
	}

	[Target, Id(1)]
	[Target(Id = 2), Id(2)]
	internal class Target : ITarget1, ITarget2
	{
		public string Text1 { get; set; }
		public string Text2 { get; set; }
	}
} */

namespace UnitsUnderTest.Positive_E2E_InterfaceTests.TwoWay_InterfaceToInterfaceToAbstract
{
	[Source, CompileTimeType]
	internal interface ISource
	{
		[BindingTarget, Id(1)] ITarget BindingTarget { get; set; }

		[Binding(BindingType.TwoWay)]
		string Text { get; set; }

		[BindingInitializer]
		void InitializeBindings();
	}

	[Source, RunTimeType]
	internal class Source : ISource
	{
		public ITarget BindingTarget { get; set; }
		public string Text { get; set; }

		public void InitializeBindings()
		{
		}
	}

	internal interface ITarget
	{
		string Text { get; set; }
	}

	internal abstract class BaseTarget : ITarget
	{
		public abstract string Text { get; set; }
	}

	[Target, Id(1)]
	internal class Target : BaseTarget
	{
		public override string Text { get; set; }
	}
}

namespace UnitsUnderTest.Positive_E2E_InterfaceTests.TwoWay_InterfaceToInterfaceToAbstract_MultipleOverrides
{
	[Source, CompileTimeType]
	internal interface ISource
	{
		[BindingTarget, Id(1)] ITarget BindingTarget1 { get; set; }
		[BindingTarget, Id(2)] ITarget BindingTarget2 { get; set; }

		[Binding(BindingType.TwoWay)]
		string Text { get; set; }

		[BindingInitializer]
		void InitializeBindings();
	}

	[Source, RunTimeType]
	internal class Source : ISource
	{
		public ITarget BindingTarget1 { get; set; }
		public ITarget BindingTarget2 { get; set; }
		public string Text { get; set; }

		public void InitializeBindings()
		{
		}
	}

	internal interface ITarget
	{
		string Text { get; set; }
	}

	internal abstract class BaseTarget : ITarget
	{
		public abstract string Text { get; set; }
	}

	[Target, Id(1)]
	internal class Target : BaseTarget
	{
		public override string Text { get; set; }
	}

	[Target, Id(2)]
	internal class DerivedTarget : Target
	{
		public override string Text { get; set; }
	}
}

/* namespace UnitsUnderTest.Positive_E2E_InterfaceTests.TwoWay_InterfaceToAbstractToInterfaceToAbstract
{
	[Source, CompileTimeType]
	internal interface ISource
	{
		[BindingTarget, Id(1)] ITarget BindingTarget { get; set; }

		[Binding(BindingType.TwoWay)]
		string Text { get; set; }

		[BindingInitializer]
		void InitializeBindings();
	}

	internal abstract class BaseSource : ISource
	{
		public abstract ITarget BindingTarget { get; set; }
		public abstract string Text { get; set; }
		public abstract void InitializeBindings();
	}

	[Source, RunTimeType]
	internal class Source : BaseSource
	{
		public override ITarget BindingTarget { get; set; }
		public override string Text { get; set; }

		public override void InitializeBindings()
		{
		}
	}

	internal interface ITarget
	{
		string Text { get; set; }
	}

	internal abstract class BaseTarget : ITarget
	{
		public abstract string Text { get; set; }
	}

	[Target, Id(1)]
	internal class Target : BaseTarget
	{
		public override string Text { get; set; }
	}
} */

namespace UnitsUnderTest.Positive_E2E_InterfaceTests.TwoWay_InterfaceToProperty_MultipleSources
{
	[Source, CompileTimeType]
	internal interface ISource
	{
		[BindingTarget, Id(1)] Target BindingTarget { get; set; }
		[Binding(BindingType.TwoWay)] string Text { get; set; }

		[BindingInitializer]
		void InitializeBindings();
	}

	[Source, RunTimeType]
	internal class Source1 : ISource
	{
		public Target BindingTarget { get; set; }
		public string Text { get; set; }

		public void InitializeBindings()
		{
		}
	}

	[Source, RunTimeType]
	internal class Source2 : ISource
	{
		public Target BindingTarget { get; set; }
		public string Text { get; set; }

		public void InitializeBindings()
		{
		}
	}

	[Source, RunTimeType]
	internal class Source3 : ISource
	{
		public Target BindingTarget { get; set; }
		public string Text { get; set; }

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
