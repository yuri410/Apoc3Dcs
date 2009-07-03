﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;
using SlimDX;
using SlimDX.Direct3D9;
using VirtualBicycle.Collections;
using VirtualBicycle.Config;
using VirtualBicycle.Graphics;
using VirtualBicycle.Graphics.Effects;
using VirtualBicycle.IO;
using VirtualBicycle.MathLib;

namespace VirtualBicycle.Scene
{
    public unsafe abstract class GameSceneBase<CType, SDType> : IDisposable, ISceneRenderer
        where CType : Cluster
        where SDType : SceneDataBase
    {
        const int MaxInstance = 25;

        Device device;

        /// <summary>
        ///  坐标系指示的VertexBuffer
        /// </summary>
        VertexBuffer axis;

        /// <summary>
        ///  坐标系指示的RenderOperation
        /// </summary>
        RenderOperation axisOp;

        /// <summary>
        /// 摄像机列表，不包含effect的
        /// </summary>
        List<ICamera> cameraList;

        ShadowMap shadowMap;

        IPostSceneRenderer postRenderer;
        Instancing instancing;

        protected FastList<CType> visibleClusters;



        /// <summary>
        ///  按效果批次分组，一个效果批次有一个RenderOperation列表
        /// </summary>
        protected Dictionary<string, FastList<RenderOperation>> batchTable;

        /// <summary>
        ///  按效果批次名称查询效果的哈希表
        /// </summary>
        protected Dictionary<string, ModelEffect> effects;

        Dictionary<string, Dictionary<MeshMaterial, Dictionary<GeomentryData, FastList<RenderOperation>>>> instanceTable;

        protected FastList<SceneObject> visibleObjects;

        PassInfo batchHelper = new PassInfo();

        //public ClusterTable ClusterTable
        //{
        //    get { return clusterTable; }
        //}

        public IPostSceneRenderer PostRenderer
        {
            get { return postRenderer; }
            set { postRenderer = value; }
        }

        public FastList<CType> VisisbleClusters
        {
            get { return visibleClusters; }
        }

        public int ClusterCount
        {
            get { return visibleClusters.Count; }
        }

        /// <summary>
        /// Gets the number of primitives being rendered.
        /// 获取渲染的图元数量
        /// </summary>
        public int PrimitiveCount
        {
            get;
            protected set;
        }

        /// <summary>
        /// 获取渲染的顶点数量
        /// </summary>
        public int VertexCount
        {
            get;
            protected set;
        }

        /// <summary>
        /// 获取渲染批次数量
        /// </summary>
        public int BatchCount
        {
            get;
            protected set;
        }

        public int RenderedObjectCount
        {
            get { return batchHelper.RenderedObjectCount; }
            protected set { batchHelper.RenderedObjectCount = value; }
        }

        ///// <summary>
        /////  获取当前渲染状态的摄像机
        ///// </summary>
        //public Camera CurrentCamera
        //{
        //    get;
        //    protected set;
        //}

        /// <summary>
        ///  场景的大气效果渲染器
        /// </summary>
        public Atmosphere Atmosphere
        {
            get;
            private set;
        }

        /// <summary>
        ///  用于Cluster查找的单位
        /// </summary>
        public float CellUnit
        {
            get;
            private set;
        }

        public SDType Data
        {
            get;
            private set;
        }

        public ShadowMap ShadowMap
        {
            get { return shadowMap; }
        }

        SkyBox LoadSkybox(string configName)
        {
            FileLocation fl = FileSystem.Instance.Locate(Path.Combine(Paths.Configs, "skyboxes.ini"), FileLocateRules.Default);
            VirtualBicycle.Config.Configuration config = ConfigurationManager.Instance.CreateInstance(fl);

            ConfigurationSection sect = config[configName];

            SkyBox result = new SkyBox(device);

            FileLocation day = FileSystem.Instance.Locate(Path.Combine(Paths.DataSkybox, sect["DayTexture"]), FileLocateRules.Default);
            FileLocation night = FileSystem.Instance.Locate(Path.Combine(Paths.DataSkybox, sect["NightTexture"]), FileLocateRules.Default);
            result.LoadTexture(day, night);

            return result;
        }


        /// <summary>
        ///  建立坐标指示
        /// </summary>
        void BuildAxis()
        {
            axis = new VertexBuffer(device, sizeof(VertexPC) * 6, Usage.None, VertexPC.Format, Pool.Managed);
            VertexPC* dst = (VertexPC*)axis.Lock(0, 0, LockFlags.None).DataPointer.ToPointer();

            Vector3 centre = new Vector3();
            //centre.Y += 15;

            float ext = 100;

            dst[0] = new VertexPC { pos = centre, diffuse = Color.Red.ToArgb() };
            dst[1] = new VertexPC { pos = new Vector3(ext + centre.X, centre.Y, centre.Z), diffuse = Color.Red.ToArgb() };
            dst[2] = new VertexPC { pos = centre, diffuse = Color.Green.ToArgb() };
            dst[3] = new VertexPC { pos = new Vector3(centre.X, centre.Y + ext, centre.Z), diffuse = Color.Green.ToArgb() };
            dst[4] = new VertexPC { pos = centre, diffuse = Color.Blue.ToArgb() };
            dst[5] = new VertexPC { pos = new Vector3(centre.X, centre.Y, centre.Z + ext), diffuse = Color.Blue.ToArgb() };

            axis.Unlock();


            axisOp.Geomentry = new GeomentryData(null);
            axisOp.Geomentry.Format = VertexPC.Format;
            axisOp.Geomentry.IndexBuffer = null;
            axisOp.Material = MeshMaterial.DefaultMaterial;
            axisOp.Geomentry.PrimCount = 3;
            axisOp.Geomentry.PrimitiveType = PrimitiveType.LineList;
            axisOp.Transformation = Matrix.Identity;
            axisOp.Geomentry.VertexBuffer = axis; ;
            axisOp.Geomentry.VertexCount = 6;
            axisOp.Geomentry.VertexDeclaration = new VertexDeclaration(device, D3DX.DeclaratorFromFVF(VertexPC.Format));
            axisOp.Geomentry.VertexSize = sizeof(VertexPC);
        }


        public GameSceneBase(Device device, SDType data)
        {
            this.device = device;
            //this.clusterTable = data.ClusterTable;
            this.cameraList = new List<ICamera>();
            this.Atmosphere = new Atmosphere(device, data.AtmosphereData, this.LoadSkybox);
            this.Data = data;

            visibleClusters = new FastList<CType>();

            BuildAxis();

            CellUnit = data.CellUnit;


            effects = new Dictionary<string, ModelEffect>();
            batchTable = new Dictionary<string, FastList<RenderOperation>>();
            instanceTable = new Dictionary<string, Dictionary<MeshMaterial, Dictionary<GeomentryData, FastList<RenderOperation>>>>();

            visibleObjects = new FastList<SceneObject>();


            batchHelper.batchTable = batchTable;
            batchHelper.effects = effects;
            batchHelper.instanceTable = instanceTable;
            batchHelper.visibleObjects = visibleObjects;

            this.instancing = new Instancing(device);

            this.shadowMap = new ShadowMap(device);
            this.postRenderer = new PostRenderer(device);
            EffectParams.ShadowMap = shadowMap;
        }


        public void RegisterCamera(ICamera cam)
        {
            cameraList.Add(cam);
            CurrentCamera = cam;
        }

        public void UnregisterCamera(ICamera cam)
        {
            cameraList.Remove(cam);
        }

        public void AddOperation(RenderOperation[] ops)
        {
            if (ops != null)
            {
                for (int k = 0; k < ops.Length; k++)
                {
                    if (ops[k].Geomentry != null)
                    {
                        MeshMaterial mate = ops[k].Material;
                        GeomentryData geoData = ops[k].Geomentry;

                        if (mate != null)
                        {
                            string desc;
                            bool supportsInst;
                            if (mate.Effect == null)
                            {
                                desc = string.Empty;
                                // if effect is null, instancing is supported by defualt
                                supportsInst = true;
                            }
                            else
                            {
                                supportsInst = mate.Effect.SupportsInstancing;
                                desc = mate.Effect.Name + "_" + mate.BatchIndex.ToString();
                            }

                            if (supportsInst)
                            {
                                ModelEffect effect;
                                if (!batchHelper.effects.TryGetValue(desc, out effect))
                                {
                                    batchHelper.effects.Add(desc, mate.Effect);
                                }

                                Dictionary<MeshMaterial, Dictionary<GeomentryData, FastList<RenderOperation>>> matTable;
                                if (!batchHelper.instanceTable.TryGetValue(desc, out matTable))
                                {
                                    matTable = new Dictionary<MeshMaterial, Dictionary<GeomentryData, FastList<RenderOperation>>>();
                                    batchHelper.instanceTable.Add(desc, matTable);
                                }

                                Dictionary<GeomentryData, FastList<RenderOperation>> geoDataTbl;
                                if (!matTable.TryGetValue(mate, out geoDataTbl))
                                {
                                    geoDataTbl = new Dictionary<GeomentryData, FastList<RenderOperation>>();
                                    matTable.Add(mate, geoDataTbl);
                                }

                                FastList<RenderOperation> instOpList;
                                if (!geoDataTbl.TryGetValue(geoData, out instOpList))
                                {
                                    instOpList = new FastList<RenderOperation>();
                                    geoDataTbl.Add(geoData, instOpList);
                                }

                                instOpList.Add(ops[k]);
                            }
                            else
                            {
                                ModelEffect effect;
                                FastList<RenderOperation> opList;

                                if (!batchHelper.effects.TryGetValue(desc, out effect))
                                {
                                    batchHelper.effects.Add(desc, mate.Effect);
                                }

                                if (!batchHelper.batchTable.TryGetValue(desc, out opList))
                                {
                                    opList = new FastList<RenderOperation>();
                                    batchHelper.batchTable.Add(desc, opList);
                                }

                                opList.Add(ops[k]);
                            }
                        }
                    }
                }
            }
        }
        void AddAxisOperation()
        {
            ModelEffect effect;
            if (!effects.TryGetValue(string.Empty, out effect))
            {
                effects.Add(string.Empty, null);
            }

            FastList<RenderOperation> opList;
            if (!batchTable.TryGetValue(string.Empty, out opList))
            {
                opList = new FastList<RenderOperation>();
                batchTable.Add(string.Empty, opList);
            }
            opList.Add(axisOp);

        }


        //public SceneObject FindObject(LineSegment ray)
        //{
        //    Vector3 direction = ray.End - ray.Start;
        //    direction.Normalize();

        //    float cos = Vector3.Dot(Vector3.UnitY, direction);

        //    Vector3 insectPt = ray.Start + direction * (ray.Start.Y / cos);

        //    float dist = Vector3.Distance(direction, insectPt);

        //    float invCellUnit = 1.0f / CellUnit;
        //    int esx = (int)((ray.Start.X - dist) * invCellUnit);
        //    int esy = (int)((ray.Start.Z - dist) * invCellUnit);


        //    // 截断到Cluster的第一顶点处
        //    if (esx < Cluster.ClusterSize)
        //    {
        //        esx = 0;
        //    }
        //    else
        //    {
        //        esx = Cluster.ClusterLength * ((esx - 1) / Cluster.ClusterLength) + 1;
        //    }

        //    if (esy < Cluster.ClusterSize)
        //    {
        //        esy = 0;
        //    }
        //    else
        //    {
        //        esy = Cluster.ClusterLength * ((esy - 1) / Cluster.ClusterLength) + 1;
        //    }
        //    float enumLength = dist * invCellUnit * 2;

        //    float minDist = float.MaxValue;
        //    SceneObject result = null;

        //    Ray ra = new Ray(ray.Start, direction);

        //    for (int x = esx; x < esx + enumLength; x += Cluster.ClusterLength)
        //    {
        //        for (int y = esy; y < esy + enumLength; y += Cluster.ClusterLength)
        //        {
        //            ClusterDescription desc = new ClusterDescription(x, y);

        //            Cluster cluster = clusterTable.TryGetCluster(desc);

        //            if (cluster != null)
        //            {
        //                SceneObject sceObj = cluster.SceneManager.FindObject(ra);
        //                if (sceObj != null)
        //                {
        //                    Vector3 pos = sceObj.BoundingSphere.Center;
        //                    pos.X += sceObj.OffsetX;
        //                    pos.Y += sceObj.OffsetY;
        //                    pos.Z += sceObj.OffsetZ;

        //                    float curDist = Vector3.Distance(pos, ray.Start);

        //                    if (curDist < minDist)
        //                    {
        //                        minDist = curDist;
        //                        result = sceObj;
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    return result;
        //}

        public abstract SceneObject FindObject(LineSegment ray);
        protected abstract void PrepareVisibleClusters(ICamera cam);

        //protected virtual void PrepareVisibleClusters(Camera cam)
        //{
        //    visibleClusters.FastClear();

        //    Frustum frus = cam.Frustum;
        //    Vector3 camPos = cam.Position;

        //    float invCellUnit = 1.0f / CellUnit;

        //    int esx = (int)((camPos.X - cam.FarPlane) * invCellUnit);
        //    int esy = (int)((camPos.Z - cam.FarPlane) * invCellUnit);

        //    // 截断到Cluster的第一顶点处
        //    if (esx < Cluster.ClusterSize)
        //    {
        //        esx = 0;
        //    }
        //    else
        //    {
        //        esx = Cluster.ClusterLength * ((esx - 1) / Cluster.ClusterLength) + 1;
        //    }

        //    if (esy < Cluster.ClusterSize)
        //    {
        //        esy = 0;
        //    }
        //    else
        //    {
        //        esy = Cluster.ClusterLength * ((esy - 1) / Cluster.ClusterLength) + 1;
        //    }


        //    //float a = 1.5f * cam.FarPlane / (float)Cluster.ClusterLength;
        //    float enumLength = cam.FarPlane * invCellUnit * 2;
        //    //int enumLength = (int)a;
        //    //float decs = enumLength - a;

        //    //if (decs < -float.Epsilon)
        //    //{
        //    //    enumLength++;
        //    //}

        //    for (int x = esx; x < esx + enumLength; x += Cluster.ClusterLength)
        //    {
        //        for (int y = esy; y < esy + enumLength; y += Cluster.ClusterLength)
        //        {
        //            ClusterDescription desc = new ClusterDescription(x, y);

        //            Cluster cluster = clusterTable.TryGetCluster(desc);

        //            if (cluster != null)
        //            {
        //                if (frus.IntersectsSphere(cluster.BoundingVolume))
        //                {
        //                    visibleClusters.Add(cluster);
        //                }
        //            }
        //        }
        //    }
        //}

        protected virtual void PreRender()
        {

        }

        public ICamera CurrentCamera
        {
            get;
            protected set;
        }

        void ISceneRenderer.RenderScenePost(Surface target)
        {
            device.SetRenderTarget(0, target);

            device.Clear(ClearFlags.ZBuffer | ClearFlags.Target, 0, 1, 0);


            device.SetTransform(TransformState.Projection, EffectParams.CurrentCamera.ProjectionMatrix);
            device.SetTransform(TransformState.World, Matrix.Identity);
            device.SetTransform(TransformState.View, EffectParams.CurrentCamera.ViewMatrix);


            Atmosphere.Render();

            device.PixelShader = null;
            device.VertexShader = null;


            device.SetRenderState(RenderState.AlphaBlendEnable, false);
            device.SetRenderState(RenderState.Lighting, false);
            device.SetTexture(0, null);

            #region 处理一般的Op
            foreach (KeyValuePair<string, FastList<RenderOperation>> e1 in batchTable)
            {
                FastList<RenderOperation> opList = e1.Value;

                if (opList.Count > 0)
                {
                    RenderList(e1.Key, opList);
                } // if (opList.Count > 0)
            }
            #endregion

            #region 处理Instancing
            foreach (KeyValuePair<string, Dictionary<MeshMaterial, Dictionary<GeomentryData, FastList<RenderOperation>>>> e2 in instanceTable)
            {
                Dictionary<MeshMaterial, Dictionary<GeomentryData, FastList<RenderOperation>>> matTable = e2.Value;
                foreach (KeyValuePair<MeshMaterial, Dictionary<GeomentryData, FastList<RenderOperation>>> e3 in matTable)
                {
                    Dictionary<GeomentryData, FastList<RenderOperation>> geoTable = e3.Value;
                    foreach (KeyValuePair<GeomentryData, FastList<RenderOperation>> e4 in geoTable)
                    {
                        FastList<RenderOperation> opList = e4.Value;
                        GeomentryData gm = e4.Key;

                        if (gm != null)
                        {
                            if (opList.Count < 50 || gm.VertexCount > 20)
                            {
                                RenderList(e2.Key, opList);
                            }
                            else
                            {
                                device.PixelShader = null;
                                device.VertexShader = null;
                                ModelEffect effect = effects[e2.Key];

                                if (effect == null)
                                {
                                    effect = EffectManager.Instance.GetModelEffect(StandardEffectFactory.Name);
                                }

                                MeshMaterial mate = e3.Key;
                                if (mate == null)
                                    mate = MeshMaterial.DefaultMaterial;
                                GeomentryData gm = e4.Key;

                                if (gm.VertexCount == 0)
                                    continue;

                                device.SetRenderState(RenderState.AlphaTestEnable, mate.IsTransparent);
                                device.SetRenderState<Cull>(RenderState.CullMode, mate.CullMode);

                                int passCount = effect.BeginInst();

                                for (int p = 0; p < passCount; p++)
                                {
                                    effect.BeginPassInst(p);

                                    int remainingInst = opList.Count;
                                    int index = 0;
                                    while (remainingInst > 0)
                                    {
                                        BatchCount++;
                                        PrimitiveCount += gm.PrimCount;
                                        VertexCount += gm.VertexCount;

                                        //device.SetRenderState(RenderState.ZWriteEnable, !mate.IsTransparent);

                                        effect.SetupInstancing(mate);

                                        device.VertexFormat = gm.Format;
                                        device.VertexDeclaration = instancing.GetInstancingDecl(gm.VertexDeclaration);

                                        int rendered = instancing.Setup(opList, index);
                                        device.SetStreamSource(0, gm.VertexBuffer, 0, gm.VertexSize);
                                        device.SetStreamSourceFrequency(0, rendered, StreamSource.IndexedData);

                                        remainingInst -= rendered;
                                        index += rendered;

                                        device.Indices = gm.IndexBuffer;
                                        device.DrawIndexedPrimitives(gm.PrimitiveType,
                                            gm.BaseVertex, 0,
                                            gm.VertexCount, gm.BaseIndexStart,
                                            gm.PrimCount);

                                    }

                                    effect.EndPassInst();
                                }
                                effect.EndInst();
                            } // if (opList.Count > 10)

                        }
                    }
                }
            }
            #endregion

            Dictionary<string, FastList<RenderOperation>>.ValueCollection vals = batchTable.Values;
            foreach (FastList<RenderOperation> opList in vals)
            {
                opList.FastClear();
            }
            Dictionary<string, Dictionary<MeshMaterial, Dictionary<GeomentryData, FastList<RenderOperation>>>>.ValueCollection instTableVals = instanceTable.Values;
            foreach (Dictionary<MeshMaterial, Dictionary<GeomentryData, FastList<RenderOperation>>> matTbl in instTableVals)
            {
                Dictionary<MeshMaterial, Dictionary<GeomentryData, FastList<RenderOperation>>>.ValueCollection matTableVals = matTbl.Values;
                foreach (Dictionary<GeomentryData, FastList<RenderOperation>> geoTable in matTableVals)
                {
                    Dictionary<GeomentryData, FastList<RenderOperation>>.ValueCollection geoTableVals = geoTable.Values;
                    foreach (FastList<RenderOperation> opList in geoTableVals)
                    {
                        opList.FastClear();
                    }
                }
            }
            effects.Clear();
        }

        void RenderList(string name, FastList<RenderOperation> opList)
        {
            device.PixelShader = null;
            device.VertexShader = null;
            ModelEffect effect = effects[name];

            if (effect == null)
            {
                effect = EffectManager.Instance.GetModelEffect(StandardEffectFactory.Name);
            }

            int passCount = effect.Begin();
            for (int p = 0; p < passCount; p++)
            {
                effect.BeginPass(p);

                for (int j = 0; j < opList.Count; j++)
                {
                    RenderOperation op = opList[j];
                    GeomentryData gm = op.Geomentry;

                    if (gm.VertexCount == 0)
                        continue;

                    BatchCount++;
                    PrimitiveCount += gm.PrimCount;
                    VertexCount += gm.VertexCount;

                    MeshMaterial mate = op.Material;
                    if (mate == null)
                        mate = MeshMaterial.DefaultMaterial;

                    //device.SetRenderState(RenderState.ZWriteEnable, !mate.IsTransparent);
                    device.SetRenderState(RenderState.AlphaTestEnable, mate.IsTransparent);
                    device.SetRenderState<Cull>(RenderState.CullMode, mate.CullMode);

                    effect.Setup(mate, ref op);

                    device.SetStreamSource(0, gm.VertexBuffer, 0, gm.VertexSize);
                    device.VertexFormat = gm.Format;
                    device.VertexDeclaration = gm.VertexDeclaration;

                    if (gm.UseIndices)
                    {
                        device.Indices = gm.IndexBuffer;
                        device.DrawIndexedPrimitives(gm.PrimitiveType,
                            gm.BaseVertex, 0,
                            gm.VertexCount, gm.BaseIndexStart,
                            gm.PrimCount);
                    }
                    else
                    {
                        device.DrawPrimitives(gm.PrimitiveType, 0, gm.PrimCount);
                    }
                } // for (int j = 0; j < opList.Count; j++)
                effect.EndPass();
            }
            effect.End();
        }

        void RenderSMList(string name, FastList<RenderOperation> opList) 
        {
            ModelEffect effect = effects[name];
            if (effect == null)
                effect = shadowMap.DefaultSMGen;

            effect.BeginShadowPass();

            for (int j = 0; j < opList.Count; j++)
            {
                RenderOperation op = opList[j];
                GeomentryData gm = op.Geomentry;

                if (gm.VertexCount == 0)
                    continue;

                BatchCount++;
                PrimitiveCount += gm.PrimCount;
                VertexCount += gm.VertexCount;

                MeshMaterial mate = op.Material;
                if (mate == null)
                    mate = MeshMaterial.DefaultMaterial;

                //device.SetRenderState(RenderState.AlphaTestEnable, mate.IsTransparent);
                device.SetRenderState<Cull>(RenderState.CullMode, mate.CullMode);

                effect.SetupShadowPass(mate, ref op);

                device.SetStreamSource(0, gm.VertexBuffer, 0, gm.VertexSize);
                device.VertexFormat = gm.Format;
                device.VertexDeclaration = gm.VertexDeclaration;

                if (gm.UseIndices)
                {
                    device.Indices = gm.IndexBuffer;
                    device.DrawIndexedPrimitives(gm.PrimitiveType,
                        gm.BaseVertex, 0,
                        gm.VertexCount, gm.BaseIndexStart,
                        gm.PrimCount);
                }
                else
                {
                    device.DrawPrimitives(gm.PrimitiveType, 0, gm.PrimCount);
                }
            }

            effect.EndShadowPass();
        }

        public virtual void RenderScene()
        {
            VertexCount = 0;
            BatchCount = 0;
            PrimitiveCount = 0;
            batchHelper.RenderedObjectCount = 0;

            EffectParams.Atmosphere = Atmosphere;
            EffectParams.TerrainHeightScale = Data.TerrainSettings.HeightScale;

            for (int i = 0; i < cameraList.Count; i++)
            {
                EffectParams.CurrentCamera = cameraList[i];
                CurrentCamera = cameraList[i];

                PrepareVisibleClusters(EffectParams.CurrentCamera);

                for (int j = 0; j < visibleClusters.Count; j++)
                {
                    visibleClusters[j].SceneManager.PrepareVisibleObjects(EffectParams.CurrentCamera, batchHelper);
                }

                AddAxisOperation();
                PreRender();


                device.PixelShader = null;
                device.VertexShader = null;

                device.SetRenderState(RenderState.ZEnable, true);

                #region Shadow Map Gen
                shadowMap.Begin(Atmosphere.LightDirection, EffectParams.CurrentCamera);

                foreach (KeyValuePair<string, FastList<RenderOperation>> e1 in batchTable)
                {
                    FastList<RenderOperation> opList = e1.Value;

                    if (opList.Count > 0)
                    {
                        RenderSMList(e1.Key, opList);
                    }
                }
                foreach (KeyValuePair<string, Dictionary<MeshMaterial, Dictionary<GeomentryData, FastList<RenderOperation>>>> e2 in instanceTable)
                {
                    Dictionary<MeshMaterial, Dictionary<GeomentryData, FastList<RenderOperation>>> matTable = e2.Value;
                    foreach (KeyValuePair<MeshMaterial, Dictionary<GeomentryData, FastList<RenderOperation>>> e3 in matTable)
                    {
                        Dictionary<GeomentryData, FastList<RenderOperation>> geoTable = e3.Value;
                        foreach (KeyValuePair<GeomentryData, FastList<RenderOperation>> e4 in geoTable)
                        {
                            GeomentryData gm = e4.Key;

                            FastList<RenderOperation> opList = e4.Value;

                            if (gm != null)
                            {
                                if (opList.Count < 50 || gm.VertexCount > 20)
                                {
                                    RenderSMList(e2.Key, opList);
                                }
                                else
                                {
                                    ModelEffect effect = effects[e2.Key];

                                    if (effect == null)
                                        effect = shadowMap.DefaultSMGen;

                                    MeshMaterial mate = e3.Key;
                                    if (mate == null)
                                        mate = MeshMaterial.DefaultMaterial;


                                    if (gm.VertexCount == 0)
                                        continue;

                                    device.SetRenderState<Cull>(RenderState.CullMode, mate.CullMode);

                                    effect.BeginShadowPass();

                                    int remainingInst = opList.Count;
                                    int index = 0;
                                    while (remainingInst > 0)
                                    {
                                        BatchCount++;
                                        PrimitiveCount += gm.PrimCount;
                                        VertexCount += gm.VertexCount;

                                        //device.SetRenderState(RenderState.ZWriteEnable, !mate.IsTransparent);
                                        RenderOperation op = new RenderOperation();
                                        effect.Setup(mate, ref op);

                                        device.VertexFormat = gm.Format;
                                        device.VertexDeclaration = instancing.GetInstancingDecl(gm.VertexDeclaration);

                                        int rendered = instancing.Setup(opList, index);

                                        device.SetStreamSource(0, gm.VertexBuffer, 0, gm.VertexSize);
                                        device.SetStreamSourceFrequency(0, rendered, StreamSource.IndexedData);

                                        remainingInst -= rendered;
                                        index += rendered;

                                        device.Indices = gm.IndexBuffer;
                                        device.DrawIndexedPrimitives(gm.PrimitiveType,
                                            gm.BaseVertex, 0,
                                            gm.VertexCount, gm.BaseIndexStart,
                                            gm.PrimCount);

                                    }
                                    effect.EndShadowPass();
                                } // if (opList.Count > 0)
                            }
                        }
                    }
                }
                shadowMap.End();

                #endregion

                postRenderer.Render(this, CurrentCamera.RenderTarget);
            }

            //shadowMap.RenderSM();
        }

        public virtual void Update(float dt)
        {
            for (int i = 0; i < cameraList.Count; i++)
            {
                cameraList[i].Update(dt);
            }
            for (int i = 0; i < visibleClusters.Count; i++)
            {
                visibleClusters[i].Update(dt);
            }
            Atmosphere.Update(dt);
        }

        #region IDisposable 成员

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                shadowMap.Dispose();
                instancing.Dispose();
                postRenderer.Dispose();

                axis.Dispose();
                visibleObjects.Clear();

                Dictionary<string, FastList<RenderOperation>>.ValueCollection vals = batchTable.Values;
                foreach (FastList<RenderOperation> opList in vals)
                {
                    opList.Clear();
                }
                batchTable.Clear();

                Dictionary<string, Dictionary<MeshMaterial, Dictionary<GeomentryData, FastList<RenderOperation>>>>.ValueCollection instTableVals = instanceTable.Values;
                foreach (Dictionary<MeshMaterial, Dictionary<GeomentryData, FastList<RenderOperation>>> matTbl in instTableVals)
                {
                    Dictionary<MeshMaterial, Dictionary<GeomentryData, FastList<RenderOperation>>>.ValueCollection matTableVals = matTbl.Values;
                    foreach (Dictionary<GeomentryData, FastList<RenderOperation>> geoTable in matTableVals)
                    {
                        Dictionary<GeomentryData, FastList<RenderOperation>>.ValueCollection geoTableVals = geoTable.Values;
                        foreach (FastList<RenderOperation> opList in geoTableVals)
                        {
                            opList.Clear();
                        }
                        geoTable.Clear();
                    }
                    matTbl.Clear();
                }
                effects.Clear();

            }
            shadowMap = null;
            instancing = null;
            postRenderer = null;
            batchTable = null;
            instanceTable = null;
            effects = null;
            visibleObjects = null;
            axis = null;
            cameraList = null;
        }

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
