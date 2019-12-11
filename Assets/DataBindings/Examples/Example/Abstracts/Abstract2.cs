using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace Realmar.DataBindings.Example.Abstracts
{
	public class Abstract2 : BaseAbstract2, IAbstract2
	{
		private string _text7;

		public InputField _input;
		public Text _text;

		[SerializeField] private Abstract1 _bt5;
		[SerializeField] private MonoBehaviour _bt6;

		public string Text1 { get; set; }
		public override string Text2 { get; set; }
		public override string Text3 { get; set; }
		public Abstract1 BT5 => _bt5;
		public IAbstract1 BT6 => (IAbstract1) _bt6;
		public string Text4 { get; set; }
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

		public void InitializeBindings()
		{
		}

		private void Awake()
		{
			InitializeBindings();
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
