using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace Realmar.DataBindings.Example.Abstracts
{
	public abstract class Abstract1 : BaseAbstract1, IAbstract1
	{
		private string _text7;

		public InputField _input;
		public Text _text;

		[SerializeField] private MonoBehaviour _bt4;
		[BindingTarget(id: 4)] IAbstract2 BT4 => (IAbstract2) _bt4;

		public override string Text1 { get; set; }
		public override string Text3 { get; set; }
		[Binding(targetId: 4)] public abstract string Text4 { get; set; }
		public string Text5 { get; set; }
		public string Text6 { get; set; }

		public string Text7
		{
			get => _text7;
			set
			{
				_text7 = value;
				_input.text = value;
			}
		}

		private void Awake()
		{
			_input.onValueChanged.AddListener(arg0 =>
			{
				Text1 = arg0;
				Text2 = arg0;
				Text3 = arg0;
				Text4 = arg0;
				Text5 = arg0;
				Text6 = arg0;
				Text7 = arg0;
			});
		}

		private StringBuilder _sb = new StringBuilder();

		private void Update()
		{
			_sb.Clear();

			foreach (var propertyInfo in GetType().GetProperties().Where(info => info.Name.StartsWith("Text")))
			{
				_sb.Append(propertyInfo.Name);
				_sb.Append(": ");
				_sb.Append(propertyInfo.GetValue(this));
				_sb.Append("\n");
			}

			_text.text = _sb.ToString();
		}
	}
}
