using NUnit.Framework;
using Realmar.DataBindings.Editor.Exceptions;
using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using static Realmar.DataBindings.Editor.YeetHelpers;
using static UnityEngine.Debug;
using Assert = NUnit.Framework.Assert;

namespace Realmar.DataBindings.Editor.TestFramework
{
	internal class CompiledTest
	{
		private Compiler _compiler;
		private CodeProvider _codeProvider;
		protected string WeavedPath { get; private set; }
		protected Type TestType { get; set; }

		[OneTimeSetUp]
		public virtual void SetupFixture()
		{
			if (TestType == null)
			{
				TestType = GetType();
			}

			_compiler = new Compiler();
			_codeProvider = new CodeProvider(TestType);
		}

		[OneTimeTearDown]
		public virtual void TeardownFixture()
		{
			_compiler?.Dispose();
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

			var exception = Assert.Throws<TException>(() => CompileAndWeave(testName));
			Assert.That(exception.SymbolName, Is.EqualTo(fullSymbolName));
			customAssertions?.Invoke(exception);
		}

		protected void CompileAndWeave([CallerMemberName] string testName = null)
		{
			YeetIfNull(testName, nameof(testName));

			var @namespace = GetNamespaceForTest(testName);
			var code = _codeProvider.FilterNamespace(@namespace);
			var path = CompileAssembly(code);
			WeavedPath = WeaveAssembly(path);
		}

		protected void CompileAndWeave()
		{
			var path = CompileAssembly();
			WeavedPath = WeaveAssembly(path);
		}

		protected string CompileAssembly()
		{
			return CompileAssembly(_codeProvider.GetCode());
		}

		protected string CompileAssembly(string code)
		{
			try
			{
				return _compiler.Compile(code);
			}
			catch (CompilationException e)
			{
				var sb = new StringBuilder();
				foreach (CompilerError error in e.Errors)
				{
					sb.AppendLine(error.ToString());
				}

				LogError(sb.ToString());

				throw;
			}
		}

		protected string GetNamespaceForTest(string testName)
		{
			return $"{_codeProvider.UUTNamespace}.{TestType.Name}.{testName}";
		}

		private string WeaveAssembly(string path)
		{
			var facade = new BindingFacade(new BindingFacade.Options {WeaveDebugSymbols = false});
			var weavedPath = CreateWeavedDllFQName(path);
			facade.WeaveAssembly(path, weavedPath);

			return weavedPath;
		}

		private string CreateWeavedDllFQName(string originalFQName)
		{
			var fileInfo = new FileInfo(originalFQName);
			return Path.Combine(fileInfo.Directory.FullName, $"{fileInfo.Name}.weaved.dll");
		}
	}
}
