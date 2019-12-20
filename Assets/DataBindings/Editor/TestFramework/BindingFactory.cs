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

			ConfigureTargets(sourceCompileTimeType, sourceObject);

			var bindingSymbols = sourceCompileTimeType.GetMembersWithAttributeInType<BindingAttribute>();
			var bindings = new List<IBinding>();
			foreach (var bindingSymbol in bindingSymbols)
			{
				bindings.AddRange(CreateBindingsFrom(sourceCompileTimeType, sourceObject, bindingSymbol));
			}

			var bindingInitializer =
				(MethodInfo) sourceCompileTimeType.GetMemberWithAttributeInType<BindingInitializerAttribute>();

			return new BindingCollection(bindings.ToArray(), bindingInitializer, sourceObject);
		}

		private void ConfigureTargets(Type sourceType, object sourceObject)
		{
			void ThrowIfNoMatches<T>(IReadOnlyCollection<T> colletion,
				BindingTargetAttribute bindingTargetAttribute, IdAttribute mappingId)
			{
				if (colletion.Count == 0)
				{
					throw new Exception(
						$"Cannot find set of {nameof(TargetAttribute)} (ID = {bindingTargetAttribute.Id}) " +
						$"and {nameof(IdAttribute)} (ID = {mappingId.Id}) on {sourceType.FullName}.");
				}
			}

			void ThrowIfMultipleMatches<T>(IReadOnlyCollection<T> colletion,
				BindingTargetAttribute bindingTargetAttribute, IdAttribute mappingId)
			{
				if (colletion.Count > 1)
				{
					throw new AmbiguousMatchException(
						$"Found multiple types with the same set of {nameof(TargetAttribute)} (ID = {bindingTargetAttribute.Id}) " +
						$"and {nameof(IdAttribute)} (ID = {mappingId.Id}) on {sourceType.FullName}.");
				}
			}

			var bindingTargets = sourceType.GetMembersWithAttributeInType<BindingTargetAttribute>();
			foreach (var bindingTarget in bindingTargets)
			{
				var bindingTargetAttribute = bindingTarget.GetCustomAttribute<BindingTargetAttribute>();
				var mappingId = bindingTarget.GetCustomAttribute<IdAttribute>();

				var runtimeTypes = _types.GetTypesWithAttributes(
						new[] { typeof(TargetAttribute), typeof(IdAttribute) },
						attributes => ContainsMatchingTargetAndIdPair(attributes, bindingTargetAttribute, mappingId))
					.ToList();

				ThrowIfMultipleMatches(runtimeTypes, bindingTargetAttribute, mappingId);
				ThrowIfNoMatches(runtimeTypes, bindingTargetAttribute, mappingId);

				var targetObject = Activator.CreateInstance(runtimeTypes[0]);
				var targetBindingSymbols = sourceType.GetMembersWithAttributesInType(
						new[] { typeof(BindingTargetAttribute), typeof(IdAttribute) },
						attributes => ContainsMatchingTargetAndIdPair(attributes, bindingTargetAttribute, mappingId))
					.ToArray();

				ThrowIfMultipleMatches(targetBindingSymbols, bindingTargetAttribute, mappingId);
				ThrowIfNoMatches(targetBindingSymbols, bindingTargetAttribute, mappingId);

				var targetBindingSymbol = targetBindingSymbols[0];
				if (targetBindingSymbol is PropertyInfo propertyInfo && propertyInfo.CanWrite == false)
				{
					throw new Exception(
						$"TestFramework requires BindingTargets to have a setter {propertyInfo.DeclaringType.FullName}::{propertyInfo.Name} is missing this setter.");
				}

				targetBindingSymbol.SetFieldOrPropertyValue(sourceObject, targetObject);
			}
		}

		private static bool ContainsMatchingTargetAndIdPair(
			IReadOnlyCollection<Attribute> attributes,
			BindingTargetAttribute bindingTargetAttribute,
			IdAttribute mappingId)
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
					case BindingTargetAttribute bindingTargetAttr:
						if (bindingTargetAttr.Id == bindingTargetAttribute.Id)
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
		}

		private IEnumerable<Binding> CreateBindingsFrom(Type sourceType, object sourceObject, MemberInfo bindingSymbol)
		{
			var bindingAttribute = bindingSymbol.GetCustomAttribute<BindingAttribute>();
			var bindingTargetSymbols =
				sourceType.GetMembersWithAttributeInType<BindingTargetAttribute>(attribute =>
					attribute.Id == bindingAttribute.TargetId);

			foreach (var bindingTargetSymbol in bindingTargetSymbols)
			{
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

				yield return new Binding(arguments);
			}
		}
	}
}
