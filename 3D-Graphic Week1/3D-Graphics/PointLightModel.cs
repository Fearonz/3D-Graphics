using Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Sample;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3D_Graphics
{
    class PointLightModel : CustomEffectModel
    {
        public PointLightModel(string asset, Vector3 position) : base(asset, position) { }

        public override void LoadContent()
        {
            CustomEffect = GameUtilities.Content.Load<Effect>("effects/NormalPointLight");

            Texture2D tex = GameUtilities.Content.Load<Texture2D>("Textures/woodtexture");
            Texture2D overlay = GameUtilities.Content.Load<Texture2D>("Textures/Woodnormal");
            Material = new DirectionalLightMaterial() { Texture = tex, NoramlTexture= overlay};

            base.LoadContent();
        }
        public override void Update()
        {
            float radius = (Material as DirectionalLightMaterial).Attenuation;
            Color color = (Material as DirectionalLightMaterial).LightColor;
            Vector3 pos = (Material as DirectionalLightMaterial).Position;

            if (InputEngine.IsKeyHeld(Keys.Add))
            {
                (Material as DirectionalLightMaterial).Attenuation += 0.5f;
            }
            else if (InputEngine.IsKeyHeld(Keys.Subtract))
            {
                (Material as DirectionalLightMaterial).Attenuation -= 0.5f;
            }

            if (InputEngine.IsKeyHeld(Keys.Left))
            {
                (Material as DirectionalLightMaterial).Position -= new Vector3 (1,0,0);
            }
            else if (InputEngine.IsKeyHeld(Keys.Right))
            {
                (Material as DirectionalLightMaterial).Position += new Vector3(1,0,0);
            }

            base.Update();
        }
    }
}
