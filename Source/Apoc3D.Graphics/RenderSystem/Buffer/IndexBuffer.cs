using System;
using System.Collections.Generic;
using System.Text;

namespace Apoc3D.Graphics
{
    /// <summary>
    ///  表示索引缓冲
    /// </summary>
    public abstract class IndexBuffer : HardwareBuffer
    {
        public IndexBufferType BufferType
        {
            get;
            private set;
        }

        public int IndexCount
        {
            get;
            private set;
        }

        public int IndexSize
        {
            get { return BufferType == IndexBufferType.Bit16 ? sizeof(ushort) : sizeof(uint); }
        }


        protected IndexBuffer(IndexBufferType type, int size, BufferUsage usage, bool useSysMem)
            : base(usage, size, useSysMem)
        {
            BufferType = type;

            IndexCount = size / IndexSize;
        }
    }
}
