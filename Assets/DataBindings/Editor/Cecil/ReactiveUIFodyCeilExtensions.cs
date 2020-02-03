using System.Collections.Generic;
using Mono.Cecil;

namespace Realmar.DataBindings.Editor.Cecil
{
	// Source: ReactiveUI.Fody
	// License: MIT
	// https://github.com/kswoll/ReactiveUI.Fody/blob/rxui7beta/ReactiveUI.Fody/CecilExtensions.cs
	// Changes:
	//   - Removed code which I didn't use
	//   - Removed logger
	//   - Little bit of code formatting to align with the rest of my code
	internal static class ReactiveUIFodyCeilExtensions
	{
		internal static bool IsAssignableFrom(this TypeReference baseType, TypeReference type)
		{
			return IsAssignableFrom(baseType.Resolve(), type.Resolve());
		}

		internal static bool IsAssignableFrom(this TypeDefinition baseType, TypeDefinition type)
		{
			var queueCache = new Queue<TypeDefinition>();
			queueCache.Enqueue(type);

			while (queueCache.Count > 0)
			{
				var current = queueCache.Dequeue();

				if (baseType.FullName == current.FullName)
				{
					return true;
				}

				if (current.BaseType != null)
				{
					queueCache.Enqueue(current.BaseType.Resolve());
				}

				for (var i = current.Interfaces.Count - 1; i >= 0; i--)
				{
					var @interface = current.Interfaces[i];
					queueCache.Enqueue(@interface.InterfaceType.Resolve());
				}
			}

			return false;
		}
	}
}
