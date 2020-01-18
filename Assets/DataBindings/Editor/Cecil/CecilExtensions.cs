using Gendarme.Framework.Rocks;
using Mono.Cecil;
using Mono.Cecil.Rocks;
using Mono.Collections.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using static Realmar.DataBindings.Editor.Exceptions.YeetHelpers;
using CustomAttributeNamedArgument = Mono.Cecil.CustomAttributeNamedArgument;
using ICustomAttributeProvider = Mono.Cecil.ICustomAttributeProvider;

namespace Realmar.DataBindings.Editor.Cecil
{
	internal static class CecilExtensions
	{
		private const BindingFlags ALL_FLAGS = BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public;

		internal static FieldDefinition GetField(this TypeDefinition type, string name)
		{
			return type.Fields.FirstOrDefault(definition => definition.Name == name);
		}

		internal static IEnumerable<FieldDefinition> GetFieldsInBaseHierarchy(this TypeDefinition type, string name = null)
		{
			return GetInBaseHierarchy(type, name, definition => definition.Fields);
		}

		internal static IEnumerable<PropertyDefinition> GetPropertiesInBaseHierarchy(this TypeDefinition type, string name = null)
		{
			return GetInBaseHierarchy(type, name, definition => definition.Properties);
		}

		internal static IEnumerable<MethodDefinition> GetMethodsInBaseHierarchy(this TypeDefinition type, string name = null)
		{
			return GetInBaseHierarchy(type, name, definition => definition.Methods);
		}

		internal static TypeDefinition GetRootBaseClass(this TypeDefinition type)
		{
			if (type.BaseType == null) return type;
			return type.BaseType.Resolve().GetRootBaseClass();
		}

		internal static bool HasCustomAttribute<T>(this ICustomAttributeProvider instance)
			where T : Attribute
		{
			return GetCustomAttribute<T>(instance) == null;
		}

		internal static CustomAttribute GetCustomAttribute<T>(this ICustomAttributeProvider instance)
			where T : Attribute => instance.GetCustomAttributes<T>().FirstOrDefault();

		internal static IEnumerable<CustomAttribute> GetCustomAttributes<T>(this ICustomAttributeProvider instance)
			where T : Attribute
		{
			var attributes = instance.CustomAttributes;

			foreach (var attribute in attributes)
			{
				if (attribute.AttributeType.FullName.Equals(typeof(T).FullName, StringComparison.Ordinal))
				{
					yield return attribute;
				}
			}
		}

		internal static PropertyDefinition GetProperty(this TypeDefinition instance, string name)
		{
			YeetIfNull(name, nameof(name));

			foreach (var preopertyDef in instance.Properties)
			{
				// Properties can only have one argument or they are an indexer. 
				if (string.CompareOrdinal(preopertyDef.Name, name) == 0 && preopertyDef.Parameters.Count == 0)
				{
					return preopertyDef;
				}
			}

			return null;
		}

		internal static MethodDefinition GetMethod(this TypeDefinition instance, string name)
		{
			return GetMethods(instance, name).FirstOrDefault();
		}

		internal static IEnumerable<MethodDefinition> GetMethods(this TypeDefinition instance, string name)
		{
			YeetIfNull(name, nameof(name));

			foreach (var methodDef in instance.Methods)
			{
				if (string.CompareOrdinal(methodDef.Name, name) == 0)
				{
					yield return methodDef;
				}
			}
		}

		internal static List<MethodDefinition> GetBaseMethods(this MethodDefinition method)
		{
			if (method.IsNewSlot == true)
			{
				return new List<MethodDefinition> { method };
			}
			else
			{
				var baseMethod = method.GetBaseMethod();
				var list = GetBaseMethods(baseMethod);
				list.Add(method);

				return list;
			}
		}

		internal static IEnumerable<TMemberDefinition> GetInBaseHierarchy<TMemberDefinition>(this TypeDefinition type,
			string name, Func<TypeDefinition, Collection<TMemberDefinition>> getter)
			where TMemberDefinition : IMemberDefinition
		{
			YeetIfNull(getter, nameof(getter));

			var types = GetInInterfaces(type, getter).Concat(GetInBaseClassHierarchy(type, getter));
			if (name != null)
			{
				return types.Where(definition => definition.Name == name);
			}

			return types;
		}

		internal static IEnumerable<TMemberDefinition> GetInInterfaces<TMemberDefinition>(this TypeDefinition type,
			string name, Func<TypeDefinition, Collection<TMemberDefinition>> getter)
			where TMemberDefinition : IMemberDefinition
		{
			return GetInInterfaces(type, getter).Where(definition => definition.Name == name);
		}

		internal static IEnumerable<TMemberDefinition> GetInInterfaces<TMemberDefinition>(this TypeDefinition type,
			Func<TypeDefinition, Collection<TMemberDefinition>> getter)
			where TMemberDefinition : IMemberDefinition
		{
			YeetIfNull(getter, nameof(getter));

			var ifaceTypes = type.Interfaces.Select(iface => iface.InterfaceType.Resolve());
			return ifaceTypes.SelectMany(definition => getter.Invoke(definition));
		}

		internal static IEnumerable<TMemberDefinition> GetInBaseClassHierarchy<TMemberDefinition>(this TypeDefinition type,
			Func<TypeDefinition, Collection<TMemberDefinition>> getter)
			where TMemberDefinition : IMemberDefinition
		{
			YeetIfNull(getter, nameof(getter));

			var currentType = type;
			while (currentType != null)
			{
				foreach (var memberDefinition in getter.Invoke(currentType))
				{
					yield return memberDefinition;
				}

				currentType = currentType.BaseType?.Resolve();
			}
		}

		/// <exception cref="T:System.ArgumentException"></exception>
		internal static T GetStoredValue<T>(this CustomAttribute attribute, string symbolName)
		{
			bool Query(Collection<CustomAttributeNamedArgument> collection, string name, out T value)
			{
				for (var i = collection.Count - 1; i >= 0; i--)
				{
					var item = collection[i];
					if (item.Name == name)
					{
						value = (T) item.Argument.Value;
						return true;
					}
				}

				value = default;
				return false;
			}

			YeetIfNull(symbolName, nameof(symbolName));

			T result;
			if (Query(attribute.Properties, symbolName, out result) == false)
			{
				if (Query(attribute.Fields, symbolName, out result) == false)
				{
					throw new ArgumentException($"Cannot find field or property with name {symbolName}");
				}
			}

			return result;
		}

		internal static object CreateInstance(this TypeReference typeReference, params object[] ctorArgs)
		{
			return Activator.CreateInstance(typeReference.GetActualType(), ctorArgs);
		}

		internal static Attribute CreateInstance(this CustomAttribute attribute)
		{
			var ctorArgs = attribute.ConstructorArguments.Select(argument => argument.Value).ToArray();
			var instance = CreateInstance(attribute.AttributeType, ctorArgs);
			var type = instance.GetType();

			foreach (var property in attribute.Properties)
			{
				type.GetProperty(property.Name, ALL_FLAGS).SetValue(instance, property.Argument.Value);
			}

			return (Attribute) instance;
		}

		internal static ModuleDefinition GetModule(this IMemberDefinition member)
		{
			switch (member)
			{
				case EventDefinition eventDefinition:
					return eventDefinition.Module;
				case FieldDefinition fieldDefinition:
					return fieldDefinition.Module;
				case MethodDefinition methodDefinition:
					return methodDefinition.Module;
				case PropertyDefinition propertyDefinition:
					return propertyDefinition.Module;
				case TypeDefinition typeDefinition:
					return typeDefinition.Module;
				default:
					throw new ArgumentException($"Cannot resolve module for {member.FullName}");
			}
		}

		internal static bool IsSame(this AssemblyDefinition a, AssemblyDefinition b)
		{
			return a == b;
		}

		internal static bool IsAccessibleFrom(this IMemberDefinition member, TypeDefinition type)
		{
			YeetIfNull(type, nameof(type));

			var module = member.GetModule();
			var sameModule = module.Assembly.IsSame(type.Module.Assembly);

			bool Check(bool isVisible, bool isPrivate)
			{
				if (sameModule == false && isVisible == false)
				{
					return false;
				}

				return isPrivate == false;
			}

			var sameType = member.DeclaringType.Equals(type);
			if (sameType)
			{
				return true;
			}
			else
			{
				switch (member)
				{
					case FieldDefinition fieldDefinition:
						return Check(fieldDefinition.IsVisible(), fieldDefinition.IsPrivate);
					case MethodDefinition methodDefinition:
						return Check(methodDefinition.IsVisible(), methodDefinition.IsPrivate);
					case TypeDefinition typeDefinition:
						return Check(typeDefinition.IsVisible(), typeDefinition.IsNestedPrivate);
					default:
						throw new ArgumentException($"{member.FullName} cannot be resolved to a concrete definition.");
				}
			}
		}

		/// <summary>
		/// Is childTypeDef a subclass of parentTypeDef. Does not test interface inheritance
		/// </summary>
		/// <param name="childTypeDef"></param>
		/// <param name="parentTypeDef"></param>
		/// <returns></returns>
		internal static bool IsSubclassOf(this TypeDefinition childTypeDef, TypeDefinition parentTypeDef) =>
			childTypeDef.MetadataToken
			!= parentTypeDef.MetadataToken
			&& childTypeDef
				.EnumerateBaseClasses()
				.Any(b => b.MetadataToken == parentTypeDef.MetadataToken);

		/// <summary>
		/// Does childType inherit from parentInterface
		/// </summary>
		/// <param name="childType"></param>
		/// <param name="parentInterfaceDef"></param>
		/// <returns></returns>
		internal static bool DoesAnySubTypeImplementInterface(this TypeDefinition childType,
			TypeDefinition parentInterfaceDef)
		{
			YeetIfNotInterface(parentInterfaceDef);
			return childType
				.EnumerateBaseClasses()
				.Any(typeDefinition => typeDefinition.DoesSpecificTypeImplementInterface(parentInterfaceDef));
		}

		/// <summary>
		/// Does the childType directly inherit from parentInterface. Base
		/// classes of childType are not tested
		/// </summary>
		/// <param name="childTypeDef"></param>
		/// <param name="parentInterfaceDef"></param>
		/// <returns></returns>
		internal static bool DoesSpecificTypeImplementInterface(this TypeDefinition childTypeDef,
			TypeDefinition parentInterfaceDef)
		{
			YeetIfNotInterface(parentInterfaceDef);
			return childTypeDef
				.Interfaces
				.Any(ifaceDef =>
					DoesSpecificInterfaceImplementInterface(ifaceDef.InterfaceType.Resolve(), parentInterfaceDef));
		}

		/// <summary>
		/// Does interface iface0 equal or implement interface iface1
		/// </summary>
		/// <param name="iface0"></param>
		/// <param name="iface1"></param>
		/// <returns></returns>
		internal static bool DoesSpecificInterfaceImplementInterface(TypeDefinition iface0, TypeDefinition iface1)
		{
			YeetIfNotInterface(iface1);
			YeetIfNotInterface(iface0);
			return iface0.MetadataToken == iface1.MetadataToken || iface0.DoesAnySubTypeImplementInterface(iface1);
		}

		/// <summary>
		/// Is source type assignable to target type
		/// </summary>
		/// <param name="target"></param>
		/// <param name="source"></param>
		/// <returns></returns>
		internal static bool IsAssignableFrom(this TypeDefinition target, TypeDefinition source)
			=> target == source
			   || target.MetadataToken == source.MetadataToken
			   || source.IsSubclassOf(target)
			   || target.IsInterface && source.DoesAnySubTypeImplementInterface(target);

		/// <summary>
		/// Enumerate the current type, it's parent and all the way to the top type
		/// </summary>
		/// <param name="klassType"></param>
		/// <returns></returns>
		internal static IEnumerable<TypeDefinition> EnumerateBaseClasses(this TypeDefinition klassType)
		{
			for (var typeDefinition = klassType;
				typeDefinition != null;
				typeDefinition = typeDefinition.BaseType?.Resolve())
			{
				yield return typeDefinition;
			}
		}

		internal static Type GetActualType(this TypeReference typeReference)
		{
			var reflectionName = GetReflectionName(typeReference);
			return Type.GetType(reflectionName);
		}

		static string GetReflectionName(TypeReference typeReference)
		{
			string typeName;

			if (typeReference.IsGenericInstance)
			{
				var genericInstance = (GenericInstanceType) typeReference;
				typeName = $"{genericInstance.Namespace}.{typeReference.Name}[{string.Join(",", genericInstance.GenericArguments.Select(p => GetReflectionName(p)).ToArray())}]";
			}
			else
			{
				typeName = typeReference.FullName;
			}

			return $"{typeName}, {typeReference.Module.Assembly.FullName}";
		}
	}
}
