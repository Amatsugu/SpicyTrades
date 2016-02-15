using UnityEngine;
using UnityEngine.UI;
using System.Collections;
namespace LuminousVector
{
	public class NodeMapUI : MonoBehaviour
	{
		//Public
		public InputField nodesToGenerate;
		public InputField maxGenerationCycles;
		public InputField mapHeight;
		public InputField mapWidth;
		public InputField minNodeDistance;
		public InputField maxConnectionDistance;
		public InputField minNodeConnections;
		public InputField maxNodeConnections;
		public InputField connectionAttemptTimeOut;
		public bool renderNodeMap
		{
			get
			{
				return _renderNodeMap;
			}
			set
			{
				_renderNodeMap = value;
				if (map == null)
					Start();
				map.renderNodeMap = value;
				nodeMapResolution.interactable = value;
			}
		}
		public InputField nodeMapResolution;

		private NodeMap map;
		private bool _renderNodeMap;

		void Start()
		{
			map = FindObjectOfType<NodeMap>();
			nodesToGenerate.text = map.nodesToGenerate.ToString();
			maxGenerationCycles.text = map.maxGenerationCycles.ToString();
			mapHeight.text = map.mapHeight.ToString();
			mapWidth.text = map.mapWidth.ToString();
			minNodeDistance.text = map.minNodeDistance.ToString();
			maxConnectionDistance.text = map.maxConnectionDistance.ToString();
			minNodeConnections.text = map.minNodeConnections.ToString();
			maxNodeConnections.text = map.maxNodeConnections.ToString();
			connectionAttemptTimeOut.text = map.connectionAttemptTimeOut.ToString();
			nodeMapResolution.text = map.nodeMapResolution.ToString();
		}

		public void Generate()
		{
			map.nodesToGenerate = int.Parse(nodesToGenerate.text);
			map.maxGenerationCycles = int.Parse(maxGenerationCycles.text);
			map.mapHeight = int.Parse(mapHeight.text);
			map.mapWidth = int.Parse(mapWidth.text);
			map.minNodeDistance = float.Parse(minNodeDistance.text);
			map.maxConnectionDistance = float.Parse(maxConnectionDistance.text);
			map.minNodeConnections = int.Parse(minNodeConnections.text);
			map.maxNodeConnections = int.Parse(maxNodeConnections.text);
			map.connectionAttemptTimeOut = int.Parse(connectionAttemptTimeOut.text);
			map.nodeMapResolution = int.Parse(nodeMapResolution.text);
			map.Generate();
		}
	}
}
