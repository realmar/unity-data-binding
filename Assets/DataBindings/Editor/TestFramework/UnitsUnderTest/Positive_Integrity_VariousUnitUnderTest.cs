using Realmar.DataBindings;
using Realmar.DataBindings.Editor.TestFramework.Attributes;

namespace UnitsUnderTest.Positive_Integrity_VariousTests.TwoWay_SingleSetterCalls_Overrides
{
	[Source, CompileTimeType, RunTimeType]
	internal class BaseSource
	{
		private string _text;
		private int _textSetCounter;

		[BindingTarget, Id(1)] public BaseTarget BindingTarget { get; set; }

		[Binding(BindingType.TwoWay)] public virtual string Text
		{
			get => _text;
			set
			{
				_text = value;
				_textSetCounter++;
			}
		}

		[BindingInitializer]
		public void InitializeBindings()
		{
		}
	}

	internal class Source : BaseSource
	{
		public override string Text { get; set; }
	}

	[Target, Id(1)]
	internal class BaseTarget
	{
		private string _text;
		private int _textSetCounter;

		public virtual string Text
		{
			get => _text;
			set
			{
				_text = value;
				_textSetCounter++;
			}
		}
	}

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
