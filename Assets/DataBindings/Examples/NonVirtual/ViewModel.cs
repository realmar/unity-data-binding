using Realmar.DataBindings.Converters;
using UnityEngine;
using Zenject;

namespace Realmar.DataBindings.Examples.NonVirtual
{
	public class ViewModel : IInitializable
	{
		[Inject,
		 BindingTarget]
		private View _view;

		[Inject, BindingTarget(2)]
		private Model _model;

		private string _surname;
		private string _lastname;
		private string _age;

		[Binding(BindingType.OneWayFromTarget)]
		[Binding(BindingType.TwoWay, 2)]
		public string Surname
		{
			get => _surname;
			set
			{
				_surname = value;
				Debug.Log($"Set {GetType().Name}::{nameof(Surname)} = {Surname}");
			}
		}

		[Binding(BindingType.OneWayFromTarget)]
		[Binding(BindingType.TwoWay, 2)]
		public string Lastname
		{
			get => _lastname;
			set
			{
				_lastname = value;
				Debug.Log($"Set {GetType().Name}::{nameof(Lastname)} = {Lastname}");
			}
		}

		[Binding(BindingType.OneWayFromTarget)]
		[Binding(BindingType.TwoWay, 2, converter: typeof(StringToIntConverter))]
		public string Age
		{
			get => _age;
			set
			{
				_age = value;
				Debug.Log($"Set {GetType().Name}::{nameof(Age)} = {Age}");
			}
		}

		[BindingInitializer(throwOnFailure: true)]
		public void Initialize()
		{
		}
	}
}
