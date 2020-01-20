using NUnit.Framework;
using Realmar.DataBindings.Editor.TestFramework.Facades;
using Realmar.DataBindings.Editor.TestFramework.Sandbox;
using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using static Realmar.DataBindings.Editor.Exceptions.YeetHelpers;
using static Realmar.DataBindings.Editor.Shared.UnsafeHelpers;
using Assert = NUnit.Framework.Assert;

namespace Realmar.DataBindings.Editor.TestFramework.BaseTests
{
	internal class SandboxedTest
	{
		private readonly SandboxTestFacade _sandboxTestFacade = new SandboxTestFacade();

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
			return (string) GetRandomObjectOfType(typeof(string));
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

		private void RunTest(Type testType, string testName, Action<IBinding, object> customAssertion)
		{
			RunTest(testType, testName, RunDefaultAssertions, customAssertion);
		}

		private void RunTest(Type testType, string testName, Func<IBindingSet, IBinding, object> assertions, Action<IBinding, object> customAssertion)
		{
			var localAssertions = assertions;
			var localCustomAssertions = customAssertion;

			RunTest(testType, testName, bindingSet =>
			{
				bindingSet.RunBindingInitializer();

				foreach (var binding in bindingSet.Bindings)
				{
					var expected = localAssertions.Invoke(bindingSet, binding);
					localCustomAssertions?.Invoke(binding, expected);
				}
			});
		}

		protected void RunTest(Action<IBindingSet> bindingSetAssertions, [CallerMemberName] string testName = null)
		{
			RunTest(GetType(), testName, bindingSetAssertions);
		}

		private void RunTest(Type testType, string testName, Action<IBindingSet> bindingSetAssertions)
		{
			YeetIfNull(testName, nameof(testName));
			var sandbox = _sandboxTestFacade.GetSandboxForTest(testType, testName);

			foreach (var bindingSet in sandbox.BindingCollection.BindingSets)
			{
				bindingSetAssertions.Invoke(bindingSet);
			}
		}

		protected void RunTest(Action<IBinding> assertions, [CallerMemberName] string testName = null)
		{
			RunTest(GetType(), testName, (_, binding) =>
			{
				assertions.Invoke(binding);
				return null;
			}, null);
		}

		protected void RunTest(Action<IBindingCollection> bindingCollectionAssertions, [CallerMemberName] string testName = null)
		{
			YeetIfNull(testName, nameof(testName));
			var sandbox = _sandboxTestFacade.GetSandboxForTest(GetType(), testName);
			bindingCollectionAssertions.Invoke(sandbox.BindingCollection);
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

		private object RunDefaultAssertions(IBindingSet set, IBinding binding)
		{
			object expected;
			var bindingAttribute = binding.BindingAttribute;

			if (bindingAttribute.BindingType == BindingType.OneTime)
			{
				expected = SetValue(binding.Source, bindingAttribute.Converter);
				set.RunBindingInitializer();
				AssertValue(binding.Target, expected, bindingAttribute.Converter);
			}
			else
			{
				expected = RunDefaultAssertions(binding);
			}

			return expected;
		}

		private object RunDefaultAssertions(IBinding binding)
		{
			object expected;
			var bindingAttribute = binding.BindingAttribute;

			switch (bindingAttribute.BindingType)
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
						$"TestFramework does not support {nameof(BindingType)} {bindingAttribute.BindingType}");
			}

			return expected;
		}


		private object AssertOneWayFromTarget(IBinding binding)
		{
			return SetAndAssert(binding.Target, binding.Source, binding.BindingAttribute.Converter);
		}

		private object AssertOneWay(IBinding binding)
		{
			return SetAndAssert(binding.Source, binding.Target, binding.BindingAttribute.Converter);
		}

		private object SetAndAssert(IUUTBindingObject source, IUUTBindingObject target, Type converterType)
		{
			var actual = SetValue(source, converterType);
			AssertValue(target, actual, converterType);

			return actual;
		}

		private object SetValue(IUUTBindingObject symbol, Type converterType)
		{
			var value = GetRandomObjectOfType(symbol.BindingValueType);
			symbol.BindingValue = value;

			return value;
		}

		private void AssertValue(IUUTBindingObject symbol, object expected, Type converterType)
		{
			if (expected.GetType() != symbol.BindingValueType)
			{
				expected = Convert(converterType, expected);
			}

			var actual = symbol.BindingValue;
			Assert.That(actual, Is.EqualTo(expected));
		}

		private object Convert(Type converterType, object original)
		{
			if (converterType == null)
			{
				return original;
			}

			var converter = Activator.CreateInstance(converterType);
			return converter
				.GetType()
				.GetMethod("Convert", new[] { original.GetType() })
				.Invoke(converter, new[] { original });
		}
	}
}
