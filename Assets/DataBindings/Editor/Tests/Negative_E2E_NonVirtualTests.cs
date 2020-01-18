using NUnit.Framework;
using Realmar.DataBindings.Editor.TestFramework.BaseTests;
using System;

namespace Realmar.DataBindings.Editor.Tests
{
	[TestFixture]
	internal class Negative_E2E_NonVirtualTests : SandboxedTest
	{
		[Test]
		public void FromTarget_BindingInitializerNotCalled() => RunTestExpectException<BindingTargetNullException>();

		[Test]
		public void OneTime_Throw_TargetNull() => RunTestExpectException<NullReferenceException>();
	}
}
