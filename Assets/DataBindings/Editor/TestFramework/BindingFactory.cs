using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Realmar.DataBindings.Editor.Extensions;
using Realmar.DataBindings.Editor.TestFramework.Attributes;

namespace Realmar.DataBindings.Editor.TestFramework
{
	internal class BindingFactory
	{
		private readonly Type[] _types;

		public BindingFactory(Type[] types)
		{
			_types = types;
		}

		public BindingCollection CreateBindings()
		{
			var sourceCompileTimeType =
				_types.GetTypesWithAttributes(typeof(SourceAttribute), typeof(CompileTimeTypeAttribute)).Single();

			var sourceRunTimeType =
				_types.GetTypesWithAttributes(typeof(SourceAttribute), typeof(RunTimeTypeAttribute)).Single();

			var sourceObject = Activator.CreateInstance(sourceRunTimeType);

			ConfigureTargets(sourceRunTimeType, sourceObject);

			var bindingSymbols = sourceCompileTimeType.GetMembersWithAttributeInType<BindingAttribute>();
			var bindings = new List<IBinding>();
			foreach (var bindingSymbol in bindingSymbols)
			{
				bindings.Add(CreateBindingFrom(sourceCompileTimeType, sourceObject, bindingSymbol));
			}

			var bindingInitializer =
				(MethodInfo) sourceCompileTimeType.GetMemberWithAttributeInType<BindingInitializerAttribute>();

			return new BindingCollection(bindings.ToArray(), bindingInitializer, sourceObject);
		}

		private void ConfigureTargets(Type sourceType, object sourceObject)
		{
			var bindingTargets = sourceType.GetMembersWithAttributeInType<BindingTargetAttribute>();
			foreach (var bindingTarget in bindingTargets)
			{
				var bindingTargetAttribute = bindingTarget.GetCustomAttribute<BindingTargetAttribute>();
				var mappingId = bindingTarget.GetCustomAttribute<IdAttribute>();

				var runtimeTypes = _types.GetTypesWithAttributes(new[] {typeof(TargetAttribute), typeof(IdAttribute)},
					attributes =>
					{
						var validTargetId = false;
						var validMappingId = false;

						foreach (var attr in attributes)
						{
							switch (attr)
							{
								case TargetAttribute targetAttr:
									if (targetAttr.Id == bindingTargetAttribute.Id)
									{
										validTargetId = true;
									}

									break;
								case IdAttribute idAttr:
									if (idAttr.Equals(mappingId))
									{
										validMappingId = true;
									}

									break;
							}
						}

						return validTargetId && validMappingId;
					}).ToList();

				if (runtimeTypes.Count > 1)
				{
					throw new AmbiguousMatchException(
						$"Found multiple types with the same set of {nameof(TargetAttribute)} (ID = {bindingTargetAttribute.Id}) and {nameof(IdAttribute)} (ID = {mappingId.Id}).");
				}
				else if (runtimeTypes.Count == 0)
				{
					throw new Exception(
						$"Cannot find set of {nameof(TargetAttribute)} (ID = {bindingTargetAttribute.Id}) and {nameof(IdAttribute)} (ID = {mappingId.Id}).");
				}

				var targetObject = Activator.CreateInstance(runtimeTypes[0]);
				var targetBindingSymbol = sourceType.GetMemberWithAttributeInType<BindingTargetAttribute>(
					attribute => attribute.Equals(bindingTargetAttribute));

				targetBindingSymbol.SetFieldOrPropertyValue(sourceObject, targetObject);
			}
		}

		private Binding CreateBindingFrom(Type sourceType, object sourceObject, MemberInfo bindingSymbol)
		{
			var bindingAttribute = bindingSymbol.GetCustomAttribute<BindingAttribute>();
			var bindingTargetSymbol =
				sourceType.GetMemberWithAttributeInType<BindingTargetAttribute>(attribute =>
					attribute.Id == bindingAttribute.TargetId);
			var targetObject = bindingTargetSymbol.GetFieldOrPropertyValue(sourceObject);
			var targetSymbol = targetObject.GetType()
				.GetFieldOrPropertyInfo(bindingAttribute.TargetPropertyName ?? bindingSymbol.Name);

			var arguments = new Binding.Arguments
			{
				BindingAttribute = bindingAttribute,
				SourceProperty = bindingSymbol,
				TargetProperty = targetSymbol,
				Source = sourceObject,
				Target = targetObject
			};

			return new Binding(arguments);
		}
	}
}
