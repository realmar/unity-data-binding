using System.Collections.Generic;
using Mono.Cecil.Cil;

namespace Realmar.DataBindings.Editor.Emitting
{
	internal class MethodMemento
	{
		internal List<VariableDefinition> Variables { get; set; } = new List<VariableDefinition>();
		internal List<ExceptionHandler> ExceptionHandlers { get; set; } = new List<ExceptionHandler>();
		internal List<Instruction> Instructions { get; set; } = new List<Instruction>();
		internal bool InitLocals { get; set; }
	}
}
