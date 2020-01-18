namespace Realmar.DataBindings.Converters
{
	public interface IValueConverter<TFrom, TTo>
	{
		TFrom Convert(TTo to);
		TTo Convert(TFrom from);
	}
}
