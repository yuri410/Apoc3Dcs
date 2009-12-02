using System;
using System.Collections.Generic;
using System.Text;

namespace Apoc3D.Graphics
{
    /// <summary>
    ///  表示顶点缓冲
    /// </summary>
    public abstract class VertexBuffer : HardwareBuffer
    {
        protected VertexBuffer(int size, BufferUsage usage, bool useSysMem)
            : base(usage, size, useSysMem)
        {

        }
    }
}
