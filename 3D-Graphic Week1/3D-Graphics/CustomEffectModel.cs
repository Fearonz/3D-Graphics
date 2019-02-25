using _3D_Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Sample;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graphics
{
    public class CustomEffectModel : SimpleModel
    {
        public Effect CustomEffect { get; set; }
        public Material Material { get; set; }
        public bool IstandardEffect { get; set; }

        public CustomEffectModel(string asset, Vector3 position)
        : base("", asset, position)
        {
            IstandardEffect = true;
        }

        public override void LoadContent()
        {
            base.LoadContent();

            if(CustomEffect != null && Model != null)
            {
                GenerateMeshTag();
                foreach (var mesh in Model.Meshes)
                    foreach (var part in mesh.MeshParts)
                        part.Effect = CustomEffect;
            }
        }

        public override void Draw(Camera camera)
        {

            if (CustomEffect != null)
            {

                SetModelEffect(CustomEffect, true);

                foreach (var mesh in Model.Meshes)
                {


                    foreach (var part in mesh.MeshParts)
                    {
                        part.Effect.Parameters["World"].SetValue(BoneTransforms[mesh.ParentBone.Index] * World);
                        part.Effect.Parameters["View"].SetValue(camera.View);
                        part.Effect.Parameters["Projetion"].SetValue(camera.Projection);

                        if (Material != null && IstandardEffect)
                            Material.SetEffectParameters(part.Effect);
                    }
                    mesh.Draw();
                }
            }
        }

        private void GenerateMeshTag()
        {
            foreach (ModelMesh mesh in Model.Meshes)
            {
                foreach (ModelMeshPart part in mesh.MeshParts)
                {
                    MeshTag tag = new MeshTag();

                    tag.Color = (part.Effect as BasicEffect).DiffuseColor;
                    tag.Texture = (part.Effect as BasicEffect).Texture;

                    part.Tag = tag;
                }
            }
        }

        public void CacheEffect()
        {
            foreach (ModelMesh mesh in Model.Meshes)
            {
                foreach (ModelMeshPart part in mesh.MeshParts)
                {
                    (part.Tag as MeshTag).ChachedEffect = part.Effect;
                }
            }
        }

        public void RestoreEffct()
        {
            foreach (ModelMesh mesh in Model.Meshes)
            {
                foreach (ModelMeshPart part in mesh.MeshParts)
                {
                    if (part.Tag != null)
                    {
                        if ((part.Tag as MeshTag).ChachedEffect != null)
                        {
                            part.Effect = (part.Tag as MeshTag).ChachedEffect;
                        }
                    }
                }
            }
        }

        public virtual void SetEffectParameter(Effect effect, string paramName, object value)
        {
            if (effect.Parameters[paramName] == null)
            {
                return;
            }

            if (value is Vector3)
            {
                effect.Parameters[paramName].SetValue((Vector3)value);
            }

            else if (value is Matrix)
            {
                   effect.Parameters[paramName].SetValue((Matrix)value);
            }

            else if (value is bool)
            {
                effect.Parameters[paramName].SetValue((bool)value);
            }

            else if (value is Texture2D)
            {
                effect.Parameters[paramName].SetValue((Texture2D)value);
            }

            else if (value is float)
            {
                effect.Parameters[paramName].SetValue((float)value);
            }

            else if (value is int)
            {
                effect.Parameters[paramName].SetValue((int)value);
            }

        }

        public virtual void SetModelEffect(Effect effect, bool copyEffect)
        {
            CacheEffect();

            foreach  (ModelMesh mesh in Model.Meshes)
            {
                foreach (ModelMeshPart part in mesh.MeshParts)
                {
                    Effect toBeSet = effect;

                    if (copyEffect)
                    {
                        toBeSet = effect.Clone();
                    }

                    var tag = (part.Tag as MeshTag);

                    if (tag.Texture != null)
                    {
                        SetEffectParameter(toBeSet, "Texture", tag.Texture);
                        SetEffectParameter(toBeSet, "TextureEnble", true);
                    }
                    else
                    {

                        SetEffectParameter(toBeSet, "TextureEnable", false);
                    }
                    SetEffectParameter(toBeSet, "DiffuseColor", tag.Color);
                    SetEffectParameter(toBeSet, "SpecularPower", tag.SpecularPower);

                    part.Effect = toBeSet;
                }
            }
        }

      
    }
}
