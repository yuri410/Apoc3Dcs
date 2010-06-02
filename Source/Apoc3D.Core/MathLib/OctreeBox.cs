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

namespace Apoc3D.MathLib
{
    public struct OctreeBox
    {
        public Vector3 Center;

        public float Length;

        public OctreeBox(float length)
        {
            this.Length = length;
            this.Center.X = 0;
            this.Center.Y = 0;
            this.Center.Z = 0;
        }
        //public OctreeBox(float length, float posY)
        //{
        //    this.Length = length;
        //    this.Center.X = length * 0.5f;
        //    this.Center.Y = posY;
        //    this.Center.Z = this.Center.X;
        //}
        public OctreeBox(BoundingBox aabb)
        {
            Length = MathEx.Distance(ref aabb.Minimum, ref aabb.Maximum) / MathEx.Root3;
            Vector3.Add(ref aabb.Minimum, ref aabb.Maximum, out Center);
            Vector3.Multiply(ref Center, 0.5f, out Center);
        }

        public OctreeBox(ref BoundingSphere sph)
        {
            Center = sph.Center;
            Length = sph.Radius * 2;
        }

        public void GetBoundingSphere(out BoundingSphere sp)
        {
            sp.Center = Center;
            sp.Radius = Length * (MathEx.Root3 / 2f);  // 0.5f;
        }
    }
}
