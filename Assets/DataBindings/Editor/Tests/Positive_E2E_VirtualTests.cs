using NUnit.Framework;
using Realmar.DataBindings.Editor.TestFramework.BaseTests;
using Realmar.DataBindings.Editor.TestFramework.Sandbox;
using Realmar.DataBindings.Editor.TestFramework.Sandbox.Visitors;

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
		public void OneWay_VirtualToOverride() => RunTest<IPropertyBinding>(AssertCustomSymbol);

		[Test]
		public void OneWay_OverrideToOverride() => RunTest<IPropertyBinding>(AssertCustomSymbol);

		[Test]
		public void OneWay_PropertyToManyOverride() => RunTest();

		[Test]
		public void FromTarget_PropertyToManyOverride() => RunTest();

		[Test]
		public void TwoWay_PropertyToManyOverride() => RunTest();

		[Test]
		public void TwoWay_PropertyToOverride() => RunTest<IPropertyBinding>(AssertCustomSymbol);

		[Test]
		public void TwoWay_OverrideToOverride() => RunTest<IPropertyBinding>(AssertCustomSymbol);

		[Test]
		public void FromTarget_PropertyToOverride() => RunTest<IPropertyBinding>(AssertCustomSymbol);

		[Test]
		public void FromTarget_OverrideToOverride() => RunTest<IPropertyBinding>(AssertCustomSymbol);

		[Test]
		public void TwoWay_OverrideToOverride_ManyToMany() => RunTest();

		[Test]
		public void TwoWay_OverrideToOverride_PrivateBindingInitializer() => RunTest();

		[Test]
		public void TwoWay_OverrideBindingInitializer_WithBaseCall() => RunTest();

		private static void AssertCustomSymbol(IPropertyBinding binding, IAssertionToolbox toolbox)
		{
			toolbox.BindingSet.RunBindingInitializer();
			var expected = toolbox.RunDefaultAssertions(binding);
			var text = binding.Target.GetValue("_text");
			Assert.That(text, Is.EqualTo(expected));
		}
	}
}
