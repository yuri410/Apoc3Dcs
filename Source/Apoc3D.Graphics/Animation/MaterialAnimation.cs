﻿using System;
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
