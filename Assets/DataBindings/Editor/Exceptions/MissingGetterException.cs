namespace Realmar.DataBindings.Editor.Exceptions
{
	internal class MissingGetterException : MissingSymbolException
	{
		public override string Message => $"{SymbolName} is missing a getter.";

		internal MissingGetterException(string symbolName) : base(symbolName)
		{
		}
	}
}
