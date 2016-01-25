using UnityEngine;
using System.Collections.Generic;

namespace LuminousVector
{
	public class NodeMap : MonoBehaviour
	{
		public int nodesToGenerate = 50;
		public int maxGenerationCycles = 1000;
		public List<Node> nodes;
		public int mapHeight = 100;
		public int mapWidth = 100;
		public float minNodeDistance = 3;
		public float maxConnectionDistance = 5;
		public int minNodeConnections = 1;
		public int maxNodeConnections = 3;
		public int connectionAttemptTimeOut = 20;

		void Start()
		{
			int passes = 0;
			System.DateTime startTime;
			nodes = new List<Node>(nodesToGenerate);
			//Generate Nodes
			Debug.Log("Generating Nodes");
			startTime = System.DateTime.Now;
			for (int i = 0; i < nodesToGenerate; i++)
			{
				if (passes >= maxGenerationCycles)
					break;
				bool validNode = true;
				Node node = new Node(new Vector2(Random.Range(0, mapWidth), Random.Range(0, mapHeight)));
				if (i != 0)
				{
					foreach (Node n in nodes)
					{
						if (Vector2.Distance(n.position, node.position) < 3)
						{
							validNode = false;
							break;
						}
					}
				}
				if (validNode)
					nodes.Add(node.Init(maxNodeConnections));
				else
					i--;
				passes++;
			}
			if (passes >= maxGenerationCycles)
			{
				Debug.Log("Failed to generate nodes in required cycles");
				return;
			}
			else
				Debug.Log("Generated in " + System.DateTime.Now.Subtract(startTime).TotalMilliseconds + "ms with " + passes + " cycles.");
			//Connect Nodes
			Debug.Log("Connecting Nodes");
			startTime = System.DateTime.Now;
			passes = 0;
			int failedConnectionAttempts = 0;
			int nodesConnected = 0;
			foreach(Node n in nodes)
			{
				if (GetLowestNodeConnections() >= minNodeConnections)
					break;
				if (failedConnectionAttempts >= connectionAttemptTimeOut)
					break;
				if (n.connectionCount >= maxNodeConnections)
					continue;
				passes++;
				List<Node> cNodes = GetClosestNodes(n);
				foreach(Node cN in cNodes)
				{
					if (failedConnectionAttempts >= connectionAttemptTimeOut)
						break;
					if (cN.connectionCount == maxNodeConnections)
					{
						failedConnectionAttempts++;
						continue;
					}else
					{
						n.AddConnection(cN);
						failedConnectionAttempts = 0;
					}
				}
				if (failedConnectionAttempts == 0)
					nodesConnected++;
				failedConnectionAttempts = 0;
				
			}
			if (failedConnectionAttempts >= connectionAttemptTimeOut)
				Debug.Log("Failed to connect nodes in required passes. " + nodesConnected + " nodes connected.");
			else
				Debug.Log("Connected all nodes in " + System.DateTime.Now.Subtract(startTime).TotalMilliseconds + "ms with " + passes + " passes.");
		}

		List<Node> GetClosestNodes(Node node)
		{
			List<Node> cNodes = new List<Node>();
			float d = float.PositiveInfinity;
			foreach(Node n in nodes)
			{
				if (cNodes.Count >= maxNodeConnections)
					break;
				if (n == node)
					continue;
				float cd = Vector2.Distance(node.position, n.position);
				if (cd < d)
				{
					d = cd;
					cNodes.Add(n);
				}
			}
			return cNodes;
		}

		int GetLowestNodeConnections()
		{
			int min = 0;
			foreach(Node n in nodes)
			{
				min = (n.connectionCount < min) ? n.connectionCount : min;
				if (min == 0)
					break;
			}
			return min;
		}
	}
}
