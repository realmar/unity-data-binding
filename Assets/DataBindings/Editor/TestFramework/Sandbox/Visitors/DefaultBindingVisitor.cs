using System;

namespace Realmar.DataBindings.Editor.TestFramework.Sandbox.Visitors
{
	internal class DefaultBindingVisitor : BindingVisitor
	{
		public DefaultBindingVisitor(IBindingSet bindingSet) : base(bindingSet)
		{
			bindingSet.RunBindingInitializer();
		}

		public override void Visit(IPropertyBinding binding)
		{
			RunDefaultAssertions(binding);
		}

		public override void Visit(IToMethodBinding binding)
		{
			RunDefaultAssertions(binding);
		}
	}
}
