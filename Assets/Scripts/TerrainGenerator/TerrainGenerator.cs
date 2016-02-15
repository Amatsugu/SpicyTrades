using UnityEngine;
using System.Collections;

namespace LuminousVector
{
	public class TerrainGenerator
	{
		private TerrainGeneratorConfig _config;
		private Mesh mesh;

		public TerrainGenerator(TerrainGeneratorConfig config)
		{
			_config = config;
		}
	
		public void PrepareMesh()
		{
			if (mesh == null)
			{
				mesh = new Mesh();
				mesh.name = "Surface";
			}
			RefreshMesh();
		}

		public void RefreshMesh()
		{

		}

		public void CreateHeightMap(out float[,] outTex)
		{
			float[,] pix = new float[_config.height, _config.width];
			int y = 0;
			while (y < _config.height)
			{
				int x = 0;
				while (x < _config.width)
				{
					//float xCoord = _config.origin.x + x / _config.width * _config.scale.x;
					//float yCoord = _config.origin.y + y / _config.height * _config.scale.y;
					//float sample = Mathf.PerlinNoise(xCoord, yCoord);
					pix[y,x] = Random.Range(0f,1f);
					x++;
				}
				y++;
			}
			outTex = pix;
		}
	}
}
