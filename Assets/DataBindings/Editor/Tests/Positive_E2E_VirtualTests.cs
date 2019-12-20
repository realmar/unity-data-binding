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
		public void OneWay_VirtualToOverride() => RunTest();
	}
}
