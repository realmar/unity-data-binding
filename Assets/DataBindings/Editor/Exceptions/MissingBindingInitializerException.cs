namespace Realmar.DataBindings.Editor.Exceptions
{
	internal class MissingBindingInitializerException : MissingSymbolException
	{
		public override string Message =>
			$"{SymbolName} is missing a binding initializer which is required for " +
			$"{BindingType.OneWayFromTarget} and {BindingType.TwoWay} bindings.";

		public MissingBindingInitializerException(string symbolName) : base(symbolName)
		{
		}
	}
}
