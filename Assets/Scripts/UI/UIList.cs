using UnityEngine;
using System.Collections.Generic;
namespace LuminousVector
{
	public enum Direction
	{
		Horizontal,
		Vertial
	}

	[ExecuteInEditMode]
	public class UIList : MonoBehaviour
	{
		//Public
		public List<RectTransform> elements;
		public Direction direction;
		public float spacing;
		public bool alignOnStart;
		public bool invertDirection;
		public bool alignChildren;
		void Start ()
		{
			if(alignOnStart)
				Align();
		}

		void Update()
		{
			if (Application.isPlaying)
				return;
			if (alignChildren)
			{
				elements.Clear();
				RectTransform[] el = GetComponentsInChildren<RectTransform>();
				foreach(RectTransform e in el)
				{
					if (e.parent == transform && e.gameObject.activeInHierarchy)
						elements.Add(e);
				}
			}
			if (elements == null)
				return;
			Align();
		}

		void Align()
		{
			int i = -1;
			float lastPos = 0;
			foreach(RectTransform e in elements)
			{
				Vector2 pos = Vector2.zero;
				if (i == -1)
				{
					e.localPosition = pos;
					i++;
					continue;
				}
				if (direction == Direction.Horizontal)
				{
					float curWidth = elements[i].rect.width * elements[i].localScale.x;
					lastPos += (curWidth + spacing);
					pos.x = (invertDirection) ? -lastPos : lastPos;
				}
				else
				{
					float curHeight = elements[i].rect.height * elements[i].localScale.y;
					lastPos -= (curHeight + spacing);
					pos.y = (invertDirection) ? -lastPos : lastPos;
				}
				e.localPosition = pos;
				i++;
			}
		}
	}
}
