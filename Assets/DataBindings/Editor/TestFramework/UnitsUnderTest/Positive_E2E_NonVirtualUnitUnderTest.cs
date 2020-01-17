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

namespace UnitsUnderTest.Positive_E2E_NonVirtualTests.TwoWay_PrivateBindingInitializer
{
	[Source, CompileTimeType, RunTimeType]
	internal class Source
	{
		[BindingTarget, Id(1)] public Target BindingTarget { get; set; }

		[Binding(BindingType.TwoWay)]
		public string Text { get; set; }

		[BindingInitializer]
		private void InitializeBindings()
		{
		}
	}

	[Target, Id(1)]
	internal class Target
	{
		public string Text { get; set; }
	}
}

namespace UnitsUnderTest.Positive_E2E_NonVirtualTests.OneWay_NullCheck_TargetNotNull
{
	[Source, CompileTimeType, RunTimeType]
	internal class Source
	{
		[BindingTarget, Id(1)] public Target BindingTarget { get; set; }
		[Binding(emitNullCheck: true)] public string Text { get; set; }
	}

	[Target, Id(1)]
	internal class Target
	{
		public string Text { get; set; }
	}
}

namespace UnitsUnderTest.Positive_E2E_NonVirtualTests.OneWay_NullCheck_TargetNull
{
	[Source, CompileTimeType, RunTimeType]
	internal class Source
	{
		[BindingTarget, Id(1)] public Target BindingTarget { get; set; }
		[Binding(emitNullCheck: true)] public string Text { get; set; }
	}

	internal class Target
	{
		public string Text { get; set; }
	}
}

namespace UnitsUnderTest.Positive_E2E_NonVirtualTests.FromTarget_Throw_TargetNotNull
{
	[Source, CompileTimeType, RunTimeType]
	internal class Source
	{
		[BindingTarget, Id(1)] public Target BindingTarget { get; set; }

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

namespace UnitsUnderTest.Positive_E2E_NonVirtualTests.FromTarget_NoThrow_TargetNull
{
	[Source, CompileTimeType, RunTimeType]
	internal class Source
	{
		[BindingTarget, Id(1), DoNotConfigure] public Target BindingTarget { get; set; }

		[Binding(BindingType.OneWayFromTarget, emitNullCheck: true)]
		public string Text { get; set; }

		[BindingInitializer(throwOnFailure: false)]
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

namespace UnitsUnderTest.Positive_E2E_NonVirtualTests.FromTarget_NullCheck_NoThrow_VerifyCustomCodeExecuted
{
	[Source, CompileTimeType, RunTimeType]
	internal class Source
	{
		[BindingTarget, Id(1), DoNotConfigure] public Target BindingTarget { get; set; }

		[Binding(BindingType.OneWayFromTarget, emitNullCheck: true)]
		public string Text { get; set; }


		private int _sample;

		[BindingInitializer(throwOnFailure: false)]
		public void InitializeBindings()
		{
			_sample = 42;
		}
	}

	[Target, Id(1)]
	internal class Target
	{
		public string Text { get; set; }
	}
}

namespace UnitsUnderTest.Positive_E2E_NonVirtualTests.FromTarget_NullCheck_NoThrow_VerifyCustomCodeExecuted_Branching
{
	[Source, CompileTimeType, RunTimeType]
	internal class Source
	{
		[BindingTarget, Id(1), DoNotConfigure] public Target BindingTarget { get; set; }

		[Binding(BindingType.OneWayFromTarget, emitNullCheck: true)]
		public string Text { get; set; }


		private int _sample;
		private int _result;

		[BindingInitializer(throwOnFailure: false)]
		public void InitializeBindings()
		{
			if (_sample < 0)
			{
				_result = 1;
			}
			else if (_sample == 0)
			{
				_result = 2;
			}
			else
			{
				_result = 3;
			}
		}
	}

	[Target, Id(1)]
	internal class Target
	{
		public string Text { get; set; }
	}
}

namespace UnitsUnderTest.Positive_E2E_NonVirtualTests.FromTarget_Throw_TargetNull_VerifyCustomCodeExecuted
{
	[Source, CompileTimeType, RunTimeType]
	internal class Source
	{
		[BindingTarget, Id(1)] public Target BindingTarget { get; set; }

		[Binding(BindingType.OneWayFromTarget, emitNullCheck: true)]
		public string Text { get; set; }


		private int _sample;

		[BindingInitializer]
		public void InitializeBindings()
		{
			_sample = 42;
		}
	}

	internal class Target
	{
		public string Text { get; set; }
	}
}

namespace UnitsUnderTest.Positive_E2E_NonVirtualTests.OneWay_Binding_NullCheck_CustomLogicExecuted
{
	[Source, CompileTimeType, RunTimeType]
	internal class Source
	{
		private int _sample;
		private string _text;

		[BindingTarget, Id(1)] public Target BindingTarget { get; set; }

		[Binding(emitNullCheck: true)]
		public string Text
		{
			get => _text;
			set
			{
				_text = value;
				_sample = 42;
			}
		}
	}

	internal class Target
	{
		public string Text { get; set; }
	}
}

namespace UnitsUnderTest.Positive_E2E_NonVirtualTests.TwoWay_Binding_CustomLogicExecuted
{
	[Source, CompileTimeType, RunTimeType]
	internal class Source
	{
		private int _sample;
		private string _text;

		[BindingTarget, Id(1)] public Target BindingTarget { get; set; }

		[Binding(BindingType.TwoWay, emitNullCheck: true)]
		public string Text
		{
			get => _text;
			set
			{
				_text = value;
				_sample = 42;
			}
		}

		[BindingInitializer]
		public void InitializeBindings()
		{
		}
	}

	[Target, Id(1)]
	internal class Target
	{
		private int _sample;
		private string _text;

		public string Text
		{
			get => _text;
			set
			{
				_text = value;
				_sample = 69;
			}
		}
	}
}

namespace UnitsUnderTest.Positive_E2E_NonVirtualTests.FromTarget_OneWay_DoubleBinding
{
	[Target, Id(1)]
	internal class View
	{
		public string Text { get; set; }
	}

	[Source, CompileTimeType, RunTimeType]
	internal class ViewModel
	{
		[BindingTarget, Id(1)] public View View { get; set; }
		[BindingTarget(2), Id(2)] public Model Model { get; set; }

		[Binding(BindingType.OneWayFromTarget)]
		[Binding(targetId: 2)]
		public string Text { get; set; }

		[BindingInitializer]
		public void InitializeBindings()
		{
		}
	}

	[Target(Id = 2), Id(2)]
	internal class Model
	{
		public string Text { get; set; }
	}
}

namespace UnitsUnderTest.Positive_E2E_NonVirtualTests.TwoWay_PropertyToProperty_Ensure_Setter_Called_Once
{
	[Source, CompileTimeType, RunTimeType]
	internal class Source
	{
		private string _text;
		private int _counter;

		[BindingTarget, Id(1)] public Target BindingTarget { get; set; }

		[Binding(BindingType.TwoWay)] public string Text
		{
			get => _text;
			set
			{
				_text = value;
				_counter++;
			}
		}

		[BindingInitializer]
		public void InitializeBindings()
		{
		}
	}

	[Target, Id(1)]
	internal class Target
	{
		private string _text;
		private int _counter;

		public string Text
		{
			get => _text;
			set
			{
				_text = value;
				_counter++;
			}
		}
	}
}

namespace UnitsUnderTest.Positive_E2E_NonVirtualTests.TwoWay_ChainedBindings
{
	[Target, Id(1)]
	internal class A
	{
		public string Text { get; set; }
	}

	[Source, CompileTimeType, RunTimeType]
	internal class B
	{
		[BindingTarget, Id(1)] public A BT_A { get; set; }
		[BindingTarget(2), Id(2)] public C BT_C { get; set; }

		[Binding(BindingType.TwoWay)]
		[Binding(BindingType.TwoWay, targetId: 2)]
		public string Text { get; set; }

		[BindingInitializer]
		public void InitializeBindings()
		{
		}
	}

	[Target, Target(2), Id(2)]
	internal class C
	{
		public string Text { get; set; }
	}

	[Source, CompileTimeType, RunTimeType]
	internal class D
	{
		[BindingTarget, Id(2)] public C BT_C { get; set; }
		[BindingTarget(2), Id(4)] public E BT_E { get; set; }

		[Binding(BindingType.TwoWay)]
		[Binding(BindingType.TwoWay, targetId: 2)]
		public string Text { get; set; }

		[BindingInitializer]
		public void InitializeBindings()
		{
		}
	}

	[Target(2), Id(4)]
	internal class E
	{
		public string Text { get; set; }
	}
}

namespace UnitsUnderTest.Positive_E2E_NonVirtualTests.OneTime_ManyToOne
{
	[Source, CompileTimeType, RunTimeType]
	internal class Source
	{
		[BindingTarget, Id(1)] public Target BT1 { get; set; }

		[Binding(BindingType.OneTime)] public string Text1 { get; set; }
		[Binding(BindingType.OneTime)] public string Text2 { get; set; }
		[Binding(BindingType.OneTime)] public string Text3 { get; set; }

		[BindingInitializer]
		public void InitializeBindings()
		{
		}
	}

	[Target, Id(1)]
	internal class Target
	{
		public string Text1 { get; set; }
		public string Text2 { get; set; }
		public string Text3 { get; set; }
	}
}
