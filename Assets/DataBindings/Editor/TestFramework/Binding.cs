using System;
using System.Reflection;
using Realmar.DataBindings.Editor.Extensions;

namespace Realmar.DataBindings.Editor.TestFramework
{
	internal class Binding : MarshalByRefObject, IBinding
	{
		internal class Arguments
		{
			internal BindingAttribute BindingAttribute { get; set; }
			internal MemberInfo SourceProperty { get; set; }
			internal MemberInfo TargetProperty { get; set; }
			internal object Source { get; set; }
			internal object Target { get; set; }
		}

		private Arguments _arguments;

		public BindingAttribute BindingAttribute => _arguments.BindingAttribute;


		public Binding(Arguments arguments)
		{
			_arguments = arguments;
		}

		public void SetSourceProperty(object value)
		{
			_arguments.SourceProperty.SetFieldOrPropertyValue(_arguments.Source, value);
		}

		public void SetTargetProperty(object value)
		{
			_arguments.TargetProperty.SetFieldOrPropertyValue(_arguments.Target, value);
		}

		public object GetSourceProperty()
		{
			return _arguments.SourceProperty.GetFieldOrPropertyValue(_arguments.Source);
		}

		public object GetTargetProperty()
		{
			return _arguments.TargetProperty.GetFieldOrPropertyValue(_arguments.Target);
		}
	}
}
