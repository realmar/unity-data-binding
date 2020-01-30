using System;
using Mono.Cecil;
using Realmar.DataBindings.Editor.Cecil;
using Realmar.DataBindings.Editor.Exceptions;
using Realmar.DataBindings.Editor.IoC;
using Realmar.DataBindings.Editor.Weaving;
using System.Collections.Generic;
using System.Linq;
using static Realmar.DataBindings.Editor.Binding.BindingHelpers;
using static Realmar.DataBindings.Editor.Shared.SharedHelpers;

namespace Realmar.DataBindings.Editor.Binding
{
	internal class FromTargetBinder : IBinder
	{
		private readonly Weaver _weaver = ServiceLocator.Current.Resolve<Weaver>();

		public void Bind(PropertyDefinition sourceProperty, in BindingSettings settings, IReadOnlyCollection<BindingTarget> targets)
		{
			foreach (var target in targets)
			{
				var targetPropertyName = settings.TargetPropertyName ?? sourceProperty.Name;
				var bindingTarget = target.Source;
				var targetType = GetReturnType(bindingTarget);
				var targetProperty = targetType.GetPropertiesInBaseHierarchy(targetPropertyName).FirstOrDefault();
				var sourceType = sourceProperty.DeclaringType;
				var (bindingInitializer, bindingInitializerSettings) = GetBindingInitializer(sourceType);

				if (targetProperty == null)
				{
					throw new MissingTargetPropertyException(sourceType.FullName, targetPropertyName);
				}

				var accessorProperty = _weaver.WeaveTargetToSourceAccessorCommand(
					new AccessorSymbolParameters
					(
						sourceType: sourceType,
						targetType: targetProperty.DeclaringType,
						bindingTarget: bindingTarget,
						bindingInitializer: bindingInitializer,
						settings: bindingInitializerSettings
					));
				var fromSetter = targetProperty.GetSetMethodOrYeet();
				var toSetter = sourceProperty.GetSetMethodOrYeet();

				DispatchUsingDataSource(
					_weaver,
					new DataSourceConfiguration(
						dataSource: settings.DataSource,
						fromGetter: new Lazy<MethodDefinition>(targetProperty.GetGetMethodOrYeet),
						methodParameter: new Lazy<ParameterDefinition>(() => fromSetter.Parameters[0])),
					new WeaveMethodParameters(
						fromSetter: fromSetter,
						toSetter: toSetter,
						bindingTarget: accessorProperty.GetGetMethodOrYeet(),
						emitNullCheck: ResolveNullCheckBehavior(settings.NullCheckBehavior, true),
						converter: settings.Converter));
			}
		}
	}
}
