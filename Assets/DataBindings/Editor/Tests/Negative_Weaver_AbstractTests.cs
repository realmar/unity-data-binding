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
				"System.String UnitsUnderTest.Negative_Weaver_AbstractTests.OneWay_NoNonAbstractSource.AbstractSource::Text()");

		[Test]
		public void FromTarget_NoNonAbstractTarget() =>
			AssertMissingSymbolExceptionThrown<MissingNonAbstractSymbolException>(
				"System.String UnitsUnderTest.Negative_Weaver_AbstractTests.FromTarget_NoNonAbstractTarget.AbstractTarget::Text()");

		[Test]
		public void OneWay_NoNonAbstractSourceAndTarget() =>
			AssertMissingSymbolExceptionThrown<MissingNonAbstractSymbolException>(
				"System.String UnitsUnderTest.Negative_Weaver_AbstractTests.OneWay_NoNonAbstractSourceAndTarget.AbstractSource::Text()");

		[Test]
		public void TwoWay_NoNonAbstractSourceAndTarget() =>
			AssertMissingSymbolExceptionThrown<MissingNonAbstractSymbolException>(
				"System.String UnitsUnderTest.Negative_Weaver_AbstractTests.TwoWay_NoNonAbstractSourceAndTarget.AbstractSource::Text()");

		[Test]
		public void TwoWay_NoNonAbstractTarget() =>
			AssertMissingSymbolExceptionThrown<MissingNonAbstractSymbolException>(
				"System.String UnitsUnderTest.Negative_Weaver_AbstractTests.TwoWay_NoNonAbstractTarget.AbstractTarget::Text()");

		[Test]
		public void TwoWay_NoNonAbstractBindingInitializer() =>
			AssertMissingSymbolExceptionThrown<MissingNonAbstractBindingInitializer>(
				"System.Void UnitsUnderTest.Negative_Weaver_AbstractTests.TwoWay_NoNonAbstractBindingInitializer.AbstractSource::InitializeBindings()");
	}
}
