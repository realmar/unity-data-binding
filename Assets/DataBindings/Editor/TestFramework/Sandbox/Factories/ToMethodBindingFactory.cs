using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Realmar.DataBindings.Editor.Shared.Extensions;
using Realmar.DataBindings.Editor.TestFramework.Attributes;
using Realmar.DataBindings.Editor.TestFramework.Sandbox.UUT;

namespace Realmar.DataBindings.Editor.TestFramework.Sandbox.Factories
{
	internal class ToMethodBindingFactory : BaseBindingFactoryBase
	{
		public ToMethodBindingFactory(IReadOnlyCollection<Type> types) : base(types)
		{
		}

		protected override IEnumerable<IBinding<Attribute>> CreateBindings(Type sourceType, object sourceObject, MemberInfo bindingSymbol)
		{
			var bindings = new List<IBinding<Attribute>>();
			var results = sourceType.GetMembersWithAttributesInType(typeof(ResultAttribute), typeof(IdAttribute));
			var sources = sourceType.GetMembersWithAttributesInType(typeof(InvokeOnChangeAttribute), typeof(IdAttribute));

			foreach (var source in sources)
			{
				var matchingResults = results.Where(info => info.GetCustomAttribute<IdAttribute>().Id == source.GetCustomAttribute<IdAttribute>().Id);
				foreach (var result in matchingResults)
				{
					var binding = new ToMethodBinding(
						source.GetCustomAttribute<InvokeOnChangeAttribute>(),
						new UUTBindingObject(bindingSymbol, sourceObject),
						new UUTBindingObject(result, sourceObject),
						result.GetCustomAttribute<ResultAttribute>());

					bindings.Add(binding);
				}
			}

			return bindings;
		}

		protected override IEnumerable<MemberInfo> GetBindingSymbols(Type sourceCompileTimeType)
		{
			return sourceCompileTimeType.GetMembersWithAttributeInType<InvokeOnChangeAttribute>();
		}
	}
}
