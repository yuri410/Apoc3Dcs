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
    ///  表示纹理中的Surface，在这里通常是RenderTarget
    /// </summary>
    public abstract class DepthBuffer : HardwareBuffer
    {
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

        public DepthFormat DepthFormat
        {
            get;
            private set;
        }

        protected DepthBuffer(int width, int height, BufferUsage usage, DepthFormat format)
            : base(usage, PixelFormat.GetMemorySize(width, height, format), false)
        {
            Width = width;
            Height = height;

            DepthFormat = format;
        }

    }
}
 