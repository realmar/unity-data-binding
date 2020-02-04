using Realmar.DataBindings.Editor.TestFramework.Sandbox.Visitors;

namespace Realmar.DataBindings.Editor.TestFramework.Sandbox
{
	// If we don't do this it will throw InvalidCastException when passing it between AppDomains
	// It seems to have problems with passing generic types between app domain boundaries
	internal interface IBinding
	{
		void Accept(IBindingVisitor visitor);
	}

	internal interface IBinding<out TAttribute> : IBinding
	{
		TAttribute BindingAttribute { get; }
	}
}
