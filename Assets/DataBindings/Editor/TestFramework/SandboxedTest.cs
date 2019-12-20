using NUnit.Framework;
using System;
using System.Runtime.CompilerServices;
using static Realmar.DataBindings.Editor.YeetHelpers;
using static UnityEngine.Debug;
using Assert = NUnit.Framework.Assert;

namespace Realmar.DataBindings.Editor.TestFramework
{
	internal class SandboxedTest : CompiledTest
	{
		private UUTSandboxFactory _uutSandboxFactory;
		private IUnitUnderTestSandbox _sandbox;

		public override void SetupFixture()
		{
			base.SetupFixture();

			try
			{
				CompileAndWeave();
				CreateSandbox();
			}
			catch (Exception e)
			{
				LogException(e);
				throw;
			}
		}

		public override void TeardownFixture()
		{
			base.TeardownFixture();
			DisposeSandbox();
		}

		private void DisposeSandbox()
		{
			_sandbox = null;
			_uutSandboxFactory?.Dispose();
		}

		private void CreateSandbox()
		{
			DisposeSandbox();

			_uutSandboxFactory = new UUTSandboxFactory();
			_sandbox = _uutSandboxFactory.CreateSandbox();
			_sandbox.InitializeSandbox(WeavedPath);
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
	}
}
