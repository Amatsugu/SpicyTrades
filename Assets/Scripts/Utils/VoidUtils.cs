using UnityEngine;
using System.Collections;
namespace LuminousVector
{
	public class VoidUtils
	{
		public static Color[] DrawCircle(Color[] colors, int w, int h, int cx, int cy, int r, Color col)
		{
			int x, y, px, nx, py, ny, d;

			for (x = 0; x <= r; x++)
			{
				d = (int)Mathf.Ceil(Mathf.Sqrt(r * r - x * x));
				for (y = 0; y <= d; y++)
				{
					px = cx + x;
					nx = cx - x;
					py = cy + y;
					ny = cy - y;

					if (py > h || py < 0)
						continue;
					if (ny > h || ny < 0)
						continue;

					if (nx > w || nx < 0)
						continue;
					if (px > w || px < 0)
						continue;
				

					colors[w * py + px] = col;
					colors[w * py + nx] = col;

					
					
					colors[w * ny + px] = col;
					colors[w * ny + nx] = col;

					//tex.SetPixel(px, py, col);
					//tex.SetPixel(nx, py, col);

					//tex.SetPixel(px, ny, col);
					//tex.SetPixel(nx, ny, col);

				}
			}
			return colors;
		}

		public static Color[] DrawLine(Color[] colors, int w, int h, int x0, int y0, int x1, int y1, int thickness, Color col)
		{
			int dy = (int)(y1 - y0);
			int dx = (int)(x1 - x0);
			int stepx, stepy;

			if (dy < 0) { dy = -dy; stepy = -1; }
			else { stepy = 1; }
			if (dx < 0) { dx = -dx; stepx = -1; }
			else { stepx = 1; }
			dy <<= 1;
			dx <<= 1;

			float fraction = 0;
			colors = DrawCircle(colors, w, h, x0, y0, thickness, col);
			//tex.SetPixel(x0, y0, col);
			if (dx > dy)
			{
				fraction = dy - (dx >> 1);
				while (Mathf.Abs(x0 - x1) > 1)
				{
					if (fraction >= 0)
					{
						y0 += stepy;
						fraction -= dx;
					}
					x0 += stepx;
					fraction += dy;
					colors = DrawCircle(colors, w, h, x0, y0, thickness, col);
					//tex.SetPixel(x0, y0, col);
				}
			}
			else {
				fraction = dx - (dy >> 1);
				while (Mathf.Abs(y0 - y1) > 1)
				{
					if (fraction >= 0)
					{
						x0 += stepx;
						fraction -= dy;
					}
					y0 += stepy;
					fraction += dx;
					colors = DrawCircle(colors, w, h, x0, y0, thickness, col);
					//tex.SetPixel(x0, y0, col);
				}
			}
			return colors;
		}

		Texture2D GetHeightMap(Texture2D input)
		{
			Texture2D heightmap = new Texture2D(input.height, input.width, TextureFormat.Alpha8, false);
			heightmap.SetPixels(input.GetPixels());
			return heightmap;
		}
	}
}
