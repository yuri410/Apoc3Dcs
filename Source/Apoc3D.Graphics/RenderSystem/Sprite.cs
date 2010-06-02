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
using System.Runtime.InteropServices;
using System.Text;
using Apoc3D.Collections;
using Apoc3D.Core;
using Apoc3D.Graphics;
using Apoc3D.MathLib;
using Code2015.Effects;

namespace Apoc3D.Graphics
{
    public abstract class Sprite : IDisposable
    {
        public abstract void Begin();
        public abstract void Begin(bool alpha);
        public abstract void End();

        public RenderSystem RenderSystem
        {
            get;
            private set;
        }

        protected Sprite(RenderSystem rs)
        {
            RenderSystem = rs;
        }
        public abstract void DrawQuad(GeomentryData quad, Code2015.Effects.PostEffect effect);

        public abstract void Draw(Texture texture, Rectangle rect, ColorValue color);
        public abstract void Draw(Texture texture, Vector2 pos, ColorValue color);
        public abstract void Draw(Texture texture, int x, int y, ColorValue color);
        public abstract void Draw(Texture texture, Rectangle dstRect, Rectangle? srcRect, ColorValue color);

        public abstract void SetTransform(Matrix matrix);
       

        #region IDisposable 成员

        public bool Disposed
        {
            get;
            protected set;
        }

        protected virtual void Dispose(bool disposing) { }

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


    public class SceneSprite : IDisposable, IRenderable
    {
        [StructLayout(LayoutKind.Sequential)]
        struct SpriteVertex
        {
            internal static VertexDeclaration vtxDecl;

            public Vector3 position;
            float dummy;
            public Vector2 texCoord;

            public SpriteVertex(Vector3 position, Vector2 texCoord)
            {
                this.position = position;
                this.texCoord = texCoord;
                dummy = 0;
            }

            //public static D3D.VertexFormat Format
            //{
            //    get { return D3D.VertexFormat.Position | D3D.VertexFormat.Diffuse; }
            //}

            public static VertexDeclaration Declaration
            {
                get { return vtxDecl; }
            }

            public static unsafe int Size
            {
                get { return sizeof(SpriteVertex); }
            }
        }

        unsafe class Entry : IDisposable
        {
            VertexBuffer buffer;
            Material material;
            GeomentryData geoData;

            public Material Material
            {
                get { return material; }
            }

            public GeomentryData Geomentry
            {
                get { return geoData; }
            }

            public Entry(RenderSystem rs, SceneSprite spr)
            {
                ObjectFactory fac = rs.ObjectFactory;

                buffer = fac.CreateVertexBuffer(5, SpriteVertex.Declaration, BufferUsage.Dynamic | BufferUsage.WriteOnly);

                material = new Material(rs);
                material.Ambient = new Color4F(1, 1, 1, 1);

                geoData = new GeomentryData();
                geoData.VertexBuffer = buffer;
                geoData.VertexSize = SpriteVertex.Size;
                geoData.VertexDeclaration = SpriteVertex.vtxDecl;
                geoData.VertexCount = 5;
                geoData.PrimitiveType = RenderPrimitiveType.TriangleStrip;
                geoData.PrimCount = 2;
            }

            public void SetVertex(Vector3 tl, Vector3 tr, Vector3 bl, Vector3 br)
            {
                SpriteVertex* dst = (SpriteVertex*)buffer.Lock(LockMode.None).ToPointer();

                dst->position = tl;
                dst->texCoord = new Vector2(0, 0);
                dst++;

                dst->position = tr;
                dst->texCoord = new Vector2(1, 0);
                dst++;

                dst->position = bl;
                dst->texCoord = new Vector2(0, 1);
                dst++;

                dst->position = br;
                dst->texCoord = new Vector2(1, 1);

                buffer.Unlock();
            }

            #region IDisposable 成员

            public void Dispose()
            {
                buffer.Dispose();
            }

            #endregion
        }

        FastList<Entry> buffers;
        FastList<RenderOperation> bufferedOp;
        int curIndex;

        StateBlock block;

        Matrix transform;

        public RenderSystem RenderSystem
        {
            get;
            private set;
        }

        public SceneSprite(RenderSystem rs)
        {
            ObjectFactory fac = rs.ObjectFactory;

            if (SpriteVertex.vtxDecl == null)
            {

                VertexElement[] elements = new VertexElement[2];
                elements[0] = new VertexElement(0, VertexElementFormat.Vector4, VertexElementUsage.Position);
                elements[1] = new VertexElement(elements[0].Size, VertexElementFormat.Vector2, VertexElementUsage.TextureCoordinate);

                SpriteVertex.vtxDecl = fac.CreateVertexDeclaration(elements);
            }

            block = fac.CreateStateBlock();

            RenderSystem = rs;
            bufferedOp = new FastList<RenderOperation>();
            buffers = new FastList<Entry>();


            transform = Matrix.Identity;
        }

        #region IRenderable 成员

        public RenderOperation[] GetRenderOperation()
        {
            return bufferedOp.Elements;
        }
        public RenderOperation[] GetRenderOperation(int level)
        {
            return bufferedOp.Elements;
        }

        #endregion

        public void Begin()
        {
            bufferedOp.Clear();
            curIndex = 0;
            block.Capture();
        }

        public void End()
        {
            block.Apply();
        }

        public void Draw(Texture texture, int color)
        {
            Draw(texture, 0, 0, color);
        }
        public void Draw(Texture texture, int x, int y, int color)
        {
            Entry buf;
            if (curIndex >= buffers.Count)
            {
                buf = new Entry(RenderSystem, this);
                buffers.Add(buf);
            }
            else
            {
                buf = buffers[curIndex];
            }

            buf.SetVertex(new Vector3(x, y, 0),
                new Vector3(x + texture.Width, y, 0),
                new Vector3(x, y + texture.Height, 0),
                new Vector3(x + texture.Width, y + texture.Height, 0));
            buf.Material.SetTexture(0, new ResourceHandle<Texture>(texture));
            buf.Material.PriorityHint = RenderPriority.Last;

            curIndex++;

            RenderOperation op;
            op.Material = buf.Material;
            op.Geomentry = buf.Geomentry;
            op.Transformation = transform;
            op.Sender = this;
            bufferedOp.Add(ref op);
        }

        public void Draw(Texture texture, Rectangle rect, int color)
        {
            Entry buf;
            if (curIndex >= buffers.Count)
            {
                buf = new Entry(RenderSystem, this);
                buffers.Add(buf);
            }
            else
            {
                buf = buffers[curIndex];
            }

            buf.SetVertex(new Vector3(rect.X, rect.Y, 0),
               new Vector3(rect.X + rect.Width, rect.Y, 0),
               new Vector3(rect.X, rect.Y + rect.Height, 0),
               new Vector3(rect.X + rect.Width, rect.Y + texture.Height, 0));
            buf.Material.SetTexture(0, new ResourceHandle<Texture>(texture));
            buf.Material.PriorityHint = RenderPriority.Last;

            curIndex++;

            RenderOperation op;
            op.Material = buf.Material;
            op.Geomentry = buf.Geomentry;
            op.Transformation = transform;
            op.Sender = this;
            bufferedOp.Add(ref op);
        }


        public Matrix Transform
        {
            get { return transform; }
            set { transform = value; }
        }



        #region IDisposable 成员

        public void Dispose()
        {
            for (int i = 0; i < buffers.Count; i++)
            {
                buffers[i].Dispose();
            }
            bufferedOp.Clear();
            buffers.Clear();
            bufferedOp = null;
            buffers = null;
        }

        #endregion
    }

}
