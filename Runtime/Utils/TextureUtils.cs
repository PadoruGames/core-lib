using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Debug = Padoru.Diagnostics.Debug;

namespace Padoru.Core.Utils
{
	public static class TextureUtils
	{
		public static Texture2D MakeTexture(int width, int height, Color color)
		{
			Color[] pix = new Color[width * height];
			for (int i = 0; i < pix.Length; i++)
			{
				pix[i] = color;
			}

			Texture2D result = new Texture2D(width, height);
			result.SetPixels(pix);
			result.Apply();

			return result;
		}
	}
}
