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
using Apoc3D.Graphics;
using X = Microsoft.Xna.Framework;
using XG = Microsoft.Xna.Framework.Graphics;
using Apoc3D.Core;

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
        }

        internal XnaVertexBuffer(XnaRenderSystem rs, XG.DynamicVertexBuffer vb)
            : base(vb.SizeInBytes, XnaUtils.ConvertEnum(vb.BufferUsage, true), false)
        {
            dynVb = vb;
        }

        public XnaVertexBuffer(XnaRenderSystem rs, int size, BufferUsage usage) 
            : base(size, usage, false)
        {
            if ((usage & BufferUsage.Dynamic) == BufferUsage.Dynamic)
            {
                dynVb = new XG.DynamicVertexBuffer(rs.Device, size, XnaUtils.ConvertEnum(usage));
            }
            else 
            {
                vertexBuffer = new XG.VertexBuffer(rs.Device, size, XnaUtils.ConvertEnum(usage));
            }
        }

        public override void SetData<T>(T[] data)
        {
            if (vertexBuffer != null)
            {
                vertexBuffer.SetData<T>(data);
            }
            else
            {
                dynVb.SetData<T>(data);
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
            }
            fixed (byte* src = &buffer[0]) 
            {
                return new IntPtr(src);
            }
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
            buffer = null;
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