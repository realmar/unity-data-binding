using NUnit.Framework;
using Realmar.DataBindings.Editor.TestFramework;

namespace Realmar.DataBindings.Editor.Tests
{
	[TestFixture]
	internal class TestFrameworkTests : SandboxedTest
	{
		public override void SetupFixture()
		{
			TestType = typeof(Positive_E2E_NonVirtualTests);
			base.SetupFixture();
		}

		[Test]
		public void VerifyBindingCount_OneToOne() =>
			AssertBindingCount("OneWay_PropertyToProperty", 1);

		[Test]
		public void VerifyBindingCount_OneToMany() =>
			AssertBindingCount("OneWay_OneToMany", 3);

		[Test]
		public void VerifyBindingCount_ManyToOne() =>
			AssertBindingCount("OneWay_ManyToOne", 3);

		private void AssertBindingCount(string testName, int expected)
		{
			var sandbox = GetSandboxForTest(testName);
			var bindings = sandbox.Bindings;

			Assert.That(bindings.Count, Is.EqualTo(expected));
		}
	}
}
