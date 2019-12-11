using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace Realmar.DataBindings.Example
{
	public class View2 : MonoBehaviour
	{
		public Text _counterText;
		public InputField _InputField;
		private string _text;

		public string Text
		{
			get => _text;
			set
			{
				_text = value;
				_InputField.text = value;
			}
		}

		public int Counter { get; set; }
		public int FrameCounter { get; set; }

		private void Awake()
		{
			_InputField.onValueChanged.AddListener(arg0 => Text = arg0);
		}

		private readonly StringBuilder _sb = new StringBuilder();

		private void Update()
		{
			_counterText.text = _sb.Clear().Append(Counter).Append(" : ").Append(FrameCounter).ToString();
		}
	}
}
