using System;
using System.Collections.Generic;
using System.Text;
using Apoc3D.MathLib;
using Apoc3D.Scene;

namespace Apoc3D.Graphics.Effects
{
    public static class EffectParams
    {
        static EffectParams()
        {
            DepthMap = new Texture[5];
        }

        public static Vector3 LightDir = new Vector3(-1, 0, 0);
        public static Color4F LightAmbient = new Color4F(1, .5f, .5f, .5f);
        public static Color4F LightDiffuse = new Color4F(1f, .8f, .8f, .8f);
        public static Color4F LightSpecular = new Color4F(1f, 1f, 1f, 1f);

        //public static ShadowMap ShadowMap
        //{
        //    get;
        //    set;
        //}

        public static Texture[] DepthMap;

        public static Atmosphere Atmosphere
        {
            get;
            set;
        }

        public static ICamera CurrentCamera
        {
            get;
            set;
        }

        public static Matrix DepthViewProj;
    }
}
