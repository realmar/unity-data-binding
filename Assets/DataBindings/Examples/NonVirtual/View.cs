using TMPro;
using UnityEngine;

namespace Realmar.DataBindings.Examples.NonVirtual
{
	public class View : MonoBehaviour
	{
		[SerializeField] private TMP_InputField _surnameInput;
		[SerializeField] private TMP_InputField _lastnameInput;
		[SerializeField] private TMP_InputField _ageInput;
		[SerializeField] private TMP_Text _summaryText;

		private string _surname;
		private string _lastname;
		private string _age;

		public string Surname
		{
			get => _surname;
			set
			{
				_surname = value;
				Debug.Log($"Set {GetType().Name}::{nameof(Surname)} = {Surname}");
			}
		}

		public string Lastname
		{
			get => _lastname;
			set
			{
				_lastname = value;
				Debug.Log($"Set {GetType().Name}::{nameof(Lastname)} = {Lastname}");
			}
		}

		public string Age
		{
			get => _age;
			set
			{
				_age = value;
				Debug.Log($"Set {GetType().Name}::{nameof(Age)} = {Age}");
			}
		}

		public string Summary
		{
			set => _summaryText.text = value;
		}

		private void Start()
		{
			_surnameInput.onValueChanged.AddListener(s => Surname = s);
			_lastnameInput.onValueChanged.AddListener(s => Lastname = s);
			_ageInput.onValueChanged.AddListener(s => Age = s);
		}

		private void OnDestroy()
		{
			_surnameInput.onValueChanged.RemoveAllListeners();
			_lastnameInput.onValueChanged.RemoveAllListeners();
			_ageInput.onValueChanged.RemoveAllListeners();
		}
	}
}
