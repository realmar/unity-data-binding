using NUnit.Framework;
using Realmar.DataBindings.Editor.Exceptions;
using Realmar.DataBindings.Editor.TestFramework.BaseTests;

namespace Realmar.DataBindings.Editor.Tests
{
	[TestFixture]
	internal class Negative_Weaver_ConverterTests : WeaverTest
	{
		[Test]
		public void ConverterType_Interface() => AssertExceptionThrown<AbstractConverterException>();

		[Test]
		public void ConverterType_Abstract() => AssertExceptionThrown<AbstractConverterException>();

		[Test]
		public void IsNotAConverter() => AssertExceptionThrown<NotAConverterException>();

		[Test]
		public void NoDefaultCtor() => AssertExceptionThrown<MissingDefaultCtorException>();

		[Test]
		public void MismatchingConverterTypes() => AssertExceptionThrown<MismatchingConverterTypesException>();

		[Test]
		public void OpenGenericConverter() => AssertExceptionThrown<OpenGenericConverterNotSupported>();
	}
}
