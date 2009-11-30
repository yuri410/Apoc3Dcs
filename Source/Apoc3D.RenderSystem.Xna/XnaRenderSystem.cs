using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Apoc3D.Graphics;
using Apoc3D.Collections;
using Apoc3D.MathLib;
using XG = Microsoft.Xna.Framework.Graphics;

namespace Apoc3D.RenderSystem.Xna
{
    class XnaRenderSystem : Apoc3D.Graphics.RenderSystem
    {
        XG.GraphicsDevice device;

        public override void Init()
        {
            throw new NotImplementedException();
        }

        public override void Clear(ClearFlags flags, int color, float depth, int stencil)
        {
            throw new NotImplementedException();
        }

        public override void SetRenderTarget(int index, RenderTarget rt)
        {
            throw new NotImplementedException();
        }

        public override RenderTarget GetRenderTarget(int index)
        {
            throw new NotImplementedException();
        }

        public override SamplerStateCollection GetSamplerStates()
        {
            throw new NotImplementedException();
        }

        public override void BindShader(VertexShader shader)
        {
            throw new NotImplementedException();
        }

        public override void BindShader(PixelShader shader)
        {
            throw new NotImplementedException();
        }

        public override Viewport Viewport
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }
    }
}