using NUnit.Framework;
using Realmar.DataBindings.Editor.TestFramework.BaseTests;

namespace Realmar.DataBindings.Editor.Tests
{
	[TestFixture]
	internal class Positive_E2E_InvokeOnChangeTests : SandboxedTest
	{
		[Test]
		public void NonAbstract_NoBindings() => RunTest();

		[Test]
		public void NonAbstract_NoBindings_ParameterlessMethod() => RunTest();
	}
}
