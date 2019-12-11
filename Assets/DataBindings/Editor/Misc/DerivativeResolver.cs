using System.Collections.Generic;
using System.Linq;
using Realmar.DataBindings.Editor.Misc;
using Mono.Cecil;
using Realmar.DataBindings.Editor.Extensions;

namespace Realmar.DataBindings.Editor
{
	internal class DerivativeResolver
	{
		private readonly ReactiveUIFodyCeilExtensions _fodyCeilExtensions = new ReactiveUIFodyCeilExtensions();

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
				if (_fodyCeilExtensions.IsAssignableFrom(originType, typeDefinition) && typeDefinition != originType)
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
					type.Interfaces != null &&
					type.Interfaces.Any(iface => iface.InterfaceType.Resolve() == originType))
				.ToList();
		}
	}
}
