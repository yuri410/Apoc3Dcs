using System;
using System.Collections.Generic;
using System.Text;
using Apoc3D.Graphics;
using Apoc3D.Media;

namespace Apoc3D.RenderSystem.D3D.Buffer
{
    internal sealed class D3D9BackBuffer : BackBuffer
    {
        public D3D9BackBuffer(int width, int height, BufferUsage usage, ImagePixelFormat format)
            : base(width, height, usage, format)
        {

        }

        protected override IntPtr @lock(int offset, int size, LockMode mode)
        {
            throw new NotImplementedException();
        }

        protected override void unlock()
        {
            throw new NotImplementedException();
        }

        public override void Dispose(bool disposing)
        {
            throw new NotImplementedException();
        }
    }
}
