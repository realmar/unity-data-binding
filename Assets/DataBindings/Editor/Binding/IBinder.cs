using System.Collections.Generic;
using Mono.Cecil;

namespace Realmar.DataBindings.Editor.Binding
{
	internal interface IBinder
	{
		void Bind(PropertyDefinition sourceProperty, in BindingSettings settings, IReadOnlyCollection<BindingTarget> targets);
	}
}
