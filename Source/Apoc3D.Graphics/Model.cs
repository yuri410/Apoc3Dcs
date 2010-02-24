﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Text;
using Apoc3D.Core;
using Apoc3D.Design;
using Apoc3D.Graphics.Animation;
using Apoc3D.Vfs;

namespace Apoc3D.Graphics
{
    /// <summary>
    ///  定义3D模型提供基础结构
    /// </summary>
    /// <typeparam name="MeshType"></typeparam>
    public abstract class ModelBase<MeshType> : Resource
        where MeshType : class
    {
        public const int MdlId = 0;

        protected readonly string EntityCountTag = "EntityCount";
        protected readonly string EntityPrefix = "Ent";

        protected MeshType[] entities;

        public Dictionary<string, TapeHelper> TapeHelpers
        {
            get;
            set;
        }

        public ResourceLocation DataSource
        {
            get;
            private set;
        }

        protected ModelBase(ResourceLocation rl)
            : base(ModelManager.Instance, rl.Name)
        {
            DataSource = rl;
        }

        protected ModelBase()
        {
        }

        protected ModelBase(string name)
            : base(ModelManager.Instance, name)
        {
        }

        public MeshType[] Entities
        {
            get
            {
                Use();
                return entities;
            }
            set { entities = value; }
        }

        protected abstract MeshType LoadMesh(BinaryDataReader data);
        protected abstract BinaryDataWriter SaveMesh(MeshType mesh);

        protected void ReadData(BinaryDataReader data)
        {
            int entCount = data.GetDataInt32(EntityCountTag);
            entities = new MeshType[entCount];

            ContentBinaryReader br;
            for (int i = 0; i < entCount; i++)
            {
                br = data.GetData(EntityPrefix + i.ToString());
                BinaryDataReader meshData = br.ReadBinaryData();
                entities[i] = LoadMesh(meshData);
                meshData.Close();
                br.Close();
            }
            
            //ModelAnimationFlags flags = (ModelAnimationFlags)data.GetDataInt32(AnimationFlagTag);
            //BinaryDataReader animData;

            //if ((flags & ModelAnimationFlags.EntityTransform) == ModelAnimationFlags.EntityTransform)
            //{
            //    br = data.GetData(AnimationTag + ModelAnimationFlags.EntityTransform.ToString());
            //    TransformAnimation transAnimDat = new TransformAnimation(entities.Length);

            //    animData = br.ReadBinaryData();
            //    transAnimDat.Load(animData);
            //    animData.Close();

            //    br.Close();

            //    transAnim = new TransformAnimationInstance(transAnimDat);
            //}
            //if ((flags & ModelAnimationFlags.Skin) == ModelAnimationFlags.Skin)
            //{

            //}

            //br = data.TryGetData(LodMeshTag);
            //if (br != null)
            //{
            //    BinaryDataReader meshData = br.ReadBinaryData();
            //    lodMesh = LoadMesh(meshData);

            //    meshData.Close();
            //    br.Close();
            //}

        }
        protected void WriteData(BinaryDataWriter data)
        {
            UseSync();
            data.AddEntry(EntityCountTag, entities.Length);

            ContentBinaryWriter bw;
            for (int i = 0; i < entities.Length; i++)
            {
                bw = data.AddEntry(EntityPrefix + i.ToString());

                BinaryDataWriter meshData = SaveMesh(entities[i]);
                bw.Write(meshData);
                meshData.Dispose();
                bw.Close();
            }

            //ModelAnimationFlags flags = ModelAnimationFlags.EntityTransform;

            //if (skinAnim != null)
            //{
            //    flags |= ModelAnimationFlags.Skin;
            //}


            //data.AddEntry(AnimationFlagTag, (int)flags);

            //BinaryDataWriter animData;

            //if ((flags & ModelAnimationFlags.EntityTransform) == ModelAnimationFlags.EntityTransform)
            //{
            //    bw = data.AddEntry(AnimationTag + ModelAnimationFlags.EntityTransform.ToString());
            //    animData = transAnim.Data.Save();
            //    bw.Write(animData);
            //    animData.Dispose();
            //}

            //if ((flags & ModelAnimationFlags.Skin) == ModelAnimationFlags.Skin)
            //{
            //    bw = data.AddEntry(AnimationTag + ModelAnimationFlags.Skin.ToString());
            //    animData = skinAnim.Data.Save();
            //    bw.Write(animData);
            //    animData.Dispose();
            //}
        }


        public override int GetSize()
        {
            if (DataSource != null)
            {
                return DataSource.Size;
            }
            return 0;
        }
        protected override void load()
        {
            if (DataSource != null)
            {
                ContentBinaryReader br = new ContentBinaryReader(DataSource);
                if (br.ReadInt32() == MdlId)
                {
                    BinaryDataReader data = br.ReadBinaryData();

                    ReadData(data);

                    data.Close();
                }

                br.Close();
            }
        }


        public static BinaryDataWriter ToBinary(ModelBase<MeshType> mdl)
        {
            BinaryDataWriter data = new BinaryDataWriter();
            mdl.WriteData(data);
            return data;
        }

        public static void ToFile(ModelBase<MeshType> mdl, string file)
        {
            FileStream fs = new FileStream(file, FileMode.OpenOrCreate, FileAccess.Write);
            fs.SetLength(0);
            ContentBinaryWriter bw = new ContentBinaryWriter(fs);

            bw.Write(ModelData.MdlId);
            BinaryDataWriter mdlData = ToBinary(mdl);
            bw.Write(mdlData);
            mdlData.Dispose();

            bw.Close();
        }

        public static void ToStream(ModelBase<MeshType> mdl, Stream stm)
        {
            ContentBinaryWriter bw = new ContentBinaryWriter(stm, Encoding.Default);

            bw.Write(MdlId);

            BinaryDataWriter mdlData = ToBinary(mdl);
            bw.Write(mdlData);
            mdlData.Dispose();

            bw.Close();
        }
    }

    public class Model: IRenderable, IUpdatable
    {
        /// <summary>
        ///  已缓存的RenderOperation
        /// </summary>
        RenderOperation[] opBuffer;

        /// <summary>
        ///  renderOpEntId[i] 表示索引为i的renderOperation的Entity索引
        /// </summary>
        int[] renderOpEntId;

        protected ResourceHandle<ModelData> data;
        protected AnimationInstance animInstance;

        public AnimationInstance CurrentAnimation
        {
            get { return animInstance; }
            set { animInstance = value; }
        }
        public Model(ResourceHandle<ModelData> data)
        {
            CurrentAnimation = new NoAnimation(); 
            this.data = data;
        }
        protected Model()
        {
            CurrentAnimation = new NoAnimation();
        }

        #region IRenderable 成员

        public RenderOperation[] GetRenderOperation()
        {
            if (data.State != ResourceState.Loaded)
            {
                data.Touch();
                return null;
            }

            GameMesh[] entities = data.Resource.Entities;

            if (opBuffer == null)
            {
                RenderOperation[][] entOps = new RenderOperation[entities.Length][];

                int opCount = 0;
                for (int i = 0; i < entities.Length; i++)
                {
                    entOps[i] = entities[i].GetRenderOperation();
                    //for (int j = 0; j < entOps[i].Length; j++)
                    //{
                    //    animation.GetTransform(i, out entOps[i][j].Transformation);
                    //}
                    opCount += entOps[i].Length;
                }

                int dstIdx = 0;
                //gmBuffer = new GeomentryData[opCount];
                opBuffer = new RenderOperation[opCount];
                renderOpEntId = new int[opCount];

                for (int i = 0; i < entities.Length; i++)
                {
                    Array.Copy(entOps[i], 0, opBuffer, dstIdx, entOps[i].Length);

                    for (int j = 0; j < entOps[i].Length; j++)
                    {
                        renderOpEntId[dstIdx + j] = i;
                        //opBuffer[dstIdx + j].Geomentry = entOps[i][j];
                        //animation.GetTransform(i, out  opBuffer[dstIdx + j].Transformation);
                        opBuffer[i].Transformation = animInstance.GetTransform(renderOpEntId[i]);
                        opBuffer[i].Priority = RenderPriority.Second;
                    }

                    dstIdx += entOps[i].Length;
                }
            }
            else
            {
                for (int i = 0; i < opBuffer.Length; i++)
                {
                    opBuffer[i].Transformation = animInstance.GetTransform(renderOpEntId[i]);
                    //animation.GetTransform(renderOpEntId[i], out opBuffer[i].Transformation);
                }
                //animation.Animate();
            }
            return opBuffer;
        }
        public RenderOperation[] GetRenderOperation(int level)
        {
            return GetRenderOperation();
        }

        #endregion

        #region IUpdatable 成员

        public void Update(GameTime dt)
        {
            if (animInstance != null)
            {
                animInstance.Update(dt);
            }
            //GameMesh[] entities = data.Resource.Entities;
            //if (entities != null)
            //{
            //    for (int i = 0; i < entities.Length; i++)
            //    {
            //        entities[i].Update(dt);
            //    }
            //}
        }

        #endregion
    }

    /// <summary>
    ///  表示3D模型的数据
    /// </summary>
    public class ModelData : ModelBase<GameMesh>, IDisposable
    {
        protected RenderSystem renderSystem;
        [Browsable(false)]
        public RenderSystem RenderSystem
        {
            get { return renderSystem; }
        }

        public ModelData(RenderSystem renderSystem, ResourceLocation rl)
            : base(rl)
        {
            this.renderSystem = renderSystem;
        }

        public ModelData(RenderSystem renderSystem, GameMesh[] entities)
        {
            this.renderSystem = renderSystem;

            this.entities = entities;
        }
        public ModelData(RenderSystem renderSystem, int entityCount)
        {
            this.renderSystem = renderSystem;

            this.entities = new GameMesh[entityCount];
        }


        protected override void unload()
        {
            if (entities != null)
            {
                for (int i = 0; i < entities.Length; i++)
                {
                    if (!entities[i].Disposed)
                    {
                        entities[i].Dispose();
                    }
                    entities[i] = null;
                }
            }
        }

        private ModelData(RenderSystem dev)
        {
            this.renderSystem = dev;
        }

        protected override GameMesh LoadMesh(BinaryDataReader data)
        {
            MeshData md = new MeshData(renderSystem);
            md.Load(data);
            return new GameMesh(renderSystem, md);
        }
        protected override BinaryDataWriter SaveMesh(GameMesh mesh)
        {
            MeshData md = new MeshData(mesh);
            return md.Save();
        }


        #region IDisposable 成员

        protected override void dispose(bool disposing)
        {
            base.dispose(disposing);

            if (disposing)
            {
                for (int i = 0; i < entities.Length; i++)
                {
                    if (!entities[i].Disposed)
                    {
                        entities[i].Dispose();
                    }
                    entities[i] = null;
                }
            }
            entities = null;
        }

        #endregion

    }
}
