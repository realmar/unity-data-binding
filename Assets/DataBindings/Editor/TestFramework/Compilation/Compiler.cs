using System;
using System.CodeDom.Compiler;
using System.Linq;
using System.Reflection;
using Realmar.DataBindings.Editor.Exceptions;
using Realmar.DataBindings.Editor.Shared.Extensions;

namespace Realmar.DataBindings.Editor.TestFramework.Compilation
{
	internal class Compiler : MarshalByRefObject, IDisposable
	{
		private AppDomain _domain;
		private Compiler _compiler;

		public void Dispose()
		{
			if (_domain != null)
			{
				AppDomain.Unload(_domain);
				_domain = null;
			}
		}

		internal string Compile(string code)
		{
			var compiler = GetCompiler();

			if (compiler == null)
			{
				throw new BigOOFException("Could not create compiler.");
			}

			var result = compiler.Compile_Internal(code);
			var errors = result.Errors;

			if (errors.HasErrors)
			{
				throw new CompilationException(errors);
			}

			var outputPath = result.PathToAssembly;
			UnityEngine.Debug.Log($"Compiled assembly: {result.PathToAssembly}");


			return outputPath;
		}

		private CompilerResults Compile_Internal(string code)
		{
			var loadedAssemblies = AppDomain.CurrentDomain.GetAssemblies();

			var provider = CodeDomProvider.CreateProvider("csharp");
			var options = new CompilerParameters();
			var referencedAssemblies = loadedAssemblies
				.Where(assembly => assembly.IsDynamic == false)
				.Select(assembly => assembly.Location).ToArray();
			options.ReferencedAssemblies.AddRange(referencedAssemblies);
			options.IncludeDebugInformation = true;

			return provider.CompileAssemblyFromSource(options, code);
		}

		private Compiler GetCompiler()
		{
			if (_compiler == null)
			{
				_domain = AppDomain.CreateDomain("Compiler");
				_domain.Load(Assembly.GetExecutingAssembly().GetName());
				_compiler = _domain.CreateInstanceFromDeclaringAssemblyOfTypeAndUnwrap<Compiler>();
			}

			return _compiler;
		}
	}
}
