namespace Realmar.DataBindings.Editor.Exceptions
{
	internal class MissingNonAbstractBindingInitializer : MissingNonAbstractSymbolException
	{
		public override string Message => $"Cannot find any non abstract overriding BindingInitializer for {SymbolName}";

		internal MissingNonAbstractBindingInitializer(string symbolName) : base(symbolName)
		{
		}
	}
}
