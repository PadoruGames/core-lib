using System.Collections.Generic;
using UnityEngine;
using Debug = Padoru.Diagnostics.Debug;

namespace Padoru.Core
{
	public static class Texture2DExtensions
	{
		public static Sprite ConvertToSprite(this Texture2D texture, bool generateFallbackPhysicsShape = false)
		{
			if (texture == null)
			{
				Debug.LogError("Could not convert null texture into sprite");
				return null;
			}
            
			var pivot = new Vector2(0.5f, 0.5f);

			var sprite = Sprite.Create(
				texture, 
				new Rect(0.0f, 0.0f, texture.width, texture.height), 
				pivot, 
				100.0f,
				0U,
				SpriteMeshType.Tight,
				Vector4.zero,
				generateFallbackPhysicsShape);

			sprite.name = texture.name;

			return sprite;
		}
		
		public static Texture2D RenderWithMaterial(this Texture2D texture, Material material, Dictionary<string, Texture2D> additionalTextures = null, bool forcePreserveAlphaColors = false)
        {
            var canvasRenderTexture = RenderTexture.GetTemporary(
                texture.width,
                texture.height,
                0,
                RenderTextureFormat.Default,
                RenderTextureReadWrite.Linear);
			
            if (additionalTextures != null)
            {
                foreach (var parameter in additionalTextures)
                {
                    if (parameter.Value == null)
                    {
                        continue;
                    }
                    
                    material.SetTexture(parameter.Key, parameter.Value);
                }
            }

            // Draw the main texture to the canvas renderer before actually applying the effect
            // if the color in pixels with alpha don't match the desired output.
            // Note: Is this a hack? Is there a setting somewhere that can achieve this without drawing twice?
            if (forcePreserveAlphaColors)
            {
                Graphics.Blit(texture, canvasRenderTexture);
            }
            
            Graphics.Blit(null, canvasRenderTexture, material, 0);
			
            var previousRenderTexture = RenderTexture.active;
            RenderTexture.active = canvasRenderTexture;
			
            var result = new Texture2D(texture.width, texture.height);
            result.ReadPixels(new Rect(0, 0, canvasRenderTexture.width, canvasRenderTexture.height), 0, 0);
            result.Apply();

            RenderTexture.active = previousRenderTexture;
            RenderTexture.ReleaseTemporary(canvasRenderTexture);

            return result;
        }
        
        /// <summary>
        /// Copy a texture using a RenderTexture.
        /// </summary>
        public static Texture2D Clone(this Texture2D texture)
        {
            var canvasRenderTexture = RenderTexture.GetTemporary(
                texture.width,
                texture.height,
                0,
                RenderTextureFormat.Default,
                RenderTextureReadWrite.Linear);
			
            Graphics.Blit(texture, canvasRenderTexture);
			
            var previousRenderTexture = RenderTexture.active;
            RenderTexture.active = canvasRenderTexture;
			
            var copy = new Texture2D(texture.width, texture.height);
            copy.ReadPixels(new Rect(0, 0, canvasRenderTexture.width, canvasRenderTexture.height), 0, 0);
            copy.Apply();

            RenderTexture.active = previousRenderTexture;
            RenderTexture.ReleaseTemporary(canvasRenderTexture);

            return copy;
        }
	}
}