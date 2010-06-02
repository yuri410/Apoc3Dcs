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
using System.ComponentModel;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Text;
using Apoc3D.Design;

namespace Apoc3D.MathLib
{
    [Serializable, StructLayout(LayoutKind.Sequential), TypeConverter(typeof(SizeConverter))]
    public struct Size
    {
        public static readonly Size Empty;
        public int Width;
        public int Height;
        public Size(Point pt)
        {
            this.Width = pt.X;
            this.Height = pt.Y;
        }

        public Size(int width, int height)
        {
            this.Width = width;
            this.Height = height;
        }

        public static Size operator +(Size sz1, Size sz2)
        {
            return Add(sz1, sz2);
        }

        public static Size operator -(Size sz1, Size sz2)
        {
            return Subtract(sz1, sz2);
        }

        public static bool operator ==(Size sz1, Size sz2)
        {
            return ((sz1.Width == sz2.Width) && (sz1.Height == sz2.Height));
        }

        public static bool operator !=(Size sz1, Size sz2)
        {
            return !(sz1 == sz2);
        }

        public static explicit operator Point(Size size)
        {
            return new Point(size.Width, size.Height);
        }

        [Browsable(false)]
        public bool IsEmpty
        {
            get
            {
                return ((this.Width == 0) && (this.Height == 0));
            }
        }
        public static Size Add(Size sz1, Size sz2)
        {
            return new Size(sz1.Width + sz2.Width, sz1.Height + sz2.Height);
        }

        //public static Size Ceiling(SizeF value)
        //{
        //    return new Size((int)Math.Ceiling((double)value.Width), (int)Math.Ceiling((double)value.Height));
        //}

        public static Size Subtract(Size sz1, Size sz2)
        {
            return new Size(sz1.Width - sz2.Width, sz1.Height - sz2.Height);
        }

        //public static Size Truncate(SizeF value)
        //{
        //    return new Size((int)value.Width, (int)value.Height);
        //}

        //public static Size Round(SizeF value)
        //{
        //    return new Size((int)Math.Round((double)value.Width), (int)Math.Round((double)value.Height));
        //}

        public override bool Equals(object obj)
        {
            if (!(obj is Size))
            {
                return false;
            }
            Size size = (Size)obj;
            return ((size.Width == this.Width) && (size.Height == this.Height));
        }

        public override int GetHashCode()
        {
            return (this.Width ^ this.Height);
        }

        public override string ToString()
        {
            return ("{Width=" + this.Width.ToString(CultureInfo.CurrentCulture) + ", Height=" + this.Height.ToString(CultureInfo.CurrentCulture) + "}");
        }

        static Size()
        {
            Empty = new Size();
        }


    }
}
