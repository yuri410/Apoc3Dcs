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