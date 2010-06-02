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
http://www.gnu.org/copyleft/lesser.txt.

-----------------------------------------------------------------------------
*/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using Apoc3D.Design;
using Apoc3D.Graphics;
using Apoc3D.MathLib;
using Apoc3D.Vfs;
using AG = Apoc3D.Graphics;

namespace Apoc3D.Scene
{
    public delegate void ProgressCallBack(int current, int total);

    public class SceneDataBase : IDisposable
    {
        #region 常量

        public const float MaxTerrainHeight = 5000f;

        public const int FileId = (((byte)'V') << 24) | (((byte)'M') << 16) | (((byte)'P') << 8) | ((byte)' ');

        static readonly string NameTag = "Name";
        
        static readonly string DescriptionTag = "Description";

        protected static readonly string TerrainTag = "Terrain";
        protected static readonly string AtmosphereTag = "Atmosphere";
        protected static readonly string BoundingBoxTag = "BoundingBox";
        protected static readonly string CellUnitTag = "CellUnit";
        protected static readonly string SceneSizeTag = "SceneSize";
        protected static readonly string ObjectCountTag = "ObjectCount";
        protected static readonly string ObjectTag = "Object";
        static readonly string TypeTagTag = "TypeTag";

        #endregion

        #region 字段

        protected SceneObject[] sceneObjects;

        protected AG.RenderSystem device;

        protected BoundingBox boundingBox;

        #endregion

        #region 属性

        public string Name
        {
            get;
            protected set;
        }

        public string Description
        {
            get;
            protected set;
        }

        public ObjectTypeManager ObjectManager
        {
            get;
            private set;
        }

        public BoundingBox AABB
        {
            get { return boundingBox; }
            protected set { boundingBox = value; }
        }

        public float CellUnit
        {
            get;
            protected set;
        }

        public TerrainSettings TerrainSettings
        {
            get;
            protected set;
        }

        public AtmosphereInfo AtmosphereData
        {
            get;
            protected set;
        }
        public SceneObject[] SceneObjects
        {
            get { return sceneObjects; }
        }

        /// <summary>
        /// 场景边长（以地形单元为单位）
        /// </summary>
        public int SceneSize
        {
            get;
            set;
        }
        #endregion

        public SceneDataBase(AG.RenderSystem device, ObjectTypeManager mgr)
        {
            this.ObjectManager = mgr;
            this.device = device;
        }

        #region 方法

        public List<SceneObject> FindObjects(IObjectFilter callBack)
        {
            List<SceneObject> result = new List<SceneObject>();
            for (int i = 0; i < sceneObjects.Length; i++)
            {
                if (callBack.Check(sceneObjects[i]))
                {
                    result.Add(sceneObjects[i]);
                }
            }
            return result;
        }

        public void SetCellUnit(float value)
        {
            CellUnit = value;
        }

        public void ReadInformation(BinaryDataReader data, ProgressCallBack cbk) 
        {
            ContentBinaryReader br = data.TryGetData(NameTag);
            if (br != null)
            {
                Name = br.ReadStringUnicode();
                br.Close();
            }
            else
            {
                Name = string.Empty;
            }

            br = data.TryGetData(DescriptionTag);
            if (br != null)
            {
                Description = br.ReadStringUnicode();
                br.Close();
            }
            else
            {
                Description = string.Empty;
            }
            br = data.GetData(TerrainTag);

            BinaryDataReader tdata = br.ReadBinaryData();
            this.TerrainSettings = new TerrainSettings();
            this.TerrainSettings.ReadData(tdata);
            tdata.Close();
            br.Close();


            br = data.GetData(AtmosphereTag);
            tdata = br.ReadBinaryData();
            AtmosphereData = new AtmosphereInfo();
            AtmosphereData.ReadData(tdata);
            tdata.Close();
            br.Close();


            br = data.GetData(BoundingBoxTag);
            boundingBox.Minimum.X = br.ReadSingle();
            boundingBox.Minimum.Y = br.ReadSingle();
            boundingBox.Minimum.Z = br.ReadSingle();
            boundingBox.Maximum.X = br.ReadSingle();
            boundingBox.Maximum.Y = br.ReadSingle();
            boundingBox.Maximum.Z = br.ReadSingle();
            br.Close();

            CellUnit = data.GetDataSingle(CellUnitTag);

            this.SceneSize = data.GetDataInt32(SceneSizeTag);

        }

        public virtual void ReadData(BinaryDataReader data, ProgressCallBack cbk)
        {
            ReadInformation(data, cbk);
 
            int objectCount = data.GetDataInt32(ObjectCountTag, 0);

            int total = objectCount + 1;
            if (cbk != null)
                cbk(1, total);

            sceneObjects = new SceneObject[objectCount];

            for (int i = 0; i < objectCount; i++)
            {
                BinaryDataReader odata = new BinaryDataReader(data.GetDataStream(ObjectTag + i.ToString()));

                ContentBinaryReader br = odata.GetData(TypeTagTag);

                string typeTag = br.ReadStringUnicode();
                br.Close();

                sceneObjects[i] = ObjectManager.DeserializeObject(typeTag, odata, this);

                odata.Close();

                if (cbk != null)
                    cbk(1 + i, total);
            }
        }

        public virtual BinaryDataWriter WriteData()
        {
            BinaryDataWriter data = new BinaryDataWriter();

            ContentBinaryWriter bw = data.AddEntry(NameTag);
            bw.WriteStringUnicode(Name);
            bw.Close();

            bw = data.AddEntry(DescriptionTag);
            bw.WriteStringUnicode(Description);
            bw.Close();

            bw = data.AddEntry(TerrainTag);
            BinaryDataWriter tdata = this.TerrainSettings.WriteData();
            bw.Write(tdata);
            tdata.Dispose();
            bw.Close();

            bw = data.AddEntry(AtmosphereTag);
            tdata = AtmosphereData.WriteData();
            bw.Write(tdata);
            tdata.Dispose();
            bw.Close();

            bw = data.AddEntry(BoundingBoxTag);
            bw.Write(boundingBox.Minimum.X);
            bw.Write(boundingBox.Minimum.Y);
            bw.Write(boundingBox.Minimum.Z);
            bw.Write(boundingBox.Maximum.X);
            bw.Write(boundingBox.Maximum.Y);
            bw.Write(boundingBox.Maximum.Z);
            bw.Close();

            data.AddEntry(CellUnitTag, this.CellUnit);

            data.AddEntry(SceneSizeTag, this.SceneSize);

            //data.AddEntry(ClusterCountTag, clusters.Length);
            //for (int i = 0; i < clusters.Length; i++)
            //{
            //    bw = data.AddEntry(ClusterTag + i.ToString());

            //    ClusterDescription desc = clusters[i].Description;
            //    bw.Write(desc.X);
            //    bw.Write(desc.Y);

            //    BinaryDataWriter cData = clusters[i].WriteData();
            //    bw.Write(cData);
            //    cData.Dispose();

            //    bw.Close();
            //}

            data.AddEntry(ObjectCountTag, sceneObjects.Length);
            for (int i = 0; i < sceneObjects.Length; i++)
            {
                BinaryDataWriter odata = new BinaryDataWriter();

                bw = odata.AddEntry(TypeTagTag);

                bw.WriteStringUnicode(sceneObjects[i].TypeTag);
                bw.Close();

                sceneObjects[i].Serialize(odata);

                odata.Save(data.AddEntryStream(ObjectTag + i.ToString()));
                odata.Dispose();
            }

            return data;
        }

        #endregion

        #region IDisposable 成员

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (sceneObjects != null)
                {
                    for (int i = 0; i < sceneObjects.Length; i++)
                    {
                        if (!sceneObjects[i].Disposed) 
                        {
                            sceneObjects[i].Dispose();
                        }
                    }
                }
                sceneObjects = null;
            }
        }

        [Browsable(false)]
        public bool Disposed
        {
            get;
            private set;
        }

        public void Dispose()
        {
            if (!Disposed)
            {
                Dispose(true);
                Disposed = true;
            }
            else
            {
                throw new ObjectDisposedException(ToString());
            }
        }

        #endregion
    }
}
