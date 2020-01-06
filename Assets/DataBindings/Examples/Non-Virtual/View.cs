using TMPro;
using UnityEngine;

namespace Realmar.DataBindings.Examples.NonVirtual
{
	public class View : MonoBehaviour
	{
		#region This should be setup using a DI framework or similar

		// View should not reference ViewModel

		private ViewModel _vm;

		private void Awake()
		{
			_vm = new ViewModel(this);
		}

		#endregion

		[SerializeField] private TMP_InputField _name;
		[SerializeField] private TMP_InputField _age;

		[SerializeField] private TMP_Text _summary;
		[SerializeField] private TMP_Text _validation;

		public string Name { get; set; }
		public string Age { get; set; }

		public string Summary
		{
			get => _summary.text;
			set => _summary.SetText(value);
		}

		public string ValidationResult
		{
			get => _validation.text;
			set => _validation.SetText(value);
		}

		private void Start()
		{
			_name.onValueChanged.AddListener(s => Name = s);
			_age.onValueChanged.AddListener(s => Age = s);
		}
	}
}
