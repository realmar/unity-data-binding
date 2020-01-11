using TMPro;
using UnityEngine;

namespace Realmar.DataBindings.Examples.NonVirtual
{
	public class View : MonoBehaviour
	{
		[SerializeField] private TMP_InputField _surnameInput;
		[SerializeField] private TMP_InputField _lastnameInput;
		[SerializeField] private TMP_InputField _ageInput;

		public string Surname { get; set; }
		public string Lastname { get; set; }
		public int Age { get; set; }

		private void Start()
		{
			_surnameInput.onValueChanged.AddListener(s => Surname = s);
			_lastnameInput.onValueChanged.AddListener(s => Lastname = s);
			_ageInput.onValueChanged.AddListener(s =>
			{
				if (int.TryParse(s, out var i))
				{
					Age = i;
				}
			});
		}

		private void OnDestroy()
		{
			_surnameInput.onValueChanged.RemoveAllListeners();
			_lastnameInput.onValueChanged.RemoveAllListeners();
			_ageInput.onValueChanged.RemoveAllListeners();
		}
	}
}
