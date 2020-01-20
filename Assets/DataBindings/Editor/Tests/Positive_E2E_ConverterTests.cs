using NUnit.Framework;
using Realmar.DataBindings.Editor.TestFramework.BaseTests;

namespace Realmar.DataBindings.Editor.Tests
{
	[TestFixture]
	internal class Positive_E2E_ConverterTests : SandboxedTest
	{
		[Test]
		public void OneWay_IntToString() => RunTest();

		[Test]
		public void TwoWay_IntToString() => RunTest();

		[Test]
		public void TwoWay_GenericConverter() => RunTest();
	}
}
