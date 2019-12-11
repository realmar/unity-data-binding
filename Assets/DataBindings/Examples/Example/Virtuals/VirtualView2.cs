using UnityEngine;
using UnityEngine.UI;

namespace Realmar.DataBindings.Example
{
	public class VirtualView2 : BaseVirtualView2
	{
		[SerializeField] private Text _text;
		[SerializeField] private Text _text2;

		[SerializeField] private InputField _input;

		public override string Text
		{
			get => _text.text + "Overriden";
			set
			{
				// Debug.Log(value + _text);
				System.Diagnostics.Debugger.Break();
				_text.text = value;
				// Debug.Log($"{nameof(VirtualView2)} {Text} {value}");
			}
		}

		public override string Text2
		{
			get => _text2.text;
			set
			{
				_text2.text = value;
				Debug.LogWarning(_input);
				if (_input != null)
				{
					Debug.LogWarning($"VirtualView2::Text2 {value}");
					_input.text = value;
				}
			}
		}

		protected override void VirtualExample()
		{
			base.VirtualExample();
		}

		private void Update()
		{
			Text = Time.time.ToString();
		}

		private void Awake()
		{
			if (_input != null)
			{
				_input.onValueChanged.AddListener(arg0 => Text2 = arg0);
			}
		}
	}
}
