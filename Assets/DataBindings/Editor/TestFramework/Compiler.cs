using System;
using System.CodeDom.Compiler;
using System.Linq;
using System.Reflection;
using Realmar.DataBindings.Editor.Extensions;

namespace Realmar.DataBindings.Editor.TestFramework
{
	internal class Compiler : MarshalByRefObject
	{
		internal string Compile(string source)
		{
			string outputPath;
			var domain = AppDomain.CreateDomain("Compiler");

			try
			{
				domain.Load(Assembly.GetExecutingAssembly().GetName());

				var compiler = domain.CreateInstanceFromDeclaringAssemblyOfTypeAndUnwrap<Compiler>();
				var result = compiler.Compile_Internal(source);
				var errors = result.Errors;

				if (errors.HasErrors)
				{
					throw new CompilationException(errors);
				}
				else
				{
					outputPath = result.PathToAssembly;
					UnityEngine.Debug.Log($"Compiled assembly: {result.PathToAssembly}");
				}
			}
			finally
			{
				AppDomain.Unload(domain);
			}

			return outputPath;
		}

		private CompilerResults Compile_Internal(string source)
		{
			var loadedAssemblies = AppDomain.CurrentDomain.GetAssemblies();

			var provider = CodeDomProvider.CreateProvider("csharp");
			var options = new CompilerParameters();
			var referencedAssemblies = loadedAssemblies
				.Where(assembly => assembly.IsDynamic == false)
				.Select(assembly => assembly.Location).ToArray();
			options.ReferencedAssemblies.AddRange(referencedAssemblies);
			options.IncludeDebugInformation = true;

			return provider.CompileAssemblyFromSource(options, source);
		}
	}
}
