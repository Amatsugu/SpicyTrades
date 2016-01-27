using UnityEngine;
using UnityEngine.UI;
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
		public Texture2D mapTexture;
		public Texture2D nodeMap;
		public int nodeMapResolution = 8;
		public RawImage renderOutput;
		public bool regenerate = false;

		void Start()
		{
			Generate();
		}

		public void Generate()
		{
			GenerateNodeMap();
			RenderNodeMap();
		}

		void Update()
		{
			if(regenerate)
			{
				Generate();
				regenerate = false;
			}
		}

		void RenderNodeMap()
		{
			nodeMap = new Texture2D(mapWidth * nodeMapResolution, mapHeight * nodeMapResolution, TextureFormat.RGBA32, false);
			for(int y = 0; y < nodeMap.height; y++)
			{
				for(int x = 0; x < nodeMap.width; x++)
				{
					nodeMap.SetPixel(x, y, Color.clear);
				}
			}
			nodeMap.wrapMode = TextureWrapMode.Clamp;
			nodeMap.filterMode = FilterMode.Point;
			foreach(Node n in nodes)
			{
				DrawNode((int)n.position.x, (int)n.position.y, 8, n.color);
			}
			foreach(Node n in nodes)
			{
				foreach (Node n2 in n.getConnections)
				{
					DrawConnection(n.position, n2.position);
				}
			}
			nodeMap.Apply();
			renderOutput.texture = nodeMap;
		}

		void DrawConnection(Vector2 pos1, Vector2 pos2)
		{
			pos1 *= nodeMapResolution;
			//pos1.y = mapHeight * nodeMapResolution - pos1.y;
			pos2 *= nodeMapResolution;
			//pos2.y = mapHeight * nodeMapResolution - pos2.y;
			VoidUtils.DrawLine(nodeMap, (int)pos1.x, (int)pos1.y, (int)pos2.x, (int)pos2.y, Color.red);
		}

		void DrawNode(int x, int y, int size, Color color)
		{
			x *= nodeMapResolution;
			y *= nodeMapResolution;
			//y = nodeMapResolution * mapHeight - y;
			VoidUtils.DrawCircle(nodeMap, x, y, size, color);
		}

		void GenerateNodeMap()
		{
			GenerateNodes();
			ConnectNodes();
			CleanUpExtraNodes();
		}

		

		void GenerateNodes()
		{
			int cycles = 0;
			System.DateTime startTime;
			nodes = new List<Node>(nodesToGenerate);
			//Generate Nodes
			Debug.Log("Generating Nodes");
			startTime = System.DateTime.Now;
			for (int i = 0; i < nodesToGenerate; i++)
			{
				if (cycles >= maxGenerationCycles)
					break;
				bool validNode = true;
				Node node;
				if (Random.Range(0, 4) == 1)
					node = new Town(new Vector2(Random.Range(0, mapWidth), Random.Range(0, mapHeight)));
				else
					node = new Village(new Vector2(Random.Range(0, mapWidth), Random.Range(0, mapHeight)));
				int x, y;
				TransformToMapTexPos(node.position, out x, out y);
				if(mapTexture.GetPixel(x,y) == Color.clear)
				{
					i--;
					continue;
				}
				if (i != 0)
				{
					foreach (Node n in nodes)
					{
						if (Vector2.Distance(n.position, node.position) < minNodeDistance)
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
				cycles++;
			}
			if (cycles >= maxGenerationCycles)
			{
				Debug.Log("Failed to generate nodes in required cycles");
				return;
			}
			else
				Debug.Log("Generated in " + System.DateTime.Now.Subtract(startTime).TotalMilliseconds + "ms with " + cycles + " cycles.");
		}

		void TransformToMapTexPos(Vector2 pos, out int x, out int y)
		{
			pos.x /= mapWidth;
			pos.y /= mapHeight;
			x = (int)(pos.x * mapTexture.width);
			y = (int)(pos.y * mapTexture.height);
		}

		void ConnectNodes()
		{
			//Connect Nodes
			Debug.Log("Connecting Nodes");
			System.DateTime startTime = System.DateTime.Now;
			int passes = 0;
			int failedConnectionAttempts = 0;
			int nodesConnected = 0;
			foreach (Node n in nodes)
			{
				if (GetLowestNodeConnections() >= minNodeConnections)
					break;
				if (failedConnectionAttempts >= connectionAttemptTimeOut)
					break;
				if (n.connectionCount >= n.maxConnections)
					continue;
				List<Node> cNodes = GetClosestNodes(n);
				foreach (Node cN in cNodes)
				{
					if (failedConnectionAttempts >= connectionAttemptTimeOut)
						break;
					if (Vector2.Distance(n.position, cN.position) > maxConnectionDistance)
						continue;
					if (cN.connectionCount == cN.maxConnections)
					{
						failedConnectionAttempts++;
						continue;
					}
					else
					{
						n.AddConnection(cN);
						failedConnectionAttempts = 0;
					}
					passes++;
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
				if (n.isConnected(node))
					continue;
				if (n == node)
					continue;
				float cd = Vector2.Distance(node.position, n.position);
				if(cd < d)
				{
					if (cNodes.Count >= node.maxConnections)
						cNodes.RemoveAt(0);
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
				//if (min == 0)
				//	break;
			}
			return min;
		}

		void CleanUpExtraNodes()
		{
			int discarded = 0;
			List<Node> remvoeList = new List<Node>();
			foreach (Node n in nodes)
			{
				if (n.connectionCount == 0)
				{
					discarded++;
					remvoeList.Add(n);
				}
			}
			foreach (Node n in remvoeList)
			{
				nodes.Remove(n);
			}
			remvoeList.Clear();
			Debug.Log(discarded + " of " + nodesToGenerate + " nodes discarded.");
		}
	}
}
