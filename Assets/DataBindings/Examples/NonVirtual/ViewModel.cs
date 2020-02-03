using System.Text;
using Realmar.DataBindings.Converters;
using UnityEngine;
using Zenject;

namespace Realmar.DataBindings.Examples.NonVirtual
{
	public class ViewModel : IInitializable
	{
		[Inject, BindingTarget]
		private View _view;

		[Inject, BindingTarget(2)]
		private Model _model;

		private string _surname;
		private string _lastname;
		private string _age;

		[InvokeOnChange(nameof(OnPropertyChanged))]
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

		[InvokeOnChange(nameof(OnPropertyChanged))]
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

		[InvokeOnChange(nameof(OnPropertyChanged))]
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

		[Binding]
		public string Summary { get; set; }

		[BindingInitializer(throwOnFailure: true)]
		public void Initialize()
		{
		}

		private void OnPropertyChanged(string _)
		{
			var sb = new StringBuilder();

			sb.AppendLine($"{nameof(Surname)} = {Surname}");
			sb.AppendLine($"{nameof(Lastname)} = {Lastname}");
			sb.AppendLine($"{nameof(Age)} = {Age}");

			Summary = sb.ToString();
		}
	}
}
