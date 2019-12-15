using System;
using System.CodeDom.Compiler;
using System.Text;

namespace Realmar.DataBindings.Editor.TestFramework
{
	public class CompilationException : Exception

	{
		public CompilerErrorCollection Errors { get; }

		public CompilationException(CompilerErrorCollection errors)
		{
			Errors = errors;
		}


		public override string ToString()
		{
			return FormatErrors(Errors);
		}

		private string FormatErrors(CompilerErrorCollection errors)
		{
			var sb = new StringBuilder();

			foreach (var error in errors)
			{
				sb.AppendLine(error.ToString());
			}

			return sb.ToString();
		}
	}
}
