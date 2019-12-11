using Mono.Cecil;

namespace Realmar.DataBindings.Editor.Binder
{
	internal class TwoWayBinder : IBinder
	{
		private readonly IBinder _oneWayBinder;
		private readonly IBinder _fromTargetBinder;

		public TwoWayBinder(IBinder oneWayBinder, IBinder fromTargetBinder)
		{
			_oneWayBinder = oneWayBinder;
			_fromTargetBinder = fromTargetBinder;
		}

		public void Bind(PropertyDefinition sourceProperty, BindingSettings settings, BindingTarget[] targets)
		{
			_oneWayBinder.Bind(sourceProperty, settings, targets);
			_fromTargetBinder.Bind(sourceProperty, settings, targets);
		}
	}
}
