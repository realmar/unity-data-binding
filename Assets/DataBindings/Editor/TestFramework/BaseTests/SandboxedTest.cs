using NUnit.Framework;
using System;
using System.Reflection;
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

		protected string GetRandomString()
		{
			return _random.Next().ToString();
		}

		protected IUnitUnderTestSandbox GetSandboxForTest([CallerMemberName] string testName = null, Type testType = null)
		{
			YeetIfNull(testName, nameof(testName));
			var type = testType ?? GetType();

			return _sandboxTestFacade.GetSandboxForTest(type, testName);
		}

		protected void RunTest(Type testType, string testName)
		{
			RunTest(testType, testName, (Action<IBinding, object>) null);
		}

		protected void RunTest([CallerMemberName] string testName = null)
		{
			RunTest(GetType(), testName, (Action<IBinding, object>) null);
		}

		protected void RunTest(Action<IBinding, object> customAssertion, [CallerMemberName] string testName = null)
		{
			RunTest(GetType(), testName, customAssertion);
		}

		protected void RunTest(Type testType, string testName, Action<IBinding, object> customAssertion)
		{
			RunTest(testType, testName, RunDefaultAssertions, customAssertion);
		}

		protected void RunTest(Type testType, string testName, Func<IBinding, object> assertions, Action<IBinding, object> customAssertion)
		{
			var localAssertions = assertions;
			var localCustomAssertions = customAssertion;

			RunTest(testType, testName, bindingSet =>
			{
				bindingSet.RunBindingInitializer();

				foreach (var binding in bindingSet.Bindings)
				{
					var expected = localAssertions.Invoke(binding);
					localCustomAssertions?.Invoke(binding, expected);
				}
			});
		}

		protected void RunTest(Action<IBindingSet> bindingSetAssertions, [CallerMemberName] string testName = null)
		{
			RunTest(GetType(), testName, bindingSetAssertions);
		}

		protected void RunTest(Type testType, string testName, Action<IBindingSet> bindingSetAssertion)
		{
			YeetIfNull(testName, nameof(testName));
			var sandbox = _sandboxTestFacade.GetSandboxForTest(testType, testName);

			foreach (var bindingSet in sandbox.BindingCollection.BindingSets)
			{
				bindingSetAssertion.Invoke(bindingSet);
			}
		}

		protected void RunTest(Action<IBinding> assertions, [CallerMemberName] string testName = null)
		{
			RunTest(GetType(), testName, binding =>
			{
				assertions.Invoke(binding);
				return null;
			}, null);
		}

		protected void RunTest(Action<IBindingCollection> bindingSetAssertions, [CallerMemberName] string testName = null)
		{
			YeetIfNull(testName, nameof(testName));
			var sandbox = _sandboxTestFacade.GetSandboxForTest(GetType(), testName);
			bindingSetAssertions.Invoke(sandbox.BindingCollection);
		}

		protected void RunTestExpectException<TException>([CallerMemberName] string testName = null)
			where TException : Exception
		{
			RunMethodExpectException<TException>(() => RunTest(testName));
		}

		protected void RunMethodExpectException<TException>(Action method)
		{
			Exception actual = null;

			try
			{
				method.Invoke();
			}
			catch (TargetInvocationException e)
			{
				actual = e.InnerException;
			}
			catch (Exception e)
			{
				actual = e;
			}

			Assert.That(actual, Is.Not.Null, "Test did not throw any exceptions.");
			Assert.That(actual, Is.TypeOf<TException>());
		}

		private object RunDefaultAssertions(IBinding binding)
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

			return expected;
		}


		private object AssertOneWayFromTarget(IBinding binding)
		{
			return SetAndAssert(binding.Target, binding.Source);
		}

		private object AssertOneWay(IBinding binding)
		{
			return SetAndAssert(binding.Source, binding.Target);
		}

		private object SetAndAssert(IUUTBindingObject source, IUUTBindingObject target)
		{
			var actual = SetValue(source);
			AssertValue(target, actual);

			return actual;
		}

		private object SetValue(IUUTBindingObject symbol, object value = null)
		{
			value = value ?? GetRandomString();
			symbol.BindingValue = value;

			return value;
		}

		private void AssertValue(IUUTBindingObject symbol, object expected)
		{
			var actual = symbol.BindingValue;
			Assert.That(actual, Is.EqualTo(expected));
		}
	}
}
