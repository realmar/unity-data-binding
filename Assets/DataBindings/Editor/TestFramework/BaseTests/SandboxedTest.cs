using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using NUnit.Framework;
using Realmar.DataBindings.Editor.TestFramework.Facades;
using Realmar.DataBindings.Editor.TestFramework.Sandbox;
using Realmar.DataBindings.Editor.TestFramework.Sandbox.UUT;
using Realmar.DataBindings.Editor.TestFramework.Sandbox.Visitors;
using static Realmar.DataBindings.Editor.Exceptions.YeetHelpers;
using static Realmar.DataBindings.Editor.Shared.UnsafeHelpers;

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

		protected IReadOnlyBindingCollection GetBindingCollectionForTest(string testName, Type testType)
		{
			var sandbox = _sandboxTestFacade.GetSandboxForTest(testType, testName);
			return sandbox.BindingCollection;
		}

		protected IUnitUnderTestSandbox GetSandboxForTest([CallerMemberName] string testName = null, Type testType = null)
		{
			YeetIfNull(testName, nameof(testName));
			var type = testType ?? GetType();

			return _sandboxTestFacade.GetSandboxForTest(type, testName);
		}

		protected void RunTest([CallerMemberName] string testName = null)
		{
			RunTest(GetType(), testName);
		}

		private void RunTest(Type testType, string testName)
		{
			RunTestUsingVisitorFactory(testType, testName, set =>
			{
				set.RunBindingInitializer();
				return new DefaultBindingVisitor(set);
			});
		}

		protected void RunTest(Action<IBindingSet> bindingSetAssertions, [CallerMemberName] string testName = null)
		{
			RunTest(GetType(), testName, bindingSetAssertions);
		}

		protected void RunTest<TBinding>(VisitorTarget<TBinding> assertions, [CallerMemberName] string testName = null)
			where TBinding : IBinding<Attribute>
		{
			RunTestUsingVisitorFactory(GetType(), testName, set =>
			{
				var visitor = new PluggableBindingVisitor(set);
				visitor.ConfigureTarget(assertions);

				return visitor;
			});
		}

		protected void RunTest(Action<IReadOnlyBindingCollection> bindingCollectionAssertions, [CallerMemberName] string testName = null)
		{
			YeetIfNull(testName, nameof(testName));
			var sandbox = _sandboxTestFacade.GetSandboxForTest(GetType(), testName);
			bindingCollectionAssertions.Invoke(sandbox.BindingCollection);
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

		private void RunTestUsingVisitorFactory(Type testType, string testName, Func<IBindingSet, IBindingVisitor> factory)
		{
			var sandbox = _sandboxTestFacade.GetSandboxForTest(testType, testName);
			var collection = sandbox.BindingCollection;

			foreach (var set in collection.BindingSets)
			{
				var visitor = factory.Invoke(set);
				foreach (var binding in set.Bindings)
				{
					binding.Accept(visitor);
				}
			}
		}
	}
}
