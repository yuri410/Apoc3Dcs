using System;
using System.Collections.Generic;
using System.Text;
using Apoc3D.Graphics;
using Apoc3D.Media;
using X = Microsoft.Xna.Framework;
using XG = Microsoft.Xna.Framework.Graphics;

namespace Apoc3D.RenderSystem.Xna
{
    class XnaRenderTarget : RenderTarget
    {
        internal XG.RenderTarget2D colorBufXna;
        internal XG.DepthStencilBuffer depthBufXna;

        XnaTexture colorBuf;
        XnaDepthBuffer depthBuf;
        XnaRenderSystem renderSys;

        internal XnaRenderTarget(XnaRenderSystem rs, XG.RenderTarget2D XnaRt)
            : base(rs, XnaRt.Width, XnaRt.Height, XnaUtils.ConvertEnum(XnaRt.Format))
        {
            this.colorBufXna = XnaRt;
            this.renderSys = rs;
        }
        internal XnaRenderTarget(XnaRenderSystem rs, XG.RenderTarget2D xnaRt, XG.DepthStencilBuffer xnaDep)
            : base(rs, xnaRt.Width, xnaRt.Height, 
                   XnaUtils.ConvertEnum(xnaRt.Format), XnaUtils.ConvertEnum(xnaDep.Format))
        {
            this.colorBufXna = xnaRt;
            this.depthBufXna = xnaDep;
            this.renderSys = rs;

            this.depthBuf = new XnaDepthBuffer(rs, xnaDep);
        }
        public XnaRenderTarget(XnaRenderSystem rs, int width, int height, ImagePixelFormat format)
            : base(rs, width, height, format)
        {
            this.colorBufXna = new XG.RenderTarget2D(rs.Device, width, height, 1, XnaUtils.ConvertEnum(format));
            this.renderSys = rs;
        }
        public XnaRenderTarget(XnaRenderSystem rs, int width, int height, ImagePixelFormat format, DepthFormat depFmt)
            : base(rs, width, height, format)
        {
            this.colorBufXna = new XG.RenderTarget2D(rs.Device, width, height, 1, XnaUtils.ConvertEnum(format));
            this.depthBufXna = new XG.DepthStencilBuffer(rs.Device, width, height, XnaUtils.ConvertEnum(depFmt));
            this.renderSys = rs;

            this.depthBuf = new XnaDepthBuffer(rs, depthBufXna);
        }

        public override Texture GetColorBufferTexture()
        {
            if (colorBuf == null)
            {
                this.colorBuf = new XnaTexture(renderSys, colorBufXna.GetTexture());
            }
            return colorBuf;
        }

        public override DepthBuffer GetDepthBufferTexture()
        {
            return depthBuf;
        }
    }
}