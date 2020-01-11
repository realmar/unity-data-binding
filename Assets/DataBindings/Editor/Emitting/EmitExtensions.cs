using Mono.Cecil;
using Realmar.DataBindings.Editor.Cecil;

namespace Realmar.DataBindings.Editor.Emitting
{
	public static class EmitExtensions
	{
		internal static MethodDefinition GetInEqualityOperator(this TypeDefinition type)
		{
			return type.GetOperator("op_Inequality");
		}

		internal static MethodDefinition GetEqualityOperator(this TypeDefinition type)
		{
			return type.GetOperator("op_Equality");
		}

		private static MethodDefinition GetOperator(this TypeDefinition type, string name)
		{
			var op = type.GetMethod(name);
			if (op == null)
			{
				return type.BaseType?.Resolve().GetOperator(name);
			}

			return op;
		}
	}
}
