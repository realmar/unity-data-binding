using NUnit.Framework;
using Realmar.DataBindings.Editor.TestFramework.BaseTests;
using Realmar.DataBindings.Editor.TestFramework.Sandbox;
using Realmar.DataBindings.Editor.TestFramework.Sandbox.UUT;

namespace Realmar.DataBindings.Editor.Tests
{
	[TestFixture]
	internal class Positive_Integrity_VariousTests : SandboxedTest
	{
		[Test]
		public void TwoWay_SingleSetterCalls_Overrides() => RunTest<IPropertyBinding>((binding, toolbox) =>
		{
			toolbox.BindingSet.RunBindingInitializer();
			toolbox.RunDefaultAssertions(binding);

			AssertSetCounter(binding.Source, 2);
			AssertSetCounter(binding.Target, 2);
		});

		private static void AssertSetCounter(IUUTBindingObject symbol, int expectedCount)
		{
			var sourceSetCounter = symbol.GetValue("_textSetCounter");
			Assert.That(sourceSetCounter, Is.EqualTo(expectedCount));
		}
	}
}
