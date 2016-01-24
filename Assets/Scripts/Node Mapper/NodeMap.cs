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
			float startTime = 0;
			nodes = new List<Node>(nodesToGenerate);
			//Generate Nodes
			Debug.Log("Generating Nodes");
			startTime = Time.time;
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
				Debug.Log("Generated in " + (Time.time - startTime) + "s with " + passes + " cycles.");
			//Connect Nodes
			Debug.Log("Connecting Nodes");
			startTime = Time.time;
			passes = 0;
			int failedConnectionAttempts = 0;
			while(GetLowestNodeConnections() < minNodeConnections)
			{
				if (failedConnectionAttempts >= connectionAttemptTimeOut)
					break;
				passes++;
				Node a = nodes[Random.Range(0, nodesToGenerate - 1)];
				Node b = nodes[Random.Range(0, nodesToGenerate - 1)];
				if (a == b)
					continue;
				if (Vector2.Distance(a.position, b.position) > maxConnectionDistance)
				{
					//Debug.Log("Loosening distance constraint");
					maxConnectionDistance += 0.01f;
					continue;
				}
				if (a.connectionCount == maxNodeConnections || b.connectionCount == maxNodeConnections)
				{
					failedConnectionAttempts++;
					continue;
				}
				a.AddConnection(b);
				failedConnectionAttempts = 0;
			}
			if (failedConnectionAttempts >= connectionAttemptTimeOut)
				Debug.Log("Failed to connect nodes in required passes.");
			else
				Debug.Log("Connected all nodes in " + (Time.time - startTime) + "s with " + passes + " passes.");
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
