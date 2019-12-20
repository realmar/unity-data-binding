using System;

namespace Realmar.DataBindings.Editor.Exceptions
{
	internal class MissingSymbolException : Exception
	{
		internal string SymbolName { get; }

		public MissingSymbolException(string symbolName)
		{
			SymbolName = symbolName;
		}
	}
}
