using NUnit.Framework;
using Realmar.DataBindings.Editor.TestFramework;

namespace Realmar.DataBindings.Editor.Tests
{
	[TestFixture]
	internal class Positive_E2E_VirtualTests : SandboxedTest
	{
		[Test]
		public void OneWay_VirtualToProperty() => RunTest();

		[Test]
		public void OneWay_VirtualToVirtual() => RunTest();

		[Test]
		public void OneWay_VirtualToOverride() => RunTest(AssertCustomSymbol);

		// TODO this fails because source is overriden and thus base impl is never called. Because of that target will never be set.
		[Test]
		public void OneWay_OverrideToOverride() => RunTest(AssertCustomSymbol);

		[Test]
		public void OneWay_PropertyToManyOverride() => RunTest();

		[Test]
		public void FromTarget_PropertyToManyOverride() => RunTest();

		[Test]
		public void TwoWay_PropertyToManyOverride() => RunTest();

		[Test]
		public void TwoWay_PropertyToOverride() => RunTest(AssertCustomSymbol);

		[Test]
		public void TwoWay_OverrideToOverride() => RunTest(AssertCustomSymbol);

		[Test]
		public void FromTarget_PropertyToOverride() => RunTest(AssertCustomSymbol);

		[Test]
		public void FromTarget_OverrideToOverride() => RunTest(AssertCustomSymbol);

		[Test]
		public void TwoWay_OverrideToOverride_ManyToMany() => RunTest();

		private static void AssertCustomSymbol(IBinding binding, object expected)
		{
			var text = binding.Target.ReflectValue("_text");
			Assert.That(text, Is.EqualTo(expected));
		}
	}
}
