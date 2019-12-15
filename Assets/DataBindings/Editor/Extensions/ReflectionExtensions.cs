using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Realmar.DataBindings.Editor.Extensions
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

			info.SetFieldOrPropertyValue(target, value, flags);
		}

		internal static void SetFieldOrPropertyValue(this MemberInfo info,
			object target, object value, BindingFlags flags = ALL)
		{
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

		internal static object GetFieldOrPropertyValue(this MemberInfo info, object target, BindingFlags flags = ALL)
		{
			object result = null;

			if (info != null)
			{
				switch (info)
				{
					case FieldInfo fieldInfo:
						result = fieldInfo.GetValue(target);
						break;
					case PropertyInfo propertyInfo:
						result = propertyInfo.GetValue(target);
						break;
				}
			}

			return result;
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

		internal static Type GetTypeWithAttributeInType<TAttribute>(this Type type)
			where TAttribute : Attribute
		{
			if (type.GetCustomAttribute<TAttribute>() != null)
			{
				return type;
			}
			else
			{
				var baseType = type.BaseType;
				if (baseType == null)
				{
					return null;
				}
				else
				{
					return baseType.GetTypeWithAttributeInType<TAttribute>();
				}
			}
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

		internal static MemberInfo GetMemberWithAttributesInType(this Type type, params Type[] attributes)
		{
			return GetMembersWithAttributesInType(type, attributes).FirstOrDefault();
		}

		internal static List<MemberInfo> GetMembersWithAttributesInType(this Type type, params Type[] attributes)
		{
			if (attributes.Length == 0)
			{
				return new List<MemberInfo>();
			}

			List<MemberInfo> list;

			if (type.BaseType == null)
			{
				list = new List<MemberInfo>();
			}
			else
			{
				list = type.BaseType.GetMembersWithAttributesInType(attributes);
			}

			list.AddRange(
				type.GetMembers(ALL_NO_FLAT)
					.Where(info =>
					{
						foreach (var attribute in attributes)
						{
							if (info.GetCustomAttribute(attribute) == null)
							{
								return false;
							}
						}

						return true;
					}));

			return list;
		}
	}
}
