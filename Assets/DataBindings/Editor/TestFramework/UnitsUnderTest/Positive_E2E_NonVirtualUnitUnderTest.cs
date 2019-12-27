using Realmar.DataBindings;
using Realmar.DataBindings.Editor.TestFramework.Attributes;

namespace UnitsUnderTest.Positive_E2E_NonVirtualTests.OneWay_PropertyToProperty
{
	[Source, CompileTimeType, RunTimeType]
	internal class Source
	{
		[BindingTarget, Id(1)] public Target BindingTarget { get; set; }
		[Binding] public string Text { get; set; }
	}

	[Target, Id(1)]
	internal class Target
	{
		public string Text { get; set; }
	}
}

namespace UnitsUnderTest.Positive_E2E_NonVirtualTests.OneWay_FieldBindingTarget
{
	[Source, CompileTimeType, RunTimeType]
	internal class Source
	{
#pragma warning disable 649
		[BindingTarget, Id(1)] public Target BindingTarget;
#pragma warning restore 649
		[Binding] public string Text { get; set; }
	}

	[Target, Id(1)]
	internal class Target
	{
		public string Text { get; set; }
	}
}

namespace UnitsUnderTest.Positive_E2E_NonVirtualTests.OneWay_FromTarget
{
	[Source, CompileTimeType, RunTimeType]
	internal class Source
	{
		[BindingTarget, Id(1)] public Target BindingTarget { get; set; }

		[Binding(BindingType.OneWayFromTarget)]
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

namespace UnitsUnderTest.Positive_E2E_NonVirtualTests.TwoWay_PropertyToProperty
{
	[Source, CompileTimeType, RunTimeType]
	internal class Source
	{
		[BindingTarget, Id(1)] public Target BindingTarget { get; set; }

		[Binding(BindingType.TwoWay)] public string Text { get; set; }

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

namespace UnitsUnderTest.Positive_E2E_NonVirtualTests.OneWay_NonDefaultBindingName
{
	[Source, CompileTimeType, RunTimeType]
	internal class Source
	{
		[BindingTarget, Id(1)] public Target BindingTarget { get; set; }

		[Binding(targetPropertyName: nameof(Target.ModifiedName))]
		public string Text { get; set; }
	}

	[Target, Id(1)]
	internal class Target
	{
		public string ModifiedName { get; set; }
	}
}

namespace UnitsUnderTest.Positive_E2E_NonVirtualTests.OneWay_NonDefaultBindingTarget
{
	[Source, CompileTimeType, RunTimeType]
	internal class Source
	{
		[BindingTarget(id: 10), Id(1)] public Target BindingTarget { get; set; }

		[Binding(targetId: 10)] public string Text { get; set; }
	}

	[Target(Id = 10), Id(1)]
	internal class Target
	{
		public string Text { get; set; }
	}
}

namespace UnitsUnderTest.Positive_E2E_NonVirtualTests.OneWay_ManyToOne
{
	[Source, CompileTimeType, RunTimeType]
	internal class Source
	{
		[BindingTarget, Id(1)] public Target BT1 { get; set; }

		[Binding] public string Text1 { get; set; }
		[Binding] public string Text2 { get; set; }
		[Binding] public string Text3 { get; set; }
	}

	[Target, Id(1)]
	internal class Target
	{
		public string Text1 { get; set; }
		public string Text2 { get; set; }
		public string Text3 { get; set; }
	}
}

namespace UnitsUnderTest.Positive_E2E_NonVirtualTests.OneWay_OneToMany
{
	[Source, CompileTimeType, RunTimeType]
	internal class Source
	{
		[BindingTarget, Id(1)] public Target BT1 { get; set; }
		[BindingTarget, Id(2)] public Target BT2 { get; set; }
		[BindingTarget, Id(3)] public Target BT3 { get; set; }

		[Binding] public string Text { get; set; }
	}

	[Target, Id(1), Id(2), Id(3)]
	internal class Target
	{
		public string Text { get; set; }
	}
}


namespace UnitsUnderTest.Positive_E2E_NonVirtualTests.OneWay_MultipleBindingsPerSource
{
	[Source, CompileTimeType, RunTimeType]
	internal class Source
	{
		[BindingTarget, Id(1)] public Target BT1 { get; set; }
		[BindingTarget(1), Id(2)] public Target BT2 { get; set; }
		[BindingTarget(2), Id(3)] public Target BT3 { get; set; }

		[Binding]
		[Binding(targetId: 1)]
		[Binding(targetId: 2)]
		public string Text { get; set; }
	}

	[Target, Id(1)]
	[Target(Id = 1), Id(2)]
	[Target(Id = 2), Id(3)]
	internal class Target
	{
		public string Text { get; set; }
	}
}

namespace UnitsUnderTest.Positive_E2E_NonVirtualTests.OneWay_ManyToMany
{
	[Source, CompileTimeType, RunTimeType]
	internal class Source
	{
		[BindingTarget, Id(1)] public Target BT1 { get; set; }
		[BindingTarget(1), Id(2)] public Target BT2 { get; set; }
		[BindingTarget(2), Id(3)] public Target BT3 { get; set; }

		[Binding]
		public string Text1 { get; set; }

		[Binding(targetId: 1)]
		public string Text2 { get; set; }

		[Binding(targetId: 2)]
		public string Text3 { get; set; }
	}

	[Target, Id(1)]
	[Target(Id = 1), Id(2)]
	[Target(Id = 2), Id(3)]
	internal class Target
	{
		public string Text1 { get; set; }
		public string Text2 { get; set; }
		public string Text3 { get; set; }
	}
}

/* namespace UnitsUnderTest.Positive_NonVirtualTests.TESTI
{
	// this will be used as SourceType (compile time type) for all set and get against source
	// there is every only once source in a conceptional binding (but not in reality
	// however, this doesn't matter here because we would think of it as a new independent set of bindings)
	[Source, CompileTimeType]
	public interface ISource
	{
		// the testframework requires BindingTargets to have setters, however this is not a requirement
		// of the data binding framework
		[BindingTarget, Id(11)] ITarget BT1_1 { get; set; }
		[BindingTarget, Id(12)] ITarget BT1_2 { get; set; }

		[BindingTarget(id: 2), Id(21)] Target BT2_1 { get; set; }

		// bindings need setters
		[Binding] string Text1_1 { get; set; }
		[Binding(targetId: 2)] string Text2_1 { get; set; }
		[Binding(targetId: 2)] string Text2_2 { get; set; }
	}

	public interface ITarget
	{
		string Text1_1 { get; }
	}

	[Source, RunTimeType]
	public class Source1 : ISource
	{
		public ITarget BT1_1 { get; set; }
		public ITarget BT1_2 { get; set; }
		public Target BT2_1 { get; set; }
		public string Text1_1 { get; set; }
		public string Text2_1 { get; set; }
		public string Text2_2 { get; set; }
	}

	[Source, RunTimeType]
	public class Source2 : ISource
	{
		public ITarget BT1_1 { get; set; }
		public ITarget BT1_2 { get; set; }
		public Target BT2_1 { get; set; }
		public string Text1_1 { get; set; }
		public string Text2_1 { get; set; }
		public string Text2_2 { get; set; }
	}

	public abstract class BaseSource : ISource
	{
		public ITarget BT1_1 { get; set; }
		public ITarget BT1_2 { get; set; }
		public Target BT2_1 { get; set; }
		public string Text1_1 { get; set; }
		public string Text2_1 { get; set; }
		public string Text2_2 { get; set; }
	}

	public abstract class BaseTarget : ITarget
	{
		public string Text1_1 { get; }
	}

	public class Source : BaseSource
	{
	}

	[Target, Id(11)]
	[Target(Id = 2), Id(21)]
	public class Target : BaseTarget
	{
		public string Text2_1 { get; }
		public string Text2_2 { get; }
	}

	// create an instance of this type for the source compile time type
	[Source, RunTimeType]
	public class DerivedSource : Source
	{
	}

	// create instance of this type for BindingTarget with id = 1
	[Target, Id(12)]
	public class DerivedTarget : Target
	{
	}
} */
