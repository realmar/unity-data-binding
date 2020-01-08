using NUnit.Framework;
using Realmar.DataBindings.Editor.TestFramework.BaseTests;
using Realmar.DataBindings.Editor.TestFramework.Sandbox;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Realmar.DataBindings.Editor.Tests
{
	[TestFixture]
	internal class TestFrameworkTests : SandboxedTest
	{
		private readonly Type _testType = typeof(Positive_E2E_NonVirtualTests);

		[Test]
		public void VerifyBindingCount_OneToOne()
		{
			AssertBindingCount(nameof(Positive_E2E_NonVirtualTests.OneWay_PropertyToProperty), 1);
		}

		[Test]
		public void VerifyBindingCount_OneToMany()
		{
			AssertBindingCount(nameof(Positive_E2E_NonVirtualTests.OneWay_OneToMany), 3);
		}

		[Test]
		public void VerifyBindingCount_ManyToOne()
		{
			AssertBindingCount(nameof(Positive_E2E_NonVirtualTests.OneWay_ManyToOne), 3);
		}

		[Test]
		public void VerifyBindingCount_MultipleBinding_RunTime_Sources()
		{
			AssertBindingCount(nameof(Positive_E2E_NonVirtualTests.OneWay_MultipleBindingsPerSource), 3);
		}

		[Test]
		public void VerifyBindingCount_MultipleBinding_CompileTime_Sources()
		{
			var sandbox = GetSandboxForTest(nameof(Positive_E2E_NonVirtualTests.TwoWay_ChainedBindings), _testType);
			var collection = sandbox.BindingCollection;

			Assert.That(collection.BindingSets.Count, Is.EqualTo(2));
			Assert.That(collection.BindingSets.SelectMany(set => set.Bindings).Count(), Is.EqualTo(4));
			Assert.That(collection.GetSymbols().Count, Is.EqualTo(5));
		}

		[Test]
		public void VerifyBindingCount_MultipleSourceImplementations()
		{
			var sandbox = AssertBindingCount(
				nameof(Positive_E2E_InterfaceTests.TwoWay_InterfaceToProperty_MultipleSources),
				3,
				typeof(Positive_E2E_InterfaceTests));

			Assert.That(sandbox.BindingCollection.BindingSets.Count, Is.EqualTo(3));
		}

		[Test]
		public void VerifyBindingCount_MultipleSourceImplementations_CheckNames()
		{
			var sandbox = GetSandboxForTest(
				nameof(Positive_E2E_InterfaceTests.TwoWay_InterfaceToProperty_MultipleSources),
				typeof(Positive_E2E_InterfaceTests));

			var sourceTypeCount = sandbox.BindingCollection.BindingSets
				.SelectMany(set => set.Bindings)
				.Select(binding => binding.Source.DeclaringTypeFQDN)
				.Count();

			Assert.That(sourceTypeCount, Is.EqualTo(3));
		}

		[Test]
		public void VerifyDifferentBindingTargetObjects_OneToMany_CheckHashCode()
		{
			var sandbox = GetSandboxForTest(nameof(Positive_E2E_NonVirtualTests.OneWay_OneToMany), _testType);
			AssertDifferentBindingTargetObjects(FlattenBindings(sandbox.BindingCollection.BindingSets));
		}

		[Test]
		public void VerifyBindingSetup_MultipleBindings()
		{
			var sandbox = GetSandboxForTest(nameof(Positive_E2E_NonVirtualTests.OneWay_ManyToMany), _testType);
			var bindings = FlattenBindings(sandbox.BindingCollection.BindingSets).ToArray();

			Assert.That(bindings.Count, Is.EqualTo(3));
			AssertDifferentBindingTargetObjects(bindings);

			for (var i = 0; i < bindings.Length; i++)
			{
				var binding = bindings[i];
				binding.Source.BindingValue = string.Empty;
				Assert.That(binding.Target.BindingValue, Is.EqualTo(string.Empty));

				for (var j = i + 1; j < bindings.Length; j++)
				{
					var otherBinding = bindings[j];
					Assert.That(otherBinding.Target.BindingValue, Is.Null);
				}
			}
		}

		[Test]
		public void Verify_BindingTargetNotConfigured()
		{
			var sandbox = GetSandboxForTest(nameof(Positive_E2E_NonVirtualTests.FromTarget_NoThrow_TargetNull), _testType);
			var bindingSet = sandbox.BindingCollection.BindingSets.First();
			var binding = bindingSet.Bindings.First();
			var btValue = binding.Source.GetValue("BindingTarget");

			Assert.That(btValue, Is.Null);
		}

		[Test]
		public void Verify_NoClassesMarkedAsTargets()
		{
			var sandbox = GetSandboxForTest(nameof(Positive_E2E_NonVirtualTests.OneWay_NullCheck_TargetNull), _testType);
			var bindingSet = sandbox.BindingCollection.BindingSets.First();
			var binding = bindingSet.Bindings.First();
			var btValue = binding.Source.GetValue("BindingTarget");

			Assert.That(btValue, Is.Null);
		}

		[Test]
		public void Verify_ReuseTargetInstanceWithSameId()
		{
			var sandbox = GetSandboxForTest(nameof(Positive_E2E_NonVirtualTests.TwoWay_ChainedBindings), _testType);
			var collection = sandbox.BindingCollection;
			var symbols = collection.GetSymbols();

			Assert.That(symbols.Count, Is.EqualTo(5));
		}

		private static void AssertDifferentBindingTargetObjects(IEnumerable<IBinding> bindings)
		{
			var targets = new HashSet<int>();

			foreach (var binding in bindings)
			{
				var hashCode = binding.Target.GetHashCodeOfSymbol();
				Assert.That(targets.Contains(hashCode), Is.False);
				targets.Add(hashCode);
			}
		}

		private IUnitUnderTestSandbox AssertBindingCount(string testName, int expected, Type testType = null)
		{
			var sandbox = GetSandboxForTest(testName, testType ?? _testType);
			var bindings = FlattenBindings(sandbox.BindingCollection.BindingSets);

			Assert.That(bindings.Count, Is.EqualTo(expected));

			return sandbox;
		}

		private IEnumerable<IBinding> FlattenBindings(IReadOnlyCollection<IBindingSet> bindingSets)
			=> bindingSets.SelectMany(set => set.Bindings);
	}
}
