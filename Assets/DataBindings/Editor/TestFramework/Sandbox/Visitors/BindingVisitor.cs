using System;
using NUnit.Framework;
using Realmar.DataBindings.Converters;
using Realmar.DataBindings.Editor.TestFramework.Sandbox.UUT;
using static Realmar.DataBindings.Editor.Shared.UnsafeHelpers;

namespace Realmar.DataBindings.Editor.TestFramework.Sandbox.Visitors
{
	internal abstract class BindingVisitor : MarshalByRefObject, IBindingVisitor, IAssertionToolbox
	{
		internal BindingVisitor(IBindingSet bindingSet)
		{
			BindingSet = bindingSet;
		}

		public IBindingSet BindingSet { get; }

		public abstract void Visit(IPropertyBinding binding);
		public abstract void Visit(IToMethodBinding binding);

		public object RunDefaultAssertions(IPropertyBinding binding)
		{
			object expected;
			var bindingAttribute = binding.BindingAttribute;

			if (bindingAttribute.BindingType == BindingType.OneTime)
			{
				expected = SetRandomValue(binding.Source, bindingAttribute.Converter);
				BindingSet.RunBindingInitializer();
				AssertValue(binding.Target, expected, bindingAttribute.Converter);
			}
			else
			{
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
			}

			return expected;
		}

		public object RunDefaultAssertions(IToMethodBinding binding)
		{
			var setValue = SetRandomValue(binding.Source);
			var resultExpected = binding.ResultAttribute.Expected;
			var result = binding.ResultObject.BindingValue;
			var expected = resultExpected ?? setValue;

			Assert.That(result, Is.EqualTo(expected));

			return expected;
		}

		private object AssertOneWayFromTarget(IPropertyBinding binding)
		{
			return SetAndAssert(binding.Target, binding.Source, binding.BindingAttribute.Converter);
		}

		private object AssertOneWay(IPropertyBinding binding)
		{
			return SetAndAssert(binding.Source, binding.Target, binding.BindingAttribute.Converter);
		}

		private object SetAndAssert(IUUTBindingObject source, IUUTBindingObject target, Type converterType = null)
		{
			var actual = SetRandomValue(source, converterType);
			AssertValue(target, actual, converterType);

			return actual;
		}

		private object SetRandomValue(IUUTBindingObject symbol, Type converterType = null)
		{
			var value = GetRandomObjectOfType(symbol.BindingValueType);
			symbol.BindingValue = value;

			return value;
		}

		private void AssertValue(IUUTBindingObject symbol, object expected, Type converterType = null)
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
				.GetMethod(nameof(IValueConverter<object, object>.Convert), new[] { original.GetType() })
				.Invoke(converter, new[] { original });
		}
	}
}
