using UnityEngine;

namespace Realmar.DataBindings.Example
{
	public class ExampleViewModel
	{
		// [BindingTarget]
		public ExampleView View { get; set; }
		// [BindingTarget(1)] public DomainModel Model { get; }

		// [Binding]
		public int Value { get; set; }

		// [Binding(targetPropertyName: nameof(ExampleView.SpecialValue))]
		// [Binding]
		public int Value2 { get; set; }

		// [Binding(BindingType.TwoWay, 1, nameof(DomainModel.Id))]
		private int _value3;

		// [Binding(BindingType.OneWay)]
		public int Value3
		{
			get => _value3;
			set
			{
				_value3 = value;
				Debug.Log(value);
			}
		}

		public int Value4 { get; set; }
		public int Value5 { get; set; }

		public void Do()
		{
			Value3 = (int) Time.time;
		}
	}
}
