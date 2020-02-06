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

		[Test]
		public void NonAbstract_WithBindings() => RunTest();

		[Test]
		public void Virtual_NoBindings() => RunTest();

		[Test]
		public void Override_NoBindings() => RunTest();

		[Test]
		public void Abstract_NoBindings() => RunTest();

		[Test]
		public void Interface_NoBindings() => RunTest();

		[Test]
		public void InterfaceAbstract_NoBindings() => RunTest();

		[Test]
		public void InterfaceInterfaceAbstract_NoBindings() => RunTest();
	}
}
