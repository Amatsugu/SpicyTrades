using UnityEngine;
using System.Collections;

namespace LuminousVector
{
	[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
	public class SurfaceCreator : MonoBehaviour
	{
		[Range(1,200)]
		public int resolution = 10;
		public float frequency = 1f;
		[Range(1, 8)]
		public int octaves = 1;
		[Range(1f, 4f)]
		public float lacunarity = 2f;
		[Range(0f, 1f)]
		public float persistence = 0.5f;
		[Range(1, 3)]
		public int dimensions = 3;
		public NoiseMethodType type;
		public Gradient coloring;

		private Mesh mesh;
		private int currentResolution;
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
			if (resolution != currentResolution)
				CreateGrid();
			Vector3 p00 = transform.TransformPoint(new Vector3(-.5f, -.5f));
			Vector3 p10 = transform.TransformPoint(new Vector3(.5f, -.5f));
			Vector3 p01 = transform.TransformPoint(new Vector3(-.5f, .5f));
			Vector3 p11 = transform.TransformPoint(new Vector3(.5f, .5f));

			//NoiseMethod
		}

		private void CreateGrid()
		{
			currentResolution = resolution;
			mesh.Clear();
			Vector3[] vertices = new Vector3[(resolution + 1) * (resolution + 1)];
			Vector2[] uv = new Vector2[vertices.Length];
			Vector3[] normals = new Vector3[vertices.Length];
			Color[] vertColors = new Color[vertices.Length];
			float stepSize = 1f / resolution;
			for(int v = 0, y = 0; y <= resolution; y++)
			{
				for (int x = 0; x <= resolution; x++, v++)
				{
					vertices[v] = new Vector3(x * stepSize - .5f, y * stepSize - .5f);
					uv[v] = new Vector2(x * stepSize, y * stepSize);
					normals[v] = Vector3.back;
					vertColors[v] = Color.black;
				}
			}
			mesh.vertices = vertices;
			mesh.uv = uv;
			mesh.normals = normals;
			mesh.colors = vertColors;
			int[] triangles = new int[resolution * resolution * 6];
			for(int t = 0, v = 0, y = 0; y < resolution; y++, v++)
			{
				for(int x = 0; x < resolution; x++, v++, t += 6)
				{
					triangles[t] = v;
					triangles[t + 1] = v + resolution + 1;
					triangles[t + 2] = v + 1;
					triangles[t + 3] = v + 1;
					triangles[t + 4] = v + resolution + 1;
					triangles[t + 5] = v + resolution + 2;
				}
			}
			mesh.triangles = triangles;
		}
	}
}
