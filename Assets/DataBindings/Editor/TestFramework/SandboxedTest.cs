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
			YeetIfNull(testName, nameof(testName));
			ConfigureSandboxFor(testName);

			_sandbox.RunBindingInitializer();

			foreach (var binding in _sandbox.Bindings)
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
			var expected = _random.Next().ToString();
			binding.SetTargetProperty(expected);
			var actual = binding.GetSourceProperty();

			Assert.That(expected, Is.EqualTo(actual));
		}

		private void AssertOneWay(IBinding binding)
		{
			var expected = _random.Next().ToString();
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
