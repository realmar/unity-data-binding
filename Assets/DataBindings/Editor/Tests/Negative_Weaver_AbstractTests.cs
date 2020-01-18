using NUnit.Framework;
using Realmar.DataBindings.Editor.Exceptions;
using Realmar.DataBindings.Editor.TestFramework.BaseTests;

namespace Realmar.DataBindings.Editor.Tests
{
	[TestFixture]
	internal class Negative_Weaver_AbstractTests : WeaverTest
	{
		[Test]
		public void OneWay_NoNonAbstractSource() =>
			AssertMissingSymbolExceptionThrown<MissingNonAbstractSymbolException>(
				"System.Void UnitsUnderTest.Negative_Weaver_AbstractTests.OneWay_NoNonAbstractSource.AbstractSource::set_Text(System.String)");

		[Test]
		public void FromTarget_NoNonAbstractTarget() =>
			AssertMissingSymbolExceptionThrown<MissingNonAbstractSymbolException>(
				"System.Void UnitsUnderTest.Negative_Weaver_AbstractTests.FromTarget_NoNonAbstractTarget.AbstractTarget::set_Text(System.String)");

		[Test]
		public void OneWay_NoNonAbstractSourceAndTarget() =>
			AssertMissingSymbolExceptionThrown<MissingNonAbstractSymbolException>(
				"System.Void UnitsUnderTest.Negative_Weaver_AbstractTests.OneWay_NoNonAbstractSourceAndTarget.AbstractSource::set_Text(System.String)");

		[Test]
		public void TwoWay_NoNonAbstractSourceAndTarget() =>
			AssertMissingSymbolExceptionThrown<MissingNonAbstractSymbolException>(
				"System.Void UnitsUnderTest.Negative_Weaver_AbstractTests.TwoWay_NoNonAbstractSourceAndTarget.AbstractSource::set_Text(System.String)");

		[Test]
		public void TwoWay_NoNonAbstractTarget() =>
			AssertMissingSymbolExceptionThrown<MissingNonAbstractSymbolException>(
				"System.Void UnitsUnderTest.Negative_Weaver_AbstractTests.TwoWay_NoNonAbstractTarget.AbstractTarget::set_Text(System.String)");

		[Test]
		public void TwoWay_NoNonAbstractBindingInitializer() =>
			AssertMissingSymbolExceptionThrown<MissingNonAbstractBindingInitializer>(
				"System.Void UnitsUnderTest.Negative_Weaver_AbstractTests.TwoWay_NoNonAbstractBindingInitializer.AbstractSource::InitializeBindings()");

		[Test]
		public void OneTime_NoNonAbstractBindingInitializer() =>
			AssertMissingSymbolExceptionThrown<MissingNonAbstractBindingInitializer>(
				"System.Void UnitsUnderTest.Negative_Weaver_AbstractTests.OneTime_NoNonAbstractBindingInitializer.AbstractSource::InitializeBindings()");
	}
}
