using System;

namespace Realmar.DataBindings
{
	[Serializable]
	public enum BindingType
	{
		OneTime,
		OneWay,
		TwoWay,
		OneWayFromTarget
	}
}
