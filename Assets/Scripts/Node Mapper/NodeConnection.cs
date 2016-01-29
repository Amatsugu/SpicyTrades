using UnityEngine;
using System.Collections;
namespace LuminousVector
{
	public class NodeConnection
	{
		//Public
		public Node n1 { get { return _n1; } }
		public Node n2 { get { return _n2; } }
		//Private
		private Node _n1;
		private Node _n2;

		public NodeConnection(Node n1, Node n2)
		{
			_n1 = n1;
			_n2 = n2;
		}

		// does a given node exsist in this connection
		public bool isConnected(Node n)
		{
			return (n1 == n) || (n2 == n);
		}

		// override object.Equals
		public override bool Equals(object obj)
		{
			if (obj == null || GetType() != obj.GetType())
			{
				return false;
			}
			// TODO: write your implementation of Equals() here
			NodeConnection connection = (NodeConnection)obj;
			return (connection.n1 == n1 && connection.n2 == n2) || (connection.n1 == n2 && connection.n2 == n2);
		}

		// override object.GetHashCode
		public override int GetHashCode()
		{
			int hash = 13;
			hash = (hash * 7) + n1.GetHashCode();
			hash = (hash * 7) + n2.GetHashCode();
			return hash;
		}
	}
}
