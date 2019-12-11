using System.Collections.Generic;
using Mono.Cecil;

namespace Realmar.DataBindingsEditor.Extensions
{
	// Source: ReactiveUI.Fody
	// License: MIT
	// https://github.com/kswoll/ReactiveUI.Fody/blob/rxui7beta/ReactiveUI.Fody/CecilExtensions.cs
	// Changes:
	//   - Removed code which I didn't use
	//   - Removed logger
	//   - Little bit of code formatting to align with the rest of my code
	internal class ReactiveUIFodyCeilExtensions
	{
		private readonly Queue<TypeDefinition> _queueCache = new Queue<TypeDefinition>();

		internal bool IsAssignableFrom(TypeReference baseType, TypeReference type)
		{
			return IsAssignableFrom(baseType.Resolve(), type.Resolve());
		}

		internal bool IsAssignableFrom(TypeDefinition baseType, TypeDefinition type)
		{
			_queueCache.Enqueue(type);

			while (_queueCache.Count > 0)
			{
				var current = _queueCache.Dequeue();

				if (baseType.FullName == current.FullName)
				{
					return true;
				}

				if (current.BaseType != null)
				{
					_queueCache.Enqueue(current.BaseType.Resolve());
				}

				for (var i = current.Interfaces.Count - 1; i >= 0; i--)
				{
					var @interface = current.Interfaces[i];
					_queueCache.Enqueue(@interface.InterfaceType.Resolve());
				}
			}

			return false;
		}
	}
}
