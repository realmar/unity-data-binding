using NUnit.Framework;
using Realmar.DataBindings.Editor.TestFramework.BaseTests;
using Realmar.DataBindings.Editor.TestFramework.Sandbox;
using System.Linq;

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
		public void OneTime_ManyToOne() => RunTest();

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
		public void OneTime_NoThrow_TargetNull() => RunTest(set =>
		{
			set.RunBindingInitializer();
			// implicit no throw
		});

		[Test]
		public void FromTarget_NullCheck_NoThrow_VerifyCustomCodeExecuted() => RunTest(binding => AssertCustomSymbol(binding.Source, 42));

		[TestCase(-1, 1)]
		[TestCase(0, 2)]
		[TestCase(1, 3)]
		public void FromTarget_NullCheck_NoThrow_VerifyCustomCodeExecuted_Branching(int setValue, int expected) => RunTest(binding =>
		{
			binding.Source.SetValue("_sample", setValue);
			binding.Source.Invoke("InitializeBindings");
			var actual = binding.Source.GetValue("_result");

			Assert.That(actual, Is.EqualTo(expected));
		});

		[Test]
		public void FromTarget_Throw_TargetNull_VerifyCustomCodeExecuted() => RunTest(bindingSet =>
		{
			RunMethodExpectException<BindingTargetNullException>(bindingSet.RunBindingInitializer);

			var binding = bindingSet.Bindings.First();
			AssertCustomSymbol(binding.Source, 42);
		});

		[Test]
		public void OneWay_Binding_NullCheck_CustomLogicExecuted() => RunTest(binding =>
		{
			binding.Source.BindingValue = GetRandomString();
			AssertCustomSymbol(binding.Source, 42);
		});

		[Test]
		public void TwoWay_Binding_CustomLogicExecuted() => RunTest((binding, o) =>
		{
			AssertCustomSymbol(binding.Source, 42);
			AssertCustomSymbol(binding.Target, 69);
		});

		[Test]
		public void FromTarget_OneWay_DoubleBinding() => RunTest(bindingCollection =>
		{
			bindingCollection.RunAllBindingInitializers();

			var view = bindingCollection.GetSymbol("View");

			var expected = GetRandomString();
			view.SetValue("Text", expected);

			AssertUUTObjects(bindingCollection, "Text", expected);
		});

		[Test]
		public void TwoWay_PropertyToProperty_Ensure_Setter_Called_Once() => RunTest(binding =>
		{
			var expected = GetRandomString();

			binding.Source.BindingValue = expected;
			Assert.That(binding.Target.BindingValue, Is.EqualTo(expected));

			Assert.That(binding.Target.GetValue("_counter"), Is.EqualTo(1));
			Assert.That(binding.Source.GetValue("_counter"), Is.EqualTo(1), "source should not be set when setting target");

			expected = GetRandomString();
			binding.Target.BindingValue = expected;
			Assert.That(binding.Source.BindingValue, Is.EqualTo(expected));

			Assert.That(binding.Source.GetValue("_counter"), Is.EqualTo(2));
			Assert.That(binding.Target.GetValue("_counter"), Is.EqualTo(2), "target should not be set when setting source");
		});

		[Test]
		public void TwoWay_ChainedBindings() => RunTest(bindingCollection =>
		{
			bindingCollection.RunAllBindingInitializers();

			var expected = GetRandomString();
			var a = bindingCollection.GetSymbol("A");
			a.SetValue("Text", expected);

			// OneWay
			AssertUUTObjects(bindingCollection, "Text", expected);

			expected = GetRandomString();
			var e = bindingCollection.GetSymbol("E");
			e.SetValue("Text", expected);

			// FromTarget
			AssertUUTObjects(bindingCollection, "Text", expected);
		});

		private static void AssertUUTObjects(IBindingCollection bindingCollection, string symbolName, string expected)
		{
			foreach (var uutObject in bindingCollection.GetSymbols())
			{
				var actual = uutObject.GetValue(symbolName);
				Assert.That(actual, Is.EqualTo(expected));
			}
		}

		private static void AssertCustomSymbol(IUUTBindingObject binding, int expected)
		{
			var value = binding.GetValue("_sample");
			Assert.That(value, Is.EqualTo(expected));
		}
	}
}
