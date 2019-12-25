namespace Realmar.DataBindings.Editor.Exceptions
{
	internal class MissingNonAbstractSymbolException : MissingSymbolException
	{
		public override string Message => $"Cannot find any non-abstract overriding symbols for {SymbolName}";

		internal MissingNonAbstractSymbolException(string symbolName) : base(symbolName)
		{
		}
	}
}
