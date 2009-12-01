using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Apoc3D.Graphics;
using Apoc3D.Media;

namespace Apoc3D.RenderSystem.Xna
{
    class XnaRenderTarget : RenderTarget
    {
        public XnaRenderTarget(XnaRenderSystem rs, int width, int height, ImagePixelFormat format)
            : base(rs, width, height, format)
        {

        }

        public override Texture GetColorBufferTexture()
        {
            throw new NotImplementedException();
        }

        public override Texture GetDepthBufferTexture()
        {
            throw new NotImplementedException();
        }
    }
}