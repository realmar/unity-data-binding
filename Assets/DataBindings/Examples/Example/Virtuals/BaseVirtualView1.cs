using UnityEngine;

namespace Realmar.DataBindings.Example
{
	public class BaseVirtualView1 : MonoBehaviour
	{
		[SerializeField /*, BindingTarget*/] protected BaseVirtualView2 _baseBiew;
		[SerializeField] private BaseVirtualView2 _baseBiew2;

		// [BindingTarget]
		public virtual BaseVirtualView2 View
		{
			get => _baseBiew;
			set => _baseBiew = value;
		}

		// [BindingTarget(Id = 1)]
		public virtual BaseVirtualView2 View2 { get; set; }

		// [Binding]
		public virtual string Text { get; protected set; }

		protected virtual void Awake()
		{
			View2 = _baseBiew2;
		}
	}
}
