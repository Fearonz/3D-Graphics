using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3D_Graphics
{
    class MeshTag
    {
        public Vector3 Color { get; set; }
        public Texture2D Texture { get; set; }
        public float SpecularPower { get; set; }
        public Effect ChachedEffect { get; set; }

        public MeshTag() { }

        public MeshTag(Color color, Texture2D texture, float specularPower)
        {
            Color = color.ToVector3();
            Texture = texture;
        }

    }
}
