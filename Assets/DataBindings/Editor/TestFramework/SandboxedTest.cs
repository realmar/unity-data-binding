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
		private readonly Random _random = new Random();

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

		protected IUnitUnderTestSandbox GetSandboxForTest([CallerMemberName] string testName = null)
		{
			YeetIfNull(testName, nameof(testName));
			ConfigureSandboxFor(testName);

			return _sandbox;
		}

		protected void RunTest([CallerMemberName] string testName = null)
		{
			RunTest(null, testName);
		}

		protected void RunTest(Action<IBinding, object> customAssertion, [CallerMemberName] string testName = null)
		{
			YeetIfNull(testName, nameof(testName));
			ConfigureSandboxFor(testName);

			_sandbox.RunBindingInitializer();

			foreach (var binding in _sandbox.Bindings)
			{
				object expected;

				switch (binding.BindingAttribute.BindingType)
				{
					case BindingType.OneWay:
						expected = AssertOneWay(binding);
						break;
					case BindingType.TwoWay:
						AssertOneWay(binding);
						expected = AssertOneWayFromTarget(binding);
						break;
					case BindingType.OneWayFromTarget:
						expected = AssertOneWayFromTarget(binding);
						break;
					default:
						throw new NotSupportedException(
							$"TestFramework does not support {nameof(BindingType)} {binding.BindingAttribute.BindingType}");
				}

				customAssertion?.Invoke(binding, expected);
			}
		}

		private object AssertOneWayFromTarget(IBinding binding)
		{
			var expected = _random.Next().ToString();
			binding.Target.BindingValue = expected;
			var actual = binding.Source.BindingValue;

			Assert.That(actual, Is.EqualTo(expected));

			return expected;
		}

		private object AssertOneWay(IBinding binding)
		{
			var expected = _random.Next().ToString();
			binding.Source.BindingValue = expected;
			var actual = binding.Target.BindingValue;

			Assert.That(actual, Is.EqualTo(expected));

			return expected;
		}

		private void ConfigureSandboxFor(string testName)
		{
			_sandbox.ChangeNamespace(GetNamespaceForTest(testName));
		}
	}
}
