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
using Apoc3D.Graphics;
using Apoc3D.Media;
using Apoc3D.Vfs;
using X = Microsoft.Xna.Framework;
using XG = Microsoft.Xna.Framework.Graphics;

namespace Apoc3D.RenderSystem.Xna
{
    class XnaObjectFactory : ObjectFactory
    {
        XG.GraphicsDevice device;
        XnaRenderSystem renderSys;

        public XnaObjectFactory(XnaRenderSystem rs)
            : base(rs)
        {
            this.device = rs.Device;
            this.renderSys = rs;
        }

        #region 创建纹理
        public override Texture CreateTexture(ResourceLocation rl, TextureUsage usage, bool managed)
        {
            return new XnaTexture(renderSys, rl, usage, managed);
        }

        public override Texture CreateTexture(int width, int height, int levelCount, TextureUsage usage, ImagePixelFormat format)
        {
            return new XnaTexture(renderSys, width, height, 1, levelCount, format, usage);
        }

        public override Texture CreateTexture(int width, int height, int depth, int levelCount, TextureUsage usage, ImagePixelFormat format)
        {
            return new XnaTexture(renderSys, width, height, depth, levelCount, format, usage);
        }

        public override Texture CreateTexture(int length, int levelCount, TextureUsage usage, ImagePixelFormat format)
        {
            return new XnaTexture(renderSys, length, levelCount, format, usage);
        }
        #endregion

        #region 渲染目标
        public override RenderTarget CreateRenderTarget(int width, int height, ImagePixelFormat clrFmt, DepthFormat depthFmt)
        {
            return new XnaRenderTarget(renderSys, width, height, clrFmt, depthFmt);
        }

        public override RenderTarget CreateRenderTarget(int width, int height, ImagePixelFormat clrFmt)
        {
            return new XnaRenderTarget(renderSys, width, height, clrFmt);
        }
        #endregion

        public override IndexBuffer CreateIndexBuffer(IndexBufferType type, int count, BufferUsage usage, bool useSysMem)
        {
            return new XnaIndexBuffer(renderSys, type, count * (type == IndexBufferType.Bit16 ? sizeof(ushort) : sizeof(uint)), usage);
        }

        public override VertexBuffer CreateVertexBuffer(int vertexCount, VertexDeclaration vtxDecl, BufferUsage usage, bool useSysMem)
        {
            return new XnaVertexBuffer(renderSys, vertexCount * vtxDecl.GetVertexSize(), usage);
        }

        public override VertexDeclaration CreateVertexDeclaration(VertexElement[] elements)
        {
            return new XnaVertexDeclaration(renderSys, elements);
        }
        public override Sprite CreateSprite()
        {
            return new XnaSprite(renderSys);
        }
        public override StateBlock CreateStateBlock()
        {
            return new XnaStateBlock(renderSys);
        }

        public override VertexShader CreateVertexShader(ResourceLocation resLoc)
        {
            return new XnaVertexShader(renderSys, resLoc);
        }

        public override PixelShader CreatePixelShader(ResourceLocation resLoc)
        {
            return new XnaPixelShader(renderSys, resLoc);
        }
    }
}