using UnityEngine;
using System.Collections;

namespace LuminousVector
{
	public class TextureCreator : MonoBehaviour
	{
		[Range(2, 512)]
		public int resolution = 256;
		public float frequency = 1f;
		[Range(1, 8)]
		public int octaves = 1;
		[Range(1f, 4f)]
		public float lacunarity = 2f;
		[Range(0f, 1f)]
		public float persistence = .5f;
		[Range(1,3)]
		public int demensions = 3;
		public NoiseMethodType type;
		public Gradient coloring;

		private Texture2D texture;

		private void OnEnable()
		{
			if(texture == null)
			{
				texture = new Texture2D(resolution, resolution, TextureFormat.RGB24, true);
				texture.name = "Procedureal Texture";
				texture.wrapMode = TextureWrapMode.Clamp;
				texture.filterMode = FilterMode.Trilinear;
				texture.anisoLevel = 9;
				GetComponent<MeshRenderer>().material.mainTexture = texture;
			}
			FillTexture();
		}

		private void Update()
		{
			if(transform.hasChanged)
			{
				transform.hasChanged = false;
				FillTexture();
			}
		}

		public void FillTexture()
		{
			if (texture.width != resolution)
				texture.Resize(resolution, resolution);

			Vector3 p00 = transform.TransformPoint(new Vector3(-.5f, -.5f));
			Vector3 p10 = transform.TransformPoint(new Vector3(.5f, -.5f));
			Vector3 p01 = transform.TransformPoint(new Vector3(-.5f, .5f));
			Vector3 p11 = transform.TransformPoint(new Vector3(.5f, .5f));

			NoiseMethod method = Noise.noiseMethods[(int) type][demensions - 1];
			float stepSize = 1f / resolution;
			Random.seed = 42;
			for (int y = 0; y < resolution; y++)
			{
				Vector3 p0 = Vector3.Lerp(p00, p01, (y + .5f) * stepSize);
				Vector3 p1 = Vector3.Lerp(p10, p11, (y + .5f) * stepSize);
				for (int x = 0; x < resolution; x++)
				{
					Vector3 p = Vector3.Lerp(p0, p1, (x + .5f) * stepSize);
					float sample = Noise.Sum(method, p, frequency, octaves, lacunarity, persistence);
					if (type != NoiseMethodType.Value)
						sample = sample * .5f + .5f;
					texture.SetPixel(x, y, coloring.Evaluate(sample));
				}
			}
			texture.Apply();
		}
	}
}
