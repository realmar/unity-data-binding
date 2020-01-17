using Mono.Cecil;
using System.Collections.Generic;

namespace Realmar.DataBindings.Editor.Binding
{
	internal class TwoWayBinder : IBinder
	{
		private readonly IBinder _oneWayBinder;
		private readonly IBinder _fromTargetBinder;

		internal TwoWayBinder(IBinder oneWayBinder, IBinder fromTargetBinder)
		{
			_oneWayBinder = oneWayBinder;
			_fromTargetBinder = fromTargetBinder;
		}

		public void Bind(PropertyDefinition sourceProperty, in BindingSettings settings, IReadOnlyCollection<BindingTarget> targets)
		{
			_oneWayBinder.Bind(sourceProperty, settings, targets);
			_fromTargetBinder.Bind(sourceProperty, settings, targets);
		}
	}
}
