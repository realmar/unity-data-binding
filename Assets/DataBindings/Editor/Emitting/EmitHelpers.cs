using Mono.Cecil;
using Mono.Cecil.Cil;
using Mono.Collections.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using static Realmar.DataBindings.Editor.Exceptions.YeetHelpers;

namespace Realmar.DataBindings.Editor.Emitting
{
	internal static class EmitHelpers
	{
		internal static Instruction GetLoadFromFieldOrCallableInstruction(IMemberDefinition bindingTarget)
		{
			YeetIfNull(bindingTarget, nameof(bindingTarget));

			switch (bindingTarget)
			{
				case FieldDefinition field:
					return Instruction.Create(OpCodes.Ldfld, field);
				case MethodDefinition method:
					return Instruction.Create(GetCallInstruction(method), method);
				default:
					throw new NotSupportedException("BindingTarget can only be field or method");
			}
		}

		internal static OpCode GetCallInstruction(MethodDefinition method)
		{
			YeetIfNull(method, nameof(method));

			var isVirtual = method.IsVirtual || method.IsAbstract;
			return isVirtual ? OpCodes.Callvirt : OpCodes.Call;
		}

		internal static Instruction GetLastInstruction(MethodDefinition method)
		{
			YeetIfNull(method, nameof(method));
			YeetIfAbstract(method);

			var methodBody = method.Body;
			var lastInstruction = methodBody.Instructions.Last();

			return lastInstruction;
		}

		internal static List<Instruction> GetInstructionsReferencing(Instruction searchInstruction, Collection<Instruction> instructions)
		{
			YeetIfNull(searchInstruction, nameof(searchInstruction));
			YeetIfNull(instructions, nameof(instructions));

			var found = new List<Instruction>();

			foreach (var instruction in instructions)
			{
				if (ReferenceEquals(instruction.Operand, searchInstruction))
				{
					found.Add(instruction);
				}
			}

			return found;
		}

		internal static string GetBackingFieldName(string normalName) => $"\u003C{normalName}\u003Ek__BackingField";
		internal static string GetGetterName(string normalName) => $"get_{normalName}";
		internal static string GetSetterName(string normalName) => $"set_{normalName}";
	}
}
