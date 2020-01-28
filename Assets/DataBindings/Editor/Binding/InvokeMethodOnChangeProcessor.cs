using Mono.Cecil;
using Realmar.DataBindings.Editor.Cecil;
using Realmar.DataBindings.Editor.Exceptions;
using Realmar.DataBindings.Editor.IoC;
using Realmar.DataBindings.Editor.Weaving;
using System.Linq;

namespace Realmar.DataBindings.Editor.Binding
{
	internal class InvokeMethodOnChangeProcessor : IPropertyProcessor
	{
		private readonly Weaver _weaver = ServiceLocator.Current.Resolve<Weaver>();

		public void Process(PropertyDefinition sourceProperty)
		{
			foreach (var attribute in sourceProperty.GetCustomAttributes<InvokeOnChangeAttribute>())
			{
				var attributeValues = (CustomAttributeArgument[]) attribute.ConstructorArguments[0].Value;
				var targetMethodNames = attributeValues.Select(argument => (string) argument.Value);
				var declaringType = sourceProperty.DeclaringType;
				var fromSetter = sourceProperty.GetSetMethodOrYeet();
				var fromGetter = sourceProperty.GetGetMethodOrYeet();

				foreach (var targetMethodName in targetMethodNames)
				{
					var method = declaringType.GetMethodsInBaseHierarchy(targetMethodName).FirstOrDefault();
					if (method == null)
					{
						throw new MissingTargetMethodException(targetMethodName);
					}

					_weaver.Weave(new WeaveMethodParameters(fromSetter, method, null, false, null), fromGetter);
				}
			}
		}
	}
}
