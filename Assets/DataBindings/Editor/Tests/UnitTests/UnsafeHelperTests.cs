using System;
using System.Reflection;
using System.Runtime.InteropServices;
using NUnit.Framework;
using Realmar.DataBindings.Editor.Shared;
using Realmar.DataBindings.Editor.Shared.Extensions;

namespace Realmar.DataBindings.Editor.Tests.UnitTests
{
	[TestFixture]
	internal class UnsafeHelperTests : MarshalByRefObject
	{
		private struct UnmanagedData
		{
			public int I;
			public int J;
		}

		private struct CascadedUnmanagedData
		{
			public int I;
			public int J;

			public UnmanagedData D1;
			public UnmanagedData D2;
			public UnmanagedData D3;
		}

		public class DataClass
		{
			public double Data { get; set; }
		}

		[Test]
		public void ReturnTypeIs_String() => AssertTypeIs(typeof(string));

		[Test]
		public void ReturnTypeIs_Int() => AssertTypeIs(typeof(int));

		[Test]
		public void ReturnTypeIs_Double() => AssertTypeIs(typeof(double));

		[Test]
		public void ReturnTypeIs_Struct() => AssertTypeIs(typeof(UnmanagedData));

		[Test]
		public void ReturnTypeIs_CascadedStruct() => AssertTypeIs(typeof(CascadedUnmanagedData));

		[Test]
		public void CorrectSize_Double()
		{
			for (var i = 100000 - 1; i >= 0; i--)
			{
				var result = UnsafeHelpers.GetRandomObjectOfType(typeof(double));
				var expected = sizeof(double);
				var actual = Marshal.SizeOf(result);

				Assert.That(actual, Is.EqualTo(expected));
			}
		}

		[Test]
		public void ValueIsUsableViaReflection()
		{
			var number = UnsafeHelpers.GetRandomObjectOfType(typeof(double));
			AssertNoThrow_SetDataViaReflection(number);
		}

		[Test]
		public void ValueIsUsableAcrossAppDomains()
		{
			AppDomain domain = null;
			try
			{
				domain = AppDomain.CreateDomain("TestCase Domain");
				domain.Load(Assembly.GetExecutingAssembly().GetName());

				var testClass = domain.CreateInstanceFromDeclaringAssemblyOfTypeAndUnwrap<UnsafeHelperTests>();
				var number = UnsafeHelpers.GetRandomObjectOfType(typeof(double));

				testClass.AssertNoThrow_SetDataViaReflection(number);
			}
			finally
			{
				if (domain != null)
				{
					AppDomain.Unload(domain);
				}
			}
		}

		private void AssertNoThrow_SetDataViaReflection(object number)
		{
			var data = new DataClass();
			var property = data.GetType().GetProperty(nameof(DataClass.Data));

			property.SetValue(data, number);
		}

		private void AssertTypeIs(Type type)
		{
			var result = UnsafeHelpers.GetRandomObjectOfType(type);
			Assert.That(result, Is.TypeOf(type));
		}
	}
}
