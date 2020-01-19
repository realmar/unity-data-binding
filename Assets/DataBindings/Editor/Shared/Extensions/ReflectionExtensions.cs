using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using static Realmar.DataBindings.Editor.Exceptions.YeetHelpers;

namespace Realmar.DataBindings.Editor.Shared.Extensions
{
	internal static class ReflectionExtensions
	{
		internal const BindingFlags ALL_NO_FLAT = BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static |
		                                          BindingFlags.Instance;

		internal const BindingFlags ALL = ALL_NO_FLAT | BindingFlags.FlattenHierarchy;

		internal static void SetFieldOrPropertyValue(this Type type,
			string name, object target, object value, BindingFlags flags = ALL)
		{
			var info = GetFieldOrPropertyInfo(type, name, flags);
			if (info == null)
			{
				throw new ArgumentException($"{name} cannot be found in {type.Name}");
			}

			info.SetFieldOrPropertyValue(target, value);
		}

		internal static void SetFieldOrPropertyValue(this MemberInfo info, object target, object value)
		{
			YeetIfNotPropertyOrField(info);

			switch (info)
			{
				case FieldInfo fieldInfo:
					fieldInfo.SetValue(target, value);
					break;
				case PropertyInfo propertyInfo:
					propertyInfo.SetValue(target, value);
					break;
			}
		}

		internal static object GetFieldOrPropertyValue(this Type type,
			string name, object target, BindingFlags flags = ALL)
		{
			var info = GetFieldOrPropertyInfo(type, name, flags);
			if (info == null)
			{
				throw new ArgumentException($"{name} cannot be found in {type.Name}");
			}

			return info.GetFieldOrPropertyValue(target);
		}

		internal static object GetFieldOrPropertyValue(this MemberInfo info, object target)
		{
			YeetIfNotPropertyOrField(info);

			switch (info)
			{
				case FieldInfo fieldInfo:
					return fieldInfo.GetValue(target);
				case PropertyInfo propertyInfo:
					return propertyInfo.GetValue(target);
				default:
					return null;
			}
		}

		internal static MemberInfo GetFieldOrPropertyInfo(this Type type, string name, BindingFlags flags = ALL)
		{
			MemberInfo info;

			info = type.GetField(name, flags);
			if (info == null)
			{
				info = type.GetProperty(name, flags);
			}

			return info;
		}

		internal static Type GetFieldOrPropertyType(this MemberInfo info)
		{
			YeetIfNotPropertyOrField(info);

			switch (info)
			{
				case FieldInfo fieldInfo:
					return fieldInfo.FieldType;
				case PropertyInfo propertyInfo:
					return propertyInfo.PropertyType;
				default:
					return null;
			}
		}

		internal static object InvokeMethod(this Type type, string name, object target, params object[] args)
		{
			return InvokeMethod(type, name, target, ALL, args);
		}

		internal static object InvokeMethod(this Type type,
			string name, object target, BindingFlags flags, params object[] args)
		{
			var method = type.GetMethod(name, flags);
			if (method == null)
			{
				throw new ArgumentException($"Could not find method {name} in type {type.Name}");
			}

			return method.Invoke(target, args);
		}

		internal static IEnumerable<Type> GetTypesWithAttribute<TAttribute>(this IEnumerable<Type> types)
			where TAttribute : Attribute
		{
			return GetTypesWithAttributes(types, typeof(TAttribute));
		}

		internal static IEnumerable<Type> GetTypesWithAttributes(this IEnumerable<Type> types, params Type[] attributes)
		{
			return GetTypesWithAttributes(types, attributes, collection => true);
		}

		internal static IEnumerable<Type> GetTypesWithAttributes(this IEnumerable<Type> types,
			Type[] attributes,
			Predicate<IReadOnlyCollection<Attribute>> predicate)
		{
			if (attributes.Length == 0)
			{
				return Enumerable.Empty<Type>();
			}

			return types.Where(type =>
			{
				var attributesOnType = new List<Attribute>(2);
				foreach (var attribute in attributes)
				{
					var customAttributes = type.GetCustomAttributes(attribute);
					if (customAttributes.Any() == false)
					{
						return false;
					}

					attributesOnType.AddRange(customAttributes);
				}

				return predicate.Invoke(attributesOnType);
			});
		}

		internal static MemberInfo GetMemberWithAttributeInType<TAttribute>(this Type type)
			where TAttribute : Attribute
		{
			return GetMembersWithAttributeInType<TAttribute>(type).FirstOrDefault();
		}

		internal static MemberInfo GetMemberWithAttributeInType<TAttribute>(this Type type,
			Predicate<TAttribute> predicate)
			where TAttribute : Attribute
		{
			return GetMembersWithAttributeInType<TAttribute>(type)
				.FirstOrDefault(info => predicate.Invoke(info.GetCustomAttribute<TAttribute>()));
		}

		internal static List<MemberInfo> GetMembersWithAttributeInType<TAttribute>(this Type type)
			where TAttribute : Attribute
		{
			return GetMembersWithAttributesInType(type, typeof(TAttribute));
		}

		internal static List<MemberInfo> GetMembersWithAttributeInType<TAttribute>(this Type type,
			Predicate<TAttribute> predicate)
			where TAttribute : Attribute
		{
			return GetMembersWithAttributeInType<TAttribute>(type)
				.Where(info => predicate.Invoke(info.GetCustomAttribute<TAttribute>()))
				.ToList();
		}

		internal static MemberInfo GetMemberWithAttributesInType(this Type type, params Type[] attributes)
		{
			return GetMembersWithAttributesInType(type, attributes).FirstOrDefault();
		}

		internal static List<MemberInfo> GetMembersWithAttributesInType(this Type type, params Type[] attributes)
		{
			return GetMembersWithAttributesInType(type, attributes, collection => true);
		}

		internal static List<MemberInfo> GetMembersWithAttributesInType(this Type type,
			Type[] attributes,
			Predicate<IReadOnlyCollection<Attribute>> predicate)
		{
			var localPredicate = predicate;

			bool Filter(MemberInfo info)
			{
				var attributeInstances = new List<Attribute>();
				foreach (var attribute in attributes)
				{
					var attributeInstance = info.GetCustomAttributes(attribute).ToArray();
					if (attributeInstance.Any() == false)
					{
						return false;
					}

					attributeInstances.AddRange(attributeInstance);
				}

				return localPredicate.Invoke(attributeInstances);
			}

			var list = new List<MemberInfo>();
			if (attributes.Length > 0)
			{
				list.AddRange(type.GetInterfaces().SelectMany(t => t.GetMembers()).Where(Filter));

				var currentType = type;
				while (currentType != null)
				{
					var members = currentType.GetMembers(ALL_NO_FLAT);
					list.AddRange(members.Where(Filter));
					currentType = currentType.BaseType;
				}
			}

			return list;
		}

		internal static IEnumerable<Type> GetBaseTypes(this Type type)
		{
			var current = type;

			while (current != null)
			{
				yield return current;
				current = current.BaseType;
			}

			foreach (var iface in type.GetInterfaces())
			{
				yield return iface;
			}
		}
	}
}
