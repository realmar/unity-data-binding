using NUnit.Framework;
using Realmar.DataBindings.Editor.TestFramework;
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
		public void VerifyBindingCount_MultipleBindingSources()
		{
			AssertBindingCount(nameof(Positive_E2E_NonVirtualTests.OneWay_MultipleBindingsPerSource), 3);
		}

		[Test]
		public void VerifyBindingCount_MultipleSourceImplementations()
		{
			var sandbox = AssertBindingCount(
				nameof(Positive_E2E_InterfaceTests.TwoWay_InterfaceToProperty_MultipleSources),
				3,
				typeof(Positive_E2E_InterfaceTests));

			Assert.That(sandbox.BindingSets.Count, Is.EqualTo(3));
		}

		[Test]
		public void VerifyBindingCount_MultipleSourceImplementations_CheckNames()
		{
			var sandbox = GetSandboxForTest(
				nameof(Positive_E2E_InterfaceTests.TwoWay_InterfaceToProperty_MultipleSources),
				typeof(Positive_E2E_InterfaceTests));

			var sourceTypeCount = sandbox.BindingSets
				.SelectMany(set => set.Bindings)
				.Select(binding => binding.Source.DeclaringTypeFQDN)
				.Count();

			Assert.That(sourceTypeCount, Is.EqualTo(3));
		}

		[Test]
		public void VerifyDifferentBindingTargetObjects_OneToMany_CheckHashCode()
		{
			var sandbox = GetSandboxForTest(nameof(Positive_E2E_NonVirtualTests.OneWay_OneToMany), _testType);
			AssertDifferentBindingTargetObjects(FlattenBindings(sandbox.BindingSets));
		}

		[Test]
		public void VerifyBindingSetup_MultipleBindings()
		{
			var sandbox = GetSandboxForTest(nameof(Positive_E2E_NonVirtualTests.OneWay_ManyToMany), _testType);
			var bindings = FlattenBindings(sandbox.BindingSets).ToArray();

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

		private static void AssertDifferentBindingTargetObjects(IEnumerable<IBinding> bindings)
		{
			var targets = new HashSet<int>();

			foreach (var binding in bindings)
			{
				var hashCode = binding.Target.GetHashCodeOfObject();
				Assert.That(targets.Contains(hashCode), Is.False);
				targets.Add(hashCode);
			}
		}

		private IUnitUnderTestSandbox AssertBindingCount(string testName, int expected, Type testType = null)
		{
			var sandbox = GetSandboxForTest(testName, testType ?? _testType);
			var bindings = FlattenBindings(sandbox.BindingSets);

			Assert.That(bindings.Count, Is.EqualTo(expected));

			return sandbox;
		}

		private IEnumerable<IBinding> FlattenBindings(IReadOnlyCollection<IBindingSet> bindingSets)
			=> bindingSets.SelectMany(set => set.Bindings);
	}
}
