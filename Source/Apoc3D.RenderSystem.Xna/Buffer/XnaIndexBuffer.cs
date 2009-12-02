using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Apoc3D.Graphics;
using X = Microsoft.Xna.Framework;
using XG = Microsoft.Xna.Framework.Graphics;

namespace Apoc3D.RenderSystem.Xna
{
    unsafe class XnaIndexBuffer : IndexBuffer
    {
        internal XG.IndexBuffer indexBuffer;
        internal XG.DynamicIndexBuffer dynIb;

        LockMode curLockMode;

        byte[] buffer;
        int lockSize;
        int lockOfs;


        internal XnaIndexBuffer(XnaRenderSystem rs, XG.IndexBuffer ib)
            : base(XnaUtils.ConvertEnum(ib.IndexElementSize),
                   ib.SizeInBytes, XnaUtils.ConvertEnum(ib.BufferUsage, false), false)
        {
            indexBuffer = ib;

            buffer = new byte[Size];
        }

        internal XnaIndexBuffer(XnaRenderSystem rs, XG.DynamicIndexBuffer ib)
            : base(XnaUtils.ConvertEnum(ib.IndexElementSize),
                   ib.SizeInBytes, XnaUtils.ConvertEnum(ib.BufferUsage, true), false)
        {
            dynIb = ib;

            buffer = new byte[Size];
        }

        public XnaIndexBuffer(XnaRenderSystem rs, IndexBufferType type, int size, BufferUsage usage)
            : base(type, size, usage, false)
        {
            if ((usage & BufferUsage.Dynamic) == BufferUsage.Dynamic)
            {
                dynIb = new XG.DynamicIndexBuffer(rs.device, size, XnaUtils.ConvertEnum(usage), XnaUtils.ConvertEnum(type));
            }
            else
            {
                indexBuffer = new XG.IndexBuffer(rs.device, size, XnaUtils.ConvertEnum(usage), XnaUtils.ConvertEnum(type));
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
                    if (indexBuffer != null)
                    {
                        indexBuffer.GetData<byte>(offset, buffer, 0, size);
                    }
                    else
                    {
                        dynIb.GetData<byte>(offset, buffer, 0, size);
                    }
                    break;
                case LockMode.Discard:
                    dynIb.GetData<byte>(offset, buffer, 0, size);
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
                    if (indexBuffer != null)
                    {
                        indexBuffer.SetData<byte>(lockOfs, buffer, 0, lockSize);
                    }
                    else
                    {
                        dynIb.SetData<byte>(lockOfs, buffer, 0, lockSize);
                    }
                    break;
            }
        }

        public override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (indexBuffer != null)
                    indexBuffer.Dispose();
                if (dynIb != null)
                    dynIb.Dispose();
            }
            indexBuffer = null;
            dynIb = null;
        }
    }
}