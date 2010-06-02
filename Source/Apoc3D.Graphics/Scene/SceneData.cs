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
http://www.gnu.org/copyleft/lesser.txt.

-----------------------------------------------------------------------------
*/
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Text;
using Apoc3D.Graphics;
using Apoc3D.Vfs;
using Apoc3D.MathLib;
using AG = Apoc3D.Graphics;

namespace Apoc3D.Scene
{
    /// <summary>
    ///  记录场景地形（全局）的参数
    /// </summary>
    public class TerrainSettings
    {
        #region 常量

        protected static readonly string HeightScaleTag = "HeightScale";
        protected static readonly string MaterialTag = "Material";

        #endregion

        #region 属性

        public float HeightScale
        {
            get;
            protected set;
        }
        public Color4F MaterialAmbient
        {
            get;
            protected set;
        }
        public Color4F MaterialDiffuse
        {
            get;
            protected set;
        }
        public Color4F MaterialSpecular
        {
            get;
            protected set;
        }
        public Color4F MaterialEmissive
        {
            get;
            protected set;
        }
        public float MaterialPower
        {
            get;
            protected set;
        }
        #endregion

        #region 方法

        public void SetHeightScale(float value)
        {
            HeightScale = value;
        }

        public void SetMaterialAmbient(Color4F color)
        {
            MaterialAmbient = color;
        }

        public void SetMaterialDiffuse(Color4F color)
        {
            MaterialDiffuse = color;
        }

        public void SetMaterialSpecular(Color4F color)
        {
            MaterialSpecular = color;
        }

        public void SetMaterialEmissive(Color4F color)
        {
            MaterialEmissive = color;
        }

        public void SetMaterialPower(float value)
        {
            MaterialPower = value;
        }

        public virtual void ReadData(BinaryDataReader data)
        {
            HeightScale = data.GetDataSingle("HeightScale");

            ContentBinaryReader br = data.GetData("Material");


            Color4F color;
            color.Alpha = br.ReadSingle();
            color.Red = br.ReadSingle();
            color.Green = br.ReadSingle();
            color.Blue = br.ReadSingle();

            this.MaterialAmbient = color;


            color.Alpha = br.ReadSingle();
            color.Red = br.ReadSingle();
            color.Green = br.ReadSingle();
            color.Blue = br.ReadSingle();

            this.MaterialDiffuse = color;


            color.Alpha = br.ReadSingle();
            color.Red = br.ReadSingle();
            color.Green = br.ReadSingle();
            color.Blue = br.ReadSingle();

            this.MaterialSpecular = color;


            color.Alpha = br.ReadSingle();
            color.Red = br.ReadSingle();
            color.Green = br.ReadSingle();
            color.Blue = br.ReadSingle();

            this.MaterialEmissive = color;



            this.MaterialPower = br.ReadSingle();

            br.Close();

        }
        public virtual BinaryDataWriter WriteData()
        {
            BinaryDataWriter data = new BinaryDataWriter();

            data.AddEntry(HeightScaleTag, HeightScale);

            ContentBinaryWriter bw = data.AddEntry(MaterialTag);

            //Material mat = new Material();
            //mat.Ambient = MaterialAmbient;
            //mat.Diffuse = MaterialDiffuse;
            //mat.Emissive = MaterialEmissive;
            //mat.Specular = MaterialSpecular;
            //mat.Power = MaterialPower;

            Color4F color = MaterialAmbient;
            bw.Write(color.Alpha);
            bw.Write(color.Red);
            bw.Write(color.Green);
            bw.Write(color.Blue);

            color = MaterialDiffuse;
            bw.Write(color.Alpha);
            bw.Write(color.Red);
            bw.Write(color.Green);
            bw.Write(color.Blue);

            color = MaterialSpecular;
            bw.Write(color.Alpha);
            bw.Write(color.Red);
            bw.Write(color.Green);
            bw.Write(color.Blue);

            color = MaterialEmissive;
            bw.Write(color.Alpha);
            bw.Write(color.Red);
            bw.Write(color.Green);
            bw.Write(color.Blue);

            bw.Write(MaterialPower);
            bw.Close();

            return data;
        }

        #endregion

    }

    /// <summary>
    ///  场景数据（地图数据）
    /// </summary>
    public class SceneData : SceneDataBase
    {
        #region 构造函数

        public SceneData(AG.RenderSystem device, ObjectTypeManager mgr)
            : base(device, mgr)
        {

        }

        #endregion

        #region 静态方法

        public static SceneData FromFile(AG.RenderSystem device, ObjectTypeManager mgr, string file, ProgressCallBack cbk)
        {
            return FromFile(device, mgr, new FileLocation(file), cbk);
        }

        public static SceneData FromFile(AG.RenderSystem device, ObjectTypeManager mgr, FileLocation fl, ProgressCallBack cbk)
        {
            ContentBinaryReader br = new ContentBinaryReader(fl);

            if (br.ReadInt32() == FileId)
            {
                SceneData result = new SceneData(device, mgr);

                BinaryDataReader data = br.ReadBinaryData();

                result.ReadData(data, cbk);

                data.Close();

                return result;
            }

            br.Close();
            throw new DataFormatException();
        }

        #endregion
    }
}
