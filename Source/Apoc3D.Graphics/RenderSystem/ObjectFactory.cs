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
http://www.gnu.org/copyleft/lesser.txt.

-----------------------------------------------------------------------------
*/
using System;
using System.Collections.Generic;
using System.Text;
using Apoc3D.Media;
using Apoc3D.Vfs;

namespace Apoc3D.Graphics
{
    /// <summary>
    ///  创建图形API资源的抽象工厂。可以创建纹理，Shader，缓冲等等。
    /// </summary>
    public abstract class ObjectFactory
    {
        public RenderSystem RenderSystem
        {
            get;
            private set;
        }

        protected ObjectFactory(RenderSystem rs)
        {
            RenderSystem = rs;
        }


        public abstract Texture CreateTexture(ResourceLocation rl, TextureUsage usage, bool managed);

        public abstract Texture CreateTexture(int width, int height, int levelCount, TextureUsage usage, ImagePixelFormat format);

        //public abstract Texture CreateTexture(Surface[] surfaces);

        /// <summary>
        ///  创建一个纹理，如果depth>1，则创建体积纹理
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="depth"></param>
        /// <param name="levelCount"></param>
        /// <param name="usage"></param>
        /// <param name="format"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public abstract Texture CreateTexture(int width, int height, int depth, int levelCount, TextureUsage usage, ImagePixelFormat format);
        public abstract Texture CreateTexture(int length, int levelCount, TextureUsage usage, ImagePixelFormat format);


        public abstract RenderTarget CreateRenderTarget(int width, int height, ImagePixelFormat clrFmt, DepthFormat depthFmt);
        public abstract RenderTarget CreateRenderTarget(int width, int height, ImagePixelFormat clrFmt);

        public abstract Sprite CreateSprite();

        public abstract IndexBuffer CreateIndexBuffer(IndexBufferType type, int count, BufferUsage usage, bool useSysMem);
        public abstract VertexBuffer CreateVertexBuffer(int vertexCount, VertexDeclaration vtxDecl, BufferUsage usage, bool useSysMem);

        public VertexBuffer CreateVertexBuffer(int vertexCount, VertexDeclaration vtxDecl, BufferUsage usage)
        {
            return CreateVertexBuffer(vertexCount, vtxDecl, usage, false);
        }
        public IndexBuffer CreateIndexBuffer(IndexBufferType type, int count, BufferUsage usage)
        {
            return CreateIndexBuffer(type, count, usage, false);
        }

        public abstract VertexDeclaration CreateVertexDeclaration(VertexElement[] elements);

        public abstract StateBlock CreateStateBlock();

        public abstract VertexShader CreateVertexShader(ResourceLocation resLoc);
        public abstract PixelShader CreatePixelShader(ResourceLocation resLoc);

    }
}
