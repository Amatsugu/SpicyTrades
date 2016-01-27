using UnityEngine;
using System.Collections;
namespace LuminousVector
{
	public class Town : Village
	{
		//Public

		//Private
		public Town(Vector2 position): base (position)
		{

		}

		protected override void OnInit()
		{
			//Debug.Log("town");
			color = Color.blue;
		}
	}
}
