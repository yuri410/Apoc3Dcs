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
using System.IO;
using System.Text;
using Apoc3D.Core;
using Apoc3D.Graphics.Effects;
using Apoc3D.MathLib;
using Apoc3D.Vfs;

namespace Apoc3D.Graphics
{
    [Flags()]
    public enum MaterialFlags
    {
        None = 0,
        RemapColor = 1,
        BlendBright = 2,
        BlendBright_Color = 3
    }

    public abstract class MaterialBase
    {
        #region Constants

        /// <summary>
        ///  获取最大允许的纹理层数
        /// </summary>
        public const int MaxTexLayers = 16;

        static readonly string IsTransparentTag = "IsTransparent";
        static readonly string CullModeTag = "CullMode";
        static readonly string ZEnabledTag = "ZEnabled";
        static readonly string ZWriteEnabledTag = "ZWriteEnabled";
        static readonly string AlphaRefTag = "AlphaRef";
        static readonly string IsVegetationTag = "IsVegetation";

        static readonly string RenderPriorityTag = "RenderPriority";
        #endregion

        protected MaterialBase()
        {
            ZWriteEnabled = true;
            ZEnabled = true;
            PriorityHint = RenderPriority.Second;
        }
        
        #region Properties

        public RenderPriority PriorityHint
        {
            get;
            set;
        }
        public CullMode CullMode
        {
            get;
            set;
        }

        public bool IsTransparent
        {
            get;
            set;
        }

        public float AlphaRef
        {
            get;
            set;
        }

        /// <summary>
        ///  获取或设置一个值表示材质的法向量是否总是朝上。
        /// </summary>
        public bool IsVegetation
        {
            get;
            set;
        }

        public bool ZEnabled
        {
            get;
            set;
        }
        public bool ZWriteEnabled
        {
            get;
            set;
        }
        #endregion

        #region Methods

        protected virtual void ReadData(BinaryDataReader data)
        {
            AlphaRef = data.GetDataSingle(AlphaRefTag, -1);

            IsVegetation = data.GetDataBool(IsVegetationTag, false);

            CullMode = (CullMode)data.GetDataInt32(CullModeTag, 0);
            IsTransparent = data.GetDataBool(IsTransparentTag, false);
            ZEnabled = data.GetDataBool(ZEnabledTag, true);
            ZWriteEnabled = data.GetDataBool(ZWriteEnabledTag, true);

            PriorityHint = (RenderPriority)data.GetDataInt32(RenderPriorityTag, (int)RenderPriority.Second);
        }

        protected virtual void WriteData(BinaryDataWriter data)
        {
            data.AddEntry(AlphaRefTag, AlphaRef);

            data.AddEntry(IsVegetationTag, IsVegetation);

            data.AddEntry(CullModeTag, (int)CullMode);
            data.AddEntry(IsTransparentTag, IsTransparent);
            data.AddEntry(ZEnabledTag, ZEnabled);
            data.AddEntry(ZWriteEnabledTag, ZWriteEnabled);

            data.AddEntry(RenderPriorityTag, (int)PriorityHint);
        }

        #endregion
    }

    /// <summary>
    ///  定义材质的基本结构
    /// </summary>
    /// <typeparam name="TexType"></typeparam>
    public abstract class MeshMaterialBase<TexType> : MaterialBase, IDisposable
        where TexType : class
    {
        #region Constants
        static readonly string MaterialColorTag = "MaterialColor";

        //static readonly string IsTexEmbededTag = "IsEmbeded";
        static readonly string MaterialFlagTag = "Flags";
        static readonly string HasTextureTag = "HasTexture";
        static readonly string TextureTag = "Texture";
        static readonly string EffectTag = "Effect";
        #endregion

        #region Field
        protected Color4F ambient;
        protected Color4F diffuse;
        protected Color4F specular;
        protected Color4F emissive;
        protected float power;

        protected string[] textureFiles = new string[MaxTexLayers];

        protected TexType[] textures = new TexType[MaxTexLayers];
        protected bool[] hasTexture = new bool[MaxTexLayers];

        string effectName;

        bool disposed;
        
        #endregion

        #region 属性


        public Color4F Ambient
        {
            get { return ambient; }
            set { ambient = value; }
        }

        public Color4F Diffuse
        {
            get { return diffuse; }
            set { diffuse = value; }
        }

        public Color4F Specular
        {
            get { return specular; }
            set { specular = value; }
        }

        public Color4F Emissive
        {
            get { return emissive; }
            set { emissive = value; }
        }

        public float Power
        {
            get { return power; }
            set { power = value; }
        }

        public Effect Effect
        {
            get;
            protected set;
        }

        public int BatchIndex
        {
            get;
            protected set;
        }

        public string GetEffectName()
        {
            return effectName;
        }
        public void SetEffectName(string v)
        {
            effectName = v;
            if (!string.IsNullOrEmpty(v))
            {
                SetEffect(EffectManager.Instance.GetModelEffect(v));
            }
            else
            {
                SetEffect(null);
            }
        }

        public string GetTextureFile(int idx)
        {
            return textureFiles[idx];
        }
        public void SetTextureFile(int idx, string filePath)
        {
            textureFiles[idx] = filePath;
        }

        public TexType GetTexture(int idx)
        {
            return textures[idx];
        }
        public void SetTexture(int idx, TexType tex)
        {
            textures[idx] = tex;
        }


        public void SetEffect(Effect eff)
        {
            Effect = eff;
        }

        public void SetBatchIndex(int index)
        {
            BatchIndex = index;
        }

        public MaterialFlags Flags
        {
            get;
            set;
        }
        #endregion

        #region Abstract Methods
        protected abstract TexType LoadTexture(ContentBinaryReader br, int index);
        protected abstract void DestroyTexture(TexType tex);

        protected abstract void SaveTexture(ContentBinaryWriter bw, TexType tex, int index);
        protected abstract Effect LoadEffect(string name);
        #endregion

        #region Methods

        protected override void ReadData(BinaryDataReader data)
        {
            base.ReadData(data);

            ContentBinaryReader br;

            Flags = (MaterialFlags)data.GetDataInt32(MaterialFlagTag);

            br = data.GetData(MaterialColorTag);
            ambient.Alpha = br.ReadSingle();
            ambient.Red = br.ReadSingle();
            ambient.Green = br.ReadSingle();
            ambient.Blue = br.ReadSingle();

            diffuse.Alpha = br.ReadSingle();
            diffuse.Red = br.ReadSingle();
            diffuse.Green = br.ReadSingle();
            diffuse.Blue = br.ReadSingle();

            specular.Alpha = br.ReadSingle();
            specular.Red = br.ReadSingle();
            specular.Green = br.ReadSingle();
            specular.Blue = br.ReadSingle();

            emissive.Alpha = br.ReadSingle();
            emissive.Red = br.ReadSingle();
            emissive.Green = br.ReadSingle();
            emissive.Blue = br.ReadSingle();

            power = br.ReadSingle();

            br.Close();


            br = data.GetData(EffectTag);
            effectName = br.ReadStringUnicode();
            if (effectName.Length == 0)
                effectName = "Standard";
            //if (!string.IsNullOrEmpty(effectName))
            Effect = LoadEffect(effectName);

            br.Close();

            //br = data.GetData(IsTexEmbededTag);
            //for (int i = 0; i < MaxTexLayers; i++)
            //{
            //    texEmbeded[i] = br.ReadBoolean();
            //}
            //br.Close();

            hasTexture = new bool[MaxTexLayers];
            br = data.GetData(HasTextureTag);
            for (int i = 0; i < MaxTexLayers; i++)
            {
                hasTexture[i] = br.ReadBoolean();
            }
            br.Close();


            for (int i = 0; i < MaxTexLayers; i++)
            {
                if (hasTexture[i])
                {
                    br = data.GetData(TextureTag + i.ToString());
                    textures[i] = LoadTexture(br, i);
                    br.Close();
                }
            }
        }

     

        protected override void WriteData(BinaryDataWriter data)
        {
            base.WriteData(data);


            data.AddEntry(MaterialFlagTag, (int)Flags);

            ContentBinaryWriter bw;

            bw = data.AddEntry(EffectTag);
            //if (Effect == null)
            //{
                bw.WriteStringUnicode(effectName);
            //}
            //else
            //{
                //bw.WriteStringUnicode(Effect.Name);
            //}
            bw.Close();

            bw = data.AddEntry(MaterialColorTag);
            bw.Write(ambient.Alpha);
            bw.Write(ambient.Red);
            bw.Write(ambient.Green);
            bw.Write(ambient.Blue);

            bw.Write(diffuse.Alpha);
            bw.Write(diffuse.Red);
            bw.Write(diffuse.Green);
            bw.Write(diffuse.Blue);

            bw.Write(specular.Alpha);
            bw.Write(specular.Red);
            bw.Write(specular.Green);
            bw.Write(specular.Blue);

            bw.Write(emissive.Alpha);
            bw.Write(emissive.Red);
            bw.Write(emissive.Green);
            bw.Write(emissive.Blue);

            bw.Write(power);

            bw.Close();



            bw = data.AddEntry(HasTextureTag);
            for (int i = 0; i < MaxTexLayers; i++)
            {
                bw.Write(textures[i] != null || !string.IsNullOrEmpty(textureFiles[i]));
            }
            bw.Close();

            for (int i = 0; i < MaxTexLayers; i++)
            {
                if (textures[i] != null || !string.IsNullOrEmpty(textureFiles[i]))
                {
                    bw = data.AddEntry(TextureTag + i.ToString());
                    SaveTexture(bw, textures[i], i);
                    bw.Close();
                }
            }
        }

        #endregion

        #region IDisposable 成员

        public bool Disposed
        {
            get { return disposed; }
        }

        public void Dispose()
        {
            if (!disposed)
            {
                if (textures != null)
                {
                    for (int i = 0; i < 4; i++)
                    {
                        if (textures[i] != null)
                        {
                            DestroyTexture(textures[i]);
                            textures[i] = null;
                        }
                    }
                    textures = null;
                }
                disposed = true;
            }
            else
                throw new ObjectDisposedException(ToString());
        }

        #endregion

        ~MeshMaterialBase()
        {
            if (!disposed)
            {
                Dispose();
            }
        }

    }

    /// <summary>
    ///  材质
    /// </summary>
    public class Material : MeshMaterialBase<ResourceHandle<Texture>>, IComparable<Material>
    {
        #region Constants

        public static Material DefaultMaterial
        {
            get;
            private set;
        }

        #endregion

        #region 构造函数
        static Material()
        {
            DefaultMaterial = new Material(null);

            Color4F clr;
            clr.Alpha = 1;
            clr.Blue = 1;
            clr.Green = 1;
            clr.Red = 1;

            DefaultMaterial.Ambient = clr;
            DefaultMaterial.Diffuse = clr;
            DefaultMaterial.Power = 0;
            clr.Alpha = 0;
            clr.Red = 0;
            clr.Green = 0;
            clr.Blue = 0;
            DefaultMaterial.Emissive = clr;
            DefaultMaterial.Specular = clr;

            DefaultMaterial.CullMode = CullMode.None;
        }

        public Material(RenderSystem dev)
        {
            device = dev;
        }
        #endregion

        #region 字段

        protected RenderSystem device;
        #endregion

        #region IComparable<MeshMaterial> 成员

        public int CompareTo(Material other)
        {
            return this.GetHashCode().CompareTo(other.GetHashCode());
        }

        #endregion

        #region 方法

        public void ReloadTextures()
        {
            for (int i = 0; i < MaxTexLayers; i++)
            {
                if (!string.IsNullOrEmpty(textureFiles[i]))
                {
                    textures[i] = LoadTexture(textureFiles[i]);
                }
            }
        }

        /// <summary>
        ///  重写以适应不同环境下的使用
        /// </summary>
        ResourceHandle<Texture> LoadTexture(string fileName)
        {
            fileName = fileName.Trim();
            if (!string.IsNullOrEmpty(fileName))
            {
                FileLocation fl = FileSystem.Instance.TryLocate(fileName, FileLocateRule.Textures);
                if (fl != null)
                {
                    return TextureManager.Instance.CreateInstance(fl);
                }
                else
                {
                    EngineConsole.Instance.Write("找不到纹理：" + fileName, ConsoleMessageType.Warning);
                    return null;
                }
            }
            return null;
        }
        protected override ResourceHandle<Texture> LoadTexture(ContentBinaryReader br, int index)
        {
            string fn = br.ReadStringUnicode();
            textureFiles[index] = fn;

            return LoadTexture(fn);
        }

        protected override void SaveTexture(ContentBinaryWriter bw, ResourceHandle<Texture> tex, int index)
        {
            bw.WriteStringUnicode(textureFiles[index]);
        }

        protected override Effect LoadEffect(string name)
        {
            return EffectManager.Instance.GetModelEffect(name);
        }
        public static Material FromBinary(RenderSystem dev, BinaryDataReader data)
        {
            Material res = new Material(dev);

            res.ReadData(data);

            return res;
        }
        public static BinaryDataWriter ToBinary(Material mat)
        {
            BinaryDataWriter data = new BinaryDataWriter();
            mat.WriteData(data);

            return data;
        }

        protected override void DestroyTexture(ResourceHandle<Texture> tex)
        {
            tex.Dispose();
        }


        public Material Clone()
        {
            Material result = new Material(device);
            result.AlphaRef = AlphaRef;
            result.Ambient = Ambient;
            result.CullMode = CullMode;
            result.Diffuse = Diffuse;
            result.Effect = Effect;
            result.Emissive = Emissive;
            result.Flags = Flags;
            result.IsTransparent = IsTransparent;
            result.IsVegetation = IsVegetation;
            result.Power = Power;
            result.Specular = Specular;
            result.ZEnabled = ZEnabled;
            result.ZWriteEnabled = ZWriteEnabled;

            for (int i = 0; i < hasTexture.Length; i++)
            {
                result.hasTexture[i] = hasTexture[i];
                result.textureFiles[i] = textureFiles[i];
                if (hasTexture[i])
                {
                    result.textures[i] = LoadTexture(textureFiles[i]);
                }

            }
            return result;
        }

        #endregion
    }
}
