/*
-----------------------------------------------------------------------------
This source file is part of Apoc3D Engine

Copyright (c) 2009+ Tao Games

This program is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with this program.  if not, write to the Free Software Foundation, 
Inc., 59 Temple Place - Suite 330, Boston, MA 02111-1307, USA, or go to
http://www.gnu.org/copyleft/gpl.txt.

-----------------------------------------------------------------------------
*/
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
        internal XnaRenderTarget(XnaRenderSystem rs, XG.DepthStencilBuffer xnaDep)
            : base(rs, xnaDep.Width, xnaDep.Height,
                   XnaUtils.ConvertEnum(rs.Device.PresentationParameters.BackBufferFormat), XnaUtils.ConvertEnum(xnaDep.Format))
        {
            this.depthBufXna = xnaDep;
            this.renderSys = rs;

            this.depthBuf = new XnaDepthBuffer(rs, xnaDep);
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