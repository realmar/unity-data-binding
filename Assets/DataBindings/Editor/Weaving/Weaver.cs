using Mono.Cecil;
using Realmar.DataBindings.Editor.Cecil;
using Realmar.DataBindings.Editor.Emitting;
using Realmar.DataBindings.Editor.Exceptions;
using Realmar.DataBindings.Editor.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using static Realmar.DataBindings.Editor.Exceptions.YeetHelpers;
using static Realmar.DataBindings.Editor.Weaving.WeaverHelpers;

namespace Realmar.DataBindings.Editor.Weaving
{
	// TODO Weave across assembly boundaries
	internal class Weaver
	{
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
				var foundNonAbstract = WeaveBindingsDerivatives(parameters.FromProperty, parameters);
				if (foundNonAbstract == false)
				{
					throw new Exception("TODO pull exception to where it actually happens");
				}
			}
		}

		private bool WeaveBindingsDerivatives(PropertyDefinition fromProperty, WeaveParameters parameters)
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

			for (var i = derivedTypes.Count - 1; i >= 0; i--)
			{
				var propertyName = fromProperty.Name;
				var nextType = derivedTypes[i];
				var nextProperty = nextType.GetProperty(propertyName);

				if (WeaveBindingsDerivatives(nextProperty, parameters))
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
					// TODO - ABSTRACT - Exception
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
			var fromGetMethod = parameters.FromProperty.GetGetMethodOrYeet();
			var fromSetMethod = parameters.FromProperty.GetSetMethodOrYeet();

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
	}
}
