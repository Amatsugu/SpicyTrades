using UnityEngine;
using System.Collections.Generic;

namespace LuminousVector
{
	[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
	public class SurfaceCreator : MonoBehaviour
	{
		[Range(1, 200)]
		public int resolution = 10;
		[Range(0f, 1f)]
		public float strength = 1;
		public bool coloredStrength;
		public float frequency = 1f;
		public bool damping;
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
		public Vector3 offset;
		public Vector3 rotation;

		private Mesh _mesh;
		private int _currentResolution;
		private Vector3[] _vertices;
		private Vector3[] _normals;
		private Color[] _vertColors;
		private List<string> ls = new List<string>();
		void OnEnable()
		{
			if(_mesh == null)
			{
				_mesh = new Mesh();
				_mesh.name = "Surface";
				GetComponent<MeshFilter>().mesh = _mesh;
			}
			Refresh();
		}

		

		public void Refresh()
		{
			if (resolution != _currentResolution)
				CreateGrid();
			Quaternion q = Quaternion.Euler(rotation);
			Vector3 p00 = q * transform.TransformPoint(new Vector3(-.5f, -.5f)) + offset;
			Vector3 p10 = q * transform.TransformPoint(new Vector3(.5f, -.5f)) + offset;
			Vector3 p01 = q * transform.TransformPoint(new Vector3(-.5f, .5f)) + offset;
			Vector3 p11 = q * transform.TransformPoint(new Vector3(.5f, .5f)) + offset;

			NoiseMethod method = Noise.noiseMethods[(int)type][dimensions - 1];
			float stepSize = 1f / resolution;
			float amplitude = damping ? strength / frequency : strength;
			for(int v = 0, y = 0; y <= resolution; y++)
			{
				Vector3 p0 = Vector3.Lerp(p00, p01, y * stepSize);
				Vector3 p1 = Vector3.Lerp(p10, p11, y * stepSize);
				for(int x = 0; x <= resolution; x++, v ++)
				{
					Vector3 p = Vector3.Lerp(p0, p1, x * stepSize);
					float sample = Noise.Sum(method, p, frequency, octaves, lacunarity, persistence);
					sample = (type == NoiseMethodType.Value) ? (sample - .5f) : (sample * .5f);
					if(coloredStrength)
					{
						_vertColors[v] = coloring.Evaluate(sample + .5f);
						sample *= amplitude;
					}else
					{
						sample *= amplitude;
						_vertColors[v] = coloring.Evaluate(sample + .5f);
					}
					_vertices[v].y = sample;
				}
			}
			_mesh.colors = _vertColors;
			_mesh.vertices = _vertices;
			_mesh.RecalculateNormals();
		}

		private void CreateGrid()
		{
			_currentResolution = resolution;
			_mesh.Clear();
			_vertices = new Vector3[(resolution + 1) * (resolution + 1)];
			Vector2[] uv = new Vector2[_vertices.Length];
			_normals = new Vector3[_vertices.Length];
			_vertColors = new Color[_vertices.Length];
			float stepSize = 1f / resolution;
			for(int v = 0, z = 0; z <= resolution; z++)
			{
				for (int x = 0; x <= resolution; x++, v++)
				{
					_vertices[v] = new Vector3(x * stepSize - .5f, 0f, z * stepSize - .5f);
					uv[v] = new Vector2(x * stepSize, z * stepSize);
					_normals[v] = Vector3.up;
				}
			}
			_mesh.vertices = _vertices;
			_mesh.uv = uv;
			_mesh.normals = _normals;
			_mesh.colors = _vertColors;
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
			_mesh.triangles = triangles;
		}
	}
}
