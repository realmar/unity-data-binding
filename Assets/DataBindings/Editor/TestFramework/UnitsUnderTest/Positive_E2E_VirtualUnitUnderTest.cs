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
		private string _text;

		public override string Text
		{
			get => _text;
			set => _text = value;
		}
	}
}

namespace UnitsUnderTest.Positive_E2E_VirtualTests.OneWay_OverrideToOverride

{
	[Source, CompileTimeType]
	internal class BaseSource
	{
		[BindingTarget, Id(1)] public BaseTarget BindingTarget { get; set; }
		[Binding] public virtual string Text { get; set; }
	}

	[Source, RunTimeType]
	internal class Source : BaseSource
	{
		public override string Text { get; set; }
	}

	internal class BaseTarget
	{
		public virtual string Text { get; set; }
	}

	[Target, Id(1)]
	internal class Target : BaseTarget
	{
		private string _text;

		public override string Text
		{
			get => _text;
			set => _text = value;
		}
	}
}

namespace UnitsUnderTest.Positive_E2E_VirtualTests.TwoWay_PropertyToOverride
{
	[Source, CompileTimeType, RunTimeType]
	internal class Source
	{
		[BindingTarget, Id(1)] public BaseTarget BindingTarget { get; set; }
		[Binding(BindingType.TwoWay)] public string Text { get; set; }

		[BindingInitializer]
		public void InitializeBindings()
		{
		}
	}


	internal class BaseTarget
	{
		public virtual string Text { get; set; }
	}

	[Target, Id(1)]
	internal class Target : BaseTarget
	{
		private string _text;

		public override string Text
		{
			get => _text;
			set => _text = value;
		}
	}
}

namespace UnitsUnderTest.Positive_E2E_VirtualTests.TwoWay_OverrideToOverride
{
	[Source, CompileTimeType]
	internal class BaseSource
	{
		[BindingTarget, Id(1)] public BaseTarget BindingTarget { get; set; }
		[Binding(BindingType.TwoWay)] public virtual string Text { get; set; }

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

	internal class BaseTarget
	{
		public virtual string Text { get; set; }
	}

	[Target, Id(1)]
	internal class Target : BaseTarget
	{
		private string _text;

		public override string Text
		{
			get => _text;
			set => _text = value;
		}
	}
}

namespace UnitsUnderTest.Positive_E2E_VirtualTests.FromTarget_PropertyToOverride
{
	[Source, CompileTimeType, RunTimeType]
	internal class Source
	{
		[BindingTarget, Id(1)] public BaseTarget BindingTarget { get; set; }

		[Binding(BindingType.OneWayFromTarget)]
		public string Text { get; set; }

		[BindingInitializer]
		public void InitializeBindings()
		{
		}
	}


	internal class BaseTarget
	{
		public virtual string Text { get; set; }
	}

	[Target, Id(1)]
	internal class Target : BaseTarget
	{
		private string _text;

		public override string Text
		{
			get => _text;
			set => _text = value;
		}
	}
}

namespace UnitsUnderTest.Positive_E2E_VirtualTests.FromTarget_OverrideToOverride
{
	[Source, CompileTimeType]
	internal class BaseSource
	{
		[BindingTarget, Id(1)] public BaseTarget BindingTarget { get; set; }

		[Binding(BindingType.OneWayFromTarget)]
		public virtual string Text { get; set; }

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

	internal class BaseTarget
	{
		public virtual string Text { get; set; }
	}

	[Target, Id(1)]
	internal class Target : BaseTarget
	{
		private string _text;

		public override string Text
		{
			get => _text;
			set => _text = value;
		}
	}
}

namespace UnitsUnderTest.Positive_E2E_VirtualTests.TwoWay_OverrideToOverride_ManyToMany
{
	[Source, CompileTimeType]
	internal class BaseSource
	{
		[BindingTarget, Id(1)] public BaseTarget BindingTarget1 { get; set; }
		[BindingTarget, Id(2)] public BaseTarget BindingTarget2 { get; set; }
		[BindingTarget, Id(3)] public BaseTarget BindingTarget3 { get; set; }
		[BindingTarget(1), Id(4)] public BaseTarget BindingTarget4 { get; set; }
		[BindingTarget(1), Id(5)] public BaseTarget BindingTarget5 { get; set; }
		[BindingTarget(1), Id(6)] public BaseTarget BindingTarget6 { get; set; }

		[Binding(BindingType.TwoWay)]
		[Binding(BindingType.TwoWay, targetId: 1)]
		public virtual string Text1 { get; set; }

		[Binding(BindingType.TwoWay)]
		[Binding(BindingType.TwoWay, targetId: 1)]
		public virtual string Text2 { get; set; }

		[Binding(BindingType.TwoWay)]
		[Binding(BindingType.TwoWay, targetId: 1)]
		public virtual string Text3 { get; set; }

		[BindingInitializer]
		public void InitializeBindings()
		{
		}
	}

	[Source, RunTimeType]
	internal class Source : BaseSource
	{
		public override string Text1 { get; set; }
		public override string Text2 { get; set; }
		public override string Text3 { get; set; }
	}

	internal class BaseTarget
	{
		public virtual string Text1 { get; set; }
		public virtual string Text2 { get; set; }
		public virtual string Text3 { get; set; }
	}

	[Target, Id(1), Id(2), Id(3)]
	internal class Target : BaseTarget
	{
		public override string Text1 { get; set; }
		public override string Text2 { get; set; }
		public override string Text3 { get; set; }
	}

	[Target(Id = 1), Id(4), Id(5), Id(6)]
	internal class DerivedTarget : Target
	{
		public override string Text1 { get; set; }
		public override string Text2 { get; set; }
		public override string Text3 { get; set; }
	}
}
