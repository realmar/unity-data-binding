using System;
using System.Collections.Generic;
using System.Reflection;
using Realmar.DataBindings.Editor.Shared.Extensions;
using Realmar.DataBindings.Editor.TestFramework.Attributes;

namespace Realmar.DataBindings.Editor.TestFramework.Sandbox.Factories
{
	internal class PropertyBindingFactory : BindingFactoryBase
	{
		internal PropertyBindingFactory(IReadOnlyCollection<Type> types) : base(types)
		{
		}

		protected override IEnumerable<IBinding<Attribute>> CreateBindings(Type sourceType, object sourceObject, MemberInfo bindingSymbol)
		{
			var bindingAttributes = bindingSymbol.GetCustomAttributes<BindingAttribute>();
			foreach (var bindingAttribute in bindingAttributes)
			{
				var bindingTargetSymbols =
					sourceType.GetMembersWithAttributeInType<BindingTargetAttribute>(
						attribute => attribute.Id == bindingAttribute.TargetId);

				foreach (var bindingTargetSymbol in bindingTargetSymbols)
				{
					var targetObject = bindingTargetSymbol.GetFieldOrPropertyValue(sourceObject);
					var targetSymbol = targetObject?.GetType()
						.GetFieldOrPropertyInfo(bindingAttribute.TargetPropertyName ?? bindingSymbol.Name);

					var doNotConfigure = bindingTargetSymbol.GetCustomAttribute<DoNotConfigureAttribute>();
					if (doNotConfigure != null)
					{
						bindingTargetSymbol.SetFieldOrPropertyValue(sourceObject, null);
					}

					var arguments = new PropertyBinding.Arguments
					{
						BindingAttribute = bindingAttribute,
						SourceProperty = bindingSymbol,
						TargetProperty = targetSymbol,
						Source = sourceObject,
						Target = targetObject
					};

					yield return new PropertyBinding(arguments);
				}
			}
		}

		protected override IEnumerable<MemberInfo> GetBindingSymbols(Type sourceCompileTimeType)
		{
			return sourceCompileTimeType.GetMembersWithAttributeInType<BindingAttribute>();
		}
	}
}
