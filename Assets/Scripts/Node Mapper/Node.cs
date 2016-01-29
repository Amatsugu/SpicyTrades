using UnityEngine;
using System.Collections.Generic;

namespace LuminousVector
{
	public abstract class Node
	{
		//public
		public Vector2 position { get { return _position; } }
		public int connectionCount { get { return _nodeConntections.Count; } }
		public int maxConnections { get { return _maxConnections; } }
		public float connectionRange { get { return _connectionRange * _rangeMultiplier; } }
		public List<Node> getConnections { get { return _nodeConntections; } }
		public Color color = Color.cyan;
		//private
		private Vector2 _position;
		protected List<Node> _nodeConntections;
		protected int _maxConnections;
		protected float _connectionRange;
		protected float _rangeMultiplier = 1;

		public Node Init(int maxConnections, float connectionRange)
		{
			_maxConnections = maxConnections;
			_connectionRange = connectionRange;
			_nodeConntections = new List<Node>(maxConnections);
			OnInit();
			return this;
		}

		protected abstract void OnInit();

		public Node(Vector2 position)
		{
			_position = position;
		}

		//Clear all Connections to this node
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
			if (node.connectionCount == node.maxConnections && recur)
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

		//Check if a given node has a connection to this node
		public bool isConnected(Node n)
		{
			return _nodeConntections.Contains(n);
		}
	}
}
