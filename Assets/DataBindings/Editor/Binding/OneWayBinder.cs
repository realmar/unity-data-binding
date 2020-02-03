using System;
using System.Collections.Generic;
using Mono.Cecil;
using Realmar.DataBindings.Editor.Exceptions;
using Realmar.DataBindings.Editor.IoC;
using Realmar.DataBindings.Editor.Weaving;
using static Realmar.DataBindings.Editor.Binding.BindingHelpers;
using static Realmar.DataBindings.Editor.Shared.SharedHelpers;

namespace Realmar.DataBindings.Editor.Binding
{
	internal class OneWayBinder : IBinder
	{
		private readonly Weaver _weaver = ServiceLocator.Current.Resolve<Weaver>();

		public void Bind(PropertyDefinition sourceProperty, in BindingSettings settings, IReadOnlyCollection<BindingTarget> targets)
		{
			foreach (var bindingTarget in targets)
			{
				var bindingTargetProperty = bindingTarget.Source;
				var targetType = GetReturnType(bindingTargetProperty);
				var targetProperty = GetTargetProperty(sourceProperty, targetType, settings.TargetPropertyName);
				var fromSetter = sourceProperty.GetSetMethodOrYeet();
				var toSetter = targetProperty.GetSetMethodOrYeet();

				DispatchUsingDataSource(
					_weaver,
					new DataSourceConfiguration
					(
						dataSource: settings.DataSource,
						fromGetter: new Lazy<MethodDefinition>(sourceProperty.GetGetMethodOrYeet),
						methodParameter: new Lazy<ParameterDefinition>(() => fromSetter.Parameters[0])
					),
					new WeaveMethodParameters(
						fromSetter: fromSetter,
						toSetter: toSetter,
						bindingTarget: bindingTarget.Source,
						emitNullCheck: ResolveNullCheckBehavior(settings.NullCheckBehavior, false),
						converter: settings.Converter));
			}
		}
	}
}
