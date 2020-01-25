using Zenject;

namespace Realmar.DataBindings.Examples.NonVirtual
{
	public class NonVirtualExampleInstaller : MonoInstaller<NonVirtualExampleInstaller>
	{
		public override void InstallBindings()
		{
			Container.BindInterfacesAndSelfTo<Model>().AsSingle();
		}
	}
}
