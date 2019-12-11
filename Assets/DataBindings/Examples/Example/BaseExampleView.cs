using UnityEngine;

namespace Realmar.DataBindings.Example
{
	public class BaseExampleView : MonoBehaviour
	{
		public string ExampleStr;
		public string ExampleStrProperty { get; set; }

		private string _ex;

		public virtual string Ex
		{
			get => _ex;
			set => _ex = value;
		}
	}
}
