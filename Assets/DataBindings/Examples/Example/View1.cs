using UnityEngine;
using UnityEngine.UI;

namespace Realmar.DataBindings.Example
{
	class Texti
	{
	}

	class Texti2
	{
		public static bool operator!=(Texti2 a, Texti2 b)
		{
			return false;
		}

		public static bool operator ==(Texti2 a, Texti2 b)
		{
			return !(a != b);
		}
	}

	public class View1 : MonoBehaviour
	{
		[BindingTarget] public View2 View { get; set; }
		[BindingTarget(id: 1)] public View3 AlternativeView { get; set; }

		public InputField _InputField;

		private string _text;
		private Texti _tt = new Texti();
		private Texti2 _tt2 = new Texti2();

		// [Binding(BindingType.TwoWay)]
		// [Binding(targetId: 1)]
		public string Text
		{
			get => _text;
			set
			{
				_text = value;
				_InputField.text = value;

				if (View != null)
				{
					View.Text = Text;
				}

				if (_text != null)
				{
					View.Text = Text;
				}

				if (_tt != null)
				{
					View.Text = Text;
				}

				if (_tt2 != null)
				{
					View.Text = Text;
				}
			}
		}

		// [Binding(targetPropertyName: nameof(View2.Counter))]
		// [Binding]
		public int FrameCounter { get; set; }

		private void Awake()
		{
			View = GetComponent<View2>();
			AlternativeView = GetComponent<View3>();
			_InputField.onValueChanged.AddListener(arg0 => Text = arg0);
		}

		private void Update()
		{
			FrameCounter++;
		}
	}
}
