using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

namespace LuminousVector
{
	public class NodeMap : MonoBehaviour
	{
		//Public
		public int nodesToGenerate = 50;
		public int maxGenerationCycles = 1000;
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

		//Private
		private List<Node> _nodes;
		private List<NodeConnection> _nodeConnections;

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
			System.DateTime startTime = System.DateTime.Now;
			Debug.Log("Rendering map");
			//Creat the nodeMap Texture
			nodeMap = new Texture2D(mapWidth * nodeMapResolution, mapHeight * nodeMapResolution, TextureFormat.RGBA32, false);
			//Clear the texture
			for(int y = 0; y < nodeMap.height; y++)
			{
				for(int x = 0; x < nodeMap.width; x++)
				{
					nodeMap.SetPixel(x, y, Color.clear);
				}
			}
			nodeMap.wrapMode = TextureWrapMode.Clamp;
			nodeMap.filterMode = FilterMode.Point;
			//Draw all nodes
			foreach (Node n in _nodes)
			{
				DrawNode((int)n.position.x, (int)n.position.y, 6, n.color);
			}
			//Draw all connections
			foreach(Node n in _nodes)
			{
				foreach (NodeConnection nc in _nodeConnections)
				{
					DrawConnection(nc.n1.position, nc.n2.position);
				}
			}
			nodeMap.Apply();
			renderOutput.texture = nodeMap;
			Debug.Log("Finished rendering in " + System.DateTime.Now.Subtract(startTime).TotalMilliseconds + "ms");
		}

		void DrawConnection(Vector2 pos1, Vector2 pos2)
		{
			pos1 *= nodeMapResolution;
			//pos1.y = mapHeight * nodeMapResolution - pos1.y;
			pos2 *= nodeMapResolution;
			//pos2.y = mapHeight * nodeMapResolution - pos2.y;
			VoidUtils.DrawLine(nodeMap, (int)pos1.x, (int)pos1.y, (int)pos2.x, (int)pos2.y, 1, Color.red);
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
			int generated = GenerateNodes();
			ConnectNodes();
			CleanUpExtraNodes(generated);
		}

		

		int GenerateNodes()
		{
			int cycles = 0;
			int nodesGenerated = 0;
			System.DateTime startTime;
			_nodes = new List<Node>(nodesToGenerate);
			//Generate Nodes
			Debug.Log("Generating Nodes");
			startTime = System.DateTime.Now;
			for (int i = 0; i < nodesToGenerate; i++)
			{
				if (cycles >= maxGenerationCycles)
					break;
				bool validNode = true;
				Node node;
				//Create a node
				if (Random.Range(0, 4) == 1)
					node = new Town(new Vector2(Random.Range(0, mapWidth), Random.Range(0, mapHeight)));
				else
					node = new Village(new Vector2(Random.Range(0, mapWidth), Random.Range(0, mapHeight)));
				//Constrain node to the mapTexture
				int x, y;
				TransformToMapTexPos(node.position, out x, out y);
				if(mapTexture.GetPixel(x,y) == Color.clear)
				{
					i--; //Go back a step and skip next tests
					cycles++;
					continue;
				}
				//Make sure the new node isn't too close to another ndoe
				if (i != 0)
				{
					foreach (Node n in _nodes)
					{
						if (Vector2.Distance(n.position, node.position) < minNodeDistance)
						{
							validNode = false;
							break;
						}
					}
				}
				if (validNode)
				{
					_nodes.Add(node.Init(maxNodeConnections, maxConnectionDistance));
					nodesGenerated++;
				}
				else
					i--; //Go back a step incase of invalid node
				cycles++;
			}
			//Determine the outcome of node generation
			if (cycles >= maxGenerationCycles)
			{
				Debug.LogWarning("Failed to generate nodes in required cycles. " + nodesGenerated + " nodes generated.");
				return nodesGenerated;
			}
			else
				Debug.Log("Generated " + nodesGenerated + " nodes in " + System.DateTime.Now.Subtract(startTime).TotalMilliseconds + "ms with " + cycles + " cycles.");
			return nodesGenerated;
		}

		//Transform node coordinate to maptexture coordinate (lossy)
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
			_nodeConnections = new List<NodeConnection>();
			Debug.Log("Connecting Nodes");
			System.DateTime startTime = System.DateTime.Now;
			int passes = 0;
			int failedConnectionAttempts = 0;
			int nodesConnected = 0;
			foreach (Node n in _nodes)
			{
				//Stop connectiing if there are no nodes with 0 connections
				if (GetLowestNodeConnections() >= minNodeConnections)
					break;
				//Stop connecting if too many attempts were made
				if (failedConnectionAttempts >= connectionAttemptTimeOut)
					break;
				//Stop if the current node already has the maximum number of connections
				if (GetConnectionCount(n) >= n.maxConnections)
					continue;
				//Find the closest nodes to the current node
				List<Node> cNodes = GetClosestNodes(n);
				foreach (Node cN in cNodes)
				{

					//Stop connecting if too many attempts were made
					if (failedConnectionAttempts >= connectionAttemptTimeOut)
						break;
					//Check if the candidate nodes are within range
					float d = Vector2.Distance(n.position, cN.position);
					if (d > n.connectionRange && d > cN.connectionRange)
						continue;
					//Stop connecting once current node has reached max connections
					if (GetConnectionCount(n) == n.maxConnections)
						break;
					//Skip this candidate node if it already has max connections
					if (GetConnectionCount(cN) == cN.maxConnections)
					{
						failedConnectionAttempts++;
						continue;
					}
					else //Connect the nodes and reset the attempts counter
					{
						_nodeConnections.Add(new NodeConnection(n, cN));
						failedConnectionAttempts = 0;
					}
					passes++;
				}

				if (failedConnectionAttempts == 0)
					nodesConnected++;
				failedConnectionAttempts = 0;

			}
			//Determine the results of the node connection process
			if (failedConnectionAttempts >= connectionAttemptTimeOut)
				Debug.LogError("Failed to connect nodes in required passes. " + nodesConnected + " nodes connected.");
			else
				Debug.Log("Connected all nodes in " + System.DateTime.Now.Subtract(startTime).TotalMilliseconds + "ms with " + passes + " passes.");
		}

		//Find the closest nodes to a given node
		List<Node> GetClosestNodes(Node node)
		{
			List<Node> cNodes = new List<Node>();
			float d = float.PositiveInfinity;
			foreach(Node n in _nodes)
			{
				if (NodeisConnected(node, n))
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

		//Check if a given node has a connection to this node
		public bool NodeisConnected(Node n1, Node n2)
		{
			NodeConnection nc = new NodeConnection(n1, n2);
			return _nodeConnections.Contains(nc);
		}

		//Get the number of connections that involve a node
		public int GetConnectionCount(Node n)
		{
			int c = 0;
			foreach(NodeConnection nc in _nodeConnections)
			{
				if (nc.isConnected(n))
					c++;
			}
			return c;
		}

		//Get the count of the node with the lowest number of connections
		int GetLowestNodeConnections()
		{
			int min = 0;
			foreach(Node n in _nodes)
			{
				min = (GetConnectionCount(n) < min) ? GetConnectionCount(n) : min;
				//if (min == 0)
				//	break;
			}
			return min;
		}

		//Discard nodes that were not connected to another node
		void CleanUpExtraNodes(int generated)
		{
			int discarded = 0;
			List<Node> remvoeList = new List<Node>();
			foreach (Node n in _nodes)
			{
				if (GetConnectionCount(n) == 0)
				{
					discarded++;
					remvoeList.Add(n);
				}
			}
			foreach (Node n in remvoeList)
			{
				_nodes.Remove(n);
			}
			remvoeList.Clear();
			Debug.Log(discarded + " of " + generated + " nodes discarded.");
		}
	}
}
