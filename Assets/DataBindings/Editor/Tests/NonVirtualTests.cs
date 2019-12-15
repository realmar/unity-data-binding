using NUnit.Framework;
using Realmar.DataBindings.Editor.TestFramework;


namespace Realmar.DataBindings.Editor.Tests
{
	[TestFixture]
	internal class NonVirtualTests : BaseTest
	{
		[Test]
		public void OneWayPropertyToProperty() => RunTest();

		[Test]
		public void OneWayFieldBindingTarget() => RunTest();

		[Test]
		public void OneWayFromTarget() => RunTest();

		[Test]
		public void TwoWay() => RunTest();

		[Test]
		public void OneWayNonDefaultBindingName() => RunTest();

		[Test]
		public void OneWayNonDefaultBindingTarget() => RunTest();

		[Test]
		public void OneWayMultipleBindingsAndTargets() => RunTest();

		[Test]
		public void TESTI() => RunTest();
	}
}
