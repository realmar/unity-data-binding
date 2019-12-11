using Mono.Cecil;

namespace Realmar.DataBindingsEditor.Binder
{
	internal interface IBinder
	{
		void Bind(PropertyDefinition sourceProperty, BindingSettings settings, BindingTarget[] targets);
	}
}
