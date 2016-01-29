using UnityEngine;
using System.Collections.Generic;
using System;

namespace LuminousVector
{
	public class Village : Node
	{
		//Public

		//Private
		public Village(Vector2 position) : base(position)
		{

		}

		protected override void OnInit()
		{
			//Debug.Log("village");
			color = Color.magenta;
			_maxConnections = 4;
			_rangeMultiplier = 0.8f;
			_nodeConntections = new List<Node>(_maxConnections);
		}
	}
}
