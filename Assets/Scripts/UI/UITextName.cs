using UnityEngine;
using UnityEngine.UI;
namespace LuminousVector
{
	[ExecuteInEditMode]
	[RequireComponent(typeof(Text))]
	public class UITextName : MonoBehaviour
	{
		public Text text;
		void Start()
		{
			text = GetComponent<Text>();
			if (!text)
				enabled = false;
		}

		void Update ()
		{
			text.text = name;
		}
	}
}
