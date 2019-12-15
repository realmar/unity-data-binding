using System;
using System.IO;
using UnityEngine;

namespace Realmar.DataBindings.Editor.TestFramework
{
	internal class CodeProvider
	{
		private const string TestClassSuffix = "Tests";
		private const string UUTSuffix = "UnitUnderTest";

		private readonly string UUTFolderPath =
			Path.Combine(Application.dataPath, "DataBindings", "Editor", "TestFramework", "UnitsUnderTest");

		internal string UUTNamespace { get; } = "UnitsUnderTest";

		internal string GetCode(Type testClass)
		{
			var uutName = GetUUTName(testClass);
			var uutFilePath = GetUUTFilePath(uutName);

			if (File.Exists(uutFilePath) == false)
			{
				throw new ArgumentException(
					$"Could not find UUT for test {testClass.Name} at path {uutFilePath}." +
					$"UUT files must be placed in the {UUTFolderPath} folder and have the same name as the corresponding unit test class but with the {UUTSuffix} suffix." +
					$"Eg. unit test class name: \"Example{TestClassSuffix}\" the UUT filename should be \"Example{UUTSuffix}.cs\"");
			}

			return ExposeSymbols(File.ReadAllText(uutFilePath));
		}

		private string GetUUTName(Type testClass)
		{
			var name = testClass.Name;
			if (name.EndsWith(TestClassSuffix) == false)
			{
				throw new ArgumentException(
					$"Unit test classes need to have \"{TestClassSuffix}\" suffix. Class {name} is missing this suffix.");
			}

			return name.Substring(0, name.Length - TestClassSuffix.Length);
		}

		private string GetUUTFilePath(string uutName)
		{
			return Path.Combine(UUTFolderPath, $"{uutName}{UUTSuffix}.cs");
		}

		private string ExposeSymbols(string sourceCode)
		{
			return sourceCode
				.Replace("internal class", "public class")
				.Replace("internal interface", "public interface");
		}
	}
}
