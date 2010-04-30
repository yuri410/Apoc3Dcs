using System;
using System.Collections.Generic;
using System.Text;
using Apoc3D.Collections;
using Apoc3D.Core;
using Apoc3D.Graphics.Effects;
using Apoc3D.MathLib;
using Apoc3D.Scene;

namespace Apoc3D.Graphics
{
    public enum RenderMode
    {
        Simple,
        Depth,
        Wireframe,
        Final
    }

    public class SceneRendererParameter
    {
        public bool UseShadow
        {
            get;
            set;
        }

        public SceneManagerBase SceneManager
        {
            get;
            set;
        }
        public IPostSceneRenderer PostRenderer
        {
            get;
            set;
        }

    }
    public class SceneRenderer : ISceneRenderer
    {
        RenderSystem renderSystem;
        ObjectFactory factory;

        //Sprite sprite;

        /// <summary>
        /// 摄像机列表，不包含effect的
        /// </summary>
        List<ICamera> cameraList;

        ShadowMap shadowMap;
        IPostSceneRenderer postRenderer;
        SceneManagerBase sceneManager;
        SceneRendererParameter sceneParams;
        PassData batchData = new PassData();

        public bool ClearScreen
        {
            get;
            set;
        }
        /// <summary>
        ///  获取或设置后期效果渲染器
        /// </summary>
        public IPostSceneRenderer PostRenderer
        {
            get { return postRenderer; }
            set { postRenderer = value; }
        }
        public Atmosphere Atmosphere
        {
            get;
            private set;
        }

        public SceneManagerBase SceneManager
        {
            get { return sceneManager; }
        }

        /// <summary>
        ///  获取渲染物体的数量
        /// </summary>
        public int RenderedObjectCount
        {
            get { return batchData.RenderedObjectCount; }
            protected set { batchData.RenderedObjectCount = value; }
        }

        public SceneRenderer(RenderSystem rs, SceneRendererParameter sm)
        {
            this.ClearScreen = true;
            this.sceneParams = sm;
            this.renderSystem = rs;
            this.factory = rs.ObjectFactory;

            this.postRenderer = sm.PostRenderer;
            this.sceneManager = sm.SceneManager;
            if (sm.UseShadow)
            {
                shadowMap = new ShadowMap(rs);
            }
            this.cameraList = new List<ICamera>();

            //sprite = factory.CreateSprite();
        }

        /// <summary>
        ///  添加摄像机
        /// </summary>
        /// <param name="cam"></param>
        public void RegisterCamera(ICamera cam)
        {
            cameraList.Add(cam);
            CurrentCamera = cam;
        }

        /// <summary>
        ///  删除摄像机
        /// </summary>
        /// <param name="cam"></param>
        public void UnregisterCamera(ICamera cam)
        {
            cameraList.Remove(cam);
        }

        /// <summary>
        ///  将RenderOperation加入哈希表，准备渲染
        /// </summary>
        /// <param name="ops"></param>
        public void AddOperation(RenderOperation[] ops)
        {
            if (ops != null)
            {
                for (int k = 0; k < ops.Length; k++)
                {
                    if (ops[k].Geomentry != null)
                    {
                        Material mate = ops[k].Material;
                        GeomentryData geoData = ops[k].Geomentry;
                        RenderPriority prio = mate.PriorityHint;

                        if (mate != null)
                        {
                            FastList<RenderOperation> opList;

                            Dictionary<Material, FastList<RenderOperation>> matTbl = batchData.batchTable[(int)prio];

                            if (!matTbl.TryGetValue(mate, out opList))
                            {
                                opList = new FastList<RenderOperation>();
                                matTbl.Add(mate, opList);
                            }

                            opList.Add(ops[k]);

                            //string desc;
                            //bool supportsInst;
                            //if (mate.Effect == null)
                            //{
                            //    desc = string.Empty;
                            //    // if effect is null, instancing is supported by defualt
                            //    supportsInst = true;
                            //}
                            //else
                            //{
                            //    supportsInst = mate.Effect.SupportsInstancing;
                            //    desc = mate.Effect.Name + "_" + mate.BatchIndex.ToString();
                            //}

                            //if (supportsInst)
                            //{
                            //    Effect effect;
                            //    if (!batchData.effects.TryGetValue(desc, out effect))
                            //    {
                            //        batchData.effects.Add(desc, mate.Effect);
                            //    }

                            //    Dictionary<Material, Dictionary<GeomentryData, FastList<RenderOperation>>> matTable;
                            //    if (!batchData.instanceTable.TryGetValue(desc, out matTable))
                            //    {
                            //        matTable = new Dictionary<Material, Dictionary<GeomentryData, FastList<RenderOperation>>>();
                            //        batchData.instanceTable.Add(desc, matTable);
                            //    }

                            //    Dictionary<GeomentryData, FastList<RenderOperation>> geoDataTbl;
                            //    if (!matTable.TryGetValue(mate, out geoDataTbl))
                            //    {
                            //        geoDataTbl = new Dictionary<GeomentryData, FastList<RenderOperation>>();
                            //        matTable.Add(mate, geoDataTbl);
                            //    }

                            //    FastList<RenderOperation> instOpList;
                            //    if (!geoDataTbl.TryGetValue(geoData, out instOpList))
                            //    {
                            //        instOpList = new FastList<RenderOperation>();
                            //        geoDataTbl.Add(geoData, instOpList);
                            //    }

                            //    instOpList.Add(ops[k]);
                            //}
                            //else
                            //{
                            //    Effect effect;
                            //    FastList<RenderOperation> opList;

                            //    if (!batchData.effects.TryGetValue(desc, out effect))
                            //    {
                            //        batchData.effects.Add(desc, mate.Effect);
                            //    }

                            //    if (!batchData.batchTable.TryGetValue(desc, out opList))
                            //    {
                            //        opList = new FastList<RenderOperation>();
                            //        batchData.batchTable.Add(desc, opList);
                            //    }

                            //    opList.Add(ops[k]);
                            //}
                        }
                    }
                }
            }
        }

        /// <summary>
        ///  在渲染的时候，获取当前渲染到的摄像机
        /// </summary>
        public ICamera CurrentCamera
        {
            get;
            protected set;
        }

        void ClearBatchData()
        {

            Dictionary<Material, FastList<RenderOperation>>[] vals = batchData.batchTable;

            foreach (Dictionary<Material, FastList<RenderOperation>> matTbl in vals)
            {
                Dictionary<Material, FastList<RenderOperation>>.ValueCollection mats = matTbl.Values;
                foreach (FastList<RenderOperation> opList in mats)
                {
                    opList.FastClear();
                }
            }

            Dictionary<Material, Dictionary<GeomentryData, FastList<RenderOperation>>>.ValueCollection matTableVals = batchData.instanceTable.Values;
            foreach (Dictionary<GeomentryData, FastList<RenderOperation>> geoTable in matTableVals)
            {
                Dictionary<GeomentryData, FastList<RenderOperation>>.ValueCollection geoTableVals = geoTable.Values;
                foreach (FastList<RenderOperation> opList in geoTableVals)
                {
                    opList.FastClear();
                }

            }
        }

        
        /// <summary>
        ///  见接口<see cref="ISceneRenderer"/>
        /// </summary>
        /// <param name="target"></param>
        void ISceneRenderer.RenderScene(RenderTarget target, RenderMode mode)
        {
            RenderStateManager states = renderSystem.RenderStates;
            renderSystem.Mode = mode;

            renderSystem.SetRenderTarget(0, target);

            if (ClearScreen)
            {
                renderSystem.Clear(ClearFlags.DepthBuffer | ClearFlags.Target, ColorValue.Black, 1, 0);
            }
            //else
            //{
            //    renderSystem.Clear(ClearFlags.DepthBuffer, ColorValue.Black, 1, 0);
            //}
            states.AlphaBlendEnable = false;
            //states.FillMode = FillMode.WireFrame;

            #region 处理一般的Op
            for (int i = 0; i < batchData.batchTable.Length; i++)
            {
                Dictionary<Material, FastList<RenderOperation>> matTbl = batchData.batchTable[i];
                foreach (KeyValuePair<Material, FastList<RenderOperation>> e1 in matTbl)
                {
                    FastList<RenderOperation> opList = e1.Value;

                    if (opList.Count > 0)
                    {
                        renderSystem.Render(e1.Key, opList.Elements, opList.Count);
                    }
                }
            }
            #endregion

            #region 处理Instancing
            //foreach (KeyValuePair<string, Dictionary<Material, Dictionary<GeomentryData, FastList<RenderOperation>>>> e2 in instanceTable)
            //{
            //    Dictionary<Material, Dictionary<GeomentryData, FastList<RenderOperation>>> matTable = e2.Value;
            //    foreach (KeyValuePair<Material, Dictionary<GeomentryData, FastList<RenderOperation>>> e3 in matTable)
            //    {
            //        Dictionary<GeomentryData, FastList<RenderOperation>> geoTable = e3.Value;
            //        foreach (KeyValuePair<GeomentryData, FastList<RenderOperation>> e4 in geoTable)
            //        {
            //            FastList<RenderOperation> opList = e4.Value;
            //            GeomentryData gm = e4.Key;

            //            if (gm != null)
            //            {
            //                if (opList.Count < 50 || gm.VertexCount > 20)
            //                {
            //                    RenderList(e2.Key, opList);
            //                }
            //                else
            //                {
            //                    renderSystem.PixelShader = null;
            //                    renderSystem.VertexShader = null;
            //                    Effect effect = effects[e2.Key];

            //                    if (effect == null)
            //                    {
            //                        effect = EffectManager.Instance.GetModelEffect(StandardEffectFactory.Name);
            //                    }

            //                    Material mate = e3.Key;
            //                    if (mate == null)
            //                        mate = Material.DefaultMaterial;

            //                    if (gm.VertexCount == 0)
            //                        continue;

            //                    renderSystem.SetRenderState(RenderState.AlphaTestEnable, mate.IsTransparent);
            //                    renderSystem.SetRenderState<Cull>(RenderState.CullMode, mate.CullMode);

            //                    int passCount = effect.BeginInst();

            //                    for (int p = 0; p < passCount; p++)
            //                    {
            //                        effect.BeginPassInst(p);

            //                        int remainingInst = opList.Count;
            //                        int index = 0;
            //                        while (remainingInst > 0)
            //                        {
            //                            BatchCount++;
            //                            PrimitiveCount += gm.PrimCount;
            //                            VertexCount += gm.VertexCount;

            //                            //device.SetRenderState(RenderState.ZWriteEnable, !mate.IsTransparent);

            //                            effect.SetupInstancing(mate);

            //                            renderSystem.VertexFormat = gm.Format;
            //                            renderSystem.VertexDeclaration = instancing.GetInstancingDecl(gm.VertexDeclaration);

            //                            int rendered = instancing.Setup(opList, index);
            //                            renderSystem.SetStreamSource(0, gm.VertexBuffer, 0, gm.VertexSize);
            //                            renderSystem.SetStreamSourceFrequency(0, rendered, StreamSource.IndexedData);

            //                            remainingInst -= rendered;
            //                            index += rendered;

            //                            renderSystem.Indices = gm.IndexBuffer;
            //                            renderSystem.DrawIndexedPrimitives(gm.PrimitiveType,
            //                                gm.BaseVertex, 0,
            //                                gm.VertexCount, gm.BaseIndexStart,
            //                                gm.PrimCount);

            //                        }

            //                        effect.EndPassInst();
            //                    }
            //                    effect.EndInst();
            //                } // if (opList.Count > 10)

            //            }
            //        }
            //    }
            //}
            #endregion
            ClearBatchData();
        }

        void RenderDepth()
        {
            
            #region Shadow Map Gen
            renderSystem.Mode = RenderMode.Depth;
            //renderSystem.RenderStates.FillMode = FillMode.Solid;

            shadowMap.Begin(EffectParams.LightDir, EffectParams.CurrentCamera);

            for (int i = 0; i < batchData.batchTable.Length; i++)
            {
                Dictionary<Material, FastList<RenderOperation>> matTbl = batchData.batchTable[i];
                foreach (KeyValuePair<Material, FastList<RenderOperation>> e1 in matTbl)
                {
                    FastList<RenderOperation> opList = e1.Value;

                    if (opList.Count > 0)
                    {
                        renderSystem.Render(e1.Key, opList.Elements, opList.Count);
                    }
                }
            }
            //#region Instancing
            ////foreach (KeyValuePair<string, Dictionary<Material, Dictionary<GeomentryData, FastList<RenderOperation>>>> e2 in instanceTable)
            ////{
            ////    Dictionary<Material, Dictionary<GeomentryData, FastList<RenderOperation>>> matTable = e2.Value;
            ////    foreach (KeyValuePair<Material, Dictionary<GeomentryData, FastList<RenderOperation>>> e3 in matTable)
            ////    {
            ////        Dictionary<GeomentryData, FastList<RenderOperation>> geoTable = e3.Value;
            ////        foreach (KeyValuePair<GeomentryData, FastList<RenderOperation>> e4 in geoTable)
            ////        {
            ////            GeomentryData gm = e4.Key;

            ////            FastList<RenderOperation> opList = e4.Value;

            ////            if (gm != null)
            ////            {
            ////                if (opList.Count < 50 || gm.VertexCount > 20)
            ////                {
            ////                    RenderSMList(e2.Key, opList);
            ////                }
            ////                else
            ////                {
            ////                    Effect effect = effects[e2.Key];

            ////                    if (effect == null)
            ////                        effect = shadowMap.DefaultSMGen;

            ////                    Material mate = e3.Key;
            ////                    if (mate == null)
            ////                        mate = Material.DefaultMaterial;


            ////                    if (gm.VertexCount == 0)
            ////                        continue;

            ////                    renderSystem.SetRenderState<Cull>(RenderState.CullMode, mate.CullMode);

            ////                    effect.BeginShadowPass();

            ////                    int remainingInst = opList.Count;
            ////                    int index = 0;
            ////                    while (remainingInst > 0)
            ////                    {
            ////                        BatchCount++;
            ////                        PrimitiveCount += gm.PrimCount;
            ////                        VertexCount += gm.VertexCount;

            ////                        //device.SetRenderState(RenderState.ZWriteEnable, !mate.IsTransparent);
            ////                        RenderOperation op = new RenderOperation();
            ////                        effect.Setup(mate, ref op);

            ////                        renderSystem.VertexFormat = gm.Format;
            ////                        renderSystem.VertexDeclaration = instancing.GetInstancingDecl(gm.VertexDeclaration);

            ////                        int rendered = instancing.Setup(opList, index);

            ////                        renderSystem.SetStreamSource(0, gm.VertexBuffer, 0, gm.VertexSize);
            ////                        renderSystem.SetStreamSourceFrequency(0, rendered, StreamSource.IndexedData);

            ////                        remainingInst -= rendered;
            ////                        index += rendered;

            ////                        renderSystem.Indices = gm.IndexBuffer;
            ////                        renderSystem.DrawIndexedPrimitives(gm.PrimitiveType,
            ////                            gm.BaseVertex, 0,
            ////                            gm.VertexCount, gm.BaseIndexStart,
            ////                            gm.PrimCount);

            ////                    }
            ////                    effect.EndShadowPass();
            ////                } // if (opList.Count > 0)
            ////            }
            ////        }
            ////    }
            ////}
            //#endregion
            shadowMap.End();

            #endregion
        }

        void ActivateScene()
        {
            for (int i = 0; i < batchData.batchTable.Length; i++)
            {
                Dictionary<Material, FastList<RenderOperation>> matTbl = batchData.batchTable[i];
                foreach (KeyValuePair<Material, FastList<RenderOperation>> e1 in matTbl)
                {
                    FastList<RenderOperation> opList = e1.Value;
                    for (int j = 0; j < opList.Count; j++)
                    {
                        for (int k = 0; k < Material.MaxTexLayers; k++)
                        {
                            ResourceHandle<Texture> tex = opList[j].Material.GetTexture(k);
                            if (tex != null)
                                tex.Touch();
                        }
                    }
                }
            }

            ClearBatchData();
        }
        public void ActivateVisibles()
        {
            renderSystem.BeginFrame();


            for (int i = 0; i < cameraList.Count; i++)
            {
                EffectParams.CurrentCamera = cameraList[i];
                CurrentCamera = cameraList[i];

                sceneManager.PrepareVisibleObjects(EffectParams.CurrentCamera, batchData);

                ActivateScene();
            }

            renderSystem.EndFrame();

        }


        /// <summary>
        ///  渲染整个场景
        /// </summary>
        /// <remarks>
        ///  在这个函数中对于每个摄像机检测可见的物体，渲染阴影贴图。
        ///  然后，渲染器会使用IPostSceneRenderer来进行剩下的渲染操作，
        ///  在一个实现IPostSceneRenderer的对象中，
        ///  IPostSceneRenderer对象可以回调该GameScene（或者说实现ISceneRenderer）的RenderScenePost方法，
        ///  渲染3D场景的。然后完成后期特效的渲染。
        ///  
        ///  调用顺序：
        ///   -> GameSceneBase.RenderScene
        ///    检测可见物体
        ///    完成阴影贴图渲染
        ///   -> IPostSceneRender.Render
        ///   -> ISceneRenderer.RenderScenePost
        ///    渲染场景
        /// </remarks>
        public virtual void RenderScene()
        {
            renderSystem.BeginFrame();


            batchData.RenderedObjectCount = 0;

            //renderSystem.RenderStates.FillMode = FillMode.WireFrame;
            //EffectParams.Atmosphere = Atmosphere;
            //EffectParams.TerrainHeightScale = Data.TerrainSettings.HeightScale;

            for (int i = 0; i < cameraList.Count; i++)
            {
                EffectParams.CurrentCamera = cameraList[i];
                CurrentCamera = cameraList[i];

                sceneManager.PrepareVisibleObjects(EffectParams.CurrentCamera, batchData);

                renderSystem.BindShader((PixelShader)null);
                renderSystem.BindShader((VertexShader)null);

                renderSystem.RenderStates.DepthBufferEnable = true;

                if (sceneParams.UseShadow)
                {
                    //EffectParams.DepthViewProj = CurrentCamera.ViewMatrix * CurrentCamera.ProjectionMatrix;
                    RenderDepth();
                    renderSystem.SetRenderTarget(0, CurrentCamera.RenderTarget);
                    EffectParams.DepthMap[0] = shadowMap.ShadowColorMap;

                }
                
               
                postRenderer.RenderFullScene(this, CurrentCamera.RenderTarget, CurrentCamera.Mode);
            }
            renderSystem.EndFrame();
        }

        /// <summary>
        ///  
        /// </summary>
        /// <param name="dt">时间间隔</param>
        public virtual void Update(GameTime time)
        {
            for (int i = 0; i < cameraList.Count; i++)
            {
                cameraList[i].Update(time);
            }
            sceneManager.Update(time);
        }

    }
}
