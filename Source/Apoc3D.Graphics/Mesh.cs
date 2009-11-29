﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using Apoc3D.Graphics;
using Apoc3D.Graphics.Animation;
using Apoc3D.MathLib;
using Apoc3D.Vfs;

namespace Apoc3D.Graphics
{
    /// <summary>
    /// mesh的最小组成部分-三角形
    /// </summary>
    public struct MeshFace
    {
        #region Fields
        int a;
        int b;
        int c;

        int materialIdx;
        #endregion

        #region Constructors
        public MeshFace(int A, int B, int C)
        {
            a = A;
            b = B;
            c = C;
            materialIdx = -1;
        }
        public MeshFace(int A, int B, int C, int matId)
        {
            a = A;
            b = B;
            c = C;
            materialIdx = matId;
        }
        #endregion

        #region Properties
        public int IndexA
        {
            get { return a; }
            set { a = value; }
        }
        public int IndexB
        {
            get { return b; }
            set { b = value; }
        }
        public int IndexC
        {
            get { return c; }
            set { c = value; }
        }

        public int MaterialIndex
        {
            get { return materialIdx; }
            set { materialIdx = value; }
        }
        #endregion
    }

    public interface IMeshTriangleCallBack
    {
        void Process(Vector3 a, Vector3 b, Vector3 c);
    }

    public abstract unsafe class MeshData<MType>
        where MType : class
    {
        #region 常量

        public const int MeshId = ((byte)'M' << 24) | ((byte)'E' << 16) | ((byte)'S' << 8) | ((byte)'H');

        protected static readonly string MaterialCountTag = "MaterialCount";
        protected static readonly string MaterialsTag = "Materials";

        protected static readonly string MaterialAnimationTag = "MaterialAnimation";
        protected static readonly string FaceCountTag = "FaceCount";
        protected static readonly string FacesTag = "Faces";
        //protected static readonly string VertexFormatTag = "VertexFormat";
        protected static readonly string VertexDeclTag = "VertexDeclaration";
        protected static readonly string VertexCountTag = "VertexCount";
        protected static readonly string VertexSizeTag = "VertexSize";

        protected static readonly string DataTag = "VertexData";

        protected static readonly string NameTag = "Name";

        #endregion

        byte[] buffer;


        bool[] hasTexCoord = new bool[Material.MaxTexLayers];

        VertexElement[] vtxElements;

        RenderSystem renderSystem;


        protected MeshData(RenderSystem rs)
        {
            renderSystem = rs;
        }

        #region 属性

        [Browsable(false)]
        public RenderSystem Device
        {
            get { return renderSystem; }
            protected set { renderSystem = value; }
        }

        public MType[][] Materials
        {
            get;
            set;
        }

        /// <summary>
        ///  先entity后frameIndex
        /// </summary>
        public MaterialAnimationInstance[] MaterialAnimation
        {
            get;
            set;
        }

        public string Name
        {
            get;
            set;
        }

        /// <summary>
        ///  表示顶点数据
        /// </summary>
        [Browsable(false)]
        public IntPtr Data
        {
            get
            {
                IntPtr res;
                fixed (byte* ptr = &buffer[0])
                {
                    res = new IntPtr(ptr);
                }
                return res;
            }
        }

        public int VertexSize
        {
            get;
            set;
        }

        public int VertexCount
        {
            get;
            set;
        }

        public MeshFace[] Faces
        {
            get;
            set;
        }
        [Browsable(false)]
        public int TextureCoordCount
        {
            get;
            private set;
        }

        [Browsable(false)]
        public VertexElement[] VertexElements
        {
            get { return vtxElements; }
            set
            {
                vtxElements = value;

                for (int i = 0; i < hasTexCoord.Length; i++)
                    hasTexCoord[i] = false;
                TextureCoordCount = 0;

                for (int i = 0; i < value.Length; i++)
                {
                    if (value[i].Semantic == VertexElementUsage.TextureCoordinate)
                    {
                        hasTexCoord[value[i].Index] = true;
                        TextureCoordCount++;
                    }
                }
            }
        }

        public bool HasTexCoord(int index)
        {
            return hasTexCoord[index];
        }

        #endregion

        #region 方法

        //public static void BuildFromMesh(Mesh mesh, MeshData<MType> data, MType[][] mats)
        //{
        //    void* src = mesh.LockVertexBuffer(LockMode.None).ToPointer();

        //    byte[] buffer = new byte[mesh.VertexCount * mesh.BytesPerVertex];

        //    fixed (byte* dst = &buffer[0])
        //    {
        //        Memory.Copy(src, dst, buffer.Length);
        //        data.SetData(dst, buffer.Length);
        //    }

        //    mesh.UnlockVertexBuffer();

        //    data.device = mesh.Device;
        //    data.Format = mesh.VertexFormat;
        //    data.Materials = mats;
        //    data.MaterialAnimation = new MaterialAnimationInstance[mats.Length]; //{ new MaterialAnimationInstance(matAnimData) };
        //    for (int i = 0; i < mats.Length; i++)
        //    {
        //        MaterialAnimation matAnimData = new MaterialAnimation(mats[i].Length, 0.025f);
        //        data.MaterialAnimation[i] = new MaterialAnimationInstance(matAnimData);
        //    }
        //    data.VertexSize = mesh.BytesPerVertex;
        //    data.VertexCount = mesh.VertexCount;

        //    VertexElement[] elements = D3DX.DeclaratorFromFVF(mesh.VertexFormat);

        //    data.VertexElements = new VertexElement[elements.Length];
        //    Array.Copy(elements, data.VertexElements, elements.Length);

        //    int faceCount = mesh.FaceCount;

        //    data.Faces = new MeshFace[faceCount];

        //    uint* ab = (uint*)mesh.LockAttributeBuffer(LockMode.ReadOnly).ToPointer();

        //    if ((mesh.CreationOptions & MeshFlags.Use32Bit) == MeshFlags.Use32Bit)
        //    {
        //        uint* ib = (uint*)mesh.LockIndexBuffer(LockMode.ReadOnly).ToPointer();
        //        for (int i = 0; i < faceCount; i++)
        //        {
        //            int idxId = i * 3;

        //            data.Faces[i] = new MeshFace((int)ib[idxId], (int)ib[idxId + 1], (int)ib[idxId + 2], (int)ab[i]);
        //        }
        //        mesh.UnlockIndexBuffer();
        //    }
        //    else
        //    {
        //        ushort* ib = (ushort*)mesh.LockIndexBuffer(LockMode.ReadOnly).ToPointer();
        //        for (int i = 0; i < faceCount; i++)
        //        {
        //            int idxId = i * 3;

        //            data.Faces[i] = new MeshFace(ib[idxId], ib[idxId + 1], ib[idxId + 2], (int)ab[i]);
        //        }
        //        mesh.UnlockIndexBuffer();
        //    }

        //    mesh.UnlockAttributeBuffer();

        //}

        protected abstract MType LoadMaterial(RenderSystem renderSystem, BinaryDataReader matData);
        protected abstract BinaryDataWriter SaveMaterial(MType mat);

        public void SetData(void* data, int size)
        {
            if (buffer == null || buffer.Length != size)
            {
                buffer = new byte[size];
            }
            Memory.Copy(data, this.Data.ToPointer(), size);
        }

        /// <summary>
        /// 从一个BinaryDataReader对象载入mesh数据
        /// </summary>
        /// <param name="data"></param>
        public virtual void Load(BinaryDataReader data)
        {
            int materialCount = data.GetDataInt32(MaterialCountTag);
            Materials = new MType[materialCount][];

            ContentBinaryReader br = data.GetData(MaterialsTag);
            for (int i = 0; i < materialCount; i++)
            {
                int frameCount = br.ReadInt32();

                Materials[i] = new MType[frameCount];

                for (int j = 0; j < frameCount; j++)
                {
                    BinaryDataReader matData = br.ReadBinaryData();
                    Materials[i][j] = LoadMaterial(renderSystem, matData);
                    matData.Close();
                }
            }
            br.Close();

            MaterialAnimation = new MaterialAnimationInstance[materialCount];
            br = data.GetData(MaterialAnimationTag);
            for (int i = 0; i < materialCount; i++)
            {
                int frameCount = br.ReadInt32();
                float frameLength = br.ReadSingle();

                MaterialAnimation animData = new MaterialAnimation(frameCount, frameLength);
                MaterialAnimation[i] = new MaterialAnimationInstance(animData);
            }
            br.Close();

            br = data.GetData(NameTag);
            Name = br.ReadStringUnicode();
            br.Close();

            #region 读取面
            int faceCount = data.GetDataInt32(FaceCountTag);
            Faces = new MeshFace[faceCount];

            br = data.GetData(FacesTag);
            for (int i = 0; i < faceCount; i++)
            {
                Faces[i].IndexA = br.ReadInt32();
                Faces[i].IndexB = br.ReadInt32();
                Faces[i].IndexC = br.ReadInt32();

                Faces[i].MaterialIndex = br.ReadInt32();
            }
            br.Close();
            #endregion

            #region 读取顶点声明元素
            br = data.GetData(VertexDeclTag);

            int elemCount = br.ReadInt32();
            VertexElement[] elements = new VertexElement[elemCount];

            for (int i = 0; i < elemCount; i++)
            {
                int emOfs = br.ReadInt32();
                VertexElementFormat emFormat = (VertexElementFormat)br.ReadInt32();
                VertexElementUsage emUsage = (VertexElementUsage)br.ReadInt32();
                int emIndex = br.ReadInt32();
                elements[i] = new VertexElement(emOfs, emFormat, emUsage, emIndex);
            }
            VertexElements = elements;

            br.Close();
            #endregion

            if (data.Contains(VertexSizeTag))
            {
                VertexSize = data.GetDataInt32(VertexSizeTag);
            }
            else
            {
                VertexSize = MeshData.ComputeVertexSize(VertexElements);
            }
            VertexCount = data.GetDataInt32(VertexCountTag);

            br = data.GetData(DataTag);
            buffer = br.ReadBytes(VertexSize * VertexCount);
            br.Close();
        }

        /// <summary>
        /// 保存mesh数据到一个BinaryDataWriter对象中
        /// </summary>
        /// <returns></returns>
        public virtual BinaryDataWriter Save()
        {
            int materialCount = Materials.Length;

            BinaryDataWriter data = new BinaryDataWriter();

            data.AddEntry(MaterialCountTag, materialCount);

            ContentBinaryWriter bw = data.AddEntry(MaterialsTag);
            for (int i = 0; i < Materials.Length; i++)
            {
                int frameCount = Materials[i].Length;

                bw.Write(frameCount);
                for (int j = 0; j < frameCount; j++)
                {
                    BinaryDataWriter matData = SaveMaterial(Materials[i][j]);
                    bw.Write(matData);
                    matData.Dispose();
                }
            }
            bw.Close();

            bw = data.AddEntry(MaterialAnimationTag);
            for (int i = 0; i < MaterialAnimation.Length; i++)
            {
                bw.Write(MaterialAnimation[i].Data.FrameCount);
                bw.Write(MaterialAnimation[i].Data.FrameLength);
            }
            bw.Close();

            bw = data.AddEntry(VertexDeclTag);

            bw.Close();


            bw = data.AddEntry(NameTag);
            bw.WriteStringUnicode(Name);
            bw.Close();

            data.AddEntry(FaceCountTag, Faces.Length);

            #region 保存表面
            bw = data.AddEntry(FacesTag);
            for (int i = 0; i < Faces.Length; i++)
            {
                bw.Write(Faces[i].IndexA);
                bw.Write(Faces[i].IndexB);
                bw.Write(Faces[i].IndexC);
                bw.Write(Faces[i].MaterialIndex);
            }
            bw.Close();
            #endregion

            #region 保存顶点声明元素
            bw = data.AddEntry(VertexDeclTag);

            bw.Write(VertexElements.Length);
            for (int i = 0; i < VertexElements.Length; i++)
            {
                bw.Write(VertexElements[i].Offset);
                bw.Write((int)VertexElements[i].Type);
                bw.Write((int)VertexElements[i].Semantic);
                bw.Write(VertexElements[i].Index);
            }

            bw.Close();
            #endregion

            data.AddEntry(VertexSizeTag, VertexSize);
            data.AddEntry(VertexCountTag, VertexCount);

            if (VertexSize * VertexCount != buffer.Length)
            {
                throw new InvalidDataException();
            }

            bw = data.AddEntry(DataTag);
            bw.Write(buffer);
            bw.Close();

            return data;
        }


        /// <summary>
        /// 从流中读取数据
        /// </summary>
        /// <param name="stm"></param>
        public void Load(Stream stm)
        {
            ContentBinaryReader br = new ContentBinaryReader(stm, Encoding.Default);

            int id = br.ReadInt32();

            if (id == (int)MeshId)
            {
                BinaryDataReader data = br.ReadBinaryData();
                Load(data);
                data.Close();
            }
            else
            {
                br.Close();
                throw new InvalidDataException(stm.ToString());
            }

            br.Close();
        }

        /// <summary>
        /// 保存数据到流中
        /// </summary>
        /// <param name="stm"></param>
        public void Save(Stream stm)
        {
            ContentBinaryWriter bw = new ContentBinaryWriter(stm, Encoding.Default);

            bw.Write((int)MeshId);
            BinaryDataWriter data = Save();
            bw.Write(data);
            data.Dispose();

            bw.Close();
        }

        #endregion
    }

    public unsafe class MeshData : MeshData<Material>
    {
        public MeshData(RenderSystem dev)
            : base(dev)
        {
        }

        public MeshData(GameMesh mesh)
            : base(mesh.RenderSystem)
        {
            this.Materials = mesh.Materials;
            this.MaterialAnimation = mesh.MaterialAnimation;
            this.Name = mesh.Name;
            this.VertexCount = mesh.VertexCount;
            this.VertexSize = mesh.VertexSize;

            void* src = mesh.VertexBuffer.Lock(0, 0, LockMode.ReadOnly).ToPointer();

            SetData(src, VertexSize * VertexCount);

            mesh.VertexBuffer.Unlock();

            GetFaces(mesh);
        }

        public static int ComputeVertexSize(VertexElement[] elements)
        {
            int vertexSize = 0;
            for (int i = 0; i < elements.Length; i++)
            {
                vertexSize += elements[i].Size;
            }
            return vertexSize;
        }

        void GetFaces(GameMesh mesh)
        {
            IndexBuffer[] ibs = mesh.IndexBuffers;

            Faces = new MeshFace[mesh.PrimitiveCount];

            int[] partPrimCount = mesh.PartPrimitiveCount;

            int faceIdx = 0;

            for (int i = 0; i < ibs.Length; i++)
            {
                if (ibs[i].BufferType == IndexBufferType.Bit16)
                {
                    ushort* isrc = (ushort*)ibs[i].Lock(0, 0, LockMode.ReadOnly);

                    for (int j = 0; j < partPrimCount[i]; j++)
                    {
                        int iPos = j * 3;
                        Faces[faceIdx++] = new MeshFace(isrc[iPos], isrc[iPos + 1], isrc[iPos + 2], i);
                    }

                    ibs[i].Unlock();
                }
                else
                {
                    int* isrc = (int*)ibs[i].Lock(0, 0, LockMode.ReadOnly);

                    for (int j = 0; j < partPrimCount[i]; j++)
                    {
                        int iPos = j * 3;
                        Faces[faceIdx++] = new MeshFace(isrc[iPos], isrc[iPos + 1], isrc[iPos + 2], i);
                    }

                    ibs[i].Unlock();
                }
            }
        }

        protected override Material LoadMaterial(RenderSystem device, BinaryDataReader matData)
        {
            return Material.FromBinary(device, matData);
        }
        protected override BinaryDataWriter SaveMaterial(Material mat)
        {
            return Material.ToBinary(mat);
        }
    }

    /// <summary>
    /// 游戏中的mesh
    /// </summary>
    public unsafe class GameMesh : IRenderable, IDisposable, IUpdatable
    {
        #region Fields
        VertexDeclaration vtxDecl;
        int vertexSize;

        RenderOperation[] bufferedOp;

        protected Material[][] materials;
        protected MaterialAnimationInstance[] matAnims;

        protected VertexBuffer vertexBuffer;
        protected IndexBuffer[] indexBuffers;
        protected int[] partPrimCount;
        protected int[] partVtxCount;

        RenderSystem renderSystem;
        ObjectFactory factory;
        string name;

        bool disposed;

        int primCount;
        int vertexCount;
        #endregion

        //#region APIMesh
        //public static Mesh BuildMesh(Device dev, int vertexCount, int faceCount, VertexFormat format)
        //{
        //    bool useIndex16 = vertexCount <= ushort.MaxValue;
        //    Mesh mesh;
        //    if (useIndex16)
        //    {
        //        mesh = new Mesh(dev, faceCount, vertexCount, MeshFlags.Managed, format);
        //    }
        //    else
        //    {
        //        mesh = new Mesh(dev, faceCount, vertexCount, MeshFlags.Managed | MeshFlags.Use32Bit, format);
        //    }
        //    return mesh;
        //}

        ///// <summary>
        ///// Build a Mesh object from GameMeshDataBase
        ///// </summary>
        ///// <typeparam name="MType"></typeparam>
        ///// <param name="dev"></param>
        ///// <param name="sounds"></param>
        ///// <returns></returns>
        //public static Mesh BuildMeshFromData<MType>(Device dev, MeshData<MType> data)
        //    where MType : class
        //{
        //    Mesh mesh;

        //    int matCount = data.Materials.Length;
        //    int vertexCount = data.VertexCount;
        //    int faceCount = data.Faces.Length;

        //    bool useIndex16 = vertexCount <= ushort.MaxValue;

        //    if (useIndex16)
        //    {
        //        mesh = new Mesh(dev, faceCount, vertexCount, MeshFlags.Managed, data.Format);
        //    }
        //    else
        //    {
        //        mesh = new Mesh(dev, faceCount, vertexCount, MeshFlags.Managed | MeshFlags.Use32Bit, data.Format);
        //    }

        //    void* vdst = mesh.LockVertexBuffer(LockMode.None).ToPointer();

        //    Memory.Copy(data.Data.ToPointer(), vdst, vertexCount * data.VertexSize);

        //    mesh.UnlockVertexBuffer();



        //    List<int>[] indices = new List<int>[matCount];
        //    for (int i = 0; i < matCount; i++)
        //    {
        //        indices[i] = new List<int>();
        //    }


        //    MeshFace[] faces = data.Faces;
        //    for (int i = 0; i < faces.Length; i++)
        //    {
        //        MeshFace face = faces[i];
        //        int matId = face.MaterialIndex;

        //        indices[matId].Add(face.IndexA);
        //        indices[matId].Add(face.IndexB);
        //        indices[matId].Add(face.IndexC);
        //    }


        //    uint* ab = (uint*)mesh.LockAttributeBuffer(LockMode.None).ToPointer();

        //    if (useIndex16)
        //    {
        //        ushort* ib = (ushort*)mesh.LockIndexBuffer(LockMode.None).ToPointer();

        //        int faceIdx = 0;
        //        for (int i = 0; i < matCount; i++)
        //        {
        //            List<int> idx = indices[i];
        //            for (int j = 0; j < idx.Count; j += 3)
        //            {
        //                ab[faceIdx] = (uint)i;

        //                int ibIdx = faceIdx * 3;
        //                ib[ibIdx] = (ushort)idx[j];
        //                ib[ibIdx + 1] = (ushort)idx[j + 1];
        //                ib[ibIdx + 2] = (ushort)idx[j + 2];

        //                faceIdx++;
        //            }
        //        }

        //        mesh.UnlockIndexBuffer();
        //    }
        //    else
        //    {
        //        uint* ib = (uint*)mesh.LockIndexBuffer(LockMode.None).ToPointer();

        //        int faceIdx = 0;
        //        for (int i = 0; i < matCount; i++)
        //        {
        //            List<int> idx = indices[i];
        //            for (int j = 0; j < idx.Count; j += 3)
        //            {
        //                ab[faceIdx] = (uint)i;

        //                int ibIdx = faceIdx * 3;
        //                ib[ibIdx] = (ushort)idx[j];
        //                ib[ibIdx + 1] = (ushort)idx[j + 1];
        //                ib[ibIdx + 2] = (ushort)idx[j + 2];

        //                faceIdx++;
        //            }
        //        }

        //        mesh.UnlockIndexBuffer();
        //    }
        //    mesh.UnlockAttributeBuffer();
        //    return mesh;
        //}
        //#endregion

        #region 获取网格数据

        public VertexBuffer VertexBuffer
        {
            get { return vertexBuffer; }
        }

        public IndexBuffer[] IndexBuffers
        {
            get { return indexBuffers; }
        }

        public int[] GetIndices()
        {
            int[] indices = new int[PrimitiveCount * 3];

            int index = 0;

            for (int i = 0; i < indexBuffers.Length; i++)
            {
                if (indexBuffers[i].BufferType == IndexBufferType.Bit16)
                {
                    ushort* isrc = (ushort*)indexBuffers[i].Lock(0, 0, LockMode.ReadOnly);

                    for (int j = 0; j < partPrimCount[i]; j++)
                    {
                        int iPos = j * 3;

                        indices[index++] = isrc[iPos];
                        indices[index++] = isrc[iPos + 1];
                        indices[index++] = isrc[iPos + 2];

                        //faces[faceIdx++] = new MeshFace(isrc[iPos], isrc[iPos + 1], isrc[iPos + 2], i);
                    }

                    indexBuffers[i].Unlock();
                }
                else
                {
                    int* isrc = (int*)indexBuffers[i].Lock(0, 0, LockMode.ReadOnly);

                    for (int j = 0; j < partPrimCount[i]; j++)
                    {
                        int iPos = j * 3;

                        indices[index++] = isrc[iPos];
                        indices[index++] = isrc[iPos + 1];
                        indices[index++] = isrc[iPos + 2];
                    }

                    indexBuffers[i].Unlock();
                }
            }
            return indices;
        }

        #endregion

        #region 方法

        public void ProcessAllTriangles(IMeshTriangleCallBack cbk)
        {
            int vertexCount = VertexCount;
            int vertexSize = VertexSize;
            int[] indices = GetIndices();

            VertexBuffer vb = VertexBuffer;

            byte* src = (byte*)vb.Lock(0, 0, LockMode.ReadOnly).ToPointer();

            for (int i = 0; i < indices.Length; i += 3)
            {
                cbk.Process(*((Vector3*)(src + vertexSize * indices[i])),
                            *((Vector3*)(src + vertexSize * indices[i + 1])),
                            *((Vector3*)(src + vertexSize * indices[i + 2])));
            }

            vb.Unlock();
        }

        public int CalculateSizeInBytes()
        {
            int size = vertexBuffer.Size;

            for (int i = 0; i < indexBuffers.Length; i++)
            {
                size += indexBuffers[i].Size;
            }

            return size + 256;
        }

        /// <summary>
        ///  从网格数据创建网格
        /// </summary>
        /// <param name="dev"></param>
        /// <param name="data">网格数据</param>
        public unsafe void BuildFromData(RenderSystem rs, MeshData data)
        {
            this.name = data.Name;

            this.materials = data.Materials;
            this.matAnims = data.MaterialAnimation;

            int matCount = data.Materials.Length;
            int vertexCount = data.VertexCount;
            int faceCount = data.Faces.Length;

            this.primCount = faceCount;
            this.vertexCount = vertexCount;

            this.vertexSize = data.VertexSize;

            this.vtxDecl = factory.CreateVertexDeclaration(data.VertexElements);

            #region 复制顶点数据
            this.vertexBuffer = factory.CreateVertexBuffer(vertexCount, vtxDecl, BufferUsage.Static);

            void* vdst = vertexBuffer.Lock(0, 0, LockMode.None).ToPointer();

            Memory.Copy(data.Data.ToPointer(), vdst, vertexSize * vertexCount);

            vertexBuffer.Unlock();
            #endregion

            #region 建立索引数据
            bool useIndex16 = vertexCount <= ushort.MaxValue;

            List<int>[] indices = new List<int>[matCount];
            for (int i = 0; i < matCount; i++)
            {
                indices[i] = new List<int>();
            }

            partPrimCount = new int[matCount];
            partVtxCount = new int[matCount];


            MeshFace[] faces = data.Faces;
            for (int i = 0; i < faces.Length; i++)
            {
                MeshFace face = faces[i];
                int matId = face.MaterialIndex;

                indices[matId].Add(face.IndexA);
                indices[matId].Add(face.IndexB);
                indices[matId].Add(face.IndexC);
                partPrimCount[matId]++;
            }

            bool[] passed = new bool[vertexCount];
            if (useIndex16)
            {
                indexBuffers = new IndexBuffer[matCount];

                for (int i = 0; i < matCount; i++)
                {
                    fixed (void* dst = &passed[0])
                    {
                        Memory.Zero(dst, vertexCount);
                    }

                    List<int> idx = indices[i];
                    indexBuffers[i] = factory.CreateIndexBuffer(IndexBufferType.Bit16, idx.Count, BufferUsage.Static);

                    ushort* ib = (ushort*)indexBuffers[i].Lock(0, 0, LockMode.None);
                    for (int j = 0; j < idx.Count; j++)
                    {
                        ib[j] = (ushort)idx[j];
                        passed[idx[j]] = true;
                    }
                    indexBuffers[i].Unlock();


                    int vtxCount = 0;
                    for (int j = 0; j < vertexCount; j++)
                        if (passed[j])
                            vtxCount++;
                    partVtxCount[i] = vtxCount;
                }
            }
            else
            {
                indexBuffers = new IndexBuffer[matCount];

                for (int i = 0; i < matCount; i++)
                {
                    fixed (void* dst = &passed[0])
                    {
                        Memory.Zero(dst, vertexCount);
                    }
                    List<int> idx = indices[i];
                    indexBuffers[i] = factory.CreateIndexBuffer(IndexBufferType.Bit32, idx.Count, BufferUsage.Static);

                    uint* ib = (uint*)indexBuffers[i].Lock(0, 0, LockMode.None);
                    for (int j = 0; j < idx.Count; j++)
                    {
                        ib[j] = (uint)idx[j];
                    }
                    indexBuffers[i].Unlock();

                    int vtxCount = 0;
                    for (int j = 0; j < vertexCount; j++)
                        if (passed[j])
                            vtxCount++;
                    partVtxCount[i] = vtxCount;
                }
            }
            #endregion
        }

        #endregion

        #region 静态方法

        /// <summary>
        ///  从流创建一个网格(Mesh)
        /// </summary>
        /// <param name="dev"></param>
        /// <param name="stm"></param>
        /// <returns></returns>
        public static GameMesh FromStream(RenderSystem dev, Stream stm)
        {
            MeshData data = new MeshData(dev);
            data.Load(stm);
            return new GameMesh(dev, data);
        }

        /// <summary>
        ///  将网格(Mesh)保存到流中
        /// </summary>
        /// <param name="mesh">网格</param>
        /// <param name="stm">要保存到的流</param>
        public static void ToStream(GameMesh mesh, Stream stm)
        {
            MeshData data = new MeshData(mesh);
            data.Save(stm);
        }

        #endregion

        #region 构造函数

        /// <summary>
        ///  从网格数据创建网格
        /// </summary>
        /// <param name="dev"></param>
        /// <param name="data"></param>
        public GameMesh(RenderSystem rs, MeshData data)
        {
            this.renderSystem = rs;
            this.factory = rs.ObjectFactory;

            BuildFromData(rs, data);
        }

        public GameMesh(RenderSystem rs, VertexPNT1[] vertices, int[] indices, Material[][] materials)
        {
            this.factory = rs.ObjectFactory;

            this.renderSystem = rs;
            this.vtxDecl = factory.CreateVertexDeclaration(VertexPNT1.Elements);
            this.materials = materials;

            this.matAnims = new MaterialAnimationInstance[materials.Length];

            this.vertexSize = sizeof(VertexPNT1);
            int vbSize = vertexSize * vertices.Length;

            this.vertexBuffer = factory.CreateVertexBuffer(vertices.Length, vtxDecl, BufferUsage.Static);

            void* vdst = vertexBuffer.Lock(0, 0, LockMode.None).ToPointer();

            fixed (VertexPNT1* src = &vertices[0])
            {
                Memory.Copy(src, vdst, vbSize);
            }

            this.vertexBuffer.Unlock();


            int ibSize = sizeof(uint) * indices.Length;

            this.indexBuffers = new IndexBuffer[1];
            this.indexBuffers[0] = factory.CreateIndexBuffer(IndexBufferType.Bit32, indices.Length, BufferUsage.Static);

            void* idst = indexBuffers[0].Lock(0, 0, LockMode.None).ToPointer();

            this.indexBuffers[0].Lock(0, 0, LockMode.None);

            fixed (int* src = &indices[0])
            {
                Memory.Copy(src, idst, ibSize);
            }

            this.indexBuffers[0].Unlock();

            partPrimCount = new int[materials.Length];
            partVtxCount = new int[materials.Length];

            partVtxCount[0] = vertices.Length;
            partPrimCount[0] = indices.Length / 3;

            this.primCount = partPrimCount[0];
            this.vertexCount = vertices.Length;

            for (int i = 0; i < materials.Length; i++)
            {
                this.matAnims[i] = new MaterialAnimationInstance(new MaterialAnimation(1, 1));
            }
        }

        #endregion

        #region 属性

        public int[] PartPrimitiveCount
        {
            get { return partPrimCount; }
        }

        public MaterialAnimationInstance[] MaterialAnimation
        {
            get { return matAnims; }
        }
        public Material[][] Materials
        {
            get { return materials; }
        }

        public int VertexSize
        {
            get { return vertexSize; }
        }

        public int VertexCount
        {
            get { return vertexCount; }
        }
        public int PrimitiveCount
        {
            get { return primCount; }
        }

        /// <summary>
        ///  获取或设置网格的名称
        /// </summary>
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public RenderSystem RenderSystem
        {
            get { return renderSystem; }
            set { renderSystem = value; }
        }

        #endregion

        #region IRenderable 成员

        public RenderOperation[] GetRenderOperation()
        {
            if (bufferedOp == null)
            {
                //bufferedGm = new GeomentryData[materials.Length];
                bufferedOp = new RenderOperation[materials.Length];
                for (int i = 0; i < bufferedOp.Length; i++)
                {
                    GeomentryData gd = new GeomentryData(this);

                    //bufferedGm[i].Material = materials[i];
                    gd.IndexBuffer = indexBuffers[i];
                    gd.PrimCount = partPrimCount[i];
                    gd.PrimitiveType = RenderPrimitiveType.TriangleList;
                    gd.VertexBuffer = vertexBuffer;
                    gd.VertexCount = partVtxCount[i];
                    gd.VertexDeclaration = vtxDecl;
                    gd.VertexSize = vertexSize;

                    bufferedOp[i].Material = materials[i][matAnims[i].CurrentFrame];
                    bufferedOp[i].Geomentry = gd;
                    //bufferedOp[i].Transformation 
                }
            }
            return bufferedOp;
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
                bufferedOp = null;
                if (vertexBuffer != null && !vertexBuffer.Disposed)
                {
                    vertexBuffer.Dispose();
                    vertexBuffer = null;
                }
                if (indexBuffers != null)
                {
                    for (int i = 0; i < indexBuffers.Length; i++)
                    {
                        if (!indexBuffers[i].Disposed)
                        {
                            indexBuffers[i].Dispose();
                        }
                    }
                    indexBuffers = null;
                }
                if (vtxDecl != null && !vtxDecl.Disposed)
                    vtxDecl.Dispose();


                for (int i = 0; i < materials.Length; i++)
                {
                    for (int j = 0; j < materials[i].Length; j++)
                    {
                        if (materials[i][j] != Material.DefaultMaterial)
                        {
                            if (!materials[i][j].Disposed)
                            {
                                materials[i][j].Dispose();
                            }
                        }
                    }
                }
                disposed = true;
            }
            else
                throw new ObjectDisposedException(ToString());
        }

        #endregion

        #region IUpdatable 成员

        public void Update(float dt)
        {
            for (int i = 0; i < matAnims.Length; i++)
            {
                matAnims[i].Update(dt);
            }
        }

        #endregion

        ~GameMesh()
        {
            if (!disposed)
                Dispose();
        }

    }
}
