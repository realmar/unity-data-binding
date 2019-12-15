using UnityEngine;
using UnityEngine.UI;

namespace Realmar.DataBindings.Example
{
	public class VirtualView1 : BaseVirtualView1
	{
		[SerializeField] private InputField _textInput;
		[SerializeField] private InputField _textInput2;

		[SerializeField] private BaseVirtualView2 _view;

		[BindingTarget]
		public override BaseVirtualView2 View
		{
			get => _view;
			set => _view = value;
		}

		/*[Binding(
			BindingType = BindingType.TwoWay,
			TargetPropertyName = nameof(BaseVirtualView2.Text2),
			EmitNullCheck = true)]*/
		public override string Text
		{
			get => base.Text; // + "Overriden";
			protected set
			{
				Debug.LogWarning($"Set Text {value}");
				base.Text = value;
				_textInput.text = value;
			}
		}

		// [Binding]
		private string _text2;

		// [Binding(targetId: 1, targetPropertyName: "Text")]
		[Binding(
			bindingType: BindingType.OneWayFromTarget,
			// targetId: 1,
			targetPropertyName: nameof(BaseVirtualView2.Text),
			emitNullCheck: true)]
		public string Text2
		{
			get => _text2;
			set
			{
				_text2 = value;
				_textInput2.text = value;
				Debug.Log($"{nameof(VirtualView1)} set_{nameof(Text2)} {value}");
			}
		}

		[BindingInitializer]
		protected override void Awake()
		{
			base.Awake();
			_textInput.onValueChanged.AddListener(arg0 => Text = arg0);
			_textInput2.onValueChanged.AddListener(arg0 => Text2 = arg0);
			Debug.LogWarning(View2);
		}

		private void Start()
		{
			// Debug.LogWarning(View2);
			// Debug.LogWarning(View2.GetType().GetField("RealmarDataBindingsExampleVirtualView1").GetValue(View2));
		}
	}
}
