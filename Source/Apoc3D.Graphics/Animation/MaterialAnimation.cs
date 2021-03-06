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
using Apoc3D.MathLib;

namespace Apoc3D.Graphics.Animation
{
    public class MaterialAnimation : AnimationData
    {

        public MaterialAnimation(int frameCount, float frameLength)
        {
            this.FrameCount = frameCount;
            this.FrameLength = frameLength;
        }
    }
    public class MaterialAnimationInstance : AnimationInstance
    {
        MaterialAnimation data;

        static MaterialAnimationInstance defAnim = new MaterialAnimationInstance(new MaterialAnimation(1, 1));
        public static MaterialAnimationInstance DefaultAnimation
        {
            get { return defAnim; }
        }

        public MaterialAnimationInstance(MaterialAnimation data)
            : base(data)
        {
            this.data = data;
        }

        public MaterialAnimation Data 
        {
            get { return data; }
        }

        public override Matrix GetTransform(int index)
        {
            throw new NotSupportedException();
        }
    }
}
