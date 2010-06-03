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

namespace Apoc3D.Media
{
    public struct DataRectangle
    {
        int pitch;
        IntPtr pointer;

        int width;
        int height;

        ImagePixelFormat format;

        public static readonly DataRectangle Empty;

        static DataRectangle() 
        {
            Empty = new DataRectangle();
        }

        public DataRectangle(int pitch, IntPtr pointer, int width, int height, ImagePixelFormat fmt)
        {
            this.pitch = pitch;
            this.pointer = pointer;

            this.width = width;
            this.height = height;

            this.format = fmt;
        }

        public ImagePixelFormat Format
        {
            get { return format; }
        }
        public int Pitch
        {
            get { return pitch; }
        }

        public IntPtr Pointer
        {
            get { return pointer; }
        }

        public int Width
        {
            get { return width; }
        }
        public int Height
        {
            get { return height; }
        }

        public int MemorySize
        {
            get
            {
                return PixelFormat.GetMemorySize(width, height, 1, format);
            }
        }

        public bool IsCompressed
        {
            get
            {
                return PixelFormat.Compressed(format);
            }
        }

        ///<summary>
        ///    Return whether this buffer is laid out consecutive in memory (ie the pitches
        ///    are equal to the dimensions)
        ///</summary>
        public bool Consecutive
        {
            get
            {
                return pitch == Width;
            }
        }
    }
    public struct DataBox
    {
        int rowPitch;
        int slicePitch;
        IntPtr pointer;

        int width;
        int height;
        int depth;

        ImagePixelFormat format;

        public static readonly DataBox Empty;

        static DataBox() 
        {
            Empty = new DataBox();
        }

        public DataBox(int width, int height, int depth, IntPtr pointer, ImagePixelFormat fmt)
		{
            this.pointer = pointer;
            this.rowPitch = width;
            this.slicePitch = width * height;
            this.format = fmt;

            this.width = width;
            this.height = height;
            this.depth = depth;
        }
        //public DataBox(int rowPitch, int slicePitch, IntPtr pointer, PixelFormat fmt)
        //{
        //    this.rowPitch = rowPitch;
        //    this.slicePitch = slicePitch;
        //    this.pointer = pointer;
        //    this.format = fmt;
        //}
        public DataBox(int rowPitch, int slicePitch, IntPtr pointer, int width, int height, int depth, ImagePixelFormat fmt)
        {
            this.rowPitch = rowPitch;
            this.slicePitch = slicePitch;
            this.pointer = pointer;

            this.width = width;
            this.height = height;
            this.depth = depth;
            this.format = fmt;
        }


        public ImagePixelFormat Format
        {
            get { return format; }
        }

        public int Width
        {
            get { return width; }
        }

        public int Height
        {
            get { return height; }
        }

        public int Depth
        {
            get { return depth; }
        }

        public bool IsCompressed
        {
            get
            {
                return PixelFormat.Compressed(format);
            }
        }
        ///<summary>
        ///    Return whether this buffer is laid out consecutive in memory (ie the pitches
        ///    are equal to the dimensions)
        ///</summary>
        public bool Consecutive
        {
            get
            {
                return rowPitch == Width && slicePitch == Width * Height;
            }
        }
        /// <summary>
        ///      Gets the number of bytes of data between two consecutive (1D) rows of data.
        /// </summary>
        public int RowPitch
        {
            get { return rowPitch; }
        }
        
        /// <summary>
        /// Gets the number of bytes of data between two consecutive (2D) slices of data.
        /// </summary>
        public int SlicePitch
        {
            get { return slicePitch; }
        }
        public IntPtr Pointer
        {
            get { return pointer; }
        }

        public int MemorySize
        {
            get
            {
                return PixelFormat.GetMemorySize(Width, Height, Depth, format);
            }
        }
    }
    
}
