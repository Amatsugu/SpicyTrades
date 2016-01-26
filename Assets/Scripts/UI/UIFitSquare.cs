using UnityEngine;
using System.Collections;
namespace LuminousVector
{
	[ExecuteInEditMode]
	public class UIFitSquare : MonoBehaviour
	{
		//Public
		public float padding = 50;
		
		//Private
		private RectTransform t;

		void Start()
		{
			t = GetComponent<RectTransform>();
		}

		void Update ()
		{
			float size = Screen.height - 50 * 2;
			t.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, size);
			t.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, size);
		}
	}
}
