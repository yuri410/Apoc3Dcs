using System;
using System.Collections.Generic;
using System.Text;
using Apoc3D.Graphics;
using X = Microsoft.Xna.Framework;
using XG = Microsoft.Xna.Framework.Graphics;

namespace Apoc3D.RenderSystem.Xna
{
    class XnaDepthBuffer : DepthBuffer
    {
        internal XG.DepthStencilBuffer buffer;

        internal XnaDepthBuffer(XnaRenderSystem rs, XG.DepthStencilBuffer buffer)
            : base(buffer.Width, buffer.Height, BufferUsage.Static, XnaUtils.ConvertEnum(buffer.Format))
        {
            this.buffer = buffer;
        }
        
        protected override IntPtr @lock(int offset, int size, LockMode mode)
        {
            throw new NotSupportedException();
        }

        protected override void unlock()
        {
            throw new NotSupportedException();
        }

        public override void Dispose(bool disposing)
        {
            if (disposing)
            {
                buffer.Dispose();
            }
            buffer = null;
        }
    }
}