using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Realmar.DataBindings.Editor.TestFramework.Compilation;
using static Realmar.DataBindings.Editor.Exceptions.YeetHelpers;
using static UnityEngine.Debug;

namespace Realmar.DataBindings.Editor.TestFramework.Facades
{
	internal class WeaverTestFacade : IDisposable
	{
		private readonly Random _random = new Random();
		private readonly Compiler _compiler = new Compiler();
		private readonly Dictionary<Type, CodeProvider> _codeProviders = new Dictionary<Type, CodeProvider>();
		private readonly Dictionary<Type, string> _compiledAssemblies = new Dictionary<Type, string>();

		public void Dispose()
		{
			_compiler?.Dispose();
		}

		internal string CompileAndWeave(Type testType, string testName)
		{
			YeetIfNull(testName, nameof(testName));

			var @namespace = GetNamespaceForTest(testType, testName);
			var code = GetCodeProvider(testType).FilterNamespace(@namespace);
			var path = CompileAssembly(code);

			return WeaveAssembly(path);
		}

		internal string CompileAndWeave(Type testType)
		{
			var path = CompileAssembly(testType);
			return WeaveAssembly(path);
		}

		internal string GetNamespaceForTest(Type testType, string testName)
		{
			return $"{GetCodeProvider(testType).UUTNamespace}.{testType.Name}.{testName}";
		}

		private string CompileAssembly(Type testType)
		{
			if (_compiledAssemblies.TryGetValue(testType, out var path) == false)
			{
				path = CompileAssembly(GetCodeProvider(testType).GetCode());
				_compiledAssemblies[testType] = path;
			}

			return path;
		}

		private string CompileAssembly(string code)
		{
			try
			{
				return _compiler.Compile(code);
			}
			catch (CompilationException e)
			{
				var sb = new StringBuilder();
				foreach (CompilerError error in e.Errors)
				{
					sb.AppendLine(error.ToString());
				}

				LogError(sb.ToString());

				throw;
			}
		}

		private string WeaveAssembly(string path)
		{
			var facade = new BindingFacade(new BindingFacade.Options { WeaveDebugSymbols = false });
			var weavedPath = CreateWeavedDllFQName(path);
			facade.WeaveAssembly(path, weavedPath);

			return weavedPath;
		}

		private string CreateWeavedDllFQName(string originalFQName)
		{
			var fileInfo = new FileInfo(originalFQName);
			return Path.Combine(fileInfo.Directory.FullName, $"{fileInfo.Name}.{_random.Next()}.weaved.dll");
		}

		private CodeProvider GetCodeProvider(Type type)
		{
			if (_codeProviders.TryGetValue(type, out var codeProvider) == false)
			{
				codeProvider = new CodeProvider(type);
				_codeProviders[type] = codeProvider;
			}

			return codeProvider;
		}
	}
}
