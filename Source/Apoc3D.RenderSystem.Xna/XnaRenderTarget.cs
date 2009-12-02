using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Apoc3D.Graphics;
using Apoc3D.Media;
using X = Microsoft.Xna.Framework;
using XG = Microsoft.Xna.Framework.Graphics;

namespace Apoc3D.RenderSystem.Xna
{
    class XnaRenderTarget : RenderTarget
    {
        XG.RenderTarget2D colorBufXna;
        XG.DepthStencilBuffer depthBufXna;

        XnaTexture colorBuf;
        XnaDepthBuffer depthBuf;


        internal XnaRenderTarget(XnaRenderSystem rs, XG.RenderTarget2D XnaRt)
            : base(rs, XnaRt.Width, XnaRt.Height, XnaUtils.ConvertEnum(XnaRt.Format))
        {
            this.colorBufXna = XnaRt;

            this.colorBuf = new XnaTexture(rs, XnaRt.GetTexture());
        }
        internal XnaRenderTarget(XnaRenderSystem rs, XG.RenderTarget2D xnaRt, XG.DepthStencilBuffer xnaDep)
            : base(rs, xnaRt.Width, xnaRt.Height, 
                   XnaUtils.ConvertEnum(xnaRt.Format), XnaUtils.ConvertEnum(xnaDep.Format))
        {
            this.colorBufXna = xnaRt;
            this.depthBufXna = xnaDep;

            this.colorBuf = new XnaTexture(rs, xnaRt.GetTexture());
            this.depthBuf = new XnaDepthBuffer(rs, xnaDep);
        }
        public XnaRenderTarget(XnaRenderSystem rs, int width, int height, ImagePixelFormat format)
            : base(rs, width, height, format)
        {
            this.colorBufXna = new XG.RenderTarget2D(rs.device, width, height, 1, XnaUtils.ConvertEnum(format));
            this.colorBuf = new XnaTexture(rs, colorBufXna.GetTexture());
        }
        public XnaRenderTarget(XnaRenderSystem rs, int width, int height, ImagePixelFormat format, DepthFormat depFmt)
            : base(rs, width, height, format)
        {
            this.colorBufXna = new XG.RenderTarget2D(rs.device, width, height, 1, XnaUtils.ConvertEnum(format));
            this.depthBufXna = new XG.DepthStencilBuffer(rs.device, width, height, XnaUtils.ConvertEnum(depFmt));

            this.colorBuf = new XnaTexture(rs, colorBufXna.GetTexture());
            this.depthBuf = new XnaDepthBuffer(rs, depthBufXna);
        }

        public override Texture GetColorBufferTexture()
        {
            return colorBuf;
        }

        public override DepthBuffer GetDepthBufferTexture()
        {
            return depthBuf;
        }
    }
}