using System.Collections.Generic;
using Mono.Cecil.Cil;

namespace Realmar.DataBindings.Editor.Emitting
{
	internal class MethodMemento
	{
		internal IReadOnlyList<VariableDefinition> Variables { get; }
		internal IReadOnlyList<ExceptionHandler> ExceptionHandlers { get; }
		internal IReadOnlyList<Instruction> Instructions { get; }
		internal bool InitLocals { get; }

		internal MethodMemento(IReadOnlyList<VariableDefinition> variables, IReadOnlyList<ExceptionHandler> exceptionHandlers, IReadOnlyList<Instruction> instructions, bool initLocals)
		{
			Variables = variables;
			ExceptionHandlers = exceptionHandlers;
			Instructions = instructions;
			InitLocals = initLocals;
		}
	}
}
