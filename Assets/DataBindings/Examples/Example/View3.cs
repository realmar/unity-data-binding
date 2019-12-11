using System.Diagnostics.Tracing;
using UnityEngine;
using UnityEngine.UI;

namespace Realmar.DataBindings.Example
{
	public class View3 : MonoBehaviour
	{
		[SerializeField] private Text _text;

		public string Text
		{
			get => _text.text;
			set => _text.text = value;
		}
	}
}
