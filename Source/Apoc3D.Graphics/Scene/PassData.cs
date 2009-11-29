using System;
using System.Collections.Generic;
using System.Text;
using Apoc3D.Collections;
using Apoc3D.Graphics;
using Apoc3D.Graphics.Effects;

namespace Apoc3D.Scene
{
    /// <summary>
    ///  渲染场景时，PassInfo对象用来从Cluster的场景管理器收集信息，并存储
    /// </summary>
    public class PassData : IDisposable
    {
        ///// <summary>
        /////  按效果批次分组，一个效果批次有一个RenderOperation列表
        ///// </summary>
        //public Dictionary<string, FastList<RenderOperation>> batchTable;

        /// <summary>
        ///  按材质分组，一个材质批次有一个RenderOperation列表
        /// </summary>
        public Dictionary<Material, FastList<RenderOperation>> batchTable;

        ///// <summary>
        /////  按效果批次名称查询效果的哈希表
        ///// </summary>
        //public Dictionary<string, Effect> effects;

        public Dictionary<Material, Dictionary<GeomentryData, FastList<RenderOperation>>> instanceTable;

        public FastList<SceneObject> visibleObjects;

        public int RenderedObjectCount;

        public PassData()
        {
            batchTable = new Dictionary<Material, FastList<RenderOperation>>();
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
                Dictionary<Material, FastList<RenderOperation>>.ValueCollection vals = batchTable.Values;
                foreach (FastList<RenderOperation> opList in vals)
                {
                    opList.Clear();
                }
                batchTable.Clear();

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
