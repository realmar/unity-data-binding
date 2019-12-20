using NUnit.Framework;
using Realmar.DataBindings.Editor.Exceptions;
using Realmar.DataBindings.Editor.TestFramework;

namespace Realmar.DataBindings.Editor.Tests
{
	[TestFixture]
	internal class Negative_Weaver_NonVirtualTests : CompiledTest
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
