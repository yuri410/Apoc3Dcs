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
using Apoc3D.Media;

namespace Apoc3D.Graphics
{
    /// <summary>
    ///  表示渲染目标
    /// </summary>
    public abstract class RenderTarget : IDisposable
    {
        protected RenderTarget(RenderSystem renderSystem, int width, int height,
            ImagePixelFormat clrBufFormat, DepthFormat depBufFmt)
        {
            Width = width;
            Height = height;
            ColorBufferFormat = clrBufFormat;
            DepthBufferFormat = depBufFmt;
        }
        protected RenderTarget(RenderSystem renderSystem, int width, int height,
                  ImagePixelFormat clrBufFormat)
        {
            Width = width;
            Height = height;
            ColorBufferFormat = clrBufFormat;
            DepthBufferFormat = DepthFormat.Count;
        }

        public abstract Texture GetColorBufferTexture();
        public abstract DepthBuffer GetDepthBufferTexture();       

        public int Width
        {
            get;
            private set;
        }
        public int Height
        {
            get;
            private set;
        }

        public DepthFormat DepthBufferFormat
        {
            get;
            private set;
        }
        public ImagePixelFormat ColorBufferFormat
        {
            get;
            private set;
        }


        #region IDisposable 成员

        protected virtual void Dispose(bool disposing) { }

        public bool Disposed
        {
            get;
            private set;
        }

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
