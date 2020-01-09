using Zenject;

namespace Realmar.DataBindings.Examples.NonVirtual
{
	public class ViewModel : IInitializable
	{
		[Inject, BindingTarget] private View _view;
		[Inject, BindingTarget(2)] private Model _model;

		[Binding(BindingType.OneWayFromTarget)]
		[Binding(BindingType.TwoWay, 2)]
		public string Surname { get; set; }

		[Binding(BindingType.OneWayFromTarget)]
		[Binding(BindingType.TwoWay, 2)]
		public string Lastname { get; set; }

		[Binding(BindingType.OneWayFromTarget)]
		[Binding(BindingType.TwoWay, 2)]
		public int Age { get; set; }

		[BindingInitializer]
		public void Initialize()
		{
		}
	}
}
