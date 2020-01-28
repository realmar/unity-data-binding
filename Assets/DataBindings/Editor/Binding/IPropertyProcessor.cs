using Mono.Cecil;

namespace Realmar.DataBindings.Editor.Binding
{
	internal interface IPropertyProcessor
	{
		void Process(PropertyDefinition property);
	}
}
