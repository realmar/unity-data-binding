using System.Collections.Generic;
using System.Linq;
using Mono.Cecil;
using Realmar.DataBindings.Editor.Utils;

namespace Realmar.DataBindings.Editor.Cecil
{
	internal class DerivativeResolver
	{
		private readonly Cache _cache = new Cache();
		private readonly Cache _directlyDerivedCache = new Cache();

		internal IReadOnlyList<TypeDefinition> GetDerivedTypes(TypeDefinition originType)
		{
			return _cache.Get(originType, GetDerivedTypes_Internal);
		}

		internal IReadOnlyList<TypeDefinition> GetDirectlyDerivedTypes(TypeDefinition originType)
		{
			return _directlyDerivedCache.Get(originType, GetDirectlyDerivedTypes_Internal);
		}

		private List<TypeDefinition> GetDerivedTypes_Internal(TypeDefinition originType)
		{
			var types = new List<TypeDefinition>();

			// TODO resolve across assembly boundaries
			foreach (var typeDefinition in originType.Module.Types)
			{
				if (typeDefinition.EnumerateBaseClasses().Any(definition => TypeInBaseHierarchy(definition, originType)))
				{
					types.Add(typeDefinition);
				}
			}

			return types;
		}

		private List<TypeDefinition> GetDirectlyDerivedTypes_Internal(TypeDefinition originType)
		{
			return GetDerivedTypes_Internal(originType)
				.Where(type =>
					type.BaseType == originType ||
					TypePartOfInterfaces(type, originType))
				.ToList();
		}

		private bool TypeInBaseHierarchy(TypeDefinition definition, TypeDefinition originType)
		{
			return definition == originType || TypePartOfInterfaces(definition, originType);
		}

		private bool TypePartOfInterfaces(TypeDefinition definition, TypeDefinition originType)
		{
			return definition.Interfaces != null &&
			       definition.Interfaces.Any(iface => iface.InterfaceType.Resolve() == originType);
		}
	}
}
