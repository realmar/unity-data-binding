using System;
using Mono.Cecil;

namespace Realmar.DataBindings.Editor.Emitting
{
	internal class EmitBindingCommand
	{
		private Action<MethodDefinition> _emitClojure;

		public EmitBindingCommand(Action<MethodDefinition> emitClojure)
		{
			_emitClojure = emitClojure;
		}

		internal void Emit(MethodDefinition method) => _emitClojure.Invoke(method);
	}
}
