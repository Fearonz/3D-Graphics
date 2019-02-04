using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3D_Graphics
{   //storing effect parameters
    public class Material
    {
        public virtual void SetEffectParameters(Effect effet) { }
        public virtual void Update() { }
    }
    public class ColorMaterial: Material
    {
        public Color Color { get; set; }
        public Color AltColor { get; set; }
        public bool DrawAlt { get; set; }

        public override void SetEffectParameters(Effect effet)
        {
            effet.Parameters["Colour"].SetValue(Color.ToVector3());
            effet.Parameters["AltColour"].SetValue(AltColor.ToVector3());
            effet.Parameters["DrawAlt"].SetValue(DrawAlt);
            base.SetEffectParameters(effet);
        }

        public ColorMaterial(): base()
        {
            Color = Color.White;
            AltColor = Color.MediumPurple;
            DrawAlt = true;
        }
    }
}
