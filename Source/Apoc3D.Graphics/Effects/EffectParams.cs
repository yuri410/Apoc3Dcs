using System;
using System.Collections.Generic;
using System.Text;
using Apoc3D.MathLib;
using Apoc3D.Scene;

namespace Apoc3D.Graphics.Effects
{
    public static class EffectParams
    {
        public static readonly Vector3 LightDir = new Vector3(-1, 0, 0);
        public static readonly Color4F LightAmbient = new Color4F(1f, 1f, 1f, 1f);
        public static readonly Color4F LightDiffuse = new Color4F(1f, 1f, 1f, 1f);
        public static readonly Color4F LightSpecular = new Color4F(1f, 1f, 1f, 1f);
        public static float TerrainHeightScale
        {
            get;
            set;
        }

        public static ShadowMap ShadowMap
        {
            get;
            set;
        }

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
    }
}
