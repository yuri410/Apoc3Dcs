using System;
using System.Collections.Generic;
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

    [AttributeUsage(AttributeTargets.All, AllowMultiple = true)]
    class VarUsageCodeName : Attribute
    {
        public string Name
        {
            get;
            private set;
        }

        public VarUsageCodeName(string name)
        {
            this.Name = name;
        }
    }

    enum AutoParamType
    {
        [VarUsageCodeName("")]
        None,

        [VarUsageCodeName("_Lpos"), VarUsageCodeName("_lightPosition")]
        LightPosition,

        [VarUsageCodeName("_Ldir"), VarUsageCodeName("_lightDir")]
        LightDirection,

        [VarUsageCodeName("_Ma"), VarUsageCodeName("_matAmbient")]
        MaterialAmbient,

        [VarUsageCodeName("_Md"), VarUsageCodeName("_matDiffuse")]
        MaterialDiffuse,

        [VarUsageCodeName("_Mspec"), VarUsageCodeName("_matSpecular")]
        MaterialSpecular,

        [VarUsageCodeName("_Mems"), VarUsageCodeName("_matEmmisive")]
        MaterialEmmisive,

        [VarUsageCodeName("_Ldif"), VarUsageCodeName("_lightDiffuse")]
        LightDiffuse,

        [VarUsageCodeName("_Lamb"), VarUsageCodeName("_lightAmbient")]
        LightAmbient,

        [VarUsageCodeName("_Lspec"), VarUsageCodeName("_lightSpecular")]
        LightSpecular,

        [VarUsageCodeName("_Mtex"), VarUsageCodeName("_matTex")]
        MaterialTexture,

        [VarUsageCodeName("_SHmap"), VarUsageCodeName("_shadowMap")]
        ShadowMap,

        [VarUsageCodeName("_Sdep"), VarUsageCodeName("_sceneDepth")]
        SceneDepth,

        [VarUsageCodeName("_Wtrans"), VarUsageCodeName("_worldTrans")]
        WorldTransform,

        [VarUsageCodeName("_WVtrans"), VarUsageCodeName("_worldViewTrans")]

        WorldViewTransform,
        [VarUsageCodeName("_WVPtrans"), VarUsageCodeName("_wvpTrans")]
        
        WVPTransform,
        [VarUsageCodeName("_PRJtrans"), VarUsageCodeName("_projTrans")]
        ProjectionTransform,

        [VarUsageCodeName("_Vtrans"), VarUsageCodeName("_viewTrans")]
        ViewTransform,

        [VarUsageCodeName("_Cpos"), VarUsageCodeName("_camPos")]
        CameraPosition,

    }
}