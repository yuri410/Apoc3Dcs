using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Apoc3D.Collections;
using Apoc3D.Graphics;
using Apoc3D.MathLib;
using X = Microsoft.Xna.Framework;
using XG = Microsoft.Xna.Framework.Graphics;

namespace Apoc3D.RenderSystem.Xna
{
    class XnaRenderSystem : Apoc3D.Graphics.RenderSystem
    {
        internal XG.GraphicsDevice device;

        internal XG.RenderTarget2D defaultRtXna;
        XnaRenderTarget defaultRt;

        XnaRenderTarget[] cachedRenderTargets;

        Capabilities devCaps;

        public XnaRenderSystem(XG.GraphicsDevice device)
            : base(XnaGraphicsAPIFactory.APIName + "RenderSystem")
        {
            this.device = device;
        }

        public override void Init()
        {

            defaultRtXna = (XG.RenderTarget2D)device.GetRenderTarget(0);



            devCaps = new Capabilities();
            

            cachedRenderTargets = new XnaRenderTarget[device.GraphicsDeviceCapabilities.MaxSimultaneousRenderTargets];
            cachedRenderTargets[0] = defaultRt;
        }

        public override void Clear(ClearFlags flags, ColorValue color, float depth, int stencil)
        {
            XG.Color clr = new XG.Color(color.R, color.G, color.B, color.A);
            device.Clear(XnaUtils.ConvertEnum(flags), clr, depth, stencil);
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