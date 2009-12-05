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
        public override Texture CreateTexture(ResourceLocation rl, TextureUsage usage)
        {
            return new XnaTexture(renderSys, rl, usage);
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