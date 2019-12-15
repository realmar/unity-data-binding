using Realmar.DataBindings;
using Realmar.DataBindings.Editor.TestFramework.Attributes;

namespace UnitsUnderTest.NonVirtualTests.OneWayPropertyToProperty
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

namespace UnitsUnderTest.NonVirtualTests.OneWayFieldBindingTarget
{
	[Source, CompileTimeType, RunTimeType]
	internal class Source
	{
		[BindingTarget, Id(1)] public Target BindingTarget;
		[Binding] public string Text { get; set; }
	}

	[Target, Id(1)]
	internal class Target
	{
		public string Text { get; set; }
	}
}

namespace UnitsUnderTest.NonVirtualTests.OneWayFromTarget
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

namespace UnitsUnderTest.NonVirtualTests.TwoWay
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

namespace UnitsUnderTest.NonVirtualTests.OneWayNonDefaultBindingName
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

namespace UnitsUnderTest.NonVirtualTests.OneWayNonDefaultBindingTarget
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

namespace UnitsUnderTest.NonVirtualTests.OneWayMultipleBindingsAndTargets
{
	[Source, CompileTimeType, RunTimeType]
	internal class Source
	{
		[BindingTarget, Id(1)] public Target BT1 { get; set; }
		[BindingTarget(id: 2), Id(2)] public Target BT2 { get; set; }

		[Binding] public string Text { get; set; }
		[Binding(targetId: 2)] public string Text2 { get; set; }
		[Binding(targetId: 2)] public string Text3 { get; set; }
	}

	[Target, Target(Id = 2), Id(1), Id(2)]
	internal class Target
	{
		public string Text { get; set; }
		public string Text2 { get; set; }
		public string Text3 { get; set; }
	}
}


/*namespace UnitsUnderTest.NonVirtualTests.TESTI
{
	// this will be used as SourceType (compile time type) for all set and get against source
	// there is every only once source in a conceptional binding (but not in reality
	// however, this doesn't matter here because we would think of it as a new independent set of bindings)
	[Source, CompileTimeType]
	public interface ISource
	{
		[BindingTarget, Id(11)] ITarget BT1_1 { get; }
		[BindingTarget, Id(12)] ITarget BT1_2 { get; }

		[BindingTarget(id: 2), Id(21)] Target BT2_1 { get; }

		// bindings need setters
		[Binding] string Text1_1 { get; set; }
		[Binding(targetId: 2)] string Text2_1 { get; set; }
		[Binding(targetId: 2)] string Text2_2 { get; set; }
	}

	public interface ITarget
	{
		string Text1_1 { get; }
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
}*/
