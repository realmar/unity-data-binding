using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Realmar.DataBindings.Editor.Shared.Extensions;
using Realmar.DataBindings.Editor.TestFramework.Attributes;

namespace Realmar.DataBindings.Editor.TestFramework.Sandbox
{
	internal class BindingFactory
	{
		private readonly Type[] _types;

		internal BindingFactory(Type[] types)
		{
			_types = types;
		}

		internal IBindingCollection CreateBindings()
		{
			var bindingSets = new List<BindingSet>();
			var objects = new List<object>();

			var sourceCompileTimeType =
				_types.GetTypesWithAttributes(typeof(SourceAttribute), typeof(CompileTimeTypeAttribute)).Single();

			var sourceRunTimeTypes =
				_types.GetTypesWithAttributes(typeof(SourceAttribute), typeof(RunTimeTypeAttribute));

			foreach (var sourceRunTimeType in sourceRunTimeTypes)
			{
				var sourceObject = Activator.CreateInstance(sourceRunTimeType);
				objects.Add(sourceObject);

				var targetObjects = ConfigureTargets(sourceCompileTimeType, sourceObject);
				objects.AddRange(targetObjects);

				var bindingSymbols = sourceCompileTimeType.GetMembersWithAttributeInType<BindingAttribute>();
				var bindings = new List<IBinding>();
				foreach (var bindingSymbol in bindingSymbols)
				{
					bindings.AddRange(CreateBindingsFrom(sourceCompileTimeType, sourceObject, bindingSymbol));
				}

				var bindingInitializer =
					(MethodInfo) sourceCompileTimeType.GetMemberWithAttributeInType<BindingInitializerAttribute>();

				bindingSets.Add(new BindingSet(bindings.ToArray(), bindingInitializer, sourceObject));
			}

			return new BindingCollection(bindingSets, objects);
		}

		private IReadOnlyCollection<object> ConfigureTargets(Type sourceType, object sourceObject)
		{
			void ThrowIfNoMatches<T>(IReadOnlyCollection<T> collection,
				BindingTargetAttribute bindingTargetAttribute, IdAttribute mappingId)
			{
				if (collection.Count == 0)
				{
					throw new Exception(
						$"Cannot find set of {nameof(TargetAttribute)} (ID = {bindingTargetAttribute.Id}) " +
						$"and {nameof(IdAttribute)} (ID = {mappingId.Id}) on {sourceType.FullName}.");
				}
			}

			void ThrowIfMultipleMatches<T>(IReadOnlyCollection<T> collection,
				BindingTargetAttribute bindingTargetAttribute, IdAttribute mappingId)
			{
				if (collection.Count > 1)
				{
					throw new AmbiguousMatchException(
						$"Found multiple types with the same set of {nameof(TargetAttribute)} (ID = {bindingTargetAttribute.Id}) " +
						$"and {nameof(IdAttribute)} (ID = {mappingId.Id}) on {sourceType.FullName}.");
				}
			}

			var targetObjects = new List<object>();
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

				object targetObject = null;
				if (runtimeTypes.Count > 0)
				{
					targetObject = Activator.CreateInstance(runtimeTypes[0]);
					targetObjects.Add(targetObject);
				}

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

			return targetObjects;
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
			var bindingAttributes = bindingSymbol.GetCustomAttributes<BindingAttribute>().ToArray();
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
}
