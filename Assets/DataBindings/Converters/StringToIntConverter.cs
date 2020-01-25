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
			if (int.TryParse(@from, out var i) == false)
			{
				i = default;
			}

			return i;
		}
	}
}
