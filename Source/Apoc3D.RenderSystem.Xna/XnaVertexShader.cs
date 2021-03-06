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
using Apoc3D.Graphics;
using Apoc3D.MathLib;
using Apoc3D.Vfs;
using X = Microsoft.Xna.Framework;
using XG = Microsoft.Xna.Framework.Graphics;

namespace Apoc3D.RenderSystem.Xna
{
    unsafe class XnaVertexShader : VertexShader
    {
        internal XG.VertexShader vsXna;
        internal XG.ShaderConstantTable table;

        XG.GraphicsDevice device;
        XG.ShaderConstantCollection constants;

        Dictionary<string, int> constIndexTable;

        float[] constHelperF = new float[4];
        int[] constHelperI = new int[4];
        bool[] constHelperB = new bool[4];

        static class Helper
        {
            public static X.Vector4[][] v4Buffer;
            public static X.Matrix[][] m4Buffer;

            public const int MaxBufferSize = 256;

            static Helper()
            {
                v4Buffer = new X.Vector4[MaxBufferSize][];
                m4Buffer = new X.Matrix[MaxBufferSize][];

                for (int i = 0; i < MaxBufferSize; i++)
                {
                    v4Buffer[i] = new X.Vector4[i];
                    m4Buffer[i] = new X.Matrix[i];
                }
            }

            public static int MemoryCost
            {
                get { return m4Buffer.Length * sizeof(X.Matrix) + v4Buffer.Length * sizeof(X.Vector4); }
            }
        }

        internal XnaVertexShader(XnaRenderSystem rs, XG.VertexShader vs)
            : base(rs)
        {
            vsXna = vs;
            device = rs.Device;

            Init(vs.GetShaderCode());
        }

        public XnaVertexShader(XnaRenderSystem rs, byte[] byteCode)
            : base(rs)
        {
            device = rs.Device;
            vsXna = new XG.VertexShader(device, byteCode);

            Init(byteCode);
        }
        public XnaVertexShader(XnaRenderSystem rs, ResourceLocation resLoc)
            : base(rs)
        {
            ContentBinaryReader br = new ContentBinaryReader(resLoc);
            byte[] byteCode = br.ReadBytes(resLoc.Size);
            br.Close();

            device = rs.Device;
            vsXna = new XG.VertexShader(device, byteCode);

            Init(byteCode);
        }

        void Init(byte[] byteCode)
        {
            table = new XG.ShaderConstantTable(byteCode);

            constants = table.Constants;

            constIndexTable = new Dictionary<string, int>();
            for (int i = 0; i < constants.Count; i++)
            {
                constIndexTable.Add(constants[i].Name, i);
            }
        }

        public override int GetConstantIndex(string name)
        {
            return constIndexTable[name];
        }

        #region Index Set
        #region Struct Set
        public override void SetValue(int index, Vector2 value)
        {
            device.SetVertexShaderConstant(constants[index].RegisterIndex, *(X.Vector2*)&value);
        }
        public override void SetValue(int index, Vector3 value)
        {
            device.SetVertexShaderConstant(constants[index].RegisterIndex, *(X.Vector3*)&value);
        }
        public override void SetValue(int index, Vector4 value)
        {
            device.SetVertexShaderConstant(constants[index].RegisterIndex, *(X.Vector4*)&value);
        }
        public override void SetValue(int index, Color4F value)
        {
            device.SetVertexShaderConstant(constants[index].RegisterIndex, *(X.Vector4*)&value);
        }
        public override void SetValue(int index, Quaternion value)
        {
            device.SetVertexShaderConstant(constants[index].RegisterIndex, *(X.Quaternion*)&value);
        }
        public override void SetValue(int index, Matrix value)
        {
            device.SetVertexShaderConstant(constants[index].RegisterIndex, *(X.Matrix*)&value);
        }
        public override void SetValue(int index, Plane value)
        {
            device.SetVertexShaderConstant(constants[index].RegisterIndex, *(X.Vector4*)&value);
        }

        public override void SetValueDirect(int reg, ref Vector2 value)
        {
            fixed (Vector2* ptr = &value)
            {
                device.SetVertexShaderConstant(reg, *(X.Vector2*)ptr);
            }
        }
        public override void SetValue(int index, ref Vector2 value)
        {
            fixed (Vector2* ptr = &value)
            {
                device.SetVertexShaderConstant(constants[index].RegisterIndex, *(X.Vector2*)ptr);
            }
        }
        public override void SetValue(int index, ref Vector3 value)
        {
            fixed (Vector3* ptr = &value)
            {
                device.SetVertexShaderConstant(constants[index].RegisterIndex, *(X.Vector3*)ptr);
            }
        }
        public override void SetValue(int index, ref Vector4 value)
        {
            fixed (Vector4* ptr = &value)
            {
                device.SetVertexShaderConstant(constants[index].RegisterIndex, *(X.Vector4*)ptr);
            }
        }
        public override void SetValue(int index, ref Quaternion value)
        {
            fixed (Quaternion* ptr = &value)
            {
                device.SetVertexShaderConstant(constants[index].RegisterIndex, *(X.Quaternion*)ptr);
            }
        }
        public override void SetValue(int index, ref Matrix value)
        {
            fixed (Matrix* ptr = &value)
            {
                device.SetVertexShaderConstant(constants[index].RegisterIndex, *(X.Matrix*)ptr);
            }
        }
        public override void SetValue(int index, ref Color4F value)
        {
            fixed (Color4F* ptr = &value)
            {
                device.SetVertexShaderConstant(constants[index].RegisterIndex, *(X.Vector4*)ptr);
            }
        }
        public override void SetValue(int index, ref Plane value)
        {
            fixed (Plane* ptr = &value)
            {
                device.SetVertexShaderConstant(constants[index].RegisterIndex, *(X.Vector4*)ptr);
            }
        }


        #endregion

        #region Struct array set
        public override void SetValue(int index, Vector2[] value)
        {
            int startRegister = constants[index].RegisterIndex;

            int length = value.Length;
            if (length > 0)
            {
                fixed (Vector2* ptr = &value[0])
                {
                    for (int i = 0; i < length; i++)
                    {
                        device.SetVertexShaderConstant(i + startRegister, *(X.Vector2*)(ptr + i));
                    }
                }
            }
        }
        public override void SetValue(int index, Vector3[] value)
        {
            int startRegister = constants[index].RegisterIndex;

            int length = value.Length;
            if (length > 0)
            {
                fixed (Vector3* ptr = &value[0])
                {
                    for (int i = 0; i < length; i++)
                    {
                        device.SetVertexShaderConstant(i + startRegister, *(X.Vector3*)(ptr + i));
                    }
                }
            }
        }
        public override void SetValue(int index, Vector4[] value)
        {
            int len = value.Length;
            if (len <= Helper.MaxBufferSize)
            {
                fixed (X.Vector4* dst = &Helper.v4Buffer[len][0])
                {
                    fixed (Vector4* src = &value[0])
                    {
                        Memory.Copy(src, dst, sizeof(Vector4) * value.Length);
                    }
                }
                device.SetVertexShaderConstant(constants[index].RegisterIndex, Helper.v4Buffer[len]);
            }
            else
            {
                X.Vector4[] buffer = new X.Vector4[value.Length];
                fixed (X.Vector4* dst = &buffer[0])
                {
                    fixed (Vector4* src = &value[0])
                    {
                        Memory.Copy(src, dst, sizeof(Vector4) * buffer.Length);
                    }
                }
                device.SetVertexShaderConstant(constants[index].RegisterIndex, buffer);
            }
        }
        public override void SetValue(int index, Color4F[] value)
        {
            int len = value.Length;
            if (len <= Helper.MaxBufferSize)
            {
                fixed (X.Vector4* dst = &Helper.v4Buffer[len][0])
                {
                    fixed (Color4F* src = &value[0])
                    {
                        Memory.Copy(src, dst, sizeof(Color4F) * value.Length);
                    }
                }
                device.SetVertexShaderConstant(constants[index].RegisterIndex, Helper.v4Buffer[len]);
            }
            else
            {
                X.Vector4[] buffer = new X.Vector4[value.Length];
                fixed (X.Vector4* dst = &buffer[0])
                {
                    fixed (Color4F* src = &value[0])
                    {
                        Memory.Copy(src, dst, sizeof(Color4F) * buffer.Length);
                    }
                }
                device.SetVertexShaderConstant(constants[index].RegisterIndex, buffer);
            }
        }
        public override void SetValue(int index, Plane[] value)
        {
            int len = value.Length;
            if (len <= Helper.MaxBufferSize)
            {
                fixed (X.Vector4* dst = &Helper.v4Buffer[len][0])
                {
                    fixed (Plane* src = &value[0])
                    {
                        Memory.Copy(src, dst, sizeof(Color4F) * value.Length);
                    }
                }
                device.SetVertexShaderConstant(constants[index].RegisterIndex, Helper.v4Buffer[len]);
            }
            else
            {
                X.Vector4[] buffer = new X.Vector4[value.Length];
                fixed (X.Vector4* dst = &buffer[0])
                {
                    fixed (Plane* src = &value[0])
                    {
                        Memory.Copy(src, dst, sizeof(Color4F) * buffer.Length);
                    }
                }
                device.SetVertexShaderConstant(constants[index].RegisterIndex, buffer);
            }
        }
        public override void SetValue(int index, Quaternion[] value)
        {
            int startRegister = constants[index].RegisterIndex;

            int length = value.Length;
            if (length > 0)
            {
                fixed (Quaternion* ptr = &value[0])
                {
                    for (int i = 0; i < length; i++)
                    {
                        device.SetVertexShaderConstant(i + startRegister, *(X.Quaternion*)(ptr + i));
                    }
                }
            }
        }
        public override void SetValue(int index, Matrix[] value)
        {
            int len = value.Length;
            if (len <= Helper.MaxBufferSize)
            {
                fixed (X.Matrix* dst = &Helper.m4Buffer[len][0])
                {
                    fixed (Matrix* src = &value[0])
                    {
                        Memory.Copy(src, dst, sizeof(Matrix) * value.Length);
                    }
                }
                device.SetVertexShaderConstant(constants[index].RegisterIndex, Helper.m4Buffer[len]);
            }
            else
            {
                X.Matrix[] buffer = new X.Matrix[value.Length];
                fixed (X.Matrix* dst = &buffer[0])
                {
                    fixed (Matrix* src = &value[0])
                    {
                        Memory.Copy(src, dst, sizeof(Matrix) * buffer.Length);
                    }
                }
                device.SetVertexShaderConstant(constants[index].RegisterIndex, buffer);
            }
        }
        #endregion

        public override void SetValue(int index, bool value)
        {
            constHelperB[0] = value;
            device.SetVertexShaderConstant(constants[index].RegisterIndex, constHelperB);
        }
        public override void SetValue(int index, float value)
        {
            constHelperF[0] = value;
            device.SetVertexShaderConstant(constants[index].RegisterIndex, constHelperF);
        }
        public override void SetValueDirect(int reg, float value)
        {
            constHelperF[0] = value;
            device.SetVertexShaderConstant(reg, constHelperF);
        }
        public override void SetValue(int index, int value)
        {
            constHelperI[0] = value;
            device.SetVertexShaderConstant(constants[index].RegisterIndex, constHelperI);
        }
        public override void SetValue(int index, bool[] value)
        {
            device.SetVertexShaderConstant(constants[index].RegisterIndex, value);
        }
        public override void SetValue(int index, float[] value)
        {
            device.SetVertexShaderConstant(constants[index].RegisterIndex, value);
        }
        public override void SetValue(int index, int[] value)
        {
            device.SetVertexShaderConstant(constants[index].RegisterIndex, value);
        }

        public override void SetSamplerStateDirect(int index, ref ShaderSamplerState state)
        {
            throw new NotImplementedException();
        }
        public override void SetTextureDirect(int index, Texture tex)
        {
            throw new NotImplementedException();
        }
        public override void SetTexture(int index, Texture tex)
        {
            int si = constants[index].SamplerIndex;
            
            if (tex.IsLoaded)
            {
                XnaTexture xnatex = (XnaTexture)tex;
                if (xnatex.tex2D != null)
                    device.VertexTextures[si] = xnatex.tex2D;
                else if (xnatex.cube != null)
                    device.VertexTextures[si] = xnatex.cube;
                else if (xnatex.tex3D != null)
                    device.VertexTextures[si] = xnatex.tex3D;
            }
        }
        public override void SetSamplerState(int index, ref ShaderSamplerState state)
        {
            int si = constants[index].SamplerIndex;

            XG.SamplerState xs = device.VertexSamplerStates[si];
            xs.AddressU = XnaUtils.ConvertEnum(state.AddressU);
            xs.AddressV = XnaUtils.ConvertEnum(state.AddressV);
            xs.AddressW = XnaUtils.ConvertEnum(state.AddressW);
            xs.BorderColor = new XG.Color(state.BorderColor.R, state.BorderColor.G, state.BorderColor.B, state.BorderColor.A);
            xs.MagFilter = XnaUtils.ConvertEnum(state.MagFilter);
            xs.MinFilter = XnaUtils.ConvertEnum(state.MinFilter);
            xs.MipFilter = XnaUtils.ConvertEnum(state.MipFilter);
            xs.MaxAnisotropy = state.MaxMipLevel;
            xs.MipMapLevelOfDetailBias = state.MipMapLODBias;
        }

        #endregion

        #region Named Set
        public override void SetValue(string paramName, ref Vector2 value)
        {
            int index = GetConstantIndex(paramName);
            SetValue(index, ref value);
        }
        public override void SetValue(string paramName, ref Vector3 value)
        {
            int index = GetConstantIndex(paramName);
            SetValue(index, ref value);
        }
        public override void SetValue(string paramName, ref Vector4 value)
        {
            int index = GetConstantIndex(paramName);
            SetValue(index, ref value);
        }
        public override void SetValue(string paramName, ref Quaternion value)
        {
            int index = GetConstantIndex(paramName);
            SetValue(index, ref value);
        }
        public override void SetValue(string paramName, ref Matrix value)
        {
            int index = GetConstantIndex(paramName);
            SetValue(index, ref value);
        }
        public override void SetValue(string paramName, ref Color4F value)
        {
            int index = GetConstantIndex(paramName);
            SetValue(index, ref value);
        }
        public override void SetValue(string paramName, ref Plane value)
        {
            int index = GetConstantIndex(paramName);
            SetValue(index, ref value);
        }

        public override void SetValue(string paramName, Vector2 value)
        {
            int index = GetConstantIndex(paramName);
            SetValue(index, ref value);
        }
        public override void SetValue(string paramName, Vector3 value)
        {
            int index = GetConstantIndex(paramName);
            SetValue(index, ref value);
        }
        public override void SetValue(string paramName, Vector4 value)
        {
            int index = GetConstantIndex(paramName);
            SetValue(index, ref value);
        }
        public override void SetValue(string paramName, Quaternion value)
        {
            int index = GetConstantIndex(paramName);
            SetValue(index, ref value);
        }
        public override void SetValue(string paramName, Matrix value)
        {
            int index = GetConstantIndex(paramName);
            SetValue(index, ref value);
        }
        public override void SetValue(string paramName, Color4F value)
        {
            int index = GetConstantIndex(paramName);
            SetValue(index, ref value);
        }
        public override void SetValue(string paramName, Plane value)
        {
            int index = GetConstantIndex(paramName);
            SetValue(index, ref value);
        }
        
        public override void SetValue(string paramName, Vector2[] value)
        {
            int index = GetConstantIndex(paramName);
            SetValue(index, value);
        }
        public override void SetValue(string paramName, Vector3[] value)
        {
            int index = GetConstantIndex(paramName);
            SetValue(index, value);
        }
        public override void SetValue(string paramName, Vector4[] value)
        {
            int index = GetConstantIndex(paramName);
            SetValue(index, value);
        }
        public override void SetValue(string paramName, Quaternion[] value)
        {
            int index = GetConstantIndex(paramName);
            SetValue(index, value);
        }
        public override void SetValue(string paramName, Matrix[] value)
        {
            int index = GetConstantIndex(paramName);
            SetValue(index, value);
        }
        public override void SetValue(string paramName, Color4F[] value)
        {
            int index = GetConstantIndex(paramName);
            SetValue(index, value);
        }
        public override void SetValue(string paramName, Plane[] value)
        {
            int index = GetConstantIndex(paramName);
            SetValue(index, value);
        }

        public override void SetValue(string paramName, bool value)
        {
            int index = GetConstantIndex(paramName);
            SetValue(index, value);
        }
        public override void SetValue(string paramName, float value)
        {
            int index = GetConstantIndex(paramName);
            SetValue(index, value);
        }
        public override void SetValue(string paramName, int value)
        {
            int index = GetConstantIndex(paramName);
            SetValue(index, value);
        }
        public override void SetValue(string paramName, bool[] value)
        {
            int index = GetConstantIndex(paramName);
            SetValue(index, value);
        }
        public override void SetValue(string paramName, float[] value)
        {
            int index = GetConstantIndex(paramName);
            SetValue(index, value);
        }
        public override void SetValue(string paramName, int[] value)
        {
            int index = GetConstantIndex(paramName);
            SetValue(index, value);
        }

        public override void SetTexture(string paramName, Texture tex)
        {
            int index = GetConstantIndex(paramName);
            SetTexture(index, tex);
        }
        public override void SetSamplerState(string paramName, ref ShaderSamplerState state)
        {
            int index = GetConstantIndex(paramName);
            SetSamplerState(index, ref state);
        }
        #endregion

        public override void AutoSetParameters()
        {
            throw new NotImplementedException();
        }
    }
}