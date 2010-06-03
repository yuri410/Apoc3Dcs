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
using System.Text;
using Apoc3D.Collections;
using Apoc3D.Design;
using Apoc3D.MathLib;

namespace Apoc3D.Graphics.Animation
{
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class EntityNode
    {
        public EntityNode(int index)
        {
            //this.Entity = mesh;
            this.Index = index;
        }

        public EntityNode(int index, EntityNodeCollection children, EntityNode parent, Matrix[] transforms)
        {
            this.Index = index;
            this.Parent = parent;
            this.Children = children;
            //this.Entity = mesh;
            this.Transforms = transforms;
        }

        public EntityNodeCollection Children
        {
            get;
            private set;
        }


        public EntityNode Parent
        {
            get;
             set;
        }

        //public Mesh Entity
        //{
        //    get;
        //    private set;
        //}

        public int Index
        {
            get;
            private set;
        }

        [TypeConverter(typeof(ArrayConverter<MatrixEditor, Matrix>))]
        public Matrix[] Transforms
        {
            get;
            set;
        }

    //    public Matrix CurrentTransform
    //    {
    //        get;
    //        set;
    //    }
    }
}
