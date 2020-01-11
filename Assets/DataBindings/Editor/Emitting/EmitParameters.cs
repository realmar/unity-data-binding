using Mono.Cecil;
using Realmar.DataBindings.Editor.Exceptions;
using System;
using System.Linq.Expressions;

namespace Realmar.DataBindings.Editor.Emitting
{
	internal readonly struct EmitParameters
	{
		internal IMemberDefinition BindingTarget { get; }
		internal MethodDefinition FromGetter { get; }
		internal MethodDefinition FromSetter { get; }
		internal MethodDefinition ToSetter { get; }
		internal bool EmitNullCheck { get; }

		internal EmitParameters(IMemberDefinition bindingTarget, MethodDefinition fromGetter, MethodDefinition fromSetter, MethodDefinition setter, bool emitNullCheck)
		{
			BindingTarget = bindingTarget;
			FromGetter = fromGetter;
			FromSetter = fromSetter;
			ToSetter = setter;
			EmitNullCheck = emitNullCheck;
		}

		/// <exception cref="T:Realmar.DataBindings.Editor.Exceptions.ValidationException">When validation fails.</exception>
		internal void Validate()
		{
			ThrowIfNull(parameters => parameters.BindingTarget, BindingTarget);
			ThrowIfNull(parameters => parameters.FromGetter, FromGetter);
			ThrowIfNull(parameters => parameters.FromSetter, FromSetter);
			ThrowIfNull(parameters => parameters.ToSetter, ToSetter);
		}

		private void ThrowIfNull<T>(Expression<Func<EmitParameters, T>> expression, object obj)
		{
			if (obj == null)
			{
				var property = (MemberExpression) expression.Body;
				var name = property.Member.Name;

				throw new ValidationException($"{nameof(EmitParameters)}::{name} cannot be null.");
			}
		}
	}
}
