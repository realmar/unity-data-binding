using Mono.Cecil;
using Realmar.DataBindings.Editor.BCL.System;
using Realmar.DataBindings.Editor.Cecil;
using Realmar.DataBindings.Editor.Emitting;
using Realmar.DataBindings.Editor.Exceptions;
using Realmar.DataBindings.Editor.IoC;
using Realmar.DataBindings.Editor.Shared.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using static Realmar.DataBindings.Editor.Exceptions.YeetHelpers;
using static Realmar.DataBindings.Editor.Shared.SharedHelpers;
using static Realmar.DataBindings.Editor.Weaving.WeaveHelpers;

namespace Realmar.DataBindings.Editor.Weaving
{
	internal class Weaver
	{
		/*
		 * We want to have following scenario:
		 *
		 * View --> ViewModel --> Model
		 * View <--> ViewModel --> Model
		 *
		 * View <-- ViewModel <-- Model
		 * View <--> ViewModel <-- Model
		 *
		 * View --> ViewModel <--> Model
		 * View <--> ViewModel <--> Model
		 *
		 * Problem is that when always using set helpers (instead of property setter) only
		 * we cannot do bindings across 3 objects because the sh do not contain
		 * the woven bindings from the actual property setter.
		 *
		 * If we would always use the property setter, then there would be a stack overflow
		 * when doing TwoWay bindings, because the setter would call each other.
		 *
		 * If we call sh in OneWay binding and property setter in FromTarget binding then
		 * we end up calling also the Model sh instead of just the ViewModel and View.
		 * eg.:
		 * Model uses VM setter --> VM setter calls Model sh <-- this call is unnecessary because data comes from Model, we dont need to set it again
		 *						--> VM setter calls View sh
		 *
		 * To solve this problem we introduce a special set helper which contains all
		 * the woven bindings from the property setter except the one which points to
		 * the Model.
		 *
		 * ---------------------------------------------------------------------------------------
		 *
		 * we need to weave special set helpers for FromTargetBindings/TwoWay bindings
		 * this special set helper should have all the woven bindings from the property setter
		 * except the one to itself.
		 *
		 * A has TwoWay binding to B
		 * A <--> B
		 *
		 * OneWay bindings
		 * A --> C
		 * A --> D
		 *
		 * A setter has following calls:
		 * A --> B
		 * A --> C
		 * A --> D
		 *
		 * A sh has none of those calls
		 *
		 * A uses sh to B
		 * B uses special sh to A which includes all other bindings from the A setter
		 *
		 * A special sh has following calls
		 * A --> C
		 * A --> D
		 *
		 * ---------------------------------------------------------------------------------------
		 *
		 * Instead of differentiating between set helper and special set helper we could
		 * tread all set helpers as special set helpers. Meaning that the sh is specific to the
		 * data source.
		 * This will probably increase the DLL size significantly as there will be a lot of
		 * duplicate code in all those set helpers.
		 *
		 * Well, we probably need to do this because even with OneWay bindings we need
		 * the woven bindings from the property setter in order to populate the data
		 * further.
		 *
		 * --> special set helpers only it is then.
		 */

		private readonly struct BindingCommand
		{
			internal EmitBindingCommand Command { get; }
			internal PropertyDefinition ToProperty { get; }

			internal BindingCommand(EmitBindingCommand command, PropertyDefinition property)
			{
				Command = command;
				ToProperty = property;
			}

			internal void Deconstruct(out EmitBindingCommand command, out PropertyDefinition toProperty)
			{
				command = Command;
				toProperty = ToProperty;
			}
		}

		private readonly Dictionary<PropertyDefinition, List<BindingCommand>> _bindings = new Dictionary<PropertyDefinition, List<BindingCommand>>();
		private readonly Dictionary<PropertyDefinition, HashSet<PropertyDefinition>> _propertiesSettingOthers = new Dictionary<PropertyDefinition, HashSet<PropertyDefinition>>();

		private readonly HashSet<TypeDefinition> _createdMethodMementos = new HashSet<TypeDefinition>();
		private readonly Dictionary<MethodDefinition, MethodMemento> _methods = new Dictionary<MethodDefinition, MethodMemento>();
		private readonly Dictionary<int, MethodDefinition> _setHelpers = new Dictionary<int, MethodDefinition>();

		private readonly Emitter _emitter = ServiceLocator.Current.Resolve<Emitter>();
		private readonly DerivativeResolver _derivativeResolver = ServiceLocator.Current.Resolve<DerivativeResolver>();
		private readonly Random _random = new Random();

		private readonly HashSet<int> _wovenBindings = new HashSet<int>();

		internal void Weave(in WeaveParameters parameters)
		{
			var fromPropertyDeclaringType = parameters.FromProperty.DeclaringType;
			YeetIfInaccessible(parameters.ToProperty.SetMethod, fromPropertyDeclaringType);
			YeetIfInaccessible(parameters.BindingTarget, fromPropertyDeclaringType);

			CreateMethodMementos(new[] { parameters.ToType, parameters.FromProperty.DeclaringType });

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

		private void CreateMethodMementos(IEnumerable<TypeDefinition> types)
		{
			foreach (var type in types)
			{
				if (_createdMethodMementos.Contains(type) == false)
				{
					var derivedProperties = _derivativeResolver
						.GetDerivedTypes(type)
						.SelectMany(definition => definition.Properties);

					var setters = type
						.GetPropertiesInBaseHierarchy()
						.Concat(derivedProperties)
						.Where(definition => definition.DeclaringType.Module.Assembly.IsSame(type.Module.Assembly))
						.Select(definition => definition.SetMethod)
						.WhereNotNull()
						.Where(definition => definition.IsAbstract == false);


					foreach (var setter in setters)
					{
						if (_methods.ContainsKey(setter) == false)
						{
							_methods[setter] = _emitter.CreateMethodMemento(setter);
						}
					}

					_createdMethodMementos.Add(type);
				}
			}
		}

		private MethodDefinition WeaveSetHelper(in WeaveParameters parameters)
		{
			MethodDefinition result = null;
			var fromProperty = parameters.FromProperty;
			var helperHash = HashCode.Combine(fromProperty, parameters.ToProperty);

			if (_setHelpers.ContainsKey(helperHash))
			{
				result = _setHelpers[helperHash];
			}
			else
			{
				var fromType = fromProperty.DeclaringType;
				var toProperty = parameters.ToProperty;
				var toSetter = toProperty.GetSetMethodOrYeet();
				var setHelperName = $"from_{fromType.Name}_to_{toProperty.DeclaringType.Name}_with_{toProperty.Name}_{_random.Next()}";

				if (toSetter.IsVirtual || toSetter.IsAbstract)
				{
					foreach (var (property, setHelper) in WeaveSetHelperRecursive(fromProperty, toProperty, setHelperName))
					{
						_setHelpers[HashCode.Combine(fromProperty, property)] = setHelper;

						if (property == toProperty)
						{
							result = setHelper;
						}
					}
				}
				else
				{
					result = WeaveSetHelper(fromProperty, toProperty, setHelperName);
					_setHelpers[helperHash] = result;
				}

				if (result == null)
				{
					throw new BigOOFException($"Weaving set helper failed name {setHelperName} from {fromProperty} to {toProperty}");
				}
			}

			return result;
		}

		private MethodDefinition WeaveSetHelper(PropertyDefinition fromProperty, PropertyDefinition toProperty, string name)
		{
			MethodDefinition setHelper = null;
			var setter = toProperty.GetSetMethodOrYeet();
			if (_methods.ContainsKey(setter) == false) 
			{
				if (setter.IsAbstract == false)
				{
					throw new BigOOFException($"Property setter is not abstract but has no method memento. {toProperty.FullName}");
				}

				setHelper = _emitter.EmitSetHelper(name, setter);
			}
			else
			{
				setHelper = _emitter.EmitSetHelper(name, setter, _methods[setter]);
				if (_bindings.ContainsKey(toProperty))
				{
					// apply existing bindings
					var bindings = _bindings[toProperty];
					foreach (var (command, targetProperty) in bindings)
					{
						if (targetProperty != fromProperty)
						{
							command.Emit(setHelper);
						}
					}
				}
			}

			return setHelper;
		}

		private IEnumerable<(PropertyDefinition Source, MethodDefinition Helper)> WeaveSetHelperRecursive(PropertyDefinition fromProperty, PropertyDefinition toProperty, string name)
		{
			var originType = toProperty.DeclaringType;
			var properties = _derivativeResolver
				.GetDerivedTypes(originType)
				.SelectMany(definition => definition.Properties)
				.Concat(originType.GetPropertiesInBaseHierarchy())
				.Where(definition => definition.Name == toProperty.Name)
				.Where(definition => definition.DeclaringType.Module.Assembly.IsSame(originType.Module.Assembly))
				.Distinct();

			foreach (var p in properties)
			{
				yield return (p, WeaveSetHelper(fromProperty, p, name));
			}
		}

		private void WeaveNonAbstractBinding(in WeaveParameters parameters)
		{
			var hash = GetBindingHashCode(parameters);

			if (_wovenBindings.Contains(hash) == false)
			{
				var bindingTarget = parameters.BindingTarget;
				var fromProperty = parameters.FromProperty;
				var fromGetter = fromProperty.GetGetMethodOrYeet();
				var fromSetter = fromProperty.GetSetMethodOrYeet();

				var toProperty = parameters.ToProperty;
				var toSetter = WeaveSetHelper(parameters);

				_wovenBindings.Add(hash);

				var emitCommand = _emitter.CreateEmitCommand(new EmitParameters(bindingTarget, fromGetter, fromSetter, toSetter, parameters.EmitNullCheck));
				emitCommand.Emit(fromSetter);

				if (_bindings.TryGetValue(fromProperty, out var commands) == false)
				{
					commands = new List<BindingCommand>();
					_bindings[fromProperty] = commands;
				}

				commands.Add(new BindingCommand(emitCommand, toProperty));

				foreach (var (origin, destinations) in _propertiesSettingOthers)
				{
					if (destinations.Contains(fromProperty) && toProperty != origin)
					{
						var setHelper = _setHelpers[HashCode.Combine(origin, fromProperty)];
						emitCommand.Emit(setHelper);
					}
				}

				if (_propertiesSettingOthers.TryGetValue(fromProperty, out var references) == false)
				{
					references = new HashSet<PropertyDefinition>();
					_propertiesSettingOthers[fromProperty] = references;
				}

				references.Add(toProperty);

				/*
				 * v to vm via sh
				 * vm to v via sh
				 *
				 * vm to m via sh | v to vm?
				 * m to vm via sh
				 *
				 * v to vm via s --> vm to v via sh | vm to v is unnecessary op
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
						property,
						parameters.ToType,
						parameters.ToProperty,
						parameters.BindingTarget,
						parameters.EmitNullCheck);

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
