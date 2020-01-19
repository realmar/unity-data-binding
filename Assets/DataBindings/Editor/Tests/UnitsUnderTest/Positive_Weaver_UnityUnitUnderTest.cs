using Realmar.DataBindings;
using Realmar.DataBindings.Editor.TestFramework.Attributes;
using UnityEngine;

namespace UnitsUnderTest.Positive_Weaver_UnityTests.NonVirtual_OneWay_MonoBehaviorToPOCO
{
	[Source, CompileTimeType, RunTimeType]
	internal class Source : MonoBehaviour
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

namespace UnitsUnderTest.Positive_Weaver_UnityTests.NonVirtual_OneWay_MonoBehaviorToMonoBehavior
{
	[Source, CompileTimeType, RunTimeType]
	internal class Source : MonoBehaviour
	{
		[BindingTarget, Id(1)] public Target BindingTarget { get; set; }
		[Binding] public string Text { get; set; }
	}

	[Target, Id(1)]
	internal class Target : MonoBehaviour
	{
		public string Text { get; set; }
	}
}

namespace UnitsUnderTest.Positive_Weaver_UnityTests.NonVirtual_TwoWay_MonoBehaviorToMonoBehavior
{
	[Source, CompileTimeType, RunTimeType]
	internal class Source : MonoBehaviour
	{
		[BindingTarget, Id(1)] public Target BindingTarget { get; set; }
		[Binding(BindingType.TwoWay)] public string Text { get; set; }

		[BindingInitializer]
		public void InitializeBindings()
		{
		}
	}

	[Target, Id(1)]
	internal class Target : MonoBehaviour
	{
		public string Text { get; set; }
	}
}

namespace UnitsUnderTest.Positive_Weaver_UnityTests.Virtual_TwoWay_OverrideToOverride_BothMonoBehaviors
{
	[Source, CompileTimeType]
	internal class BaseSource : MonoBehaviour
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

	internal class BaseTarget : MonoBehaviour
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
