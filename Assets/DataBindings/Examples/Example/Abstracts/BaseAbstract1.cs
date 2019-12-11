using JetBrains.Annotations;
using UnityEngine;

namespace Realmar.DataBindings.Example.Abstracts
{
	public abstract class BaseAbstract1 : MonoBehaviour
	{
		[BindingTarget] public Abstract2 BT1;
		[BindingTarget(id: 1)] public BaseAbstract2 BT2;

		// [Binding]
		public abstract string Text1 { get; set; }

		// [Binding(TargetId = 1)]
		public string Text2 { get; set; }

		// [Binding(TargetId = 1)]
		public abstract string Text3 { get; set; }

		[UsedImplicitly, BindingInitializer]
		protected virtual void Awake()
		{
		}
	}
}
