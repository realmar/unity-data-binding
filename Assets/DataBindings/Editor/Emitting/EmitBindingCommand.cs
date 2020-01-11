using Mono.Cecil;
using System;

namespace Realmar.DataBindings.Editor.Emitting
{
	internal class EmitBindingCommand
	{
		private readonly Action<MethodDefinition> _emitClojure;

		internal EmitBindingCommand(Action<MethodDefinition> emitClojure)
		{
			_emitClojure = emitClojure;
		}

		internal void Emit(MethodDefinition method) => _emitClojure.Invoke(method);
	}
}
