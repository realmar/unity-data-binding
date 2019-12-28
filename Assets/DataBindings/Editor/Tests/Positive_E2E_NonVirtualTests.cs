using System;
using System.Linq;
using System.Runtime.InteropServices;
using NUnit.Framework;
using Realmar.DataBindings.Editor.TestFramework.BaseTests;
using Realmar.DataBindings.Editor.TestFramework.Sandbox;


namespace Realmar.DataBindings.Editor.Tests
{
	[TestFixture]
	internal class Positive_E2E_NonVirtualTests : SandboxedTest
	{
		[Test]
		public void OneWay_PropertyToProperty() => RunTest();

		[Test]
		public void OneWay_FieldBindingTarget() => RunTest();

		[Test]
		public void OneWay_FromTarget() => RunTest();

		[Test]
		public void TwoWay_PropertyToProperty() => RunTest();

		[Test]
		public void OneWay_NonDefaultBindingName() => RunTest();

		[Test]
		public void OneWay_NonDefaultBindingTarget() => RunTest();

		[Test]
		public void OneWay_ManyToOne() => RunTest();

		[Test]
		public void OneWay_OneToMany() => RunTest();

		[Test]
		public void OneWay_ManyToMany() => RunTest();

		[Test]
		public void OneWay_MultipleBindingsPerSource() => RunTest();

		[Test]
		public void TwoWay_PrivateBindingInitializer() => RunTest();

		[Test]
		public void OneWay_NullCheck_TargetNotNull() => RunTest();

		[Test]
		public void OneWay_NullCheck_TargetNull() => RunTest(binding =>
		{
			binding.Source.BindingValue = GetRandomString();
			// implicit no throw
		});

		[Test]
		public void FromTarget_Throw_TargetNotNull() => RunTest();

		[Test]
		public void FromTarget_NoThrow_TargetNull() => RunTest(binding =>
		{
			binding.Target.BindingValue = GetRandomString();
			// implicit no throw

			var sourceValue = binding.Source.BindingValue;
			Assert.That(sourceValue, Is.Null);
		});

		[Test]
		public void FromTarget_NullCheck_NoThrow_VerifyCustomCodeExecuted() => RunTest(AssertCustomSymbol);

		[TestCase(-1, 1)]
		[TestCase(0, 2)]
		[TestCase(1, 3)]
		public void FromTarget_NullCheck_NoThrow_VerifyCustomCodeExecuted_Branching(int setValue, int expected) => RunTest(binding =>
		{
			binding.Source.SetValue("_sample", setValue);
			binding.Source.Invoke("InitializeBindings");
			var actual = binding.Source.ReflectValue("_result");

			Assert.That(actual, Is.EqualTo(expected));
		});

		[Test]
		public void FromTarget_Throw_TargetNull_VerifyCustomCodeExecuted() => RunTest(bindingSet =>
		{
			RunMethodExpectException<BindingTargetNullException>(bindingSet.RunBindingInitializer);

			var binding = bindingSet.Bindings.First();
			AssertCustomSymbol(binding);
		});

		[Test]
		public void OneWay_Binding_NullCheck_CustomLogicExecuted() => RunTest(binding =>
		{
			binding.Source.BindingValue = GetRandomString();
			AssertCustomSymbol(binding);
		});

		[Test]
		public void TwoWay_Binding_CustomLogicExecuted() => RunTest((binding, o) =>
		{
			{
				var value = binding.Source.ReflectValue("_sample");
				Assert.That(value, Is.EqualTo(42));
			}

			{
				var value = binding.Target.ReflectValue("_sample");
				Assert.That(value, Is.EqualTo(69));
			}
		});

		private static void AssertCustomSymbol(IBinding binding)
		{
			var value = binding.Source.ReflectValue("_sample");
			Assert.That(value, Is.EqualTo(42));
		}
	}
}
