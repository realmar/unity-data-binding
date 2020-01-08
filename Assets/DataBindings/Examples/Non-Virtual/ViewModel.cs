using System.Text;

namespace Realmar.DataBindings.Examples.NonVirtual
{
	public class ViewModel
	{
		[BindingTarget] private View _view;

		[BindingTarget(2)]
		private Model _model;

		[Binding(BindingType.OneWayFromTarget)]
		[Binding(BindingType.TwoWay, targetId: 2)]
		public string Name
		{
			get => _model.Name;
			set
			{
				_model.Name = value;
				UpdateSummary();
			}
		}

		[Binding(BindingType.OneWayFromTarget)]
		public string Age
		{
			get => _model.Age.ToString();
			set
			{
				if (int.TryParse(value, out var result))
				{
					_model.Age = result;
					ValidationResult = "";
				}
				else
				{
					ValidationResult = "Age must be a number.";
				}

				UpdateSummary();
			}
		}

		[Binding] private string Summary { get; set; }
		[Binding] private string ValidationResult { get; set; }

		public ViewModel(View view)
		{
			_view = view;
			_model = new Model();

			InitializeBindings();
		}

		private void UpdateSummary()
		{
			var sb = new StringBuilder();
			sb.AppendLine($"Name: {Name}");
			sb.AppendLine($"Age: {Age}");

			Summary = sb.ToString();
		}

		[BindingInitializer]
		private void InitializeBindings()
		{
		}
	}
}
