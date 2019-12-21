using System;

namespace Realmar.DataBindings.Editor.Exceptions
{
	internal class MissingSymbolException : Exception
	{
		internal string SymbolName { get; }

		internal MissingSymbolException(string symbolName)
		{
			SymbolName = symbolName;
		}
	}
}
