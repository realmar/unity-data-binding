using Mono.Cecil;
using Realmar.DataBindings.Editor.Exceptions;
using Realmar.DataBindings.Editor.IoC;
using Realmar.DataBindings.Editor.Weaving;
using System.Collections.Generic;
using static Realmar.DataBindings.Editor.Binding.BindingHelpers;
using static Realmar.DataBindings.Editor.Shared.SharedHelpers;

namespace Realmar.DataBindings.Editor.Binding
{
	internal class OneTimeBinder : IBinder
	{
		private readonly Weaver _weaver = ServiceLocator.Current.Resolve<Weaver>();

		public void Bind(PropertyDefinition sourceProperty, in BindingSettings settings, IReadOnlyCollection<BindingTarget> targets)
		{
			foreach (var bindingTarget in targets)
			{
				var bindingTargetProperty = bindingTarget.Source;
				var targetType = GetReturnType(bindingTargetProperty);
				var targetProperty = GetTargetProperty(sourceProperty, targetType, settings.TargetPropertyName);

				var sourceType = sourceProperty.DeclaringType;
				var (bindingInitializer, _) = GetBindingInitializer(sourceType);

				_weaver.Weave(
					new WeaveMethodParameters(
						fromGetter: sourceProperty.GetGetMethodOrYeet(),
						fromSetter: bindingInitializer,
						toSetter: targetProperty.GetSetMethodOrYeet(),
						bindingTarget: bindingTargetProperty,
						emitNullCheck: settings.EmitNullCheck
					));
			}
		}
	}
}
