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
using System.ComponentModel;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Text;
using Apoc3D.Design;

namespace Apoc3D.MathLib
{
    [Serializable, StructLayout(LayoutKind.Sequential), TypeConverter(typeof(PointConverter))]
    public struct Point : IEquatable<Point>
    {
        public int X;
        public int Y;
        private static Point _zero;
        public static Point Zero
        {
            get
            {
                return _zero;
            }
        }
        public Point(int x, int y)
        {
            this.X = x;
            this.Y = y;
        }

        public bool Equals(Point other)
        {
            return ((this.X == other.X) && (this.Y == other.Y));
        }

        public override bool Equals(object obj)
        {
            bool flag = false;
            if (obj is Point)
            {
                flag = this.Equals((Point)obj);
            }
            return flag;
        }

        public override int GetHashCode()
        {
            return (this.X.GetHashCode() + this.Y.GetHashCode());
        }

        public override string ToString()
        {
            CultureInfo currentCulture = CultureInfo.CurrentCulture;
            return string.Format(currentCulture, "{{X:{0} Y:{1}}}", new object[] { this.X.ToString(currentCulture), this.Y.ToString(currentCulture) });
        }

        public static bool operator ==(Point a, Point b)
        {
            return a.Equals(b);
        }

        public static bool operator !=(Point a, Point b)
        {
            if (a.X == b.X)
            {
                return (a.Y != b.Y);
            }
            return true;
        }

        static Point()
        {
            _zero = new Point();
        }
    }
}
