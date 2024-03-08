using System.Collections.Generic;
using Padoru.Core;
using UnityEngine;

namespace Pandoru.Core
{
    public class TextureRenderer
    {
        public bool ForcePreserveAlphaColors;
        
        private readonly Material material;
        private readonly Dictionary<string, Texture2D> additionalTextures = new();
        // TODO: Shader parameters other than textures should be able to be handled through this class.
            
        public TextureRenderer(Shader shaderReference)
        {
            material = new Material(shaderReference);
        }
        
        public TextureRenderer(Material materialReference)
        {
            material = materialReference;
        }

        public void SetTexture(string id, Texture2D texture)
        {
            additionalTextures[id] = texture;
        }

        public Texture2D Render(Texture2D texture)
        {
            return texture.RenderWithMaterial(material, additionalTextures, ForcePreserveAlphaColors);
        }
    }
}