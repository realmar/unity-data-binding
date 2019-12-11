using Mono.Cecil;

namespace Realmar.DataBindings.Editor.Binder
{
	internal interface IBinder
	{
		void Bind(PropertyDefinition sourceProperty, BindingSettings settings, BindingTarget[] targets);
	}
}
