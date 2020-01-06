using Mono.Cecil;
using Realmar.DataBindings.Editor.Cecil;
using Realmar.DataBindings.Editor.Emitting;
using Realmar.DataBindings.Editor.Exceptions;
using Realmar.DataBindings.Editor.IoC;
using Realmar.DataBindings.Editor.Shared.Extensions;
using System.Collections.Generic;
using System.Linq;
using static Realmar.DataBindings.Editor.Exceptions.YeetHelpers;
using static Realmar.DataBindings.Editor.Shared.SharedHelpers;
using static Realmar.DataBindings.Editor.Weaving.WeaveHelpers;

namespace Realmar.DataBindings.Editor.Weaving
{
	internal class Weaver
	{
		private readonly HashSet<MethodDefinition> _wovenSetHelpers = new HashSet<MethodDefinition>();
		private readonly HashSet<int> _wovenBindings = new HashSet<int>();
		private readonly Emitter _emitter = ServiceLocator.Current.Resolve<Emitter>();
		private readonly DerivativeResolver _derivativeResolver = ServiceLocator.Current.Resolve<DerivativeResolver>();

		internal void Weave(in WeaveParameters parameters)
		{
			var fromPropertyDeclaringType = parameters.FromProperty.DeclaringType;
			YeetIfInaccessible(parameters.ToProperty.SetMethod, fromPropertyDeclaringType);
			YeetIfInaccessible(parameters.BindingTarget, fromPropertyDeclaringType);

			WeaveSetHelperForTypes(new[] { parameters.ToType, parameters.FromProperty.DeclaringType });

			if (parameters.FromProperty.GetSetMethodOrYeet().IsVirtual == false)
			{
				WeaveNonAbstractBinding(parameters);
			}
			else
			{
				WeaveBindingInHierarchy(parameters);
			}
		}

		internal PropertyDefinition WeaveTargetToSourceAccessorCommand(AccessorSymbolParameters parameters)
		{
			var accessorSymbol = GetAccessorPropertyInHierarchy(parameters.SourceType, parameters.TargetType);

			if (accessorSymbol == null)
			{
				// WEAVE ACCESSOR METHOD
				accessorSymbol = _emitter.EmitAccessor(parameters.TargetType, parameters.SourceType, false);
				if (parameters.TargetType.IsInterface)
				{
					var list = _derivativeResolver.GetDirectlyDerivedTypes(parameters.TargetType);
					foreach (var derivedType in list)
					{
						if (GetAccessorProperty(parameters.SourceType, derivedType) == null)
						{
							_emitter.EmitAccessor(derivedType, parameters.SourceType, true);
						}
					}
				}
			}

			var accessorSymbolSetter = accessorSymbol.SetMethod;
			if (parameters.BindingInitializer.IsAbstract)
			{
				WeaveAbstractAccessorInitialization(accessorSymbolSetter, parameters);
			}
			else
			{
				_emitter.EmitAccessorInitialization(accessorSymbolSetter, parameters.BindingInitializer, parameters.BindingTarget, parameters.Settings.ThrowOnFailure);
			}

			return accessorSymbol;
		}

		private void WeaveSetHelperForTypes(IEnumerable<TypeDefinition> types)
		{
			foreach (var type in types)
			{
				var setMethods = type
					.GetPropertiesInBaseHierarchy()
					.Where(definition => definition.DeclaringType.Module.Assembly.IsSame(type.Module.Assembly))
					.Select(definition => definition.SetMethod)
					.WhereNotNull();

				foreach (var setMethod in setMethods)
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

			if (targetSetHelperMethod == null && _wovenSetHelpers.Contains(setMethod) == false)
			{
				_emitter.EmitSetHelper(targetSetHelperMethodName, setMethod);
				_wovenSetHelpers.Add(setMethod);
			}
		}

		private void WeaveSetHelperRecursive(MethodDefinition setMethod)
		{
			var originType = setMethod.DeclaringType;
			var baseMethods = setMethod.GetBaseMethods();
			var derivedTypes = _derivativeResolver.GetDerivedTypes(originType);

			var allSetters = baseMethods
				.Concat(
					derivedTypes
						.Where(definition => definition != originType)
						.Where(definition => definition.Module.Assembly.IsSame(originType.Module.Assembly))
						.SelectMany(definition => definition.Properties
							.Select(propertyDefinition => propertyDefinition.SetMethod)
							.WhereNotNull()))
				.ToArray();

			foreach (var methodDefinition in allSetters)
			{
				WeaveSetHelper(methodDefinition);
			}
		}

		private void WeaveNonAbstractBinding(in WeaveParameters parameters)
		{
			var hash = GetBindingHashCode(parameters);

			if (_wovenBindings.Contains(hash) == false)
			{
				_wovenBindings.Add(hash);
				_emitter.EmitBinding(EmitParameters.FromWeaveParameters(parameters));

				//var setHelperMethod = GetSetHelperMethod(parameters.FromProperty, parameters.FromProperty.DeclaringType);
				//_emitter.EmitBinding(
				//	new EmitParameters(
				//		parameters.BindingTarget,
				//		parameters.FromProperty.GetGetMethodOrYeet(),
				//		setHelperMethod,
				//		GetSetHelperMethod(parameters.ToProperty, parameters.ToType),
				//		parameters.EmitNullCheck));


				/*
				 * v to vm via sh
				 * vm to v via sh
				 *
				 * vm to m via sh | v to vm?
				 * m to vm via sh
				 *
				 * v to vm via s --> vm to v via sh | vm to v is unnecessary OP
				 *				 --> vm to m via sh
				 *
				 * m to vm via s --> vm to m via sh | vm to m is unnecessary op
				 *				 --> vm to v via sh
				 *
				 * --> TwoWay: in vm you want to have a setter which contains all bindings except the one to the data source
				 *			   when adding new bindings, those special setters need to be kept up to date
				 *
				 */
			}
		}

		private void WeaveBindingInHierarchy(in WeaveParameters parameters)
		{
			var foundNonAbstract = false;
			var fromProperty = parameters.FromProperty;
			var fromPropertyName = fromProperty.Name;
			var derivedTypes = _derivativeResolver.GetDerivedTypes(fromProperty.DeclaringType);

			foreach (var typeDefinition in derivedTypes)
			{
				var property = typeDefinition.GetProperty(fromPropertyName);
				if (property != null && property.GetSetMethodOrYeet().IsAbstract == false)
				{
					var newParameters = new WeaveParameters(
						fromProperty: property,
						toType: parameters.ToType,
						toProperty: parameters.ToProperty,
						bindingTarget: parameters.BindingTarget,
						emitNullCheck: parameters.EmitNullCheck);

					WeaveNonAbstractBinding(newParameters);
					foundNonAbstract = true;
				}
			}

			if (foundNonAbstract == false)
			{
				throw new MissingNonAbstractSymbolException(fromProperty.FullName);
			}
		}

		private void WeaveAbstractAccessorInitialization(MethodDefinition accessorSymbol, AccessorSymbolParameters accessorSymbolParameters)
		{
			var originType = accessorSymbolParameters.BindingInitializer.DeclaringType;

			var found = WeaveAccessorInitializationInType(accessorSymbol, originType, accessorSymbolParameters);
			if (found == false)
			{
				throw new MissingNonAbstractBindingInitializer(accessorSymbolParameters.BindingInitializer.FullName);
			}
		}

		private bool WeaveAccessorInitializationInType(MethodDefinition accessorSymbol, TypeDefinition derivedType, AccessorSymbolParameters parameters)
		{
			var initializer = derivedType.GetMethod(parameters.BindingInitializer.Name);
			bool found;

			if (initializer != null)
			{
				if (initializer.IsAbstract == false)
				{
					_emitter.EmitAccessorInitialization(accessorSymbol, initializer, parameters.BindingTarget, parameters.Settings.ThrowOnFailure);
					found = true;
				}
				else
				{
					found = WeaveAccessorInitializationInDerivedTypes(accessorSymbol, derivedType, parameters);
				}
			}
			else
			{
				found = WeaveAccessorInitializationInDerivedTypes(accessorSymbol, derivedType, parameters);
			}

			return found;
		}

		private bool WeaveAccessorInitializationInDerivedTypes(MethodDefinition accessorSymbol, TypeDefinition derivedType, AccessorSymbolParameters parameters)
		{
			var newDerivedTypes = _derivativeResolver.GetDirectlyDerivedTypes(derivedType);

			var found = false;

			foreach (var newDerivedType in newDerivedTypes)
			{
				found |= WeaveAccessorInitializationInType(accessorSymbol, newDerivedType, parameters);
			}

			return found;
		}
	}
}
