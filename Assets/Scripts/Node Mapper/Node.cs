using UnityEngine;
using System.Collections.Generic;

namespace LuminousVector
{
	public class Node
	{
		//public
		public Vector2 position { get { return _position; } }
		public int connectionCount { get { return _nodeConntections.Count; } }
		//private
		private Vector2 _position;
		private List<Node> _nodeConntections;
		private int _maxConnections;

		public Node Init(int maxConnections)
		{
			_maxConnections = maxConnections;
			_nodeConntections = new List<Node>(maxConnections);
			return this;
		}

		public Node(Vector2 position)
		{
			_position = position;
		}

		//Clear Connections
		public Node ClearConnections(bool recur)
		{
			if (_nodeConntections.Count == 0)
				return this;
			foreach(Node n in _nodeConntections)
			{
				RemoveConnection(n, true);
			}
			return this;
		}

		//Connect to other nodes
		public Node AddConnection(Node node)
		{
			return AddConnection(node, true);
		}

		public Node AddConnection(Node node, bool recur)
		{
			if (_nodeConntections.Count == _maxConnections)
			{
				Debug.LogWarning("Node: too many node connections");
				return this;
			}
			if (node.connectionCount == _maxConnections && recur)
			{
				Debug.LogWarning("Node: target node has too many connections");
				return this;
			}
			_nodeConntections.Add(node);
			if (recur)
				node.AddConnection(this, false);
			return this;
		}

		//Remove connection between nodes
		public Node RemoveConnection(Node node)
		{
			return RemoveConnection(node, true);
		}

		public Node RemoveConnection(Node node, bool recur)
		{
			if (_nodeConntections.Contains(node))
			{
				Debug.Log("Node: there is no connection to the given node");
				return this;
			}
			_nodeConntections.Remove(node);
			if (recur)
				node.RemoveConnection(this, false);
			return this;
		}
	}
}
