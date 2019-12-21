namespace Realmar.DataBindings.Editor.Exceptions
{
	internal class MissingBindingTargetException : MissingSymbolException
	{
		internal int TargetId { get; }
		public override string Message => $"{SymbolName} is missing a BindingTarget with TargetId {TargetId}.";

		internal MissingBindingTargetException(string symbolName, int targetId) : base(symbolName)
		{
			TargetId = targetId;
		}
	}
}
