namespace Realmar.DataBindings.Editor.Exceptions
{
	internal class MissingGetterException : MissingSymbolException
	{
		public override string Message => $"{SymbolName} is missing a getter.";

		public MissingGetterException(string symbolName) : base(symbolName)
		{
		}
	}
}
