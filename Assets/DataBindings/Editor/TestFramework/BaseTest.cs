using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using NUnit.Framework;
using static Realmar.DataBindings.Editor.YeetHelpers;

namespace Realmar.DataBindings.Editor.TestFramework
{
	internal class BaseTest
	{
		private Compiler _compiler;
		private CodeProvider _codeProvider;

		private UUTSandboxFactory _uutSandboxFactory;
		private IUnitUnderTestSandbox _sandbox;

		protected virtual bool AutoWeave { get; } = true;

		[OneTimeSetUp]
		public virtual void SetupFixture()
		{
			_compiler = new Compiler();
			_codeProvider = new CodeProvider();

			if (AutoWeave)
			{
				Weave();
			}
		}

		[OneTimeTearDown]
		public virtual void TeardownFixture()
		{
			DisposeSandbox();
		}

		private void DisposeSandbox()
		{
			_sandbox = null;
			_uutSandboxFactory?.Dispose();
		}

		protected void Weave()
		{
			try
			{
				DisposeSandbox();

				var testClass = GetType();
				var code = _codeProvider.GetCode(testClass);
				var path = _compiler.Compile(code);

				var facade = new BindingFacade(new BindingFacade.Options {WeaveDebugSymbols = false});
				var weavedPath = CreateWeavedDllFQName(path);
				facade.WeaveAssembly(path, weavedPath);

				_uutSandboxFactory = new UUTSandboxFactory();
				_sandbox = _uutSandboxFactory.CreateSandbox();
				_sandbox.InitializeSandbox(weavedPath);
			}
			catch (CompilationException e)
			{
				var sb = new StringBuilder();
				foreach (CompilerError error in e.Errors)
				{
					sb.AppendLine(error.ToString());
				}

				UnityEngine.Debug.LogError(sb.ToString());

				throw;
			}
			catch (Exception e)
			{
				UnityEngine.Debug.LogException(e);
				throw;
			}
		}

		protected IUnitUnderTestSandbox GetSandboxForCurrentTest([CallerMemberName] string testName = null)
		{
			YeetIfNull(testName, nameof(testName));
			ConfigureSandboxFor(testName);

			return _sandbox;
		}

		protected void RunTest([CallerMemberName] string testName = null)
		{
			YeetIfNull(testName, nameof(testName));
			ConfigureSandboxFor(testName);

			var bindings = _sandbox.GetBindings();
			_sandbox.RunBindingInitializer();

			foreach (var binding in bindings)
			{
				switch (binding.BindingAttribute.BindingType)
				{
					case BindingType.OneWay:
						AssertOneWay(binding);
						break;
					case BindingType.TwoWay:
						AssertOneWay(binding);
						AssertOneWayFromTarget(binding);
						break;
					case BindingType.OneWayFromTarget:
						AssertOneWayFromTarget(binding);
						break;
				}
			}
		}

		private void AssertOneWayFromTarget(IBinding binding)
		{
			var expected = "Example Sample";
			binding.SetTargetProperty(expected);
			var actual = binding.GetSourceProperty();

			Assert.That(expected, Is.EqualTo(actual));
		}

		private void AssertOneWay(IBinding binding)
		{
			var expected = "Hello World";
			binding.SetSourceProperty(expected);
			var actual = binding.GetTargetProperty();

			Assert.That(expected, Is.EqualTo(actual));
		}

		private void ConfigureSandboxFor(string testName)
		{
			_sandbox.ChangeNamespace(GetNamespaceForTest(testName));
		}

		private string GetNamespaceForTest(string testName)
		{
			return $"{_codeProvider.UUTNamespace}.{GetType().Name}.{testName}";
		}

		private string CreateWeavedDllFQName(string originalFQName)
		{
			var fileInfo = new FileInfo(originalFQName);
			return Path.Combine(fileInfo.Directory.FullName, $"{fileInfo.Name}.weaved.dll");
		}
	}
}
