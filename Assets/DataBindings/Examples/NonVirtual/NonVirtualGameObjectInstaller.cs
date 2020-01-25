using UnityEngine;
using Zenject;

namespace Realmar.DataBindings.Examples.NonVirtual
{
	public class NonVirtualGameObjectInstaller : MonoInstaller<NonVirtualGameObjectInstaller>
	{
		[SerializeField] private View _view;

		public override void InstallBindings()
		{
			Container.BindInterfacesAndSelfTo<View>().FromInstance(_view).AsSingle();
			Container.BindInterfacesAndSelfTo<ViewModel>().AsSingle().NonLazy();
		}
	}
}
