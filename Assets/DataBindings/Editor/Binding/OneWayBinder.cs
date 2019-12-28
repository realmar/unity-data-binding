using Mono.Cecil;
using Realmar.DataBindings.Editor.Weaving;
using Realmar.DataBindings.Editor.Weaving.Commands;
using static Realmar.DataBindings.Editor.Binding.BindingHelpers;
using static Realmar.DataBindings.Editor.Shared.SharedHelpers;

namespace Realmar.DataBindings.Editor.Binding
{
	internal class OneWayBinder : IBinder
	{
		public void Bind(PropertyDefinition sourceProperty, BindingSettings settings, BindingTarget[] targets)
		{
			foreach (var bindingTarget in targets)
			{
				var bindingTargetProperty = bindingTarget.Source;

				var targetType = GetReturnType(bindingTargetProperty);
				var targetProperty = GetTargetProperty(sourceProperty, targetType, settings.TargetPropertyName);

				var command = WeaveBindingRootCommand.Create(
					new WeaveParameters
					{
						FromProperty = sourceProperty,
						ToType = targetType,
						ToProperty = targetProperty,
						BindingTarget = bindingTarget.Source,
						EmitNullCheck = settings.EmitNullCheck
					});
				command.Execute();
			}
		}
	}
}
