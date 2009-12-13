﻿using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using Apoc3D.Collections;
using Apoc3D.Graphics;
using Apoc3D.MathLib;
using Apoc3D.Media;

namespace Apoc3D.Graphics
{
    
    //public class Line : IRenderable
    //{
    //    [StructLayout(LayoutKind.Sequential)]
    //    struct LineVertex
    //    {
    //        internal static VertexDeclaration vtxDecl;
            
    //        public Vector3 position;
    //        float dummy;
    //        public int diffuse;

    //        public LineVertex(Vector3 position, int color)
    //        {
    //            this.position = position;
    //            this.diffuse = color;
    //            dummy = 0;
    //        }

    //        //public static D3D.VertexFormat Format
    //        //{
    //        //    get { return D3D.VertexFormat.Position | D3D.VertexFormat.Diffuse; }
    //        //}

    //        public static  VertexDeclaration Declaration
    //        {
    //            get { return vtxDecl; }
    //        }

    //        public static unsafe int Size
    //        {
    //            get { return sizeof(LineVertex); }
    //        }
    //    }

    //    unsafe class Entry
    //    {
    //        const int InitCapacity = 100;

    //        VertexBuffer bufferedVB;
    //        RenderSystem rs;

    //        int capacity;
    //        bool isLocked;

    //        LineVertex* dataPtr;

    //        public bool IsLocked
    //        {
    //            get { return isLocked; }
    //        }

    //        public int Capacity
    //        {
    //            get { return capacity; }
    //        }

    //        public Entry(RenderSystem rs)
    //        {
    //            this.rs = rs;
    //            Resize(InitCapacity);
    //        }

    //        public void Resize(int count)
    //        {
    //            if (bufferedVB != null)
    //            {
    //                bufferedVB.Dispose();
    //            }

    //            ObjectFactory fac = rs.ObjectFactory;

    //            bufferedVB = fac.CreateVertexBuffer(count, LineVertex.Declaration, BufferUsage.Dynamic);
    //            this.capacity = count;
    //        }

    //        public void Lock()
    //        {
    //            if (!isLocked)
    //            {
    //                dataPtr = (LineVertex*)bufferedVB.Lock(LockMode.None).ToPointer();

    //                isLocked = true;
    //            }
    //            else
    //            {
    //                throw new InvalidOperationException();
    //            }
    //        }
    //        public void Unlock()
    //        {
    //            if (isLocked)
    //            {
    //                dataPtr = null;
    //                isLocked = false;
    //            }
    //            else
    //            {
    //                throw new InvalidOperationException();
    //            }
    //        }
    //        public void AddVertex(Vector2[] vtx, int color)
    //        {
    //            if (!isLocked)
    //            {
    //                throw new InvalidOperationException();
    //            }

    //            int len = vtx.Length;
    //            if (capacity < vtx.Length)
    //            {
    //                int newC = capacity * 2;
    //                if (len > newC)
    //                {
    //                    newC = len;
    //                }
    //                Resize(newC);
    //            }

    //            for (int i = 0; i < len; i++)
    //            {
    //                (dataPtr + i)->position.X = vtx[i].X;
    //                (dataPtr + i)->position.Y = vtx[i].Y;
    //                (dataPtr + i)->position.Z = 0;

    //                (dataPtr + i)->diffuse = color;
    //            }
    //        }
    //    }

    //    int curIndex;

    //    FastList<Entry> buffer;
    //    FastList<RenderOperation> opBuffer;       

    //    public Line(RenderSystem rs)
    //    {
    //        if (LineVertex.vtxDecl == null)
    //        {
    //            ObjectFactory fac = rs.ObjectFactory;

    //            VertexElement[] elements = new VertexElement[2];
    //            elements[0] = new VertexElement(0, VertexElementFormat.Vector4, VertexElementUsage.Position);
    //            elements[1] = new VertexElement(elements[0].Size, VertexElementFormat.Color, VertexElementUsage.Color);

    //            LineVertex.vtxDecl = fac.CreateVertexDeclaration(elements);
    //        }

    //        RenderSystem = rs;
    //        buffer = new FastList<Entry>();
    //        opBuffer = new FastList<RenderOperation>();
    //    }

    //    public RenderSystem RenderSystem
    //    {
    //        get;
    //        private set;
    //    }

    //    public void Draw(Vector2[] vertices, ColorValue color)
    //    {
    //        Entry buf;
    //        if (curIndex >= buffer.Count)
    //        {
    //            buf = new Entry(RenderSystem);
    //            buffer.Add(buf);
    //        }
    //        else
    //        {
    //            buf = buffer[curIndex];
    //        }

    //        buf.AddVertex(vertices, (int)color.PackedValue);
    //        curIndex++;
    //    }
    //    public void Draw(Vector3[] vertices, Matrix transformation, ColorValue color)
    //    {
    //        Entry buf;
    //        if (curIndex >= buffer.Count)
    //        {
    //            buf = new Entry(RenderSystem);
    //            buffer.Add(buf);
    //        }
    //        else
    //        {
    //            buf = buffer[curIndex];
    //        }

    //        curIndex++;
    //    }
    //    public void DrawProj(Vector3[] vertices, Matrix transformation, ColorValue color)
    //    {
    //        Entry buf;
    //        if (curIndex >= buffer.Count)
    //        {
    //            buf = new Entry(RenderSystem);
    //            buffer.Add(buf);
    //        }
    //        else
    //        {
    //            buf = buffer[curIndex];
    //        }
    //        curIndex++;
    //    }
    //    public float Width
    //    {
    //        get;
    //        set;
    //    }

    //    public void Begin()
    //    {
    //        curIndex = 0;
    //        opBuffer.Clear();
    //    }

    //    public void End()
    //    {

    //    }

    //    #region IRenderable 成员

    //    public RenderOperation[] GetRenderOperation()
    //    {
    //        return opBuffer.Elements;
    //    }        

    //    #endregion
    //}
}
