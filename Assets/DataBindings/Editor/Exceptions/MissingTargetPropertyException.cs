namespace Realmar.DataBindings.Editor.Exceptions
{
	internal class MissingTargetPropertyException : MissingSymbolException
	{
		internal string SourceType { get; }
		public override string Message => $"{SourceType} is missing a BindingTarget with name {SymbolName}.";

		internal MissingTargetPropertyException(string sourceType, string symbolName) : base(symbolName)
		{
			SourceType = sourceType;
		}
	}
}
