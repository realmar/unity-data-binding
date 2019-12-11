using Mono.Cecil;
using Realmar.DataBindings.Editor.Emitter;
using Realmar.DataBindings.Editor.Exceptions;
using Realmar.DataBindings.Editor.Extensions;
using static Realmar.DataBindings.Editor.WeaverHelpers;

namespace Realmar.DataBindings.Editor.Binder
{
	internal class FromTargetBinder : IBinder
	{
		private readonly Weaver _weaver;

		public FromTargetBinder(DerivativeResolver derivativeResolver)
		{
			_weaver = new Weaver(derivativeResolver);
		}

		public void Bind(PropertyDefinition sourceProperty, BindingSettings settings, BindingTarget[] targets)
		{
			foreach (var target in targets)
			{
				var targetPropertyName = settings.TargetPropertyName ?? sourceProperty.Name;
				var bindingTarget = target.Source;
				var targetType = GetReturnType(bindingTarget);
				var targetProperty = targetType.GetProperty(targetPropertyName);
				var sourceType = sourceProperty.DeclaringType;
				var bindingInitializer = GetBindingInitializer(sourceType);

				if (targetProperty == null)
				{
					throw new MissingSymbolException(
						$"BindingTarget {targetType.FullName} does not have property {targetPropertyName}, cannot weave binding");
				}

				var targetToSourceProperty =
					_weaver.WeaveAccessor(sourceType, targetType, bindingTarget, bindingInitializer);
				_weaver.WeaveBinding(
					new WeaveParameters
					{
						FromProperty = targetProperty,
						ToType = sourceProperty.DeclaringType,
						ToProperty = sourceProperty,
						BindingTarget = targetToSourceProperty.GetMethod,
						EmitNullCheck = settings.EmitNullCheck
					});
			}
		}
	}
}
