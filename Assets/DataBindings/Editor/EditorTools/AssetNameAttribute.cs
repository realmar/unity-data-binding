using System;

namespace Realmar.DataBindings.Editor.EditorTools
{
	[AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
	internal class AssetNameAttribute : Attribute
	{
		internal string Name { get; }

		public AssetNameAttribute(string name)
		{
			Name = name;
		}
	}
}
