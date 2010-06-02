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
using System.Runtime.InteropServices;

namespace Apoc3D.MathLib
{
    /// <summary>
    /// Defines a volume.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct Box : IEquatable<Box>
    {
        public int Left;
        public int Top;
        public int Right;

        public int Bottom;
        public int Front;
        public int Back;

       
        public Box(int top, int left, int front, int width, int height, int depth)
        {
            this.Top = top;
            this.Left = left;
            this.Front = front;

            this.Bottom = top + height;
            this.Right = left + width;
            this.Back = front + depth;
        }

        public int Width
        {
            get { return Right - Left; }           
        }

        public int Height 
        {
            get { return Bottom - Top; }
        }

        public int Depth
        {
            get { return Back - Front; }
        }


        public static bool operator ==(Box left, Box right)
        {
            return Equals(ref left, ref right);
        }


        public static bool operator !=(Box left, Box right)
        {
            return !Equals(ref left, ref right);
        }

        public override int GetHashCode()
        {
            return Left.GetHashCode() + Top.GetHashCode() + Right.GetHashCode() + 
                Bottom.GetHashCode() + Front.GetHashCode() + Back.GetHashCode();
        }

        public static bool Equals(ref Box value1, ref Box value2)
        {
            if (value1.Left == value2.Left && value1.Top == value2.Top && 
                value1.Right == value2.Right && value1.Bottom == value2.Bottom && 
                value1.Front == value2.Front && value1.Back == value2.Back)
            {
                return true;
            }
            return false;
        }

        public bool Equals(Box other)
        {
            if (this.Left == other.Left && this.Top == other.Top && 
                this.Right == other.Right && this.Bottom == other.Bottom && 
                this.Front == other.Front && this.Back == other.Back)
            {
                return true;
            }
            return false;
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }
            if (obj.GetType() != base.GetType())
            {
                return false;
            }
            return this.Equals((Box)obj);
        }
    }
}
