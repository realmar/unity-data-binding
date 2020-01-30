using System.Diagnostics;
using Mono.Cecil;

namespace Realmar.DataBindings.Editor.Binding
{
	[DebuggerStepThrough]
	internal readonly struct BindingSettings
	{
		public BindingType Type { get; }
		public int TargetId { get; }
		public string TargetPropertyName { get; }
		public NullCheckBehavior NullCheckBehavior { get; }
		public DataSource DataSource { get; }
		public TypeReference Converter { get; }

		public BindingSettings(BindingType type, int targetId, string targetPropertyName, NullCheckBehavior nullCheckBehavior, DataSource dataSource, TypeReference converter)
		{
			Type = type;
			TargetId = targetId;
			TargetPropertyName = targetPropertyName;
			NullCheckBehavior = nullCheckBehavior;
			DataSource = dataSource;
			Converter = converter;
		}
	}
}
