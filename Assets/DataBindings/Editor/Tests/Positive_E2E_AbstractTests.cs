using NUnit.Framework;
using Realmar.DataBindings.Editor.TestFramework;

namespace Realmar.DataBindings.Editor.Tests
{
	internal class Positive_E2E_AbstractTests : SandboxedTest
	{
		[Test]
		public void OneWay_AbstractToProperty() => RunTest();

		[Test]
		public void OneWay_PropertyToAbstract() => RunTest();

		[Test]
		public void OneWay_AbstractToAbstract() => RunTest();

		[Test]
		public void FromTarget_AbstractToProperty() => RunTest();

		[Test]
		public void FromTarget_PropertyToAbstract() => RunTest();

		[Test]
		public void FromTarget_AbstractToAbstract() => RunTest();

		[Test]
		public void FromTarget_AbstractToAbstract_AbstractBindingInitializer() => RunTest();

		[Test]
		public void TwoWay_AbstractToProperty() => RunTest();

		[Test]
		public void TwoWay_PropertyToAbstract() => RunTest();

		[Test]
		public void TwoWay_AbstractToAbstract() => RunTest();

		[Test]
		public void OneWay_AbstractToAbstract_MultipleOverrides() => RunTest();

		[Test]
		public void FromTarget_AbstractToAbstract_MultipleOverrides() => RunTest();

		[Test]
		public void TwoWay_AbstractToAbstract_AbstractBindingInitializer_MultipleOverrides() => RunTest();

		[Test]
		public void OneWay_AbstractToAbstract_MultipleAbstracts() => RunTest();
	}
}
