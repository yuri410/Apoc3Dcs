using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Apoc3D.Graphics;
using Apoc3D.MathLib;
using Apoc3D.Media;
using Apoc3D.Vfs;
using XG = Microsoft.Xna.Framework.Graphics;

namespace Apoc3D.RenderSystem.Xna
{
    class XnaTexture : Texture
    {
        XG.Texture texture;

        public XnaTexture(XnaRenderSystem rs, ResourceLocation rl)
            : base(rs, rl)
        {
            
        }

        public override void Save(Stream stm)
        {
            throw new NotImplementedException();
        }

        protected override DataRectangle @lock(int surface, LockMode mode, Rectangle rect)
        {
            throw new NotImplementedException();
        }

        protected override DataBox @lock(int surface, LockMode mode, Box box)
        {
            throw new NotImplementedException();
        }

        protected override DataRectangle @lock(int surface, CubeMapFace cubemapFace, LockMode mode, Rectangle rect)
        {
            throw new NotImplementedException();
        }

        protected override void unlock(int surface)
        {
            throw new NotImplementedException();
        }

        protected override void unlock(CubeMapFace cubemapFace, int surface)
        {
            throw new NotImplementedException();
        }

        protected override void load()
        {
            throw new NotImplementedException();
        }

        protected override void unload()
        {
            throw new NotImplementedException();
        }
    }
}