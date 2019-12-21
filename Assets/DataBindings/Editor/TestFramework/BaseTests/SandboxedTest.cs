using NUnit.Framework;
using System;
using System.Runtime.CompilerServices;
using Realmar.DataBindings.Editor.TestFramework.Facades;
using Realmar.DataBindings.Editor.TestFramework.Sandbox;
using static Realmar.DataBindings.Editor.Exceptions.YeetHelpers;
using Assert = NUnit.Framework.Assert;

namespace Realmar.DataBindings.Editor.TestFramework.BaseTests
{
	internal class SandboxedTest
	{
		private readonly SandboxTestFacade _sandboxTestFacade = new SandboxTestFacade();
		private readonly Random _random = new Random();

		[OneTimeSetUp]
		public virtual void SetupFixture()
		{
		}

		[OneTimeTearDown]
		public virtual void TeardownFixture()
		{
			_sandboxTestFacade?.Dispose();
		}

		protected IUnitUnderTestSandbox GetSandboxForTest([CallerMemberName] string testName = null, Type testType = null)
		{
			YeetIfNull(testName, nameof(testName));
			var type = testType ?? GetType();

			return _sandboxTestFacade.GetSandboxForTest(type, testName);
		}

		protected void RunTest([CallerMemberName] string testName = null)
		{
			RunTest(null, testName);
		}

		protected void RunTest(Action<IBinding, object> customAssertion, [CallerMemberName] string testName = null)
		{
			YeetIfNull(testName, nameof(testName));
			var sandbox = _sandboxTestFacade.GetSandboxForTest(GetType(), testName);

			foreach (var bindingSet in sandbox.BindingSets)
			{
				bindingSet.RunBindingInitializer();

				foreach (var binding in bindingSet.Bindings)
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
	}
}
