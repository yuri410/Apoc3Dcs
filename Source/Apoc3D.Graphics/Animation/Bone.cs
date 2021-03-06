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
using Apoc3D.Collections;
using Apoc3D.MathLib;

namespace Apoc3D.Graphics.Animation
{
    public class Bone
    {
        public Bone(int index)
        {
            this.Index = index;
        }

        public Bone(int index, Matrix transform, int[] children, int parent, string name)
        {
            this.Name = name;
            this.Index = index;
            this.Transforms = transform;
            this.Children = children;
            this.Parent = parent;
        }

        public Matrix Transforms
        {
            get;
            set;
        }

        public string Name
        {
            get;
            set;
        }

        public int Parent
        {
            get;
            set;
        }

        public int[] Children
        {
            get;
            private set;
        }

        public int Index
        {
            get;
            set;
        }
    }
}
