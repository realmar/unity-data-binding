using System.Collections.Generic;
using System.Linq;
using Mono.Cecil;
using Realmar.DataBindings.Editor.Cecil;
using Realmar.DataBindings.Editor.Exceptions;
using Realmar.DataBindings.Editor.IoC;
using Realmar.DataBindings.Editor.Weaving;
using static Realmar.DataBindings.Editor.Binding.BindingHelpers;
using static Realmar.DataBindings.Editor.Shared.SharedHelpers;

namespace Realmar.DataBindings.Editor.Binding
{
	internal class OneTimeBinder : IBinder
	{
		private readonly Weaver _weaver = ServiceLocator.Current.Resolve<Weaver>();
		private readonly DerivativeResolver _derivativeResolver = ServiceLocator.Current.Resolve<DerivativeResolver>();

		public void Bind(PropertyDefinition sourceProperty, in BindingSettings settings, IReadOnlyCollection<BindingTarget> targets)
		{
			foreach (var bindingTarget in targets)
			{
				var bindingTargetProperty = bindingTarget.Source;
				var targetType = GetReturnType(bindingTargetProperty);
				var targetProperty = GetTargetProperty(sourceProperty, targetType, settings.TargetPropertyName);

				var sourceType = sourceProperty.DeclaringType;
				var (bindingInitializer, _) = GetBindingInitializer(sourceType);

				YeetIfNoNonAbstractBindingInitializer(bindingInitializer);

				_weaver.Weave(
					new WeaveMethodParameters(
						fromSetter: bindingInitializer,
						toSetter: targetProperty.GetSetMethodOrYeet(),
						bindingTarget: bindingTargetProperty,
						emitNullCheck: ResolveNullCheckBehavior(settings.NullCheckBehavior, false),
						converter: settings.Converter
					),
					sourceProperty.GetGetMethodOrYeet());
			}
		}

		private void YeetIfNoNonAbstractBindingInitializer(MethodDefinition bindingInitializer)
		{
			var hasNonAbstract = _derivativeResolver
				.GetDerivedTypes(bindingInitializer.DeclaringType)
				.Select(definition => definition.GetMethod(bindingInitializer.Name))
				.Any(definition => definition.IsAbstract == false);

			if (hasNonAbstract == false)
			{
				throw new MissingNonAbstractBindingInitializer(bindingInitializer.FullName);
			}
		}
	}
}
