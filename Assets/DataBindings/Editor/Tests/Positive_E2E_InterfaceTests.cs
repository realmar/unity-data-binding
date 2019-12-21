using NUnit.Framework;
using Realmar.DataBindings.Editor.TestFramework.BaseTests;

namespace Realmar.DataBindings.Editor.Tests
{
	[TestFixture]
	internal class Positive_E2E_InterfaceTests : SandboxedTest
	{
		[Test]
		public void OneWay_InterfaceToProperty() => RunTest();

		[Test]
		public void OneWay_PropertyToInterface() => RunTest();

		[Test]
		public void OneWay_InterfaceToInterface() => RunTest();

		[Test]
		public void FromTarget_InterfaceToProperty() => RunTest();

		[Test]
		public void FromTarget_PropertyToInterface() => RunTest();

		[Test]
		public void FromTarget_InterfaceToInterface() => RunTest();

		[Test]
		public void TwoWay_InterfaceToProperty() => RunTest();

		[Test]
		public void TwoWay_PropertyToInterface() => RunTest();

		[Test]
		public void TwoWay_InterfaceToInterface() => RunTest();

		// TODO breaks weaver
		// [Test]
		// public void TwoWay_InterfaceToInterface_MultipleInterfaceInheritance() => RunTest();

		[Test]
		public void TwoWay_InterfaceToInterfaceToAbstract() => RunTest();

		[Test]
		public void TwoWay_InterfaceToInterfaceToAbstract_MultipleOverrides() => RunTest();

		// TODO breaks weaver
		// [Test]
		// public void TwoWay_InterfaceToAbstractToInterfaceToAbstract() => RunTest();

		[Test]
		public void TwoWay_InterfaceToProperty_MultipleSources() => RunTest();
	}
}
