namespace Realmar.DataBindings.Editor.Exceptions
{
	internal class MissingSetterException : MissingSymbolException
	{
		public override string Message => $"{SymbolName} is missing a setter.";

		public MissingSetterException(string symbolName) : base(symbolName)
		{
		}
	}
}
