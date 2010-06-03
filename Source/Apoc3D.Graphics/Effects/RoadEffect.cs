﻿/*
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
using Apoc3D.Vfs;
using Apoc3D.MathLib;

namespace Apoc3D.Graphics.Effects
{
    public class RoadEffectFactory : ModelEffectFactory
    {
        static readonly string typeName = "Road";


        public static string Name
        {
            get { return typeName; }
        }


        RenderSystem device;

        public RoadEffectFactory(RenderSystem dev)
        {
            device = dev;
        }

        public override ModelEffect CreateInstance()
        {
            return new RoadEffect(device);
        }

        public override void DestroyInstance(ModelEffect fx)
        {
            fx.Dispose();
        }
    }

    class RoadEffect : ModelEffect
    {
        bool stateSetted;

        RenderSystem device;

        Effect effect;
        EffectHandle tlParamLa;
        EffectHandle tlParamLd;
        EffectHandle tlParamLs;
        EffectHandle tlParamKa;
        EffectHandle tlParamKd;
        EffectHandle tlParamKs;
        EffectHandle tlParamKe;
        EffectHandle tlParamPwr;
        EffectHandle tlParamLdir;
        EffectHandle tlParamVpos;
        EffectHandle tlParamClrMap;
        EffectHandle tlParamMVP;

        EffectHandle normalMapParam;

        EffectHandle tlParamWorldT;
        EffectHandle shadowMapParam;
        EffectHandle shadowMapTransform;
        EffectHandle fogDensityParam;
        EffectHandle fogColorParam;


        Effect shadowMapGen;

        Texture noTexture;

        public unsafe RoadEffect(RenderSystem dev)
            : base(false, RoadEffectFactory.Name)
        {
            device = dev;

            noTexture = new Texture(dev, 1, 1, 1, Usage.None, Format.A8R8G8B8, Pool.Managed);
            *((int*)noTexture.Lock(0, LockMode.None).Pointer) = (int)ColorValue.Gray.PackedValue;
            noTexture.Unlock(0);

            FileLocation fl = FileSystem.Instance.Locate(FileSystem.CombinePath(Paths.Effects, "Road.fx"), FileLocateRules.Default);
            ContentStreamReader sr = new ContentStreamReader(fl);

            string code = sr.ReadToEnd();
            string err;
            effect = Effect.FromString(dev, code, null, IncludeHandler.Instance, null, ShaderFlags.OptimizationLevel3, null, out err);
            sr.Close();

            effect.Technique = new EffectHandle("Road");

            tlParamLa = new EffectHandle("I_a");
            tlParamLd = new EffectHandle("I_d");
            tlParamLs = new EffectHandle("I_s");
            tlParamKa = new EffectHandle("k_a");
            tlParamKd = new EffectHandle("k_d");
            tlParamKs = new EffectHandle("k_s");
            tlParamKe = new EffectHandle("k_e");
            tlParamPwr = new EffectHandle("power");
            tlParamLdir = new EffectHandle("lightDir");

            tlParamClrMap = new EffectHandle("clrMap");

            tlParamVpos = new EffectHandle("cameraPos");
            tlParamMVP = new EffectHandle("mvp");

            tlParamWorldT = new EffectHandle("worldTrans");

            fogDensityParam = new EffectHandle("fogDensity");
            fogColorParam = new EffectHandle("fogColor");

            shadowMapParam = new EffectHandle("shadowMap");
            shadowMapTransform = new EffectHandle("smTrans");
            normalMapParam = new EffectHandle("normalMap");

            // ======================================================================
            fl = FileSystem.Instance.Locate(FileSystem.CombinePath(Paths.Effects, "StandardShadow.fx"), FileLocateRules.Default);
            sr = new ContentStreamReader(fl);
            code = sr.ReadToEnd();
            shadowMapGen = Effect.FromString(dev, code, null, IncludeHandler.Instance, null, ShaderFlags.OptimizationLevel3, null, out err);
            sr.Close();
            shadowMapGen.Technique = new EffectHandle("StandardShadow");
        }


        protected override int begin()
        {
            stateSetted = false;
            return effect.Begin(FX.DoNotSaveState | FX.DoNotSaveShaderState | FX.DoNotSaveSamplerState);
        }

        protected override void end()
        {
            effect.End();
        }

        public override void BeginPass(int passId)
        {
            effect.BeginPass(passId);
        }

        public override void EndPass()
        {
            effect.EndPass();
        }

        public override void Setup(Material mat, ref RenderOperation op)
        {
            if (!stateSetted)
            {
                Light light = EffectParams.Atmosphere.Light;
                Vector3 lightDir = light.Direction;
                effect.SetValue(tlParamLa, light.Ambient);
                effect.SetValue(tlParamLd, light.Diffuse);
                effect.SetValue(tlParamLs, light.Specular);
                effect.SetValue(tlParamLdir, new float[3] { lightDir.X, lightDir.Y, lightDir.Z });

                Vector3 pos = EffectParams.CurrentCamera.Position;
                effect.SetValue(tlParamVpos, new float[3] { pos.X, pos.Y, pos.Z });

                effect.SetTexture(shadowMapParam, EffectParams.ShadowMap.ShadowColorMap);

                stateSetted = true;
            }
            effect.SetValue(tlParamKa, mat.mat.Ambient);
            effect.SetValue(tlParamKd, mat.mat.Diffuse);
            effect.SetValue(tlParamKs, mat.mat.Specular);
            effect.SetValue(tlParamKe, mat.mat.Emissive);

            effect.SetValue(tlParamPwr, mat.mat.Power);

            GameTexture clrTex = mat.GetTexture(0);
            if (clrTex == null)
            {
                effect.SetTexture(tlParamClrMap, noTexture);
            }
            else
            {
                effect.SetTexture(tlParamClrMap, clrTex.GetTexture);
            }
            GameTexture nrmTex = mat.GetTexture(1);
            if (nrmTex == null)
            {
                effect.SetTexture(normalMapParam, noTexture);
            }
            else
            {
                effect.SetTexture(normalMapParam, nrmTex.GetTexture);
            }

            effect.SetValue(tlParamMVP, op.Transformation * EffectParams.CurrentCamera.ViewMatrix * EffectParams.CurrentCamera.ProjectionMatrix);

            effect.SetValue(fogColorParam, new Color4(EffectParams.Atmosphere.FogColor));
            effect.SetValue(fogDensityParam, EffectParams.Atmosphere.FogDensity);

            Matrix lightPrjTrans;
            Matrix.Multiply(ref op.Transformation, ref EffectParams.ShadowMap.ViewProj, out lightPrjTrans);

            effect.SetValue(shadowMapTransform, lightPrjTrans);
            effect.SetValue(tlParamWorldT, op.Transformation);

            effect.CommitChanges();
        }
        public override void SetupShadowPass(Material mat, ref RenderOperation op)
        {
            GameTexture clrTex = mat.GetTexture(0);
            if (clrTex == null)
            {
                shadowMapGen.SetTexture(tlParamClrMap, noTexture);
            }
            else
            {
                shadowMapGen.SetTexture(tlParamClrMap, clrTex.GetTexture);
            }

            shadowMapGen.SetValue(tlParamMVP, op.Transformation * EffectParams.ShadowMap.ViewProj);

            shadowMapGen.CommitChanges();
        }

        public override void BeginShadowPass()
        {
            shadowMapGen.Begin(FX.DoNotSaveState | FX.DoNotSaveShaderState | FX.DoNotSaveSamplerState);
            shadowMapGen.BeginPass(0);
        }
        public override void EndShadowPass()
        {
            shadowMapGen.EndPass();
            shadowMapGen.End();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                effect.Dispose();

                tlParamLa.Dispose();
                tlParamLd.Dispose();
                tlParamLs.Dispose();
                tlParamKa.Dispose();
                tlParamKd.Dispose();
                tlParamKs.Dispose();
                tlParamPwr.Dispose();
                tlParamLdir.Dispose();

                tlParamWorldT.Dispose();

                tlParamVpos.Dispose();
                tlParamMVP.Dispose();

                shadowMapParam.Dispose();
                shadowMapTransform.Dispose();

                shadowMapGen.Dispose();
            }
            effect = null;
            tlParamLa = null;
            tlParamLd = null;
            tlParamLs = null;
            tlParamKa = null;
            tlParamKd = null;
            tlParamKs = null;
            tlParamPwr = null;
            tlParamLdir = null;

            tlParamWorldT = null;

            tlParamVpos = null;
            tlParamMVP = null;

            shadowMapParam = null;
            shadowMapTransform = null;

            shadowMapGen = null;
        }
    }
}
