using System.CodeDom;
using Mono.Cecil;
using Realmar.DataBindingsEditor.Emitter;
using static Realmar.DataBindingsEditor.WeaverHelpers;

namespace Realmar.DataBindingsEditor.Binder
{
	internal class OneWayBinder : IBinder
	{
		private readonly Weaver _weaver;

		public OneWayBinder(DerivativeResolver derivativeResolver)
		{
			_weaver = new Weaver(derivativeResolver);
		}

		public void Bind(PropertyDefinition sourceProperty, BindingSettings settings, BindingTarget[] targets)
		{
			foreach (var bindingTarget in targets)
			{
				var bindingTargetProperty = bindingTarget.Source;

				var targetType = GetReturnType(bindingTargetProperty);
				var targetProperty = GetTargetProperty(sourceProperty, targetType, settings.TargetPropertyName);

				_weaver.WeaveBinding(
					new WeaveParameters
					{
						FromProperty = sourceProperty,
						ToType = targetType,
						ToProperty = targetProperty,
						BindingTarget = bindingTarget.Source,
						EmitNullCheck = settings.EmitNullCheck
					});
			}
		}
	}
}
