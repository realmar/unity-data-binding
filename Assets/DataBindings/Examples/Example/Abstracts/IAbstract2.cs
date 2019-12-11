namespace Realmar.DataBindings.Example.Abstracts
{
	public interface IAbstract2
	{
		[BindingTarget] Abstract1 BT5 { get; }

		[BindingTarget(id: 1)]
		[BindingTarget(id: 2)]
		IAbstract1 BT6 { get; }

		string Text4 { get; set; }
		// [Binding]
		string Text5 { get; set; }

		// [Binding(bindingType: BindingType.OneWayFromTarget, targetId: 1)]
		string Text6 { get; set; }

		// [Binding(bindingType: BindingType.TwoWay, targetId: 2)]
		string Text7 { get; set; }

		[BindingInitializer]
		void InitializeBindings();
	}
}
