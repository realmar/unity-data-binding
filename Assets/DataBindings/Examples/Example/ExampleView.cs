using UnityEngine;
using UnityEngine.UI;

namespace Realmar.DataBindings.Example
{
	public class ExampleView : BaseExampleView
	{
		public Text Text;
		public ExampleViewModel ViewModel = new ExampleViewModel();

		public ExampleView()
		{
			ViewModel.View = this;
		}

		// public new string ExampleStrProperty { get; set; }

		public int Value { get; set; }

		public int Value3 { get; set; }

		public int SpecialValue { get; set; }

		public override string Ex { get; set; }

		private void Update()
		{
			// ViewModel.Do();
			// Text.text = Value3.ToString();
			Value3 = (int) Time.time;
		}
	}
}
