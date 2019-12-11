using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Gendarme.Framework.Rocks;
using Mono.Cecil;
using Mono.Cecil.Rocks;
using Mono.Collections.Generic;
using CustomAttributeNamedArgument = Mono.Cecil.CustomAttributeNamedArgument;
using ICustomAttributeProvider = Mono.Cecil.ICustomAttributeProvider;

namespace Realmar.DataBindings.Editor.Extensions
{
	internal static class WeaverExtensions
	{
		private const BindingFlags ALL_FLAGS = BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public;

		internal static FieldDefinition GetField(this TypeDefinition type, string name)
		{
			return type.Fields.FirstOrDefault(definition => definition.Name == name);
		}

		internal static List<FieldDefinition> GetFieldsInHierarchy(this TypeDefinition type, string name)
		{
			return GetInHierarchy(type, name, definition => definition.Fields);
		}

		internal static List<PropertyDefinition> GetPropertiesInHierarchy(this TypeDefinition type, string name)
		{
			return GetInHierarchy(type, name, definition => definition.Properties);
		}

		internal static List<MethodDefinition> GetMethodsInHierarchy(this TypeDefinition type, string name)
		{
			return GetInHierarchy(type, name, definition => definition.Methods);
		}

		internal static TypeDefinition GetFirstClassInHierarchy(this TypeDefinition type)
		{
			if (type.BaseType == null) return type;
			return type.BaseType.Resolve().GetFirstClassInHierarchy();
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

			for (var i = 0; i < attributes.Count; i++)
			{
				if (attributes[i].AttributeType.FullName.Equals(typeof(T).FullName, StringComparison.Ordinal))
				{
					yield return attributes[i];
				}
			}
		}

		internal static PropertyDefinition GetProperty(this TypeDefinition instance, string name)
		{
			for (int i = 0; i < instance.Properties.Count; i++)
			{
				var preopertyDef = instance.Properties[i];

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
			for (var i = 0; i < instance.Methods.Count; i++)
			{
				var methodDef = instance.Methods[i];

				if (string.CompareOrdinal(methodDef.Name, name) == 0)
				{
					return methodDef;
				}
			}

			return null;
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

		private static List<TMemberDefinition> GetInHierarchy<TMemberDefinition>(
			this TypeDefinition type,
			string name,
			Func<TypeDefinition, Collection<TMemberDefinition>> getter)
			where TMemberDefinition : class, IMemberDefinition
		{
			List<TMemberDefinition> members;
			var baseType = type.BaseType;
			if (baseType == null)
			{
				members = new List<TMemberDefinition>();
			}
			else
			{
				members = GetInHierarchy(baseType.Resolve(), name, getter);
			}

			var member = getter.Invoke(type).FirstOrDefault(definition => definition.Name == name);
			if (member != null)
			{
				members.Add(member);
			}

			return members;
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
			var type = Type.GetType($"{typeReference.FullName}, {typeReference.Module.Assembly.FullName}");
			return Activator.CreateInstance(type, ctorArgs);
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

		internal static MethodDefinition GetInEqualityOperator(this TypeDefinition type)
		{
			var op = type.GetMethod("op_Inequality");
			if (op == null)
			{
				return type.BaseType?.Resolve().GetInEqualityOperator();
			}

			return op;
		}

		internal static bool HasInEqualityOperator(this TypeDefinition type)
		{
			return type.GetInEqualityOperator() != null;
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
			return a.MetadataToken.Equals(b.MetadataToken);
		}

		internal static bool IsAccessibleFrom(this IMemberDefinition member, TypeDefinition type)
		{
			var sameType = member.DeclaringType.Equals(type);
			if (sameType)
			{
				return true;
			}
			else
			{
				var module = member.GetModule();
				var sameModule = module.Assembly.IsSame(type.Module.Assembly);

				switch (member)
				{
					// TODO clean code
					case FieldDefinition fieldDefinition:
						if (sameModule == false && fieldDefinition.IsVisible() == false)
						{
							return false;
						}

						return fieldDefinition.IsPrivate == false;
					case MethodDefinition methodDefinition:
						if (sameModule == false && methodDefinition.IsVisible() == false)
						{
							return false;
						}

						return methodDefinition.IsPrivate == false;
					case TypeDefinition typeDefinition:
						if (sameModule == false && typeDefinition.IsVisible() == false)
						{
							return false;
						}

						return typeDefinition.IsNestedPrivate == false;
					default:
						throw new ArgumentException($"{member.FullName} cannot be resolved to a concrete definition.");
				}
			}
		}

		/*
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
			Debug.Assert(parentInterfaceDef.IsInterface);
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
			Debug.Assert(parentInterfaceDef.IsInterface);
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
			Debug.Assert(iface1.IsInterface);
			Debug.Assert(iface0.IsInterface);
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
		*/

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
	}
}
