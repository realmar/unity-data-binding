using System;

namespace Realmar.DataBindings.Editor.Exceptions
{
	[Serializable]
	internal class MissingTargetMethodException : MissingSymbolException
	{
		internal MissingTargetMethodException(string symbolName) : base(symbolName)
		{
		}
	}
}
