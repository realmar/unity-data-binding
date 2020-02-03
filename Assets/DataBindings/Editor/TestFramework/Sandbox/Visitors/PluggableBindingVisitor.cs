using System;
using System.Collections.Generic;
using Realmar.DataBindings.Editor.Exceptions;
using Realmar.DataBindings.Editor.TestFramework.Sandbox.Visitors;

namespace Realmar.DataBindings.Editor.TestFramework.Sandbox.Visitors
{
	internal class PluggableBindingVisitor : BindingVisitor
	{
		private readonly Dictionary<Type, object> _visitorTargetMap = new Dictionary<Type, object>();

		internal PluggableBindingVisitor(IBindingSet bindingSet) : base(bindingSet)
		{
		}

		public override void Visit(IPropertyBinding binding)
		{
			Internal_Visit(binding);
		}

		public override void Visit(IToMethodBinding binding)
		{
			Internal_Visit(binding);
		}

		internal void ConfigureTarget<TBinding>(VisitorTarget<TBinding> target)
			where TBinding : IBinding<Attribute>

		{
			YeetHelpers.YeetIfNull(target, nameof(target));
			_visitorTargetMap[typeof(TBinding)] = target;
		}

		private void Internal_Visit<TBinding>(TBinding binding)
			where TBinding : IBinding<Attribute>
		{
			var type = typeof(TBinding);
			if (_visitorTargetMap.TryGetValue(type, out var target))
			{
				((VisitorTarget<TBinding>) target).Invoke(binding, this);
			}
		}
	}
}

namespace Realmar.DataBindings.Editor.TestFramework.Sandbox
{
	internal delegate void VisitorTarget<in TBinding>(TBinding binding, IAssertionToolbox toolbox);
}
