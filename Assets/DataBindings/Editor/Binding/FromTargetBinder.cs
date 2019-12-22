using Mono.Cecil;
using Realmar.DataBindings.Editor.Exceptions;
using Realmar.DataBindings.Editor.Extensions;
using Realmar.DataBindings.Editor.Weaving;
using Realmar.DataBindings.Editor.Weaving.Commands;
using static Realmar.DataBindings.Editor.Weaving.WeaverHelpers;

namespace Realmar.DataBindings.Editor.Binding
{
	internal class FromTargetBinder : IBinder
	{
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
					throw new MissingTargetPropertyException(sourceType.FullName, targetPropertyName);
				}

				var accessorCommand = WeaveTargetToSourceAccessorCommand.Create(sourceType, targetType, bindingTarget, bindingInitializer);
				accessorCommand.Execute();

				var accessorProperty = GetAccessorProperty(sourceProperty.DeclaringType, targetProperty.DeclaringType);
				var weaveCommand = WeaveBindingRootCommand.Create(
					new WeaveParameters
					{
						FromProperty = targetProperty,
						ToType = sourceProperty.DeclaringType,
						ToProperty = sourceProperty,
						BindingTarget = accessorProperty.GetMethod,
						EmitNullCheck = settings.EmitNullCheck
					});
				weaveCommand.Execute();
			}
		}
	}
}
