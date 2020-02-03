using System;
using System.Diagnostics;
using Mono.Cecil;

namespace Realmar.DataBindings.Editor.Binding
{
	[DebuggerStepThrough]
	internal readonly struct DataSourceConfiguration
	{
		internal DataSource DataSource { get; }
		internal Lazy<MethodDefinition> FromGetter { get; }
		internal Lazy<ParameterDefinition> MethodParameter { get; }

		public DataSourceConfiguration(DataSource dataSource, Lazy<MethodDefinition> fromGetter, Lazy<ParameterDefinition> methodParameter)
		{
			DataSource = dataSource;
			FromGetter = fromGetter;
			MethodParameter = methodParameter;
		}
	}
}
