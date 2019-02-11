using Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Sample;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3D_Graphics
{
    class TextureModel   : CustomEffectModel
    {
        public TextureModel(string asset, Vector3 position) : base(asset, position) { }

        public override void LoadContent()
        {
            CustomEffect = GameUtilities.Content.Load<Effect>("effects/BasicTexture");

            Texture2D tex = GameUtilities.Content.Load<Texture2D>("Textures/isa");
            Texture2D overlay = GameUtilities.Content.Load<Texture2D>("Textures/furtext");
            Material = new TextureMaterial(Color.White, tex, overlay);

            base.LoadContent();
        }
    }
}
