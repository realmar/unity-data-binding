using NUnit.Framework;
using Realmar.DataBindings.Editor.Exceptions;
using Realmar.DataBindings.Editor.TestFramework;

namespace Realmar.DataBindings.Editor.Tests
{
	[TestFixture]
	internal class Negative_Weaver_NonVirtualTests : WeaverTest
	{
		[Test]
		public void OneWay_NoGetterSource() =>
			AssertMissingSymbolExceptionThrown<MissingGetterException>(
				"System.String UnitsUnderTest.Negative_Weaver_NonVirtualTests.OneWay_NoGetterSource.Source::TextNoGetter()");

		[Test]
		public void OneWay_NoSetterSource() =>
			AssertMissingSymbolExceptionThrown<MissingSetterException>(
				"System.String UnitsUnderTest.Negative_Weaver_NonVirtualTests.OneWay_NoSetterSource.Source::TextNoSetter()");

		[Test]
		public void OneWay_NoBindingTarget() =>
			AssertMissingSymbolExceptionThrown<MissingBindingTargetException>(
				"System.String UnitsUnderTest.Negative_Weaver_NonVirtualTests.OneWay_NoBindingTarget.Source::Text()",
				exception => Assert.That(exception.TargetId, Is.EqualTo(0)));

		[Test]
		public void OneWay_PartiallyNoBindingTarget() =>
			AssertMissingSymbolExceptionThrown<MissingBindingTargetException>(
				"System.String UnitsUnderTest.Negative_Weaver_NonVirtualTests.OneWay_PartiallyNoBindingTarget.Source::Text_1()",
				exception => Assert.That(exception.TargetId, Is.EqualTo(1)));

		[Test]
		public void OneWay_NoTargetProperty() =>
			AssertMissingSymbolExceptionThrown<MissingTargetPropertyException>(
				"Text",
				exception => Assert.That(exception.SourceType,
					Is.EqualTo("UnitsUnderTest.Negative_Weaver_NonVirtualTests.OneWay_NoTargetProperty.Target")));

		[Test]
		public void OneWay_NoTargetPropertyWithCustomName() =>
			AssertMissingSymbolExceptionThrown<MissingTargetPropertyException>(
				"TextCustomName",
				exception => Assert.That(exception.SourceType,
					Is.EqualTo(
						"UnitsUnderTest.Negative_Weaver_NonVirtualTests.OneWay_NoTargetPropertyWithCustomName.Target")));

		[Test]
		public void FromTarget_NoBindingInitializer() =>
			AssertMissingSymbolExceptionThrown<MissingBindingInitializerException>(
				"UnitsUnderTest.Negative_Weaver_NonVirtualTests.FromTarget_NoBindingInitializer.Source");

		[Test]
		public void FromTarget_NoTargetSetter() =>
			AssertMissingSymbolExceptionThrown<MissingSetterException>(
				"System.String UnitsUnderTest.Negative_Weaver_NonVirtualTests.FromTarget_NoTargetSetter.Target::Text()");

		[Test]
		public void FromTarget_NoTargetGetter() =>
			AssertMissingSymbolExceptionThrown<MissingGetterException>(
				"System.String UnitsUnderTest.Negative_Weaver_NonVirtualTests.FromTarget_NoTargetGetter.Target::Text()");
	}
}
