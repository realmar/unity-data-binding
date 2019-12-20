using Realmar.DataBindings;

namespace UnitsUnderTest.Negative_Weaver_NonVirtualTests.OneWay_NoSetterSource
{
	internal class Source
	{
		[BindingTarget] public Target BindingTarget { get; set; }
		[Binding] public string TextNoSetter { get; }
	}

	internal class Target
	{
		public string TextNoSetter { get; set; }
	}
}

namespace UnitsUnderTest.Negative_Weaver_NonVirtualTests.OneWay_NoGetterSource
{
	internal class Source
	{
		private string _text;

		[BindingTarget] public Target BindingTarget { get; set; }

		[Binding]
		public string TextNoGetter
		{
			set => _text = value;
		}
	}

	internal class Target
	{
		public string TextNoGetter { get; set; }
	}
}

namespace UnitsUnderTest.Negative_Weaver_NonVirtualTests.FromTarget_NoBindingInitializer
{
	internal class Source
	{
		[BindingTarget] public Target BindingTarget { get; set; }

		[Binding(BindingType.OneWayFromTarget)]
		public string Text { get; set; }
	}

	internal class Target
	{
		public string Text { get; set; }
	}
}

namespace UnitsUnderTest.Negative_Weaver_NonVirtualTests.FromTarget_NoTargetSetter
{
	internal class Source
	{
		[BindingTarget] public Target BindingTarget { get; set; }

		[Binding(BindingType.OneWayFromTarget)]
		public string Text { get; set; }

		[BindingInitializer]
		public void InitializeBindings()
		{
		}
	}

	internal class Target
	{
		public string Text { get; }
	}
}

namespace UnitsUnderTest.Negative_Weaver_NonVirtualTests.FromTarget_NoTargetGetter
{
	internal class Source
	{
		[BindingTarget] public Target BindingTarget { get; set; }

		[Binding(BindingType.OneWayFromTarget)]
		public string Text { get; set; }

		[BindingInitializer]
		public void InitializeBindings()
		{
		}
	}

	internal class Target
	{
		private string _text;

		public string Text
		{
			set => _text = value;
		}
	}
}

namespace UnitsUnderTest.Negative_Weaver_NonVirtualTests.OneWay_NoBindingTarget
{
	internal class Source
	{
		[Binding] public string Text { get; set; }
	}

	internal class Target
	{
		public string Text { get; set; }
	}
}

namespace UnitsUnderTest.Negative_Weaver_NonVirtualTests.OneWay_PartiallyNoBindingTarget
{
	internal class Source
	{
		[BindingTarget] public Target BindingTarget { get; set; }

		[Binding] public string Text_0 { get; set; }
		[Binding(targetId: 1)] public string Text_1 { get; set; }
	}

	internal class Target
	{
		public string Text_0 { get; set; }
		public string Text_1 { get; set; }
	}
}

namespace UnitsUnderTest.Negative_Weaver_NonVirtualTests.OneWay_NoTargetProperty
{
	internal class Source
	{
		[BindingTarget] public Target BindingTarget { get; set; }

		[Binding] public string Text { get; set; }
	}

	internal class Target
	{
	}
}

namespace UnitsUnderTest.Negative_Weaver_NonVirtualTests.OneWay_NoTargetPropertyWithCustomName
{
	internal class Source
	{
		[BindingTarget] public Target BindingTarget { get; set; }

		[Binding(targetPropertyName: "TextCustomName")]
		public string Text { get; set; }
	}

	internal class Target
	{
		public string Text { get; set; }
	}
}
