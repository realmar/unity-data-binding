namespace Realmar.DataBindings.Example.Abstracts
{
	public class Abstract2_2 : IAbstract2
	{
		public Abstract1 BT5 { get; set; }
		public IAbstract1 BT6 { get; set; }
		public string Text4 { get; set; }
		public string Text5 { get; set; }
		public string Text6 { get; set; }
		public string Text7 { get; set; }

		public void InitializeBindings()
		{
		}
	}
}
