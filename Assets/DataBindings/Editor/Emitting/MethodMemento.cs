using Mono.Cecil.Cil;
using System.Collections.Generic;

namespace Realmar.DataBindings.Editor.Emitting
{
	internal class MethodMemento
	{
		internal List<VariableDefinition> Variables { get; } = new List<VariableDefinition>();
		internal List<ExceptionHandler> ExceptionHandlers { get; } = new List<ExceptionHandler>();
		internal List<Instruction> Instructions { get; } = new List<Instruction>();
		internal bool InitLocals { get; set; }
	}
}
