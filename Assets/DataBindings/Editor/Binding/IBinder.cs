using Mono.Cecil;

namespace Realmar.DataBindings.Editor.Binding
{
	internal interface IBinder
	{
		void Bind(PropertyDefinition sourceProperty, BindingSettings settings, BindingTarget[] targets);
	}
}
