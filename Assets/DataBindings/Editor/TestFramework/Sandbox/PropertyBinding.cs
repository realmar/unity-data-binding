using System;
using System.Reflection;
using JetBrains.Annotations;
using Realmar.DataBindings.Editor.TestFramework.Sandbox.UUT;
using Realmar.DataBindings.Editor.TestFramework.Sandbox.Visitors;

namespace Realmar.DataBindings.Editor.TestFramework.Sandbox
{
	internal class PropertyBinding : MarshalByRefObject, IPropertyBinding
	{
		internal class Arguments
		{
			[NotNull] internal BindingAttribute BindingAttribute { get; set; }
			[NotNull] internal MemberInfo SourceProperty { get; set; }
			[CanBeNull] internal MemberInfo TargetProperty { get; set; }
			[NotNull] internal object Source { get; set; }
			[CanBeNull] internal object Target { get; set; }
		}

		private readonly Arguments _arguments;

		[CanBeNull] public IUUTBindingObject Target { get; }
		[NotNull] public IUUTBindingObject Source { get; }
		[NotNull] public BindingAttribute BindingAttribute => _arguments.BindingAttribute;


		internal PropertyBinding(Arguments arguments)
		{
			_arguments = arguments;
			Target = new UUTBindingObject(_arguments.TargetProperty, _arguments.Target);
			Source = new UUTBindingObject(_arguments.SourceProperty, _arguments.Source);
		}

		public void Accept(IBindingVisitor visitor) => visitor.Visit(this);
	}
}
