namespace Realmar.DataBindings.Editor.Exceptions
{
	internal class MissingSetterException : MissingSymbolException
	{
		public override string Message => $"{SymbolName} is missing a setter.";

		internal MissingSetterException(string symbolName) : base(symbolName)
		{
		}
	}
}
