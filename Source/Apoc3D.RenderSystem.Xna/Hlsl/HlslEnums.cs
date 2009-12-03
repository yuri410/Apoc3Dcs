using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Apoc3D.RenderSystem.Xna
{
    enum HlslRegisterType 
    {
        Unknown,
        Sampler,
        Constant
    }

    enum HlslType
    {
        Unknown,

        /// <summary>
        /// sampler, sampler1D, sampler2D, sampler3D, samplerCUBE, sampler_state, SamplerState
        /// </summary>
        Sampler,
        Boolean,
        Int32,
        UInt32,
        Single,
        HalfSingle,
        Double,
        Vector2,
        Vector3,
        Vector4,
        Matrix1x1,
        Matrix2x2,
        Matrix3x3,
        Matrix4x4,
    }

    enum AutoParamType
    {
        None,

        LightPosition,
        LightDirection,

        MaterialAmbient,
        MaterialDiffuse,
        MaterialSpecular,
        MaterialEmmisive,
        
        MaterialTexture,
        ShadowMap,
        SceneDepth,
    }
}