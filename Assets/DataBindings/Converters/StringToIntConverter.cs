namespace Realmar.DataBindings.Converters
{
	public class StringToIntConverter : IValueConverter<string, int>
	{
		public string Convert(int to)
		{
			return to.ToString();
		}

		public int Convert(string @from)
		{
			return int.Parse(@from);
		}
	}
}
