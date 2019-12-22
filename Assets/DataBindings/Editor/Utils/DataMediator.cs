namespace Realmar.DataBindings.Editor.Utils
{
	internal class DataMediator<TData>
	{
		internal TData Data { get; set; }

		public override string ToString()
		{
			return Data?.ToString() ?? "null";
		}
	}
}
