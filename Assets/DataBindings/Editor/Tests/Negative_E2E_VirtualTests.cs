using NUnit.Framework;
using Realmar.DataBindings.Editor.TestFramework.BaseTests;
using System;

namespace Realmar.DataBindings.Editor.Tests
{
	[TestFixture]
	internal class Negative_E2E_VirtualTests : SandboxedTest
	{
		[Test]
		public void TwoWay_OverrideBindingInitializer_NoBaseCall() => RunTestExpectException<NullReferenceException>();
	}
}
