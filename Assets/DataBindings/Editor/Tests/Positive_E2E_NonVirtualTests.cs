using NUnit.Framework;
using Realmar.DataBindings.Editor.TestFramework.BaseTests;


namespace Realmar.DataBindings.Editor.Tests
{
	[TestFixture]
	internal class Positive_E2E_NonVirtualTests : SandboxedTest
	{
		[Test]
		public void OneWay_PropertyToProperty() => RunTest();

		[Test]
		public void OneWay_FieldBindingTarget() => RunTest();

		[Test]
		public void OneWay_FromTarget() => RunTest();

		[Test]
		public void TwoWay_PropertyToProperty() => RunTest();

		[Test]
		public void OneWay_NonDefaultBindingName() => RunTest();

		[Test]
		public void OneWay_NonDefaultBindingTarget() => RunTest();

		[Test]
		public void OneWay_ManyToOne() => RunTest();

		[Test]
		public void OneWay_OneToMany() => RunTest();

		[Test]
		public void OneWay_ManyToMany() => RunTest();

		[Test]
		public void OneWay_MultipleBindingsPerSource() => RunTest();

		// [Test]
		// public void TESTI() => RunTest();
	}
}