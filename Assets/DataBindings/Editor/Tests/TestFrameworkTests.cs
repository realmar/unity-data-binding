using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Realmar.DataBindings.Editor.TestFramework;

namespace Realmar.DataBindings.Editor.Tests
{
	[TestFixture]
	internal class TestFrameworkTests : SandboxedTest
	{
		public override void SetupFixture()
		{
			TestType = typeof(Positive_E2E_NonVirtualTests);
			base.SetupFixture();
		}

		[Test]
		public void VerifyBindingCount_OneToOne()
		{
			AssertBindingCount("OneWay_PropertyToProperty", 1);
		}

		[Test]
		public void VerifyBindingCount_OneToMany()
		{
			AssertBindingCount("OneWay_OneToMany", 3);
		}

		[Test]
		public void VerifyBindingCount_ManyToOne()
		{
			AssertBindingCount("OneWay_ManyToOne", 3);
		}

		[Test]
		public void VerifyBindingCount_MultipleBindingSources()
		{
			AssertBindingCount("OneWay_MultipleBindingsPerSource", 3);
		}

		[Test]
		public void VerifyDifferentBindingTargetObjects_OneToMany_CheckHashCode()
		{
			var sandbox = GetSandboxForTest("OneWay_OneToMany");
			AssertDifferentBindingTargetObjects(sandbox.Bindings);
		}

		[Test]
		public void VerifyBindingSetup_MultipleBindings()
		{
			var sandbox = GetSandboxForTest("OneWay_ManyToMany");
			var bindings = sandbox.Bindings.ToArray();

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

		private static void AssertDifferentBindingTargetObjects(IReadOnlyCollection<IBinding> bindings)
		{
			var targets = new HashSet<int>();

			foreach (var binding in bindings)
			{
				var hashCode = binding.Target.GetHashCodeOfObject();
				Assert.That(targets.Contains(hashCode), Is.False);
				targets.Add(hashCode);
			}
		}

		private void AssertBindingCount(string testName, int expected)
		{
			var sandbox = GetSandboxForTest(testName);
			var bindings = sandbox.Bindings;

			Assert.That(bindings.Count, Is.EqualTo(expected));
		}
	}
}
