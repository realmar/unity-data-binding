using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mono.Cecil;
using Realmar.DataBindings.Editor.Exceptions;
using Realmar.DataBindings.Editor.Extensions;
using static Realmar.DataBindings.Editor.WeaverHelpers;
using static Realmar.DataBindings.Editor.YeetHelpers;

namespace Realmar.DataBindings.Editor.Emitter
{
	internal class Weaver
	{
		private readonly HashSet<TypeDefinition> _affectedTypesCache = new HashSet<TypeDefinition>();
		private readonly HashSet<TypeDefinition> _baseTypes = new HashSet<TypeDefinition>();
		private readonly DerivativeResolver _derivativeResolver;
		private readonly ILEmitter _emitter = new ILEmitter();
		private readonly HashSet<object> _wovenSetHelpers = new HashSet<object>();

		internal Weaver(DerivativeResolver derivativeResolve)
		{
			_derivativeResolver = derivativeResolve;
		}

		internal void WeaveBinding(WeaveParameters parameters)
		{
			var fromPropertyDeclaringType = parameters.FromProperty.DeclaringType;
			YeetIfInaccessible(parameters.ToProperty.SetMethod, fromPropertyDeclaringType);
			YeetIfInaccessible(parameters.BindingTarget, fromPropertyDeclaringType);

			if (parameters.FromProperty.GetSetMethodOrYeet().IsVirtual == false)
			{
				WeaveBindingSingle(parameters);
			}
			else
			{
				var baseTypes = new HashSet<TypeDefinition>();
				var foundNonAbstract = WeaveBindingsDerivatives(parameters.FromProperty, parameters, baseTypes);

				if (foundNonAbstract == false)
				{
					ThrowNoNonAbstractFound(parameters.FromProperty.Name, baseTypes);
				}
			}
		}

		private bool WeaveBindingsDerivatives(
			PropertyDefinition fromProperty,
			WeaveParameters parameters,
			HashSet<TypeDefinition> baseTypes)
		{
			if (fromProperty != null)
			{
				WeaveBindingSingle(new WeaveParameters(parameters) { FromProperty = fromProperty });

				if (fromProperty.GetSetMethodOrYeet().IsAbstract == false)
				{
					return true;
				}
			}

			var derivedTypes = _derivativeResolver.GetDirectlyDerivedTypes(fromProperty.DeclaringType);
			// TODO eeeh... is that really needed? can we do that more efficient, there will be a lot of duplicates.
			baseTypes.AddRange(derivedTypes.SelectMany(definition => definition.EnumerateBaseClasses()));

			for (var i = derivedTypes.Count - 1; i >= 0; i--)
			{
				var propertyName = fromProperty.Name;
				var nextType = derivedTypes[i];
				var nextProperty = nextType.GetProperty(propertyName);

				if (WeaveBindingsDerivatives(nextProperty, parameters, baseTypes))
				{
					return true;
				}
			}

			return false;
		}

		internal PropertyDefinition WeaveAccessor(
			TypeDefinition sourceType,
			TypeDefinition targetType,
			IMemberDefinition bindingTarget,
			MethodDefinition bindingInitializer)
		{
			var injectedSourceName = sourceType.FullName.Replace(".", "");
			var properties = targetType.GetPropertiesInHierarchy(injectedSourceName);

			PropertyDefinition targetToSourceSymbol;

			if (properties.Count == 0)
			{
				// WEAVE ACCESSOR METHOD
				targetToSourceSymbol = _emitter.EmitAccessor(targetType, injectedSourceName, sourceType, false);
				if (targetType.IsInterface)
				{
					var list = _derivativeResolver.GetDirectlyDerivedTypes(targetType);
					for (var i = list.Count - 1; i >= 0; i--)
					{
						var subject = list[i];
						_emitter.EmitAccessor(
							subject,
							injectedSourceName,
							sourceType,
							true);
					}
				}
			}
			else
			{
				if (properties.Count > 1)
				{
					throw new InvalidProgramException(
						"FATAL ERROR: Cannot weave assembly because multiple target to source fields of the same type are found on the target");
				}

				targetToSourceSymbol = properties[0];
			}

			if (bindingInitializer.IsAbstract)
			{
				var originType = bindingInitializer.DeclaringType;
				var derivedTypes = _derivativeResolver.GetDirectlyDerivedTypes(originType);
				var found = false;

				for (var i = derivedTypes.Count - 1; i >= 0; i--)
				{
					var derivedType = derivedTypes[i];
					var initializer = derivedType.GetMethod(bindingInitializer.Name);
					if (initializer != null)
					{
						_emitter.EmitAccessorInitialization(initializer, bindingTarget, targetToSourceSymbol.SetMethod);
						found = true;
					}
				}

				if (found == false)
				{
					throw new MissingSymbolException(
						$"Could not find overriding non-abstract binding target for {bindingTarget.FullName}");
				}
			}
			else
			{
				_emitter.EmitAccessorInitialization(bindingInitializer, bindingTarget, targetToSourceSymbol.SetMethod);
			}

			return targetToSourceSymbol;
		}

		private void WeaveBindingSingle(WeaveParameters parameters)
		{
			// WEAVE SET HELPERS
			WeaveSetHelpers(parameters.ToType);
			WeaveSetHelpers(parameters.FromProperty.DeclaringType);

			// WEAVE BINDING
			var (fromGetMethod, fromSetMethod) = GetGetAndSetMethod(parameters.FromProperty);
			if (fromSetMethod.IsAbstract == false)
			{
				var toSetHelperMethod = GetSetHelperMethod(parameters.ToProperty, parameters.ToType);
				_emitter.EmitBinding(parameters.BindingTarget, fromGetMethod, fromSetMethod, toSetHelperMethod,
					parameters.EmitNullCheck);
			}
		}

		private void WeaveSetHelpers(TypeDefinition type)
		{
			var setMethods = type.Properties.Select(definition => definition.SetMethod).WhereNotNull();
			foreach (var setMethod in setMethods)
			{
				if (_wovenSetHelpers.Contains(setMethod) == false)
				{
					if (setMethod.IsVirtual || setMethod.IsAbstract)
					{
						WeaveSetHelperRecursive(setMethod);
					}
					else
					{
						WeaveSetHelper(setMethod);
					}
				}
			}
		}

		private void WeaveSetHelper(MethodDefinition setMethod)
		{
			var type = setMethod.DeclaringType;
			var targetSetHelperMethodName = GetTargetSetHelperMethodName(setMethod);
			var targetSetHelperMethod = type.GetMethod(targetSetHelperMethodName);
			if (targetSetHelperMethod == null)
			{
				_emitter.EmitSetHelperMethod(targetSetHelperMethodName, setMethod);
				_wovenSetHelpers.Add(setMethod);
			}
		}

		private void WeaveSetHelperRecursive(MethodDefinition setMethod)
		{
			var originType = setMethod.DeclaringType;
			var baseMethods = setMethod.GetBaseMethods();
			var derivedTypes = _derivativeResolver.GetDerivedTypes(originType);

			for (var i = baseMethods.Count - 1; i >= 0; i--)
			{
				WeaveSetHelper(baseMethods[i]);
			}

			for (var i = derivedTypes.Count - 1; i >= 0; i--)
			{
				var properties = derivedTypes[i].Properties;
				for (var i1 = properties.Count - 1; i1 >= 0; i1--)
				{
					var property = properties[i1];
					var method = property.SetMethod;

					if (method != null)
					{
						WeaveSetHelper(method);
					}
				}
			}
		}

		private void ThrowNoNonAbstractFound(string propertyName, HashSet<TypeDefinition> baseTypes)
		{
			var sb = new StringBuilder();
			sb.Append("Cannot find type->property ");

			foreach (var type in _affectedTypesCache)
			{
				// those issues here only arise with interface bindings
				if (baseTypes.Contains(type))
				{
					// TODO better error message
					// from interface to concrete
					// property is implemented in base class
					// ie.:
					//
					// Derived : Base, IInterface
					// {
					// }
					//
					// Base
					// {
					//         Property
					// }
					//
					// IInterface
					// {
					//         Property
					// }
					//
					//

					sb.Append(type.Name);
					sb.Append("->");
					sb.Append(propertyName);
					sb.Append(", ");
				}
				else
				{
					throw new InvalidProgramException($"FATAL ERROR: Trying weave {propertyName} from {type.Name}" +
					                                  $"but could not find symbol neither in derived types nor in base types");
				}
			}

			sb.Append("nor in derived types. However, the property has been found in a base type." +
			          "This binding configuration is not possible, property must be in the same type or derived types but not base types.");

			throw new MissingSymbolException(sb.ToString());
		}
	}
}
