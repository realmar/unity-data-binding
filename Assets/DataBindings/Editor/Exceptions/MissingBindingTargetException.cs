namespace Realmar.DataBindings.Editor.Exceptions
{
	internal class MissingBindingTargetException : MissingSymbolException
	{
		public int TargetId { get; }
		public override string Message => $"{SymbolName} is missing a BindingTarget with TargetId {TargetId}.";

		public MissingBindingTargetException(string symbolName, int targetId) : base(symbolName)
		{
			TargetId = targetId;
		}
	}
}
