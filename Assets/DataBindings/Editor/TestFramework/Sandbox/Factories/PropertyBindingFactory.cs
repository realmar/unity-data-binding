using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Realmar.DataBindings.Editor.Shared.Extensions;
using Realmar.DataBindings.Editor.TestFramework.Attributes;
using static Realmar.DataBindings.Editor.TestFramework.Sandbox.SandboxHelpers;

namespace Realmar.DataBindings.Editor.TestFramework.Sandbox.Factories
{
	internal class PropertyBindingFactory : IBindingFactory
	{
		private readonly IReadOnlyCollection<Type> _types;

		internal PropertyBindingFactory(IReadOnlyCollection<Type> types)
		{
			_types = types;
		}

		public void CreateBindings(IBindingCollection collection)
		{
			var targetObjectsToId = new Dictionary<int, object>();

			var sourceCompileTimeTypes =
				_types.GetTypesWithAttributes(typeof(SourceAttribute), typeof(CompileTimeTypeAttribute));

			var sourceRunTimeTypes =
				_types.GetTypesWithAttributes(typeof(SourceAttribute), typeof(RunTimeTypeAttribute));

			foreach (var sourceCompileTimeType in sourceCompileTimeTypes)
			{
				foreach (var sourceRunTimeType in sourceRunTimeTypes.Where(type => sourceCompileTimeType.IsAssignableFrom(type)))
				{
					var objects = new List<object>();
					var sourceObject = CreateInstance(sourceRunTimeType);
					objects.Add(sourceObject);

					var targetObjects = ConfigureTargets(sourceCompileTimeType, sourceObject, targetObjectsToId);
					objects.AddRange(targetObjects);

					var bindingSymbols = sourceCompileTimeType.GetMembersWithAttributeInType<BindingAttribute>();
					var bindings = new List<IPropertyBinding>();

					foreach (var bindingSymbol in bindingSymbols)
					{
						bindings.AddRange(CreateBindingsFrom(sourceCompileTimeType, sourceObject, bindingSymbol));
					}

					var bindingInitializer = (MethodInfo) sourceCompileTimeType.GetMemberWithAttributeInType<BindingInitializerAttribute>();
					var bindingSet = new BindingSet(bindings.ToArray(), bindingInitializer, sourceObject);

					collection.AddBindingSet(bindingSet, objects);
				}
			}
		}

		private IReadOnlyCollection<object> ConfigureTargets(Type sourceType, object sourceObject, Dictionary<int, object> targetObjectsToId)
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
					// reuse target instance if Id matches
					var runtimeType = runtimeTypes[0];
					var id = mappingId.Id;

					if (targetObjectsToId.ContainsKey(id))
					{
						targetObject = targetObjectsToId[id];
					}
					else
					{
						targetObject = CreateInstance(runtimeType);
						targetObjects.Add(targetObject);
						targetObjectsToId[id] = targetObject;
					}
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

		private IEnumerable<PropertyBinding> CreateBindingsFrom(Type sourceType, object sourceObject, MemberInfo bindingSymbol)
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
	}
}
