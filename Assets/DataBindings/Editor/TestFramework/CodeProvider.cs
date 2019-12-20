using System;
using System.IO;
using System.Text;
using UnityEngine;

namespace Realmar.DataBindings.Editor.TestFramework
{
	internal class CodeProvider
	{
		private const string TestClassSuffix = "Tests";
		private const string UUTSuffix = "UnitUnderTest";

		private readonly string UUTFolderPath =
			Path.Combine(Application.dataPath, "DataBindings", "Editor", "TestFramework", "UnitsUnderTest");

		private Type _testClass;

		public CodeProvider(Type testClass)
		{
			_testClass = testClass;
		}

		internal string UUTNamespace { get; } = "UnitsUnderTest";

		private string UUTName
		{
			get
			{
				var name = _testClass.Name;
				if (name.EndsWith(TestClassSuffix) == false)
					throw new ArgumentException(
						$"Unit test classes need to have \"{TestClassSuffix}\" suffix. Class {name} is missing this suffix.");

				return name.Substring(0, name.Length - TestClassSuffix.Length);
			}
		}

		private string UUTFilePath => Path.Combine(UUTFolderPath, $"{UUTName}{UUTSuffix}.cs");

		internal string GetCode()
		{
			if (File.Exists(UUTFilePath) == false)
				throw new ArgumentException(
					$"Could not find UUT for test {_testClass.Name} at path {UUTFilePath}." +
					$"UUT files must be placed in the {UUTFolderPath} folder and have the same name as the corresponding unit test class but with the {UUTSuffix} suffix." +
					$"Eg. unit test class name: \"Example{TestClassSuffix}\" the UUT filename should be \"Example{UUTSuffix}.cs\"");

			return ExposeSymbols(File.ReadAllText(UUTFilePath));
		}

		internal string FilterNamespace(string @namespace)
		{
			var code = GetCode();
			var sb = new StringBuilder();

			using (var reader = new StringReader(code))
			{
				var withinTargetNamespace = false;
				string line;
				while ((line = reader.ReadLine()) != null)
				{
					if (line.StartsWith("using "))
					{
						sb.AppendLine(line);
					}
					else if (line.StartsWith($"namespace {@namespace}"))
					{
						sb.AppendLine(line);
						withinTargetNamespace = true;
					}
					else if (withinTargetNamespace && line.StartsWith("}"))
					{
						sb.AppendLine(line);
						break;
					}
					else if (withinTargetNamespace)
					{
						sb.AppendLine(line);
					}
				}
			}

			return sb.ToString();
		}

		private string ExposeSymbols(string sourceCode)
		{
			return sourceCode
				.Replace("internal class", "public class")
				.Replace("internal abstract class", "public abstract class")
				.Replace("internal new class", "public new class")
				.Replace("internal interface", "public interface");
		}
	}
}
