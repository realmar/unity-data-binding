using System;
using System.Runtime.CompilerServices;
using NUnit.Framework;
using Realmar.DataBindings.Editor.Exceptions;
using Realmar.DataBindings.Editor.TestFramework.Facades;
using static Realmar.DataBindings.Editor.Exceptions.YeetHelpers;

namespace Realmar.DataBindings.Editor.TestFramework.BaseTests
{
	internal class WeaverTest
	{
		private readonly WeaverTestFacade _weaverTestFacade = new WeaverTestFacade();

		[OneTimeSetUp]
		public virtual void SetupFixture()
		{
		}

		[OneTimeTearDown]
		public virtual void TeardownFixture()
		{
			_weaverTestFacade?.Dispose();
		}

		protected void AssertMissingSymbolExceptionThrown<TException>(
			string fullSymbolName,
			[CallerMemberName] string testName = null)
			where TException : MissingSymbolException
		{
			AssertMissingSymbolExceptionThrown<TException>(fullSymbolName, null, testName);
		}

		protected void AssertMissingSymbolExceptionThrown<TException>(
			string fullSymbolName,
			Action<TException> customAssertions,
			[CallerMemberName] string testName = null)
			where TException : MissingSymbolException
		{
			YeetIfNull(testName, nameof(testName));

			var exception = Assert.Throws<TException>(
				() => _weaverTestFacade.CompileAndWeave(GetType(), testName));
			Assert.That(exception.SymbolName, Is.EqualTo(fullSymbolName));
			customAssertions?.Invoke(exception);
		}

		protected void AssertExceptionThrown<TException>([CallerMemberName] string testName = null)
			where TException : Exception
		{
			Assert.Throws<TException>(() => _weaverTestFacade.CompileAndWeave(GetType(), testName));
		}

		protected void AssetNoThrow([CallerMemberName] string testName = null)
		{
			_weaverTestFacade.CompileAndWeave(GetType(), testName);
		}
	}
}
