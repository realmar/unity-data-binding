using System;
using System.Collections.Generic;
using System.Linq;
using Mono.Cecil;
using Realmar.DataBindings.Editor.Extensions;

namespace Realmar.DataBindings.Editor.Misc
{
	internal class AttributeResolver
	{
		private readonly Cache _cache = new Cache();

		internal IReadOnlyList<(ICustomAttributeProvider Source, CustomAttribute Attribute)>
			GetCustomAttributesOfSymbolsInType<T>(TypeDefinition type)
			where T : Attribute
		{
			return _cache.Get(type, GetCustomAttributesOfSymbolsInType_Internal<T>);
		}

		private static List<(ICustomAttributeProvider Source, CustomAttribute Attribute)>
			GetCustomAttributesOfSymbolsInType_Internal<T>(TypeDefinition type)
			where T : Attribute
		{
			return type.Fields
				.Cast<ICustomAttributeProvider>()
				.Concat(type.Properties)
				.Select(x => (Source: x, Attributes: x.GetCustomAttributes<T>().ToArray()))
				.Where(tuple => tuple.Attributes.Length > 0)
				.SelectMany(tuple => tuple.Attributes, (tuple, attribute) => (tuple.Source, attribute))
				.ToList();
		}
	}
}
