using System;
using System.Collections.Generic;
using System.Text;
using Apoc3D.Collections;
using Apoc3D.Graphics;
using Apoc3D.Graphics.Effects;

namespace Apoc3D.Scene
{
    /// <summary>
    ///  渲染场景时，PassInfo对象用来从场景管理器收集信息，并存储
    /// </summary>
    public class PassData : IDisposable
    {
        /// <summary>
        ///  普通渲染表。分为若干不同渲染次序的组，每组按材质分组，一个材质组有一个RenderOperation列表
        /// </summary>
        public Dictionary<Material, FastList<RenderOperation>>[] batchTable;

        /// <summary>
        ///  
        /// </summary>
        public Dictionary<Material, Dictionary<GeomentryData, FastList<RenderOperation>>> instanceTable;

        public FastList<SceneObject> visibleObjects;

        public int RenderedObjectCount;

        public PassData()
        {
            batchTable = new Dictionary<Material, FastList<RenderOperation>>[4];
            for (int i = 0; i < batchTable.Length; i++)
            {
                batchTable[i] = new Dictionary<Material, FastList<RenderOperation>>();
            }

            instanceTable = new Dictionary<Material, Dictionary<GeomentryData, FastList<RenderOperation>>>();

            visibleObjects = new FastList<SceneObject>();
        }

        #region IDisposable 成员

        public bool Disposed
        {
            get;
            private set;
        }

        public void Dispose()
        {
            if (!Disposed)
            {
                for (int i = 0; i < batchTable.Length; i++)
                {
                    Dictionary<Material, FastList<RenderOperation>>.ValueCollection mats = batchTable[i].Values;

                    foreach (FastList<RenderOperation> opList in mats)
                    {
                        opList.Clear();
                    }

                    batchTable[i].Clear();
                }

               

                Dictionary<Material, Dictionary<GeomentryData, FastList<RenderOperation>>>.ValueCollection matTableVals = instanceTable.Values;
                foreach (Dictionary<GeomentryData, FastList<RenderOperation>> geoTable in matTableVals)
                {
                    Dictionary<GeomentryData, FastList<RenderOperation>>.ValueCollection geoTableVals = geoTable.Values;
                    foreach (FastList<RenderOperation> opList in geoTableVals)
                    {
                        opList.Clear();
                    }
                    geoTable.Clear();
                }
                instanceTable.Clear();

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
