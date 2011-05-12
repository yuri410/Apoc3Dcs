using System;
using System.Collections.Generic;
using System.Text;
using Apoc3D.Graphics.Animation;
using Apoc3D.MathLib;

namespace Apoc3D.Graphics.Animation
{
    public class NoAnimaionPlayer : ModelAnimationPlayerBase
    {
        Matrix transform;

        public NoAnimaionPlayer() : this(Matrix.Identity) { }
        public NoAnimaionPlayer(Matrix transform) 
        {
            this.transform = transform;
        }
        public override Matrix GetTransform(int boneId)
        {
            return transform;
        }
    }
}
