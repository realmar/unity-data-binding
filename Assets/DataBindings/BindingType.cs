using System;

namespace Realmar.DataBindings
{
	[Serializable]
	public enum BindingType
	{
		OneWay = 0,
		OneTime = 1,
		TwoWay = 2,
		OneWayFromTarget = 3
	}
}
