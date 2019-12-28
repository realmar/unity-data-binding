using NUnit.Framework;
using Realmar.DataBindings.Editor.TestFramework.BaseTests;

namespace Realmar.DataBindings.Editor.Tests
{
	[TestFixture]
	internal class Negative_E2E_NonVirtualTests : SandboxedTest
	{
		[Test]
		public void FromTarget_BindingInitializerNotCalled() => RunTestExpectException<BindingTargetNullException>();
	}
}
