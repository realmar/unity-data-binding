using Mono.Cecil;
using System.Collections.Generic;

namespace Realmar.DataBindings.Editor.Binding
{
	internal interface IBinder
	{
		void Bind(PropertyDefinition sourceProperty, in BindingSettings settings, IReadOnlyCollection<BindingTarget> targets);
	}
}
