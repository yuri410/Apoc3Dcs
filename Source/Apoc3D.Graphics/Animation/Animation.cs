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
using Apoc3D.Design;
using Apoc3D.MathLib;

namespace Apoc3D.Graphics.Animation
{
    [Flags()]
    public enum ModelAnimationFlags
    {
        EntityTransform = 0,
        Skin = 1 << 1,
        Morph = 1 << 2
    }
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public abstract class AnimationData
    {
        int frameCount;

        public int FrameCount
        {
            get { return frameCount; }
            protected set { frameCount = value; }
        }

        public float FrameLength
        {
            get;
            protected set;
        }
    }
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public abstract class AnimationInstance : IUpdatable
    {
        AnimationData data;


        int currentFrame;

        float currentTime;
        float totalTime;

        protected AnimationInstance(AnimationData data)
        {
            this.data = data;

            totalTime = data.FrameCount * data.FrameLength;
        }
        protected float CurrentTime
        {
            get { return currentTime; }            
        }
        protected float TotalTime
        {
            get { return totalTime; }
            set { totalTime = value; }
        }


        public int CurrentFrame
        {
            get { return currentFrame; }
            protected set { currentFrame = value; }
        }

        public abstract Matrix GetTransform(int index);               
        
        #region IUpdatable 成员

        public virtual void Update(GameTime dt)
        {
            if (data.FrameLength > float.Epsilon)
            {
                currentTime += dt.ElapsedGameTimeSeconds;

                if (currentTime >= totalTime)
                {
                    currentTime = 0;
                }

                float v = (currentTime / data.FrameLength);
                currentFrame = (int)v;
                if (currentFrame > v) currentFrame--;
            }
        }

        #endregion
    }

    public class NoAnimation : AnimationInstance 
    {
        Matrix transform;

        public NoAnimation(Matrix matrix)
            : base(new TransformAnimation(1))
        {
            this.transform = matrix;
        }
        public NoAnimation()
            : base(new TransformAnimation(1))
        {
            this.transform = Matrix.Identity;
        }
        public override Matrix GetTransform(int index)
        {
            return transform;
        }
        public void SetTransform(Matrix matrix)
        {
            transform = matrix;
        }
    }
}
