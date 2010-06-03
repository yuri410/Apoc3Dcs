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
        }

        internal XnaIndexBuffer(XnaRenderSystem rs, XG.DynamicIndexBuffer ib)
            : base(XnaUtils.ConvertEnum(ib.IndexElementSize),
                   ib.SizeInBytes, XnaUtils.ConvertEnum(ib.BufferUsage, true), false)
        {
            dynIb = ib;
        }

        public XnaIndexBuffer(XnaRenderSystem rs, IndexBufferType type, int size, BufferUsage usage)
            : base(type, size, usage, false)
        {
            if ((usage & BufferUsage.Dynamic) == BufferUsage.Dynamic)
            {
                dynIb = new XG.DynamicIndexBuffer(rs.Device, size, XnaUtils.ConvertEnum(usage), XnaUtils.ConvertEnum(type));
            }
            else
            {
                indexBuffer = new XG.IndexBuffer(rs.Device, size, XnaUtils.ConvertEnum(usage), XnaUtils.ConvertEnum(type));
            }
        }
        public override void SetData<T>(T[] data)
        {
            if (indexBuffer != null)
            {
                indexBuffer.SetData<T>(data);
            }
            else
            {
                dynIb.SetData<T>(data);
            }
        }
        protected override IntPtr @lock(int offset, int size, LockMode mode)
        {
            if (size == 0)
                size = Size;

            buffer = new byte[size];
            lockSize = size;
            curLockMode = mode;
            lockOfs = offset;
            if (Usage != BufferUsage.WriteOnly)
            {
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
                case LockMode.ReadOnly:
                    // ²»¹Ü
                    break;
                case LockMode.NoOverwrite:
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
            buffer = null;
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