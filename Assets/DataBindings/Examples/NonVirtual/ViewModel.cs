using Realmar.DataBindings.Converters;
using System.Diagnostics;
using System.Text;
using Zenject;
using Debug = UnityEngine.Debug;

namespace Realmar.DataBindings.Examples.NonVirtual
{
	public class ViewModel : IInitializable
	{
		[Inject, BindingTarget]
		private View _view;

		[Inject, BindingTarget(2)]
		private Model _model;

		[InvokeOnChange(nameof(OnPropertyChanged), nameof(LogPropertyChanged))]
		[Binding(BindingType.OneWayFromTarget)]
		[Binding(BindingType.TwoWay, 2)]
		public string Surname { get; set; }

		[InvokeOnChange(nameof(OnPropertyChanged), nameof(LogPropertyChanged))]
		[Binding(BindingType.OneWayFromTarget)]
		[Binding(BindingType.TwoWay, 2)]
		public string Lastname { get; set; }

		[InvokeOnChange(nameof(OnPropertyChanged), nameof(LogPropertyChanged))]
		[Binding(BindingType.OneWayFromTarget)]
		[Binding(BindingType.TwoWay, 2, converter: typeof(StringToIntConverter))]
		public string Age { get; set; }

		[Binding]
		public string Summary { get; set; }

		[BindingInitializer]
		public void Initialize()
		{
		}

		private void LogPropertyChanged(string value)
		{
			var frame = new StackFrame(2);
			var methodName = frame.GetMethod().Name;

			Debug.Log($"Set {GetType().Name}::{methodName} = {value}");
		}

		private void OnPropertyChanged()
		{
			var sb = new StringBuilder();

			sb.AppendLine($"{nameof(Surname)} = {Surname}");
			sb.AppendLine($"{nameof(Lastname)} = {Lastname}");
			sb.AppendLine($"{nameof(Age)} = {Age}");

			Summary = sb.ToString();
		}
	}
}
