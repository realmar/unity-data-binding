using System.Runtime.CompilerServices;
using UnityEngine;

namespace Realmar.DataBindings.Examples.NonVirtual
{
	public class Model
	{
		private string _surname;
		private string _lastname;
		private int _age;

		public string Surname
		{
			get => _surname;
			set
			{
				_surname = value;
				Print();
			}
		}

		public string Lastname
		{
			get => _lastname;
			set
			{
				_lastname = value;
				Print();
			}
		}

		public int Age
		{
			get => _age;
			set
			{
				_age = value;
				Print();
			}
		}

		private void Print([CallerMemberName] string name = null)
		{
			Debug.Log($"{nameof(Model)}::{name} = {GetType().GetProperty(name).GetValue(this)}");
		}
	}
}
