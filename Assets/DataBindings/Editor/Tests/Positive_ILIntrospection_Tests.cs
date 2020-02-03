using System.Linq;
using Mono.Cecil;
using Mono.Cecil.Cil;
using NUnit.Framework;
using Realmar.DataBindings.Editor.Cecil;
using Realmar.DataBindings.Editor.Exceptions;
using Realmar.DataBindings.Editor.TestFramework.Facades;

namespace Realmar.DataBindings.Editor.Tests
{
	[TestFixture]
	internal class Positive_ILIntrospection_Tests
	{
		private WeaverTestFacade _facade;

		[OneTimeSetUp]
		public void Setup()
		{
			_facade = new WeaverTestFacade();
		}

		[OneTimeTearDown]
		public void Teardown()
		{
			_facade?.Dispose();
		}

		[Test]
		public void OneWay_CallUsingCorrectBindingTargetType()
		{
			var weavedAssembly = _facade.CompileAndWeave(typeof(Positive_E2E_AbstractTests), nameof(Positive_E2E_AbstractTests.OneWay_CallToCorrectBindingTargetType));
			using (var assembly = ModuleDefinition.ReadModule(weavedAssembly))
			{
				var sourceType = assembly.Types.First(definition => definition.Name == "BaseSource");
				var bindingProperty = sourceType.GetProperty("Text");
				var fromSetter = bindingProperty.GetSetMethodOrYeet();

				foreach (var instruction in fromSetter.Body.Instructions)
				{
					if (instruction.OpCode == OpCodes.Callvirt)
					{
						var toSetter = (MethodDefinition) instruction.Operand;
						var toSetterType = toSetter.DeclaringType;

						Assert.That(toSetterType.FullName, Is.EqualTo("UnitsUnderTest.Positive_E2E_AbstractTests.OneWay_CallToCorrectBindingTargetType.BaseTarget"));
					}
				}
			}
		}
	}
}
