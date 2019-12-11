/*namespace Realmar.DataBindings.Example.Performance1
{
	public interface IV1
	{
		[BindingTarget]
		[BindingTarget(id: 1)]
		IV2 B1 { get; }

		[BindingTarget(id: 2)]
		[BindingTarget(id: 3)]
		V1_1 B2 { get; }

		[Binding]
		[Binding(targetId: 1, bindingType:  BindingType.OneWayFromTarget)]
		[Binding(targetId: 2, bindingType:  BindingType.TwoWay)]
		[Binding(targetId: 3, bindingType:  BindingType.TwoWay)]
		object P1 { get; set; }

		[Binding]
		[Binding(targetId: 1, bindingType:  BindingType.OneWayFromTarget)]
		[Binding(targetId: 2, bindingType:  BindingType.TwoWay)]
		[Binding(targetId: 3, bindingType:  BindingType.TwoWay)]
		object P2 { get; set; }

		[Binding]
		[Binding(targetId: 1, bindingType:  BindingType.OneWayFromTarget)]
		[Binding(targetId: 2, bindingType:  BindingType.TwoWay)]
		[Binding(targetId: 3, bindingType:  BindingType.TwoWay)]
		object P3 { get; set; }

		[Binding]
		[Binding(targetId: 1, bindingType:  BindingType.OneWayFromTarget)]
		[Binding(targetId: 2, bindingType:  BindingType.TwoWay)]
		[Binding(targetId: 3, bindingType:  BindingType.TwoWay)]
		object P4 { get; set; }

		[Binding]
		[Binding(targetId: 1, bindingType:  BindingType.OneWayFromTarget)]
		[Binding(targetId: 2, bindingType:  BindingType.TwoWay)]
		[Binding(targetId: 3, bindingType:  BindingType.TwoWay)]
		object P5 { get; set; }

		[Binding]
		[Binding(targetId: 1, bindingType:  BindingType.OneWayFromTarget)]
		[Binding(targetId: 2, bindingType:  BindingType.TwoWay)]
		[Binding(targetId: 3, bindingType:  BindingType.TwoWay)]
		object P6 { get; set; }

		[BindingInitializer]
		void InitializeBindings();
	}

	public interface IV2
	{
		object P1 { get; set; }
		object P2 { get; set; }
		object P3 { get; set; }
		object P4 { get; set; }
		object P5 { get; set; }
		object P6 { get; set; }
	}

	public class V1_1 : IV1
	{
		public IV2 B1 { get; set; }
		public V1_1 B2 { get; set; }

		public object P1 { get; set; }
		public object P2 { get; set; }
		public object P3 { get; set; }
		public object P4 { get; set; }
		public object P5 { get; set; }
		public object P6 { get; set; }

		public void InitializeBindings()
		{
		}
	}

	public class V1_2 : IV1
	{
		public IV2 B1 { get; set; }
		public V1_1 B2 { get; set; }

		public object P1 { get; set; }
		public object P2 { get; set; }
		public object P3 { get; set; }
		public object P4 { get; set; }
		public object P5 { get; set; }
		public object P6 { get; set; }

		public void InitializeBindings()
		{
		}
	}

	public class V1_3 : IV1
	{
		public IV2 B1 { get; set; }
		public V1_1 B2 { get; set; }

		public object P1 { get; set; }
		public object P2 { get; set; }
		public object P3 { get; set; }
		public object P4 { get; set; }
		public object P5 { get; set; }
		public object P6 { get; set; }

		public void InitializeBindings()
		{
		}
	}

	public class V1_4 : IV1
	{
		public IV2 B1 { get; set; }
		public V1_1 B2 { get; set; }

		public object P1 { get; set; }
		public object P2 { get; set; }
		public object P3 { get; set; }
		public object P4 { get; set; }
		public object P5 { get; set; }
		public object P6 { get; set; }

		public void InitializeBindings()
		{
		}
	}

	public class V1_5 : IV1
	{
		public IV2 B1 { get; set; }
		public V1_1 B2 { get; set; }

		public object P1 { get; set; }
		public object P2 { get; set; }
		public object P3 { get; set; }
		public object P4 { get; set; }
		public object P5 { get; set; }
		public object P6 { get; set; }

		public void InitializeBindings()
		{
		}
	}

	public class V1_6 : IV1
	{
		public IV2 B1 { get; set; }
		public V1_1 B2 { get; set; }

		public object P1 { get; set; }
		public object P2 { get; set; }
		public object P3 { get; set; }
		public object P4 { get; set; }
		public object P5 { get; set; }
		public object P6 { get; set; }

		public void InitializeBindings()
		{
		}
	}

	public class V2_1 : IV2
	{
		public object P1 { get; set; }
		public object P2 { get; set; }
		public object P3 { get; set; }
		public object P4 { get; set; }
		public object P5 { get; set; }
		public object P6 { get; set; }
	}

	public class V2_2 : IV2
	{
		public object P1 { get; set; }
		public object P2 { get; set; }
		public object P3 { get; set; }
		public object P4 { get; set; }
		public object P5 { get; set; }
		public object P6 { get; set; }
	}

	public class V2_3 : IV2
	{
		public object P1 { get; set; }
		public object P2 { get; set; }
		public object P3 { get; set; }
		public object P4 { get; set; }
		public object P5 { get; set; }
		public object P6 { get; set; }
	}

	public class V2_4 : IV2
	{
		public object P1 { get; set; }
		public object P2 { get; set; }
		public object P3 { get; set; }
		public object P4 { get; set; }
		public object P5 { get; set; }
		public object P6 { get; set; }
	}

	public class V2_5 : IV2
	{
		public object P1 { get; set; }
		public object P2 { get; set; }
		public object P3 { get; set; }
		public object P4 { get; set; }
		public object P5 { get; set; }
		public object P6 { get; set; }
	}

	public class V2_6 : IV2
	{
		public object P1 { get; set; }
		public object P2 { get; set; }
		public object P3 { get; set; }
		public object P4 { get; set; }
		public object P5 { get; set; }
		public object P6 { get; set; }
	}
}

namespace Realmar.DataBindings.Example.Performance2
{
	public interface IV1
	{
		[BindingTarget]
		[BindingTarget(id: 1)]
		IV2 B1 { get; }

		[BindingTarget(id: 2)]
		[BindingTarget(id: 3)]
		V1_1 B2 { get; }

		[Binding]
		[Binding(targetId: 1, bindingType:  BindingType.OneWayFromTarget)]
		[Binding(targetId: 2, bindingType:  BindingType.TwoWay)]
		[Binding(targetId: 3, bindingType:  BindingType.TwoWay)]
		object P1 { get; set; }

		[Binding]
		[Binding(targetId: 1, bindingType:  BindingType.OneWayFromTarget)]
		[Binding(targetId: 2, bindingType:  BindingType.TwoWay)]
		[Binding(targetId: 3, bindingType:  BindingType.TwoWay)]
		object P2 { get; set; }

		[Binding]
		[Binding(targetId: 1, bindingType:  BindingType.OneWayFromTarget)]
		[Binding(targetId: 2, bindingType:  BindingType.TwoWay)]
		[Binding(targetId: 3, bindingType:  BindingType.TwoWay)]
		object P3 { get; set; }

		[Binding]
		[Binding(targetId: 1, bindingType:  BindingType.OneWayFromTarget)]
		[Binding(targetId: 2, bindingType:  BindingType.TwoWay)]
		[Binding(targetId: 3, bindingType:  BindingType.TwoWay)]
		object P4 { get; set; }

		[Binding]
		[Binding(targetId: 1, bindingType:  BindingType.OneWayFromTarget)]
		[Binding(targetId: 2, bindingType:  BindingType.TwoWay)]
		[Binding(targetId: 3, bindingType:  BindingType.TwoWay)]
		object P5 { get; set; }

		[Binding]
		[Binding(targetId: 1, bindingType:  BindingType.OneWayFromTarget)]
		[Binding(targetId: 2, bindingType:  BindingType.TwoWay)]
		[Binding(targetId: 3, bindingType:  BindingType.TwoWay)]
		object P6 { get; set; }

		[BindingInitializer]
		void InitializeBindings();
	}

	public interface IV2
	{
		object P1 { get; set; }
		object P2 { get; set; }
		object P3 { get; set; }
		object P4 { get; set; }
		object P5 { get; set; }
		object P6 { get; set; }
	}

	public class V1_1 : IV1
	{
		public IV2 B1 { get; set; }
		public V1_1 B2 { get; set; }

		public object P1 { get; set; }
		public object P2 { get; set; }
		public object P3 { get; set; }
		public object P4 { get; set; }
		public object P5 { get; set; }
		public object P6 { get; set; }

		public void InitializeBindings()
		{
		}
	}

	public class V1_2 : IV1
	{
		public IV2 B1 { get; set; }
		public V1_1 B2 { get; set; }

		public object P1 { get; set; }
		public object P2 { get; set; }
		public object P3 { get; set; }
		public object P4 { get; set; }
		public object P5 { get; set; }
		public object P6 { get; set; }

		public void InitializeBindings()
		{
		}
	}

	public class V1_3 : IV1
	{
		public IV2 B1 { get; set; }
		public V1_1 B2 { get; set; }

		public object P1 { get; set; }
		public object P2 { get; set; }
		public object P3 { get; set; }
		public object P4 { get; set; }
		public object P5 { get; set; }
		public object P6 { get; set; }

		public void InitializeBindings()
		{
		}
	}

	public class V1_4 : IV1
	{
		public IV2 B1 { get; set; }
		public V1_1 B2 { get; set; }

		public object P1 { get; set; }
		public object P2 { get; set; }
		public object P3 { get; set; }
		public object P4 { get; set; }
		public object P5 { get; set; }
		public object P6 { get; set; }

		public void InitializeBindings()
		{
		}
	}

	public class V1_5 : IV1
	{
		public IV2 B1 { get; set; }
		public V1_1 B2 { get; set; }

		public object P1 { get; set; }
		public object P2 { get; set; }
		public object P3 { get; set; }
		public object P4 { get; set; }
		public object P5 { get; set; }
		public object P6 { get; set; }

		public void InitializeBindings()
		{
		}
	}

	public class V1_6 : IV1
	{
		public IV2 B1 { get; set; }
		public V1_1 B2 { get; set; }

		public object P1 { get; set; }
		public object P2 { get; set; }
		public object P3 { get; set; }
		public object P4 { get; set; }
		public object P5 { get; set; }
		public object P6 { get; set; }

		public void InitializeBindings()
		{
		}
	}

	public class V2_1 : IV2
	{
		public object P1 { get; set; }
		public object P2 { get; set; }
		public object P3 { get; set; }
		public object P4 { get; set; }
		public object P5 { get; set; }
		public object P6 { get; set; }
	}

	public class V2_2 : IV2
	{
		public object P1 { get; set; }
		public object P2 { get; set; }
		public object P3 { get; set; }
		public object P4 { get; set; }
		public object P5 { get; set; }
		public object P6 { get; set; }
	}

	public class V2_3 : IV2
	{
		public object P1 { get; set; }
		public object P2 { get; set; }
		public object P3 { get; set; }
		public object P4 { get; set; }
		public object P5 { get; set; }
		public object P6 { get; set; }
	}

	public class V2_4 : IV2
	{
		public object P1 { get; set; }
		public object P2 { get; set; }
		public object P3 { get; set; }
		public object P4 { get; set; }
		public object P5 { get; set; }
		public object P6 { get; set; }
	}

	public class V2_5 : IV2
	{
		public object P1 { get; set; }
		public object P2 { get; set; }
		public object P3 { get; set; }
		public object P4 { get; set; }
		public object P5 { get; set; }
		public object P6 { get; set; }
	}

	public class V2_6 : IV2
	{
		public object P1 { get; set; }
		public object P2 { get; set; }
		public object P3 { get; set; }
		public object P4 { get; set; }
		public object P5 { get; set; }
		public object P6 { get; set; }
	}
}

namespace Realmar.DataBindings.Example.Performance3
{
	public interface IV1
	{
		[BindingTarget]
		[BindingTarget(id: 1)]
		IV2 B1 { get; }

		[BindingTarget(id: 2)]
		[BindingTarget(id: 3)]
		V1_1 B2 { get; }

		[Binding]
		[Binding(targetId: 1, bindingType:  BindingType.OneWayFromTarget)]
		[Binding(targetId: 2, bindingType:  BindingType.TwoWay)]
		[Binding(targetId: 3, bindingType:  BindingType.TwoWay)]
		object P1 { get; set; }

		[Binding]
		[Binding(targetId: 1, bindingType:  BindingType.OneWayFromTarget)]
		[Binding(targetId: 2, bindingType:  BindingType.TwoWay)]
		[Binding(targetId: 3, bindingType:  BindingType.TwoWay)]
		object P2 { get; set; }

		[Binding]
		[Binding(targetId: 1, bindingType:  BindingType.OneWayFromTarget)]
		[Binding(targetId: 2, bindingType:  BindingType.TwoWay)]
		[Binding(targetId: 3, bindingType:  BindingType.TwoWay)]
		object P3 { get; set; }

		[Binding]
		[Binding(targetId: 1, bindingType:  BindingType.OneWayFromTarget)]
		[Binding(targetId: 2, bindingType:  BindingType.TwoWay)]
		[Binding(targetId: 3, bindingType:  BindingType.TwoWay)]
		object P4 { get; set; }

		[Binding]
		[Binding(targetId: 1, bindingType:  BindingType.OneWayFromTarget)]
		[Binding(targetId: 2, bindingType:  BindingType.TwoWay)]
		[Binding(targetId: 3, bindingType:  BindingType.TwoWay)]
		object P5 { get; set; }

		[Binding]
		[Binding(targetId: 1, bindingType:  BindingType.OneWayFromTarget)]
		[Binding(targetId: 2, bindingType:  BindingType.TwoWay)]
		[Binding(targetId: 3, bindingType:  BindingType.TwoWay)]
		object P6 { get; set; }

		[BindingInitializer]
		void InitializeBindings();
	}

	public interface IV2
	{
		object P1 { get; set; }
		object P2 { get; set; }
		object P3 { get; set; }
		object P4 { get; set; }
		object P5 { get; set; }
		object P6 { get; set; }
	}

	public class V1_1 : IV1
	{
		public IV2 B1 { get; set; }
		public V1_1 B2 { get; set; }

		public object P1 { get; set; }
		public object P2 { get; set; }
		public object P3 { get; set; }
		public object P4 { get; set; }
		public object P5 { get; set; }
		public object P6 { get; set; }

		public void InitializeBindings()
		{
		}
	}

	public class V1_2 : IV1
	{
		public IV2 B1 { get; set; }
		public V1_1 B2 { get; set; }

		public object P1 { get; set; }
		public object P2 { get; set; }
		public object P3 { get; set; }
		public object P4 { get; set; }
		public object P5 { get; set; }
		public object P6 { get; set; }

		public void InitializeBindings()
		{
		}
	}

	public class V1_3 : IV1
	{
		public IV2 B1 { get; set; }
		public V1_1 B2 { get; set; }

		public object P1 { get; set; }
		public object P2 { get; set; }
		public object P3 { get; set; }
		public object P4 { get; set; }
		public object P5 { get; set; }
		public object P6 { get; set; }

		public void InitializeBindings()
		{
		}
	}

	public class V1_4 : IV1
	{
		public IV2 B1 { get; set; }
		public V1_1 B2 { get; set; }

		public object P1 { get; set; }
		public object P2 { get; set; }
		public object P3 { get; set; }
		public object P4 { get; set; }
		public object P5 { get; set; }
		public object P6 { get; set; }

		public void InitializeBindings()
		{
		}
	}

	public class V1_5 : IV1
	{
		public IV2 B1 { get; set; }
		public V1_1 B2 { get; set; }

		public object P1 { get; set; }
		public object P2 { get; set; }
		public object P3 { get; set; }
		public object P4 { get; set; }
		public object P5 { get; set; }
		public object P6 { get; set; }

		public void InitializeBindings()
		{
		}
	}

	public class V1_6 : IV1
	{
		public IV2 B1 { get; set; }
		public V1_1 B2 { get; set; }

		public object P1 { get; set; }
		public object P2 { get; set; }
		public object P3 { get; set; }
		public object P4 { get; set; }
		public object P5 { get; set; }
		public object P6 { get; set; }

		public void InitializeBindings()
		{
		}
	}

	public class V2_1 : IV2
	{
		public object P1 { get; set; }
		public object P2 { get; set; }
		public object P3 { get; set; }
		public object P4 { get; set; }
		public object P5 { get; set; }
		public object P6 { get; set; }
	}

	public class V2_2 : IV2
	{
		public object P1 { get; set; }
		public object P2 { get; set; }
		public object P3 { get; set; }
		public object P4 { get; set; }
		public object P5 { get; set; }
		public object P6 { get; set; }
	}

	public class V2_3 : IV2
	{
		public object P1 { get; set; }
		public object P2 { get; set; }
		public object P3 { get; set; }
		public object P4 { get; set; }
		public object P5 { get; set; }
		public object P6 { get; set; }
	}

	public class V2_4 : IV2
	{
		public object P1 { get; set; }
		public object P2 { get; set; }
		public object P3 { get; set; }
		public object P4 { get; set; }
		public object P5 { get; set; }
		public object P6 { get; set; }
	}

	public class V2_5 : IV2
	{
		public object P1 { get; set; }
		public object P2 { get; set; }
		public object P3 { get; set; }
		public object P4 { get; set; }
		public object P5 { get; set; }
		public object P6 { get; set; }
	}

	public class V2_6 : IV2
	{
		public object P1 { get; set; }
		public object P2 { get; set; }
		public object P3 { get; set; }
		public object P4 { get; set; }
		public object P5 { get; set; }
		public object P6 { get; set; }
	}
}

namespace Realmar.DataBindings.Example.Performance4
{
	public interface IV1
	{
		[BindingTarget]
		[BindingTarget(id: 1)]
		IV2 B1 { get; }

		[BindingTarget(id: 2)]
		[BindingTarget(id: 3)]
		V1_1 B2 { get; }

		[Binding]
		[Binding(targetId: 1, bindingType:  BindingType.OneWayFromTarget)]
		[Binding(targetId: 2, bindingType:  BindingType.TwoWay)]
		[Binding(targetId: 3, bindingType:  BindingType.TwoWay)]
		object P1 { get; set; }

		[Binding]
		[Binding(targetId: 1, bindingType:  BindingType.OneWayFromTarget)]
		[Binding(targetId: 2, bindingType:  BindingType.TwoWay)]
		[Binding(targetId: 3, bindingType:  BindingType.TwoWay)]
		object P2 { get; set; }

		[Binding]
		[Binding(targetId: 1, bindingType:  BindingType.OneWayFromTarget)]
		[Binding(targetId: 2, bindingType:  BindingType.TwoWay)]
		[Binding(targetId: 3, bindingType:  BindingType.TwoWay)]
		object P3 { get; set; }

		[Binding]
		[Binding(targetId: 1, bindingType:  BindingType.OneWayFromTarget)]
		[Binding(targetId: 2, bindingType:  BindingType.TwoWay)]
		[Binding(targetId: 3, bindingType:  BindingType.TwoWay)]
		object P4 { get; set; }

		[Binding]
		[Binding(targetId: 1, bindingType:  BindingType.OneWayFromTarget)]
		[Binding(targetId: 2, bindingType:  BindingType.TwoWay)]
		[Binding(targetId: 3, bindingType:  BindingType.TwoWay)]
		object P5 { get; set; }

		[Binding]
		[Binding(targetId: 1, bindingType:  BindingType.OneWayFromTarget)]
		[Binding(targetId: 2, bindingType:  BindingType.TwoWay)]
		[Binding(targetId: 3, bindingType:  BindingType.TwoWay)]
		object P6 { get; set; }

		[BindingInitializer]
		void InitializeBindings();
	}

	public interface IV2
	{
		object P1 { get; set; }
		object P2 { get; set; }
		object P3 { get; set; }
		object P4 { get; set; }
		object P5 { get; set; }
		object P6 { get; set; }
	}

	public class V1_1 : IV1
	{
		public IV2 B1 { get; set; }
		public V1_1 B2 { get; set; }

		public object P1 { get; set; }
		public object P2 { get; set; }
		public object P3 { get; set; }
		public object P4 { get; set; }
		public object P5 { get; set; }
		public object P6 { get; set; }

		public void InitializeBindings()
		{
		}
	}

	public class V1_2 : IV1
	{
		public IV2 B1 { get; set; }
		public V1_1 B2 { get; set; }

		public object P1 { get; set; }
		public object P2 { get; set; }
		public object P3 { get; set; }
		public object P4 { get; set; }
		public object P5 { get; set; }
		public object P6 { get; set; }

		public void InitializeBindings()
		{
		}
	}

	public class V1_3 : IV1
	{
		public IV2 B1 { get; set; }
		public V1_1 B2 { get; set; }

		public object P1 { get; set; }
		public object P2 { get; set; }
		public object P3 { get; set; }
		public object P4 { get; set; }
		public object P5 { get; set; }
		public object P6 { get; set; }

		public void InitializeBindings()
		{
		}
	}

	public class V1_4 : IV1
	{
		public IV2 B1 { get; set; }
		public V1_1 B2 { get; set; }

		public object P1 { get; set; }
		public object P2 { get; set; }
		public object P3 { get; set; }
		public object P4 { get; set; }
		public object P5 { get; set; }
		public object P6 { get; set; }

		public void InitializeBindings()
		{
		}
	}

	public class V1_5 : IV1
	{
		public IV2 B1 { get; set; }
		public V1_1 B2 { get; set; }

		public object P1 { get; set; }
		public object P2 { get; set; }
		public object P3 { get; set; }
		public object P4 { get; set; }
		public object P5 { get; set; }
		public object P6 { get; set; }

		public void InitializeBindings()
		{
		}
	}

	public class V1_6 : IV1
	{
		public IV2 B1 { get; set; }
		public V1_1 B2 { get; set; }

		public object P1 { get; set; }
		public object P2 { get; set; }
		public object P3 { get; set; }
		public object P4 { get; set; }
		public object P5 { get; set; }
		public object P6 { get; set; }

		public void InitializeBindings()
		{
		}
	}

	public class V2_1 : IV2
	{
		public object P1 { get; set; }
		public object P2 { get; set; }
		public object P3 { get; set; }
		public object P4 { get; set; }
		public object P5 { get; set; }
		public object P6 { get; set; }
	}

	public class V2_2 : IV2
	{
		public object P1 { get; set; }
		public object P2 { get; set; }
		public object P3 { get; set; }
		public object P4 { get; set; }
		public object P5 { get; set; }
		public object P6 { get; set; }
	}

	public class V2_3 : IV2
	{
		public object P1 { get; set; }
		public object P2 { get; set; }
		public object P3 { get; set; }
		public object P4 { get; set; }
		public object P5 { get; set; }
		public object P6 { get; set; }
	}

	public class V2_4 : IV2
	{
		public object P1 { get; set; }
		public object P2 { get; set; }
		public object P3 { get; set; }
		public object P4 { get; set; }
		public object P5 { get; set; }
		public object P6 { get; set; }
	}

	public class V2_5 : IV2
	{
		public object P1 { get; set; }
		public object P2 { get; set; }
		public object P3 { get; set; }
		public object P4 { get; set; }
		public object P5 { get; set; }
		public object P6 { get; set; }
	}

	public class V2_6 : IV2
	{
		public object P1 { get; set; }
		public object P2 { get; set; }
		public object P3 { get; set; }
		public object P4 { get; set; }
		public object P5 { get; set; }
		public object P6 { get; set; }
	}
}

namespace Realmar.DataBindings.Example.Performance5
{
	public interface IV1
	{
		[BindingTarget]
		[BindingTarget(id: 1)]
		IV2 B1 { get; }

		[BindingTarget(id: 2)]
		[BindingTarget(id: 3)]
		V1_1 B2 { get; }

		[Binding]
		[Binding(targetId: 1, bindingType:  BindingType.OneWayFromTarget)]
		[Binding(targetId: 2, bindingType:  BindingType.TwoWay)]
		[Binding(targetId: 3, bindingType:  BindingType.TwoWay)]
		object P1 { get; set; }

		[Binding]
		[Binding(targetId: 1, bindingType:  BindingType.OneWayFromTarget)]
		[Binding(targetId: 2, bindingType:  BindingType.TwoWay)]
		[Binding(targetId: 3, bindingType:  BindingType.TwoWay)]
		object P2 { get; set; }

		[Binding]
		[Binding(targetId: 1, bindingType:  BindingType.OneWayFromTarget)]
		[Binding(targetId: 2, bindingType:  BindingType.TwoWay)]
		[Binding(targetId: 3, bindingType:  BindingType.TwoWay)]
		object P3 { get; set; }

		[Binding]
		[Binding(targetId: 1, bindingType:  BindingType.OneWayFromTarget)]
		[Binding(targetId: 2, bindingType:  BindingType.TwoWay)]
		[Binding(targetId: 3, bindingType:  BindingType.TwoWay)]
		object P4 { get; set; }

		[Binding]
		[Binding(targetId: 1, bindingType:  BindingType.OneWayFromTarget)]
		[Binding(targetId: 2, bindingType:  BindingType.TwoWay)]
		[Binding(targetId: 3, bindingType:  BindingType.TwoWay)]
		object P5 { get; set; }

		[Binding]
		[Binding(targetId: 1, bindingType:  BindingType.OneWayFromTarget)]
		[Binding(targetId: 2, bindingType:  BindingType.TwoWay)]
		[Binding(targetId: 3, bindingType:  BindingType.TwoWay)]
		object P6 { get; set; }

		[BindingInitializer]
		void InitializeBindings();
	}

	public interface IV2
	{
		object P1 { get; set; }
		object P2 { get; set; }
		object P3 { get; set; }
		object P4 { get; set; }
		object P5 { get; set; }
		object P6 { get; set; }
	}

	public class V1_1 : IV1
	{
		public IV2 B1 { get; set; }
		public V1_1 B2 { get; set; }

		public object P1 { get; set; }
		public object P2 { get; set; }
		public object P3 { get; set; }
		public object P4 { get; set; }
		public object P5 { get; set; }
		public object P6 { get; set; }

		public void InitializeBindings()
		{
		}
	}

	public class V1_2 : IV1
	{
		public IV2 B1 { get; set; }
		public V1_1 B2 { get; set; }

		public object P1 { get; set; }
		public object P2 { get; set; }
		public object P3 { get; set; }
		public object P4 { get; set; }
		public object P5 { get; set; }
		public object P6 { get; set; }

		public void InitializeBindings()
		{
		}
	}

	public class V1_3 : IV1
	{
		public IV2 B1 { get; set; }
		public V1_1 B2 { get; set; }

		public object P1 { get; set; }
		public object P2 { get; set; }
		public object P3 { get; set; }
		public object P4 { get; set; }
		public object P5 { get; set; }
		public object P6 { get; set; }

		public void InitializeBindings()
		{
		}
	}

	public class V1_4 : IV1
	{
		public IV2 B1 { get; set; }
		public V1_1 B2 { get; set; }

		public object P1 { get; set; }
		public object P2 { get; set; }
		public object P3 { get; set; }
		public object P4 { get; set; }
		public object P5 { get; set; }
		public object P6 { get; set; }

		public void InitializeBindings()
		{
		}
	}

	public class V1_5 : IV1
	{
		public IV2 B1 { get; set; }
		public V1_1 B2 { get; set; }

		public object P1 { get; set; }
		public object P2 { get; set; }
		public object P3 { get; set; }
		public object P4 { get; set; }
		public object P5 { get; set; }
		public object P6 { get; set; }

		public void InitializeBindings()
		{
		}
	}

	public class V1_6 : IV1
	{
		public IV2 B1 { get; set; }
		public V1_1 B2 { get; set; }

		public object P1 { get; set; }
		public object P2 { get; set; }
		public object P3 { get; set; }
		public object P4 { get; set; }
		public object P5 { get; set; }
		public object P6 { get; set; }

		public void InitializeBindings()
		{
		}
	}

	public class V2_1 : IV2
	{
		public object P1 { get; set; }
		public object P2 { get; set; }
		public object P3 { get; set; }
		public object P4 { get; set; }
		public object P5 { get; set; }
		public object P6 { get; set; }
	}

	public class V2_2 : IV2
	{
		public object P1 { get; set; }
		public object P2 { get; set; }
		public object P3 { get; set; }
		public object P4 { get; set; }
		public object P5 { get; set; }
		public object P6 { get; set; }
	}

	public class V2_3 : IV2
	{
		public object P1 { get; set; }
		public object P2 { get; set; }
		public object P3 { get; set; }
		public object P4 { get; set; }
		public object P5 { get; set; }
		public object P6 { get; set; }
	}

	public class V2_4 : IV2
	{
		public object P1 { get; set; }
		public object P2 { get; set; }
		public object P3 { get; set; }
		public object P4 { get; set; }
		public object P5 { get; set; }
		public object P6 { get; set; }
	}

	public class V2_5 : IV2
	{
		public object P1 { get; set; }
		public object P2 { get; set; }
		public object P3 { get; set; }
		public object P4 { get; set; }
		public object P5 { get; set; }
		public object P6 { get; set; }
	}

	public class V2_6 : IV2
	{
		public object P1 { get; set; }
		public object P2 { get; set; }
		public object P3 { get; set; }
		public object P4 { get; set; }
		public object P5 { get; set; }
		public object P6 { get; set; }
	}
}

namespace Realmar.DataBindings.Example.Performance6
{
	public interface IV1
	{
		[BindingTarget]
		[BindingTarget(id: 1)]
		IV2 B1 { get; }

		[BindingTarget(id: 2)]
		[BindingTarget(id: 3)]
		V1_1 B2 { get; }

		[Binding]
		[Binding(targetId: 1, bindingType:  BindingType.OneWayFromTarget)]
		[Binding(targetId: 2, bindingType:  BindingType.TwoWay)]
		[Binding(targetId: 3, bindingType:  BindingType.TwoWay)]
		object P1 { get; set; }

		[Binding]
		[Binding(targetId: 1, bindingType:  BindingType.OneWayFromTarget)]
		[Binding(targetId: 2, bindingType:  BindingType.TwoWay)]
		[Binding(targetId: 3, bindingType:  BindingType.TwoWay)]
		object P2 { get; set; }

		[Binding]
		[Binding(targetId: 1, bindingType:  BindingType.OneWayFromTarget)]
		[Binding(targetId: 2, bindingType:  BindingType.TwoWay)]
		[Binding(targetId: 3, bindingType:  BindingType.TwoWay)]
		object P3 { get; set; }

		[Binding]
		[Binding(targetId: 1, bindingType:  BindingType.OneWayFromTarget)]
		[Binding(targetId: 2, bindingType:  BindingType.TwoWay)]
		[Binding(targetId: 3, bindingType:  BindingType.TwoWay)]
		object P4 { get; set; }

		[Binding]
		[Binding(targetId: 1, bindingType:  BindingType.OneWayFromTarget)]
		[Binding(targetId: 2, bindingType:  BindingType.TwoWay)]
		[Binding(targetId: 3, bindingType:  BindingType.TwoWay)]
		object P5 { get; set; }

		[Binding]
		[Binding(targetId: 1, bindingType:  BindingType.OneWayFromTarget)]
		[Binding(targetId: 2, bindingType:  BindingType.TwoWay)]
		[Binding(targetId: 3, bindingType:  BindingType.TwoWay)]
		object P6 { get; set; }

		[BindingInitializer]
		void InitializeBindings();
	}

	public interface IV2
	{
		object P1 { get; set; }
		object P2 { get; set; }
		object P3 { get; set; }
		object P4 { get; set; }
		object P5 { get; set; }
		object P6 { get; set; }
	}

	public class V1_1 : IV1
	{
		public IV2 B1 { get; set; }
		public V1_1 B2 { get; set; }

		public object P1 { get; set; }
		public object P2 { get; set; }
		public object P3 { get; set; }
		public object P4 { get; set; }
		public object P5 { get; set; }
		public object P6 { get; set; }

		public void InitializeBindings()
		{
		}
	}

	public class V1_2 : IV1
	{
		public IV2 B1 { get; set; }
		public V1_1 B2 { get; set; }

		public object P1 { get; set; }
		public object P2 { get; set; }
		public object P3 { get; set; }
		public object P4 { get; set; }
		public object P5 { get; set; }
		public object P6 { get; set; }

		public void InitializeBindings()
		{
		}
	}

	public class V1_3 : IV1
	{
		public IV2 B1 { get; set; }
		public V1_1 B2 { get; set; }

		public object P1 { get; set; }
		public object P2 { get; set; }
		public object P3 { get; set; }
		public object P4 { get; set; }
		public object P5 { get; set; }
		public object P6 { get; set; }

		public void InitializeBindings()
		{
		}
	}

	public class V1_4 : IV1
	{
		public IV2 B1 { get; set; }
		public V1_1 B2 { get; set; }

		public object P1 { get; set; }
		public object P2 { get; set; }
		public object P3 { get; set; }
		public object P4 { get; set; }
		public object P5 { get; set; }
		public object P6 { get; set; }

		public void InitializeBindings()
		{
		}
	}

	public class V1_5 : IV1
	{
		public IV2 B1 { get; set; }
		public V1_1 B2 { get; set; }

		public object P1 { get; set; }
		public object P2 { get; set; }
		public object P3 { get; set; }
		public object P4 { get; set; }
		public object P5 { get; set; }
		public object P6 { get; set; }

		public void InitializeBindings()
		{
		}
	}

	public class V1_6 : IV1
	{
		public IV2 B1 { get; set; }
		public V1_1 B2 { get; set; }

		public object P1 { get; set; }
		public object P2 { get; set; }
		public object P3 { get; set; }
		public object P4 { get; set; }
		public object P5 { get; set; }
		public object P6 { get; set; }

		public void InitializeBindings()
		{
		}
	}

	public class V2_1 : IV2
	{
		public object P1 { get; set; }
		public object P2 { get; set; }
		public object P3 { get; set; }
		public object P4 { get; set; }
		public object P5 { get; set; }
		public object P6 { get; set; }
	}

	public class V2_2 : IV2
	{
		public object P1 { get; set; }
		public object P2 { get; set; }
		public object P3 { get; set; }
		public object P4 { get; set; }
		public object P5 { get; set; }
		public object P6 { get; set; }
	}

	public class V2_3 : IV2
	{
		public object P1 { get; set; }
		public object P2 { get; set; }
		public object P3 { get; set; }
		public object P4 { get; set; }
		public object P5 { get; set; }
		public object P6 { get; set; }
	}

	public class V2_4 : IV2
	{
		public object P1 { get; set; }
		public object P2 { get; set; }
		public object P3 { get; set; }
		public object P4 { get; set; }
		public object P5 { get; set; }
		public object P6 { get; set; }
	}

	public class V2_5 : IV2
	{
		public object P1 { get; set; }
		public object P2 { get; set; }
		public object P3 { get; set; }
		public object P4 { get; set; }
		public object P5 { get; set; }
		public object P6 { get; set; }
	}

	public class V2_6 : IV2
	{
		public object P1 { get; set; }
		public object P2 { get; set; }
		public object P3 { get; set; }
		public object P4 { get; set; }
		public object P5 { get; set; }
		public object P6 { get; set; }
	}
}

namespace Realmar.DataBindings.Example.Performance7
{
	public interface IV1
	{
		[BindingTarget]
		[BindingTarget(id: 1)]
		IV2 B1 { get; }

		[BindingTarget(id: 2)]
		[BindingTarget(id: 3)]
		V1_1 B2 { get; }

		[Binding]
		[Binding(targetId: 1, bindingType:  BindingType.OneWayFromTarget)]
		[Binding(targetId: 2, bindingType:  BindingType.TwoWay)]
		[Binding(targetId: 3, bindingType:  BindingType.TwoWay)]
		object P1 { get; set; }

		[Binding]
		[Binding(targetId: 1, bindingType:  BindingType.OneWayFromTarget)]
		[Binding(targetId: 2, bindingType:  BindingType.TwoWay)]
		[Binding(targetId: 3, bindingType:  BindingType.TwoWay)]
		object P2 { get; set; }

		[Binding]
		[Binding(targetId: 1, bindingType:  BindingType.OneWayFromTarget)]
		[Binding(targetId: 2, bindingType:  BindingType.TwoWay)]
		[Binding(targetId: 3, bindingType:  BindingType.TwoWay)]
		object P3 { get; set; }

		[Binding]
		[Binding(targetId: 1, bindingType:  BindingType.OneWayFromTarget)]
		[Binding(targetId: 2, bindingType:  BindingType.TwoWay)]
		[Binding(targetId: 3, bindingType:  BindingType.TwoWay)]
		object P4 { get; set; }

		[Binding]
		[Binding(targetId: 1, bindingType:  BindingType.OneWayFromTarget)]
		[Binding(targetId: 2, bindingType:  BindingType.TwoWay)]
		[Binding(targetId: 3, bindingType:  BindingType.TwoWay)]
		object P5 { get; set; }

		[Binding]
		[Binding(targetId: 1, bindingType:  BindingType.OneWayFromTarget)]
		[Binding(targetId: 2, bindingType:  BindingType.TwoWay)]
		[Binding(targetId: 3, bindingType:  BindingType.TwoWay)]
		object P6 { get; set; }

		[BindingInitializer]
		void InitializeBindings();
	}

	public interface IV2
	{
		object P1 { get; set; }
		object P2 { get; set; }
		object P3 { get; set; }
		object P4 { get; set; }
		object P5 { get; set; }
		object P6 { get; set; }
	}

	public class V1_1 : IV1
	{
		public IV2 B1 { get; set; }
		public V1_1 B2 { get; set; }

		public object P1 { get; set; }
		public object P2 { get; set; }
		public object P3 { get; set; }
		public object P4 { get; set; }
		public object P5 { get; set; }
		public object P6 { get; set; }

		public void InitializeBindings()
		{
		}
	}

	public class V1_2 : IV1
	{
		public IV2 B1 { get; set; }
		public V1_1 B2 { get; set; }

		public object P1 { get; set; }
		public object P2 { get; set; }
		public object P3 { get; set; }
		public object P4 { get; set; }
		public object P5 { get; set; }
		public object P6 { get; set; }

		public void InitializeBindings()
		{
		}
	}

	public class V1_3 : IV1
	{
		public IV2 B1 { get; set; }
		public V1_1 B2 { get; set; }

		public object P1 { get; set; }
		public object P2 { get; set; }
		public object P3 { get; set; }
		public object P4 { get; set; }
		public object P5 { get; set; }
		public object P6 { get; set; }

		public void InitializeBindings()
		{
		}
	}

	public class V1_4 : IV1
	{
		public IV2 B1 { get; set; }
		public V1_1 B2 { get; set; }

		public object P1 { get; set; }
		public object P2 { get; set; }
		public object P3 { get; set; }
		public object P4 { get; set; }
		public object P5 { get; set; }
		public object P6 { get; set; }

		public void InitializeBindings()
		{
		}
	}

	public class V1_5 : IV1
	{
		public IV2 B1 { get; set; }
		public V1_1 B2 { get; set; }

		public object P1 { get; set; }
		public object P2 { get; set; }
		public object P3 { get; set; }
		public object P4 { get; set; }
		public object P5 { get; set; }
		public object P6 { get; set; }

		public void InitializeBindings()
		{
		}
	}

	public class V1_6 : IV1
	{
		public IV2 B1 { get; set; }
		public V1_1 B2 { get; set; }

		public object P1 { get; set; }
		public object P2 { get; set; }
		public object P3 { get; set; }
		public object P4 { get; set; }
		public object P5 { get; set; }
		public object P6 { get; set; }

		public void InitializeBindings()
		{
		}
	}

	public class V2_1 : IV2
	{
		public object P1 { get; set; }
		public object P2 { get; set; }
		public object P3 { get; set; }
		public object P4 { get; set; }
		public object P5 { get; set; }
		public object P6 { get; set; }
	}

	public class V2_2 : IV2
	{
		public object P1 { get; set; }
		public object P2 { get; set; }
		public object P3 { get; set; }
		public object P4 { get; set; }
		public object P5 { get; set; }
		public object P6 { get; set; }
	}

	public class V2_3 : IV2
	{
		public object P1 { get; set; }
		public object P2 { get; set; }
		public object P3 { get; set; }
		public object P4 { get; set; }
		public object P5 { get; set; }
		public object P6 { get; set; }
	}

	public class V2_4 : IV2
	{
		public object P1 { get; set; }
		public object P2 { get; set; }
		public object P3 { get; set; }
		public object P4 { get; set; }
		public object P5 { get; set; }
		public object P6 { get; set; }
	}

	public class V2_5 : IV2
	{
		public object P1 { get; set; }
		public object P2 { get; set; }
		public object P3 { get; set; }
		public object P4 { get; set; }
		public object P5 { get; set; }
		public object P6 { get; set; }
	}

	public class V2_6 : IV2
	{
		public object P1 { get; set; }
		public object P2 { get; set; }
		public object P3 { get; set; }
		public object P4 { get; set; }
		public object P5 { get; set; }
		public object P6 { get; set; }
	}
}

namespace Realmar.DataBindings.Example.Performance8
{
	public interface IV1
	{
		[BindingTarget]
		[BindingTarget(id: 1)]
		IV2 B1 { get; }

		[BindingTarget(id: 2)]
		[BindingTarget(id: 3)]
		V1_1 B2 { get; }

		[Binding]
		[Binding(targetId: 1, bindingType:  BindingType.OneWayFromTarget)]
		[Binding(targetId: 2, bindingType:  BindingType.TwoWay)]
		[Binding(targetId: 3, bindingType:  BindingType.TwoWay)]
		object P1 { get; set; }

		[Binding]
		[Binding(targetId: 1, bindingType:  BindingType.OneWayFromTarget)]
		[Binding(targetId: 2, bindingType:  BindingType.TwoWay)]
		[Binding(targetId: 3, bindingType:  BindingType.TwoWay)]
		object P2 { get; set; }

		[Binding]
		[Binding(targetId: 1, bindingType:  BindingType.OneWayFromTarget)]
		[Binding(targetId: 2, bindingType:  BindingType.TwoWay)]
		[Binding(targetId: 3, bindingType:  BindingType.TwoWay)]
		object P3 { get; set; }

		[Binding]
		[Binding(targetId: 1, bindingType:  BindingType.OneWayFromTarget)]
		[Binding(targetId: 2, bindingType:  BindingType.TwoWay)]
		[Binding(targetId: 3, bindingType:  BindingType.TwoWay)]
		object P4 { get; set; }

		[Binding]
		[Binding(targetId: 1, bindingType:  BindingType.OneWayFromTarget)]
		[Binding(targetId: 2, bindingType:  BindingType.TwoWay)]
		[Binding(targetId: 3, bindingType:  BindingType.TwoWay)]
		object P5 { get; set; }

		[Binding]
		[Binding(targetId: 1, bindingType:  BindingType.OneWayFromTarget)]
		[Binding(targetId: 2, bindingType:  BindingType.TwoWay)]
		[Binding(targetId: 3, bindingType:  BindingType.TwoWay)]
		object P6 { get; set; }

		[BindingInitializer]
		void InitializeBindings();
	}

	public interface IV2
	{
		object P1 { get; set; }
		object P2 { get; set; }
		object P3 { get; set; }
		object P4 { get; set; }
		object P5 { get; set; }
		object P6 { get; set; }
	}

	public class V1_1 : IV1
	{
		public IV2 B1 { get; set; }
		public V1_1 B2 { get; set; }

		public object P1 { get; set; }
		public object P2 { get; set; }
		public object P3 { get; set; }
		public object P4 { get; set; }
		public object P5 { get; set; }
		public object P6 { get; set; }

		public void InitializeBindings()
		{
		}
	}

	public class V1_2 : IV1
	{
		public IV2 B1 { get; set; }
		public V1_1 B2 { get; set; }

		public object P1 { get; set; }
		public object P2 { get; set; }
		public object P3 { get; set; }
		public object P4 { get; set; }
		public object P5 { get; set; }
		public object P6 { get; set; }

		public void InitializeBindings()
		{
		}
	}

	public class V1_3 : IV1
	{
		public IV2 B1 { get; set; }
		public V1_1 B2 { get; set; }

		public object P1 { get; set; }
		public object P2 { get; set; }
		public object P3 { get; set; }
		public object P4 { get; set; }
		public object P5 { get; set; }
		public object P6 { get; set; }

		public void InitializeBindings()
		{
		}
	}

	public class V1_4 : IV1
	{
		public IV2 B1 { get; set; }
		public V1_1 B2 { get; set; }

		public object P1 { get; set; }
		public object P2 { get; set; }
		public object P3 { get; set; }
		public object P4 { get; set; }
		public object P5 { get; set; }
		public object P6 { get; set; }

		public void InitializeBindings()
		{
		}
	}

	public class V1_5 : IV1
	{
		public IV2 B1 { get; set; }
		public V1_1 B2 { get; set; }

		public object P1 { get; set; }
		public object P2 { get; set; }
		public object P3 { get; set; }
		public object P4 { get; set; }
		public object P5 { get; set; }
		public object P6 { get; set; }

		public void InitializeBindings()
		{
		}
	}

	public class V1_6 : IV1
	{
		public IV2 B1 { get; set; }
		public V1_1 B2 { get; set; }

		public object P1 { get; set; }
		public object P2 { get; set; }
		public object P3 { get; set; }
		public object P4 { get; set; }
		public object P5 { get; set; }
		public object P6 { get; set; }

		public void InitializeBindings()
		{
		}
	}

	public class V2_1 : IV2
	{
		public object P1 { get; set; }
		public object P2 { get; set; }
		public object P3 { get; set; }
		public object P4 { get; set; }
		public object P5 { get; set; }
		public object P6 { get; set; }
	}

	public class V2_2 : IV2
	{
		public object P1 { get; set; }
		public object P2 { get; set; }
		public object P3 { get; set; }
		public object P4 { get; set; }
		public object P5 { get; set; }
		public object P6 { get; set; }
	}

	public class V2_3 : IV2
	{
		public object P1 { get; set; }
		public object P2 { get; set; }
		public object P3 { get; set; }
		public object P4 { get; set; }
		public object P5 { get; set; }
		public object P6 { get; set; }
	}

	public class V2_4 : IV2
	{
		public object P1 { get; set; }
		public object P2 { get; set; }
		public object P3 { get; set; }
		public object P4 { get; set; }
		public object P5 { get; set; }
		public object P6 { get; set; }
	}

	public class V2_5 : IV2
	{
		public object P1 { get; set; }
		public object P2 { get; set; }
		public object P3 { get; set; }
		public object P4 { get; set; }
		public object P5 { get; set; }
		public object P6 { get; set; }
	}

	public class V2_6 : IV2
	{
		public object P1 { get; set; }
		public object P2 { get; set; }
		public object P3 { get; set; }
		public object P4 { get; set; }
		public object P5 { get; set; }
		public object P6 { get; set; }
	}
}

namespace Realmar.DataBindings.Example.Performance9
{
	public interface IV1
	{
		[BindingTarget]
		[BindingTarget(id: 1)]
		IV2 B1 { get; }

		[BindingTarget(id: 2)]
		[BindingTarget(id: 3)]
		V1_1 B2 { get; }

		[Binding]
		[Binding(targetId: 1, bindingType:  BindingType.OneWayFromTarget)]
		[Binding(targetId: 2, bindingType:  BindingType.TwoWay)]
		[Binding(targetId: 3, bindingType:  BindingType.TwoWay)]
		object P1 { get; set; }

		[Binding]
		[Binding(targetId: 1, bindingType:  BindingType.OneWayFromTarget)]
		[Binding(targetId: 2, bindingType:  BindingType.TwoWay)]
		[Binding(targetId: 3, bindingType:  BindingType.TwoWay)]
		object P2 { get; set; }

		[Binding]
		[Binding(targetId: 1, bindingType:  BindingType.OneWayFromTarget)]
		[Binding(targetId: 2, bindingType:  BindingType.TwoWay)]
		[Binding(targetId: 3, bindingType:  BindingType.TwoWay)]
		object P3 { get; set; }

		[Binding]
		[Binding(targetId: 1, bindingType:  BindingType.OneWayFromTarget)]
		[Binding(targetId: 2, bindingType:  BindingType.TwoWay)]
		[Binding(targetId: 3, bindingType:  BindingType.TwoWay)]
		object P4 { get; set; }

		[Binding]
		[Binding(targetId: 1, bindingType:  BindingType.OneWayFromTarget)]
		[Binding(targetId: 2, bindingType:  BindingType.TwoWay)]
		[Binding(targetId: 3, bindingType:  BindingType.TwoWay)]
		object P5 { get; set; }

		[Binding]
		[Binding(targetId: 1, bindingType:  BindingType.OneWayFromTarget)]
		[Binding(targetId: 2, bindingType:  BindingType.TwoWay)]
		[Binding(targetId: 3, bindingType:  BindingType.TwoWay)]
		object P6 { get; set; }

		[BindingInitializer]
		void InitializeBindings();
	}

	public interface IV2
	{
		object P1 { get; set; }
		object P2 { get; set; }
		object P3 { get; set; }
		object P4 { get; set; }
		object P5 { get; set; }
		object P6 { get; set; }
	}

	public class V1_1 : IV1
	{
		public IV2 B1 { get; set; }
		public V1_1 B2 { get; set; }

		public object P1 { get; set; }
		public object P2 { get; set; }
		public object P3 { get; set; }
		public object P4 { get; set; }
		public object P5 { get; set; }
		public object P6 { get; set; }

		public void InitializeBindings()
		{
		}
	}

	public class V1_2 : IV1
	{
		public IV2 B1 { get; set; }
		public V1_1 B2 { get; set; }

		public object P1 { get; set; }
		public object P2 { get; set; }
		public object P3 { get; set; }
		public object P4 { get; set; }
		public object P5 { get; set; }
		public object P6 { get; set; }

		public void InitializeBindings()
		{
		}
	}

	public class V1_3 : IV1
	{
		public IV2 B1 { get; set; }
		public V1_1 B2 { get; set; }

		public object P1 { get; set; }
		public object P2 { get; set; }
		public object P3 { get; set; }
		public object P4 { get; set; }
		public object P5 { get; set; }
		public object P6 { get; set; }

		public void InitializeBindings()
		{
		}
	}

	public class V1_4 : IV1
	{
		public IV2 B1 { get; set; }
		public V1_1 B2 { get; set; }

		public object P1 { get; set; }
		public object P2 { get; set; }
		public object P3 { get; set; }
		public object P4 { get; set; }
		public object P5 { get; set; }
		public object P6 { get; set; }

		public void InitializeBindings()
		{
		}
	}

	public class V1_5 : IV1
	{
		public IV2 B1 { get; set; }
		public V1_1 B2 { get; set; }

		public object P1 { get; set; }
		public object P2 { get; set; }
		public object P3 { get; set; }
		public object P4 { get; set; }
		public object P5 { get; set; }
		public object P6 { get; set; }

		public void InitializeBindings()
		{
		}
	}

	public class V1_6 : IV1
	{
		public IV2 B1 { get; set; }
		public V1_1 B2 { get; set; }

		public object P1 { get; set; }
		public object P2 { get; set; }
		public object P3 { get; set; }
		public object P4 { get; set; }
		public object P5 { get; set; }
		public object P6 { get; set; }

		public void InitializeBindings()
		{
		}
	}

	public class V2_1 : IV2
	{
		public object P1 { get; set; }
		public object P2 { get; set; }
		public object P3 { get; set; }
		public object P4 { get; set; }
		public object P5 { get; set; }
		public object P6 { get; set; }
	}

	public class V2_2 : IV2
	{
		public object P1 { get; set; }
		public object P2 { get; set; }
		public object P3 { get; set; }
		public object P4 { get; set; }
		public object P5 { get; set; }
		public object P6 { get; set; }
	}

	public class V2_3 : IV2
	{
		public object P1 { get; set; }
		public object P2 { get; set; }
		public object P3 { get; set; }
		public object P4 { get; set; }
		public object P5 { get; set; }
		public object P6 { get; set; }
	}

	public class V2_4 : IV2
	{
		public object P1 { get; set; }
		public object P2 { get; set; }
		public object P3 { get; set; }
		public object P4 { get; set; }
		public object P5 { get; set; }
		public object P6 { get; set; }
	}

	public class V2_5 : IV2
	{
		public object P1 { get; set; }
		public object P2 { get; set; }
		public object P3 { get; set; }
		public object P4 { get; set; }
		public object P5 { get; set; }
		public object P6 { get; set; }
	}

	public class V2_6 : IV2
	{
		public object P1 { get; set; }
		public object P2 { get; set; }
		public object P3 { get; set; }
		public object P4 { get; set; }
		public object P5 { get; set; }
		public object P6 { get; set; }
	}
}

namespace Realmar.DataBindings.Example.Performance10
{
	public interface IV1
	{
		[BindingTarget]
		[BindingTarget(id: 1)]
		IV2 B1 { get; }

		[BindingTarget(id: 2)]
		[BindingTarget(id: 3)]
		V1_1 B2 { get; }

		[Binding]
		[Binding(targetId: 1, bindingType:  BindingType.OneWayFromTarget)]
		[Binding(targetId: 2, bindingType:  BindingType.TwoWay)]
		[Binding(targetId: 3, bindingType:  BindingType.TwoWay)]
		object P1 { get; set; }

		[Binding]
		[Binding(targetId: 1, bindingType:  BindingType.OneWayFromTarget)]
		[Binding(targetId: 2, bindingType:  BindingType.TwoWay)]
		[Binding(targetId: 3, bindingType:  BindingType.TwoWay)]
		object P2 { get; set; }

		[Binding]
		[Binding(targetId: 1, bindingType:  BindingType.OneWayFromTarget)]
		[Binding(targetId: 2, bindingType:  BindingType.TwoWay)]
		[Binding(targetId: 3, bindingType:  BindingType.TwoWay)]
		object P3 { get; set; }

		[Binding]
		[Binding(targetId: 1, bindingType:  BindingType.OneWayFromTarget)]
		[Binding(targetId: 2, bindingType:  BindingType.TwoWay)]
		[Binding(targetId: 3, bindingType:  BindingType.TwoWay)]
		object P4 { get; set; }

		[Binding]
		[Binding(targetId: 1, bindingType:  BindingType.OneWayFromTarget)]
		[Binding(targetId: 2, bindingType:  BindingType.TwoWay)]
		[Binding(targetId: 3, bindingType:  BindingType.TwoWay)]
		object P5 { get; set; }

		[Binding]
		[Binding(targetId: 1, bindingType:  BindingType.OneWayFromTarget)]
		[Binding(targetId: 2, bindingType:  BindingType.TwoWay)]
		[Binding(targetId: 3, bindingType:  BindingType.TwoWay)]
		object P6 { get; set; }

		[BindingInitializer]
		void InitializeBindings();
	}

	public interface IV2
	{
		object P1 { get; set; }
		object P2 { get; set; }
		object P3 { get; set; }
		object P4 { get; set; }
		object P5 { get; set; }
		object P6 { get; set; }
	}

	public class V1_1 : IV1
	{
		public IV2 B1 { get; set; }
		public V1_1 B2 { get; set; }

		public object P1 { get; set; }
		public object P2 { get; set; }
		public object P3 { get; set; }
		public object P4 { get; set; }
		public object P5 { get; set; }
		public object P6 { get; set; }

		public void InitializeBindings()
		{
		}
	}

	public class V1_2 : IV1
	{
		public IV2 B1 { get; set; }
		public V1_1 B2 { get; set; }

		public object P1 { get; set; }
		public object P2 { get; set; }
		public object P3 { get; set; }
		public object P4 { get; set; }
		public object P5 { get; set; }
		public object P6 { get; set; }

		public void InitializeBindings()
		{
		}
	}

	public class V1_3 : IV1
	{
		public IV2 B1 { get; set; }
		public V1_1 B2 { get; set; }

		public object P1 { get; set; }
		public object P2 { get; set; }
		public object P3 { get; set; }
		public object P4 { get; set; }
		public object P5 { get; set; }
		public object P6 { get; set; }

		public void InitializeBindings()
		{
		}
	}

	public class V1_4 : IV1
	{
		public IV2 B1 { get; set; }
		public V1_1 B2 { get; set; }

		public object P1 { get; set; }
		public object P2 { get; set; }
		public object P3 { get; set; }
		public object P4 { get; set; }
		public object P5 { get; set; }
		public object P6 { get; set; }

		public void InitializeBindings()
		{
		}
	}

	public class V1_5 : IV1
	{
		public IV2 B1 { get; set; }
		public V1_1 B2 { get; set; }

		public object P1 { get; set; }
		public object P2 { get; set; }
		public object P3 { get; set; }
		public object P4 { get; set; }
		public object P5 { get; set; }
		public object P6 { get; set; }

		public void InitializeBindings()
		{
		}
	}

	public class V1_6 : IV1
	{
		public IV2 B1 { get; set; }
		public V1_1 B2 { get; set; }

		public object P1 { get; set; }
		public object P2 { get; set; }
		public object P3 { get; set; }
		public object P4 { get; set; }
		public object P5 { get; set; }
		public object P6 { get; set; }

		public void InitializeBindings()
		{
		}
	}

	public class V2_1 : IV2
	{
		public object P1 { get; set; }
		public object P2 { get; set; }
		public object P3 { get; set; }
		public object P4 { get; set; }
		public object P5 { get; set; }
		public object P6 { get; set; }
	}

	public class V2_2 : IV2
	{
		public object P1 { get; set; }
		public object P2 { get; set; }
		public object P3 { get; set; }
		public object P4 { get; set; }
		public object P5 { get; set; }
		public object P6 { get; set; }
	}

	public class V2_3 : IV2
	{
		public object P1 { get; set; }
		public object P2 { get; set; }
		public object P3 { get; set; }
		public object P4 { get; set; }
		public object P5 { get; set; }
		public object P6 { get; set; }
	}

	public class V2_4 : IV2
	{
		public object P1 { get; set; }
		public object P2 { get; set; }
		public object P3 { get; set; }
		public object P4 { get; set; }
		public object P5 { get; set; }
		public object P6 { get; set; }
	}

	public class V2_5 : IV2
	{
		public object P1 { get; set; }
		public object P2 { get; set; }
		public object P3 { get; set; }
		public object P4 { get; set; }
		public object P5 { get; set; }
		public object P6 { get; set; }
	}

	public class V2_6 : IV2
	{
		public object P1 { get; set; }
		public object P2 { get; set; }
		public object P3 { get; set; }
		public object P4 { get; set; }
		public object P5 { get; set; }
		public object P6 { get; set; }
	}
}

namespace Realmar.DataBindings.Example.Performance11
{
	public interface IV1
	{
		[BindingTarget]
		[BindingTarget(id: 1)]
		IV2 B1 { get; }

		[BindingTarget(id: 2)]
		[BindingTarget(id: 3)]
		V1_1 B2 { get; }

		[Binding]
		[Binding(targetId: 1, bindingType:  BindingType.OneWayFromTarget)]
		[Binding(targetId: 2, bindingType:  BindingType.TwoWay)]
		[Binding(targetId: 3, bindingType:  BindingType.TwoWay)]
		object P1 { get; set; }

		[Binding]
		[Binding(targetId: 1, bindingType:  BindingType.OneWayFromTarget)]
		[Binding(targetId: 2, bindingType:  BindingType.TwoWay)]
		[Binding(targetId: 3, bindingType:  BindingType.TwoWay)]
		object P2 { get; set; }

		[Binding]
		[Binding(targetId: 1, bindingType:  BindingType.OneWayFromTarget)]
		[Binding(targetId: 2, bindingType:  BindingType.TwoWay)]
		[Binding(targetId: 3, bindingType:  BindingType.TwoWay)]
		object P3 { get; set; }

		[Binding]
		[Binding(targetId: 1, bindingType:  BindingType.OneWayFromTarget)]
		[Binding(targetId: 2, bindingType:  BindingType.TwoWay)]
		[Binding(targetId: 3, bindingType:  BindingType.TwoWay)]
		object P4 { get; set; }

		[Binding]
		[Binding(targetId: 1, bindingType:  BindingType.OneWayFromTarget)]
		[Binding(targetId: 2, bindingType:  BindingType.TwoWay)]
		[Binding(targetId: 3, bindingType:  BindingType.TwoWay)]
		object P5 { get; set; }

		[Binding]
		[Binding(targetId: 1, bindingType:  BindingType.OneWayFromTarget)]
		[Binding(targetId: 2, bindingType:  BindingType.TwoWay)]
		[Binding(targetId: 3, bindingType:  BindingType.TwoWay)]
		object P6 { get; set; }

		[BindingInitializer]
		void InitializeBindings();
	}

	public interface IV2
	{
		object P1 { get; set; }
		object P2 { get; set; }
		object P3 { get; set; }
		object P4 { get; set; }
		object P5 { get; set; }
		object P6 { get; set; }
	}

	public class V1_1 : IV1
	{
		public IV2 B1 { get; set; }
		public V1_1 B2 { get; set; }

		public object P1 { get; set; }
		public object P2 { get; set; }
		public object P3 { get; set; }
		public object P4 { get; set; }
		public object P5 { get; set; }
		public object P6 { get; set; }

		public void InitializeBindings()
		{
		}
	}

	public class V1_2 : IV1
	{
		public IV2 B1 { get; set; }
		public V1_1 B2 { get; set; }

		public object P1 { get; set; }
		public object P2 { get; set; }
		public object P3 { get; set; }
		public object P4 { get; set; }
		public object P5 { get; set; }
		public object P6 { get; set; }

		public void InitializeBindings()
		{
		}
	}

	public class V1_3 : IV1
	{
		public IV2 B1 { get; set; }
		public V1_1 B2 { get; set; }

		public object P1 { get; set; }
		public object P2 { get; set; }
		public object P3 { get; set; }
		public object P4 { get; set; }
		public object P5 { get; set; }
		public object P6 { get; set; }

		public void InitializeBindings()
		{
		}
	}

	public class V1_4 : IV1
	{
		public IV2 B1 { get; set; }
		public V1_1 B2 { get; set; }

		public object P1 { get; set; }
		public object P2 { get; set; }
		public object P3 { get; set; }
		public object P4 { get; set; }
		public object P5 { get; set; }
		public object P6 { get; set; }

		public void InitializeBindings()
		{
		}
	}

	public class V1_5 : IV1
	{
		public IV2 B1 { get; set; }
		public V1_1 B2 { get; set; }

		public object P1 { get; set; }
		public object P2 { get; set; }
		public object P3 { get; set; }
		public object P4 { get; set; }
		public object P5 { get; set; }
		public object P6 { get; set; }

		public void InitializeBindings()
		{
		}
	}

	public class V1_6 : IV1
	{
		public IV2 B1 { get; set; }
		public V1_1 B2 { get; set; }

		public object P1 { get; set; }
		public object P2 { get; set; }
		public object P3 { get; set; }
		public object P4 { get; set; }
		public object P5 { get; set; }
		public object P6 { get; set; }

		public void InitializeBindings()
		{
		}
	}

	public class V2_1 : IV2
	{
		public object P1 { get; set; }
		public object P2 { get; set; }
		public object P3 { get; set; }
		public object P4 { get; set; }
		public object P5 { get; set; }
		public object P6 { get; set; }
	}

	public class V2_2 : IV2
	{
		public object P1 { get; set; }
		public object P2 { get; set; }
		public object P3 { get; set; }
		public object P4 { get; set; }
		public object P5 { get; set; }
		public object P6 { get; set; }
	}

	public class V2_3 : IV2
	{
		public object P1 { get; set; }
		public object P2 { get; set; }
		public object P3 { get; set; }
		public object P4 { get; set; }
		public object P5 { get; set; }
		public object P6 { get; set; }
	}

	public class V2_4 : IV2
	{
		public object P1 { get; set; }
		public object P2 { get; set; }
		public object P3 { get; set; }
		public object P4 { get; set; }
		public object P5 { get; set; }
		public object P6 { get; set; }
	}

	public class V2_5 : IV2
	{
		public object P1 { get; set; }
		public object P2 { get; set; }
		public object P3 { get; set; }
		public object P4 { get; set; }
		public object P5 { get; set; }
		public object P6 { get; set; }
	}

	public class V2_6 : IV2
	{
		public object P1 { get; set; }
		public object P2 { get; set; }
		public object P3 { get; set; }
		public object P4 { get; set; }
		public object P5 { get; set; }
		public object P6 { get; set; }
	}
}

namespace Realmar.DataBindings.Example.Performance12
{
	public interface IV1
	{
		[BindingTarget]
		[BindingTarget(id: 1)]
		IV2 B1 { get; }

		[BindingTarget(id: 2)]
		[BindingTarget(id: 3)]
		V1_1 B2 { get; }

		[Binding]
		[Binding(targetId: 1, bindingType:  BindingType.OneWayFromTarget)]
		[Binding(targetId: 2, bindingType:  BindingType.TwoWay)]
		[Binding(targetId: 3, bindingType:  BindingType.TwoWay)]
		object P1 { get; set; }

		[Binding]
		[Binding(targetId: 1, bindingType:  BindingType.OneWayFromTarget)]
		[Binding(targetId: 2, bindingType:  BindingType.TwoWay)]
		[Binding(targetId: 3, bindingType:  BindingType.TwoWay)]
		object P2 { get; set; }

		[Binding]
		[Binding(targetId: 1, bindingType:  BindingType.OneWayFromTarget)]
		[Binding(targetId: 2, bindingType:  BindingType.TwoWay)]
		[Binding(targetId: 3, bindingType:  BindingType.TwoWay)]
		object P3 { get; set; }

		[Binding]
		[Binding(targetId: 1, bindingType:  BindingType.OneWayFromTarget)]
		[Binding(targetId: 2, bindingType:  BindingType.TwoWay)]
		[Binding(targetId: 3, bindingType:  BindingType.TwoWay)]
		object P4 { get; set; }

		[Binding]
		[Binding(targetId: 1, bindingType:  BindingType.OneWayFromTarget)]
		[Binding(targetId: 2, bindingType:  BindingType.TwoWay)]
		[Binding(targetId: 3, bindingType:  BindingType.TwoWay)]
		object P5 { get; set; }

		[Binding]
		[Binding(targetId: 1, bindingType:  BindingType.OneWayFromTarget)]
		[Binding(targetId: 2, bindingType:  BindingType.TwoWay)]
		[Binding(targetId: 3, bindingType:  BindingType.TwoWay)]
		object P6 { get; set; }

		[BindingInitializer]
		void InitializeBindings();
	}

	public interface IV2
	{
		object P1 { get; set; }
		object P2 { get; set; }
		object P3 { get; set; }
		object P4 { get; set; }
		object P5 { get; set; }
		object P6 { get; set; }
	}

	public class V1_1 : IV1
	{
		public IV2 B1 { get; set; }
		public V1_1 B2 { get; set; }

		public object P1 { get; set; }
		public object P2 { get; set; }
		public object P3 { get; set; }
		public object P4 { get; set; }
		public object P5 { get; set; }
		public object P6 { get; set; }

		public void InitializeBindings()
		{
		}
	}

	public class V1_2 : IV1
	{
		public IV2 B1 { get; set; }
		public V1_1 B2 { get; set; }

		public object P1 { get; set; }
		public object P2 { get; set; }
		public object P3 { get; set; }
		public object P4 { get; set; }
		public object P5 { get; set; }
		public object P6 { get; set; }

		public void InitializeBindings()
		{
		}
	}

	public class V1_3 : IV1
	{
		public IV2 B1 { get; set; }
		public V1_1 B2 { get; set; }

		public object P1 { get; set; }
		public object P2 { get; set; }
		public object P3 { get; set; }
		public object P4 { get; set; }
		public object P5 { get; set; }
		public object P6 { get; set; }

		public void InitializeBindings()
		{
		}
	}

	public class V1_4 : IV1
	{
		public IV2 B1 { get; set; }
		public V1_1 B2 { get; set; }

		public object P1 { get; set; }
		public object P2 { get; set; }
		public object P3 { get; set; }
		public object P4 { get; set; }
		public object P5 { get; set; }
		public object P6 { get; set; }

		public void InitializeBindings()
		{
		}
	}

	public class V1_5 : IV1
	{
		public IV2 B1 { get; set; }
		public V1_1 B2 { get; set; }

		public object P1 { get; set; }
		public object P2 { get; set; }
		public object P3 { get; set; }
		public object P4 { get; set; }
		public object P5 { get; set; }
		public object P6 { get; set; }

		public void InitializeBindings()
		{
		}
	}

	public class V1_6 : IV1
	{
		public IV2 B1 { get; set; }
		public V1_1 B2 { get; set; }

		public object P1 { get; set; }
		public object P2 { get; set; }
		public object P3 { get; set; }
		public object P4 { get; set; }
		public object P5 { get; set; }
		public object P6 { get; set; }

		public void InitializeBindings()
		{
		}
	}

	public class V2_1 : IV2
	{
		public object P1 { get; set; }
		public object P2 { get; set; }
		public object P3 { get; set; }
		public object P4 { get; set; }
		public object P5 { get; set; }
		public object P6 { get; set; }
	}

	public class V2_2 : IV2
	{
		public object P1 { get; set; }
		public object P2 { get; set; }
		public object P3 { get; set; }
		public object P4 { get; set; }
		public object P5 { get; set; }
		public object P6 { get; set; }
	}

	public class V2_3 : IV2
	{
		public object P1 { get; set; }
		public object P2 { get; set; }
		public object P3 { get; set; }
		public object P4 { get; set; }
		public object P5 { get; set; }
		public object P6 { get; set; }
	}

	public class V2_4 : IV2
	{
		public object P1 { get; set; }
		public object P2 { get; set; }
		public object P3 { get; set; }
		public object P4 { get; set; }
		public object P5 { get; set; }
		public object P6 { get; set; }
	}

	public class V2_5 : IV2
	{
		public object P1 { get; set; }
		public object P2 { get; set; }
		public object P3 { get; set; }
		public object P4 { get; set; }
		public object P5 { get; set; }
		public object P6 { get; set; }
	}

	public class V2_6 : IV2
	{
		public object P1 { get; set; }
		public object P2 { get; set; }
		public object P3 { get; set; }
		public object P4 { get; set; }
		public object P5 { get; set; }
		public object P6 { get; set; }
	}
}*/
