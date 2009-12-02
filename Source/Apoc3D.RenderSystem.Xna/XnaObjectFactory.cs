using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
            device = rs.device;
            renderSys = rs;
        }

        #region 创建纹理
        public override Texture CreateTexture(ResourceLocation rl, TextureUsage usage)
        {
            return new XnaTexture(renderSys, rl, usage);
        }

        public override Texture CreateTexture(int width, int height, int levelCount, TextureUsage usage, ImagePixelFormat format)
        {
            throw new NotImplementedException();
        }

        public override Texture CreateTexture(int width, int height, int depth, int levelCount, TextureUsage usage, ImagePixelFormat format)
        {
            return new XnaTexture(renderSys, width, height, depth, levelCount, format, usage);
        }

        public override Texture CreateTexture(int length, int levelCount, TextureUsage usage, ImagePixelFormat format)
        {
            throw new NotImplementedException();
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
            throw new NotImplementedException();
        }

        public override VertexShader CreateVertexShader(string code, Macro[] defines, Include include, string profile, string functionName)
        {
            throw new NotImplementedException();
        }

        public override PixelShader CreatePixelShader(string code, Macro[] defines, Include include, string profile, string functionName)
        {
            throw new NotImplementedException();
        }
    }
}