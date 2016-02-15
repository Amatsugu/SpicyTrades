using UnityEngine;
using System.Collections;
namespace LuminousVector
{
	[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
	public class SurfaceCreator : MonoBehaviour
	{
		private Mesh mesh;

		void OnEnable()
		{
			if(mesh == null)
			{
				mesh = new Mesh();
				mesh.name = "Surface";
				GetComponent<MeshFilter>().mesh = mesh;
			}
			Refresh();
		}

		public void Refresh()
		{
			mesh.vertices = new Vector3[]
			{
				new Vector3(0,0),
				new Vector3(1,0),
				new Vector3(0,1)
			};
			mesh.triangles = new int[]
			{
				0,1,2
			};
		}
	}
}
