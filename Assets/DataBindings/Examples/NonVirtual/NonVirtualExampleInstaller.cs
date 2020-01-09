using Zenject;

namespace Realmar.DataBindings.Examples.NonVirtual
{
	public class NonVirtualExampleInstaller : MonoInstaller<NonVirtualExampleInstaller>
	{
		public override void InstallBindings()
		{
			Container.BindInterfacesAndSelfTo<View>().FromComponentsInHierarchy().AsSingle();
			Container.BindInterfacesAndSelfTo<ViewModel>().AsSingle();
			Container.BindInterfacesAndSelfTo<Model>().AsSingle();
		}
	}
}
