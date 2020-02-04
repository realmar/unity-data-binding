using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Realmar.DataBindings.Editor.TestFramework.BaseTests;
using Realmar.DataBindings.Editor.TestFramework.Sandbox;

namespace Realmar.DataBindings.Editor.Tests
{
	[TestFixture]
	internal class TestFrameworkTests : SandboxedTest
	{
		private readonly Type _testType = typeof(Positive_E2E_NonVirtualTests);

		[Test]
		public void DindDong()
		{
			var collection = GetBindingCollectionForTest(nameof(Positive_E2E_NonVirtualTests.TwoWay_ChainedBindings), _testType);
			var set = (BindingSet)collection.BindingSets.First();
			var b = set.Bindings;
		}

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
			var collection = GetBindingCollectionForTest(nameof(Positive_E2E_NonVirtualTests.TwoWay_ChainedBindings), _testType);

			Assert.That(collection.BindingSets.Count, Is.EqualTo(2));
			Assert.That(collection.BindingSets.SelectMany(set => set.Bindings).Count(), Is.EqualTo(4));
			Assert.That(collection.GetSymbols().Count, Is.EqualTo(5));
		}

		[Test]
		public void VerifyBindingCount_MultipleSourceImplementations()
		{
			var collection = AssertBindingCount(
				nameof(Positive_E2E_InterfaceTests.TwoWay_InterfaceToProperty_MultipleSources),
				3,
				typeof(Positive_E2E_InterfaceTests));

			Assert.That(collection.BindingSets.Count, Is.EqualTo(3));
		}

		[Test]
		public void VerifyBindingCount_MultipleSourceImplementations_CheckNames()
		{
			var collection = GetBindingCollectionForTest(
				nameof(Positive_E2E_InterfaceTests.TwoWay_InterfaceToProperty_MultipleSources),
				typeof(Positive_E2E_InterfaceTests));

			var sourceTypeCount = collection.BindingSets
				.SelectMany(set => set.Bindings)
				.Cast<IPropertyBinding>()
				.Select(binding => binding.Source.DeclaringTypeFQDN)
				.Count();

			Assert.That(sourceTypeCount, Is.EqualTo(3));
		}

		[Test]
		public void VerifyDifferentBindingTargetObjects_OneToMany_CheckHashCode()
		{
			var collection = GetBindingCollectionForTest(nameof(Positive_E2E_NonVirtualTests.OneWay_OneToMany), _testType);
			AssertDifferentBindingTargetObjects(FlattenBindings(collection.BindingSets).Cast<IPropertyBinding>());
		}

		[Test]
		public void VerifyBindingSetup_MultipleBindings()
		{
			var collection = GetBindingCollectionForTest(nameof(Positive_E2E_NonVirtualTests.OneWay_ManyToMany), _testType);
			var bindings = FlattenBindings(collection.BindingSets).Cast<IPropertyBinding>().ToArray();

			Assert.That(bindings.Count, Is.EqualTo(3));
			AssertDifferentBindingTargetObjects(bindings);

			for (var i = 0; i < bindings.Length; i++)
			{
				var binding = (IPropertyBinding) bindings[i];
				binding.Source.BindingValue = string.Empty;
				Assert.That(binding.Target.BindingValue, Is.EqualTo(string.Empty));

				for (var j = i + 1; j < bindings.Length; j++)
				{
					var otherBinding = (IPropertyBinding) bindings[j];
					Assert.That(otherBinding.Target.BindingValue, Is.Null);
				}
			}
		}

		[Test]
		public void Verify_BindingTargetNotConfigured()
		{
			var collection = GetBindingCollectionForTest(nameof(Positive_E2E_NonVirtualTests.FromTarget_NoThrow_TargetNull_Cgt_Un), _testType);
			var bindingSet = collection.BindingSets.First();
			var b = bindingSet.Bindings;

			// var binding = bindingSet.Bindings.Cast<IPropertyBinding>().First();
			// var btValue = binding.Source.GetValue("BindingTarget");
			// Assert.That(btValue, Is.Null);
		}

		[Test]
		public void Verify_NoClassesMarkedAsTargets()
		{
			var collection = GetBindingCollectionForTest(nameof(Positive_E2E_NonVirtualTests.OneWay_NullCheck_TargetNull_Cgt_Un), _testType);
			var bindingSet = collection.BindingSets.First();
			var binding = (IPropertyBinding) bindingSet.Bindings.First();
			var btValue = binding.Source.GetValue("BindingTarget");

			Assert.That(btValue, Is.Null);
		}

		[Test]
		public void Verify_ReuseTargetInstanceWithSameId()
		{
			var collection = GetBindingCollectionForTest(nameof(Positive_E2E_NonVirtualTests.TwoWay_ChainedBindings), _testType);
			var symbols = collection.GetSymbols();

			Assert.That(symbols.Count, Is.EqualTo(5));
		}

		private static void AssertDifferentBindingTargetObjects(IEnumerable<IPropertyBinding> bindings)
		{
			var targets = new HashSet<int>();

			foreach (var binding in bindings)
			{
				var hashCode = binding.Target.GetHashCodeOfSymbol();
				Assert.That(targets.Contains(hashCode), Is.False);
				targets.Add(hashCode);
			}
		}

		private IReadOnlyBindingCollection AssertBindingCount(string testName, int expected, Type testType = null)
		{
			var collection = GetBindingCollectionForTest(testName, testType ?? _testType);
			var bindings = FlattenBindings(collection.BindingSets);

			Assert.That(bindings.Count, Is.EqualTo(expected));

			return collection;
		}

		private IEnumerable<IBinding> FlattenBindings(IReadOnlyCollection<IBindingSet> bindingSets)
			=> bindingSets.SelectMany(set => set.Bindings);
	}
}
