using System;
using NUnit.Framework;
using Realmar.DataBindings.Editor.TestFramework.BaseTests;

namespace Realmar.DataBindings.Editor.Tests
{
	[TestFixture]
	internal class Negative_E2E_NonVirtualTests : SandboxedTest
	{
		[Test]
		public void FromTarget_BindingInitializerNotCalled() => RunTestExpectException<BindingTargetNullException>();

		[Test]
		public void OneTime_Throw_TargetNull_Using_Cgt_Un() => RunTestExpectException<NullReferenceException>();

		[Test]
		public void OneWay_Throw_TargetNull_Using_Cgt_Un() => RunTestExpectException<NullReferenceException>();

		[Test]
		public void OneWay_Throw_TargetNull_Using_op_Equality() => RunTestExpectException<NullReferenceException>();

		[Test]
		public void FromTarget_Throw_TargetNull_Using_op_Equality() => RunTestExpectException<BindingTargetNullException>();

		[Test]
		public void FromTarget_Throw_TargetNull_Using_Cgt_Un() => RunTestExpectException<BindingTargetNullException>();
	}
}
