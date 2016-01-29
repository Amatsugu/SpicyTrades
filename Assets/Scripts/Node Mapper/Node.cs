using UnityEngine;
using System.Collections.Generic;

namespace LuminousVector
{
	public abstract class Node
	{
		//public
		public Vector2 position { get { return _position; } }
		public int maxConnections { get { return _maxConnections; } }
		public float connectionRange { get { return _connectionRange * _rangeMultiplier; } }
		public Color color = Color.cyan;
		//private
		private Vector2 _position;
		protected int _maxConnections;
		protected float _connectionRange;
		protected float _rangeMultiplier = 1;

		public Node Init(int maxConnections, float connectionRange)
		{
			_maxConnections = maxConnections;
			_connectionRange = connectionRange;
			OnInit();
			return this;
		}

		protected abstract void OnInit();

		public Node(Vector2 position)
		{
			_position = position;
		}
	}
}
