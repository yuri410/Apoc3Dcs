using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Apoc3D.Graphics;
using X = Microsoft.Xna.Framework;
using XG = Microsoft.Xna.Framework.Graphics;

namespace Apoc3D.RenderSystem.Xna
{
    unsafe class XnaVertexBuffer : VertexBuffer
    {
        internal XG.VertexBuffer vertexBuffer;
        internal XG.DynamicVertexBuffer dynVb;

        LockMode curLockMode;

        byte[] buffer;
        int lockSize;
        int lockOfs;

        internal XnaVertexBuffer(XnaRenderSystem rs, XG.VertexBuffer vb)
            : base(vb.SizeInBytes, XnaUtils.ConvertEnum(vb.BufferUsage,false), false)
        {
            vertexBuffer = vb;

            buffer = new byte[Size];
        }

        internal XnaVertexBuffer(XnaRenderSystem rs, XG.DynamicVertexBuffer vb)
            : base(vb.SizeInBytes, XnaUtils.ConvertEnum(vb.BufferUsage, true), false)
        {
            dynVb = vb;

            buffer = new byte[Size];
        }

        public XnaVertexBuffer(XnaRenderSystem rs, int size, BufferUsage usage) 
            : base(size, usage, false)
        {
            if ((usage & BufferUsage.Dynamic) == BufferUsage.Dynamic)
            {
                dynVb = new XG.DynamicVertexBuffer(rs.device, size, XnaUtils.ConvertEnum(usage));
            }
            else 
            {
                vertexBuffer = new XG.VertexBuffer(rs.device, size, XnaUtils.ConvertEnum(usage));
            }
        }

        protected override IntPtr @lock(int offset, int size, LockMode mode)
        {
            if (size == 0)             
                size = Size;

            lockSize = size;
            curLockMode = mode;
            lockOfs = offset;
            switch (mode) 
            {
                case LockMode.None:
                case LockMode.ReadOnly:
                case LockMode.NoOverwrite:
                    if (vertexBuffer != null)
                    {
                        vertexBuffer.GetData<byte>(offset, buffer, 0, size, 0);
                    }
                    else 
                    {
                        dynVb.GetData<byte>(offset, buffer, 0, size, 0);
                    }
                    break;
                case LockMode.Discard:
                    dynVb.GetData<byte>(offset, buffer, 0, size, 0);
                    break;
            }
            void* result;
            fixed (byte* src = &buffer[0]) 
            {
                result = src;
            }
            return new IntPtr(result);
        }

        protected override void unlock()
        {

            switch (curLockMode) 
            {
                case LockMode.NoOverwrite:
                case LockMode.ReadOnly:
                    // ²»¹Ü
                    break;
                case LockMode.None:
                case LockMode.Discard:
                    if (vertexBuffer != null)
                    {
                        vertexBuffer.SetData<byte>(lockOfs, buffer, 0, lockSize, 0);
                    }
                    else
                    {
                        dynVb.SetData<byte>(lockOfs, buffer, 0, lockSize, 0);
                    }
                    break;
            }
        }

        public override void Dispose(bool disposing)
        {
            if (disposing) 
            {
                if (vertexBuffer != null)
                    vertexBuffer.Dispose();
                if (dynVb != null)
                    dynVb.Dispose();
            }
            vertexBuffer = null;
            dynVb = null;
        }
    }
}