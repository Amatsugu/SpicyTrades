using UnityEngine;
using System.Collections.Generic;
namespace LuminousVector
{
	public class PlayField : MonoBehaviour
	{
		//Public
		public float fieldScale = 1;
		//Private
		private List<Node> nodes;


		void Start ()
		{
			EventManager.StartListening(GameEvent.NODE_MAP_GENERATED, CreatePlayField);
		}

		void CreatePlayField()
		{
			EventManager.StopListening(GameEvent.NODE_MAP_GENERATED, CreatePlayField);
			nodes = NodeMap.GetNodes();

		}
	}
}
