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

namespace Apoc3D.Graphics
{
    public unsafe abstract class HardwareBuffer : IDisposable
    {
        public BufferUsage Usage
        {
            get;
            private set;
        }

        public bool UseSystemMemory
        {
            get;
            private set;
        }

        public int Size
        {
            get;
            private set;
        }

        public bool IsLocked
        {
            get;
            private set;
        }

        protected int LockOffset
        {
            get;
            private set;
        }
        protected int LockSize
        {
            get;
            private set;
        }

        protected HardwareBuffer(BufferUsage usage, int sizeInBytes, bool useSysMem)
        {
            Usage = usage;
            Size = sizeInBytes;
            UseSystemMemory = useSysMem;
        }

        protected abstract IntPtr @lock(int offset, int size, LockMode mode);

        
        public IntPtr Lock(LockMode mode)
        {
            if (!IsLocked)
            {
                IntPtr res = @lock(0, Size, mode);
                IsLocked = true;
                return res;
            }
            throw new InvalidOperationException();
        }
        public unsafe DataStream LockStream(LockMode mode)
        {
            if (!IsLocked)
            {
                IntPtr res = @lock(0, Size, mode);
                IsLocked = true;
                return new DataStream(res.ToPointer(), Size, mode == LockMode.ReadOnly);
            }
            throw new InvalidOperationException();
        }
        public IntPtr Lock(int offset, int size, LockMode mode)
        {
            if (!IsLocked)
            {
                IntPtr res = @lock(offset, size, mode);
                IsLocked = true;
                return res;
            }
            throw new InvalidOperationException();
        }
        public unsafe DataStream LockStream(int offset, int size, LockMode mode)
        {
            if (!IsLocked)
            {
                IntPtr res = @lock(offset, size, mode);
                IsLocked = true;
                return new DataStream(res.ToPointer(), Size, mode == LockMode.ReadOnly);
            }
            throw new InvalidOperationException();
        }
        public void Unlock()
        {
            if (IsLocked)
            {
                unlock();
                IsLocked = false;
            }
            else
            {
                throw new InvalidOperationException();
            }
        }

        protected abstract void unlock();

        #region IDisposable 成员

        public bool Disposed
        {
            get;
            private set;
        }

        public abstract void Dispose(bool disposing);

        public void Dispose()
        {
            if (!Disposed)
            {
                Dispose(true);
                Disposed = true;
            }
            else
            {
                throw new ObjectDisposedException(ToString());
            }
        }

        #endregion
    }
}
