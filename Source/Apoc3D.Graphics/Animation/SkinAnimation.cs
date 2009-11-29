using System;
using System.Collections.Generic;
using System.Text;
using Apoc3D.MathLib;
using Apoc3D.Vfs;

namespace Apoc3D.Graphics.Animation
{
    public class SkinAnimation : AnimationData
    {
        public void Load(BinaryDataReader data) 
        {

        }
        public BinaryDataWriter Save()
        {
            throw new NotImplementedException();
        }
    }
    public class SkinAnimationInstance : AnimationInstance
    {
        SkinAnimation data;

        public SkinAnimationInstance(SkinAnimation data)
            : base(data)
        {
            this.data = data;
        }


        public SkinAnimation Data
        {
            get { return data; }
        }

        public override Matrix GetTransform(int index)
        {
            throw new NotImplementedException();
        }
        //public void Load(BinaryDataReader data) { }
        //public BinaryDataWriter Save() { throw new NotImplementedException(); }
    }
}
