using UnityEngine;
using UnityEngine.AI;

namespace Realmar.DataBindings.Example
{
	public class BaseVirtualView2 : MonoBehaviour
	{
		private string _textBase;
		public object A;

		public virtual string Text
		{
			get => _textBase;
			set
			{
				_textBase = value;
				// Debug.Log($"{nameof(BaseVirtualView2)} {Text} {value}");
			}
		}

		public virtual string Text2 { get; set; }

		protected virtual void VirtualExample()
		{
		}
	}
}
