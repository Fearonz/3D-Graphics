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
    public class TextureMaterial: Material
    {
        public Color Color { get; set; }
        public Texture2D Texture { get; set; }
        public Texture2D OverlayTexture { get; set; }
        public TextureMaterial(Color color, Texture2D texture, Texture2D overlay): base()
        {
            Color = color;
            Texture = texture;
            OverlayTexture = overlay;
        }
        public override void SetEffectParameters(Effect effet)
        {
            effet.Parameters["Color"].SetValue(Color.ToVector3());
            effet.Parameters["ModelTexture"].SetValue(Texture);
           // effet.Parameters["OverlayTexture"].SetValue(OverlayTexture);
            base.SetEffectParameters(effet);
        }
    }
    public class ColorMaterial: Material
    {
        public Color Color { get; set; }
        public Color AltColor { get; set; }
        public bool DrawAlt { get; set; }

        public override void SetEffectParameters(Effect effet)
        {
            //effet.Parameters["Color"].SetValue(Color.ToVector3());
            //effet.Parameters["ModelTexture"].SetValue(texture);
            //base.SetEffectParameters(effet);
        }

        public ColorMaterial(): base()
        {
            Color = Color.White;
            AltColor = Color.MediumPurple;
            DrawAlt = true;
        }
    }
    public class DirectionalLightMaterial    : Material
    {
        public Color LightColor { get; set; }
        public Color AmbientColor { get; set; }
        public Color DiffuseColor { get; set; } 
        public Vector3 Direction { get; set; }
        public Vector3 Position { get; set; }

        public Texture2D Texture { get; set; }
        public Texture2D NoramlTexture { get; set; }
        public float Attenuation { get; set; }


        public DirectionalLightMaterial()
        {
            LightColor = Color.White;
            DiffuseColor = Color.White;
            AmbientColor = Color.Black;
            Position = new Vector3(0, 5, 0);
            Attenuation = 60;
        }

        public override void SetEffectParameters(Effect effect)
        {
            effect.Parameters["LightColor"].SetValue(LightColor.ToVector3());
            effect.Parameters["AmbientColor"].SetValue(AmbientColor.ToVector3());
            effect.Parameters["DiffuseColor"].SetValue(DiffuseColor.ToVector3());
            effect.Parameters["Position"].SetValue(Position);
            effect.Parameters["ModelTexture"].SetValue(Texture);
            effect.Parameters["Attenuation"].SetValue(Attenuation);
            effect.Parameters["NormalTexture"].SetValue(NoramlTexture);


            base.SetEffectParameters(effect);
        }
    }
}
