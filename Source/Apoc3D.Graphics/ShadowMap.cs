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
http://www.gnu.org/copyleft/gpl.txt.

-----------------------------------------------------------------------------
*/
using System;
using System.Collections.Generic;
using System.Text;
using Apoc3D;
using Apoc3D.Graphics;
using Apoc3D.Graphics.Effects;
using Apoc3D.MathLib;
using Apoc3D.Media;
using Apoc3D.Vfs;
using Code2015.EngineEx;
using Code2015.World;
using Code2015.Effects;

namespace Apoc3D.Graphics
{
    public class GuassBlurFilter
    {
        public float BlurAmount;
        public int SampleCount;
        public float[] SampleWeights;
        public Vector2[] SampleOffsetsX;
        public Vector2[] SampleOffsetsY;


        public GuassBlurFilter(int sampleCount, float blurAmount, int mapWidth, int mapHeight)
        {
            this.SampleCount = sampleCount;
            this.BlurAmount = blurAmount;

            ComputeFilter(1 / (float)mapWidth, 0, out SampleWeights, out SampleOffsetsX);
            ComputeFilter(0, 1 / (float)mapHeight, out SampleWeights, out SampleOffsetsY);
        }


        void ComputeFilter(float dx, float dy, out float[] sampleWeights, out Vector2[] sampleOffsets)
        {
            // Create temporary arrays for computing our filter settings.
            sampleWeights = new float[SampleCount];
            sampleOffsets = new Vector2[SampleCount];

            // The first sample always has a zero offset.
            sampleWeights[0] = ComputeGaussian(0);
            sampleOffsets[0] = new Vector2(0);

            // Maintain a sum of all the weighting values.
            float totalWeights = sampleWeights[0];

            // Add pairs of additional sample taps, positioned
            // along a line in both directions from the center.
            for (int i = 0; i < SampleCount / 2; i++)
            {
                // Store weights for the positive and negative taps.
                float weight = ComputeGaussian(i + 1);

                sampleWeights[i * 2 + 1] = weight;
                sampleWeights[i * 2 + 2] = weight;

                totalWeights += weight * 2;

                // To get the maximum amount of blurring from a limited number of
                // pixel shader samples, we take advantage of the bilinear filtering
                // hardware inside the texture fetch unit. If we position our texture
                // coordinates exactly halfway between two texels, the filtering unit
                // will average them for us, giving two samples for the price of one.
                // This allows us to step in units of two texels per sample, rather
                // than just one at a time. The 1.5 offset kicks things off by
                // positioning us nicely in between two texels.
                float sampleOffset = i * 2 + 1.5f;

                Vector2 delta = new Vector2(dx, dy) * sampleOffset;

                // Store texture coordinate offsets for the positive and negative taps.
                sampleOffsets[i * 2 + 1] = delta;
                sampleOffsets[i * 2 + 2] = -delta;
            }

            // Normalize the list of sample weightings, so they will always sum to one.
            for (int i = 0; i < sampleWeights.Length; i++)
            {
                sampleWeights[i] /= totalWeights;
            }
        }

        float ComputeGaussian(float n)
        {
            float theta = BlurAmount;

            return (float)((1.0 / Math.Sqrt(2 * Math.PI * theta)) *
                           Math.Exp(-(n * n) / (2 * theta * theta)));
        }

    }

    /// <summary>
    ///  定义 Shadow Mapping 所需要的阴影贴图
    /// </summary>
    public class ShadowMap : UnmanagedResource, IDisposable
    {
        struct RectVertex
        {
            public Vector4 Position;

            public Vector2 TexCoord;

            static readonly VertexElement[] elements;

            static RectVertex()
            {
                elements = new VertexElement[2];
                elements[0] = new VertexElement(0, VertexElementFormat.Vector4, VertexElementUsage.PositionTransformed);
                elements[1] = new VertexElement(Vector4.SizeInBytes, VertexElementFormat.Vector2, VertexElementUsage.TextureCoordinate, 0);
            }

            public static VertexElement[] Elements
            {
                get { return elements; }
            }


            public static int Size
            {
                get { return Vector4.SizeInBytes + Vector2.SizeInBytes; }
            }
        }

        public const int ShadowMapLength = 512;

        GuassBlurFilter guassFilter;

        RenderTarget shadowRt;
        RenderTarget shadowRt2;
        RenderTarget stdRenderTarget;
        Viewport stdVp;


        VertexDeclaration vtxDecl;
        IndexBuffer indexBuffer;
        VertexBuffer smallQuad;
        GeomentryData smallQuadOp;

        GaussBlurShd gaussBlur;
        public Matrix LightProjection;
        public Matrix ViewTransform;
        public Matrix ViewProj;

        Viewport smVp;
        RenderSystem renderSys;
        ObjectFactory factory;

        public unsafe ShadowMap(RenderSystem dev)
        {
            this.renderSys = dev;
            this.factory = dev.ObjectFactory;
            this.gaussBlur = new GaussBlurShd(dev);
            this.vtxDecl = factory.CreateVertexDeclaration(RectVertex.Elements);

            LoadUnmanagedResources();

            smVp.MinZ = 0;
            smVp.MaxZ = 1;
            smVp.Height = ShadowMapLength;
            smVp.Width = ShadowMapLength;
            smVp.X = 0;
            smVp.Y = 0;
        }

        void DrawSmallQuad()
        {
            renderSys.RenderSimple(smallQuadOp);
        }

        public void End()
        {
            #region 高斯X
            renderSys.SetRenderTarget(0, shadowRt2);

            gaussBlur.Begin();


            gaussBlur.SetTexture("tex", shadowRt.GetColorBufferTexture());

            for (int i = 0; i < guassFilter.SampleCount; i++)
            {
                gaussBlur.SetValueDirect(i, ref guassFilter.SampleOffsetsX[i]);
                gaussBlur.SetValueDirect(i + 15, guassFilter.SampleWeights[i]);
            }
            //gaussBlur.SetValue("SampleOffsets", SampleOffsetsX);
            //gaussBlur.SetValue("SampleWeights", SampleWeights);

            DrawSmallQuad();

            gaussBlur.End();
            #endregion

            #region 高斯Y

            renderSys.SetRenderTarget(0, shadowRt);
            gaussBlur.Begin();
            gaussBlur.SetTexture("tex", shadowRt2.GetColorBufferTexture());

            for (int i = 0; i < guassFilter.SampleCount; i++)
            {
                gaussBlur.SetValueDirect(i, ref guassFilter.SampleOffsetsY[i]);
                gaussBlur.SetValueDirect(i + 15, guassFilter.SampleWeights[i]);
            }
            DrawSmallQuad();

            gaussBlur.End();


            #endregion

            renderSys.Viewport = stdVp;

            renderSys.SetRenderTarget(0, stdRenderTarget);
            stdRenderTarget = null;
        }

        Matrix GetLightView(float longitude, float latitude, float h)
        {
            const float rotation = -MathEx.PIf / 6f;
            const float yaw = -MathEx.PiOver4;


            float p = h * 0.022f;

            Vector3 target = PlanetEarth.GetPosition(longitude - p * MathEx.PIf / 90f, latitude);

            Vector3 axis = target;
            axis.Normalize();

            //float sign = latitude > 0 ? 1 : -1;
            Vector3 up = Vector3.UnitY;

            Vector3 rotAxis = axis;

            Vector3 yawAxis = Vector3.Cross(axis, up);
            yawAxis.Normalize();

            Quaternion rotTrans = Quaternion.RotationAxis(rotAxis, rotation);
            axis = Vector3.TransformSimple(axis, Quaternion.RotationAxis(yawAxis, yaw) * rotTrans);

            Vector3 position = target + axis * h * 35;
            return Matrix.LookAtRH(position, target,
                Vector3.TransformSimple(up, rotTrans));

        }
      

        public void Begin(Vector3 lightDir, ICamera cam)
        {
            stdVp = renderSys.Viewport;

            renderSys.Viewport = smVp;

            stdRenderTarget = renderSys.GetRenderTarget(0);

            renderSys.SetRenderTarget(0, shadowRt);

            renderSys.Clear(ClearFlags.Target | ClearFlags.DepthBuffer, ColorValue.White, 1, 0);

            float scale = cam.GetSMScale();

            Matrix.OrthoRH((float)ShadowMapLength * scale,
               (float)ShadowMapLength * scale,
               cam.NearPlane, cam.FarPlane, out LightProjection);

            //RtsCamera rtsCamera = (RtsCamera)cam;
            //float height = 2000;// cam.Position.Length() - PlanetEarth.PlanetRadius; // rtsCamera.Height;
            //float lng, lat;
            //PlanetEarth.GetCoord(cam.Position, out lng, out lat);

            //Matrix iv = Matrix.Invert (cam.ViewMatrix );
            ViewTransform = cam.GetSMTrans();
            // GetLightView(cam.Position, iv.Forward, iv.Up, iv.Right, cam.Position.Length() - PlanetEarth.PlanetRadius); // GetLightView(lng, lat, height);

            ViewProj = ViewTransform * LightProjection;
            EffectParams.DepthViewProj = ViewProj;
        }

        public Texture ShadowColorMap
        {
            get { return shadowRt.GetColorBufferTexture(); }
        }

        protected unsafe override void loadUnmanagedResources()
        {
            shadowRt = factory.CreateRenderTarget(ShadowMapLength, ShadowMapLength, ImagePixelFormat.G32R32F, DepthFormat.Depth24X8);
            shadowRt2 = factory.CreateRenderTarget(ShadowMapLength, ShadowMapLength, ImagePixelFormat.G32R32F);

            guassFilter = new GuassBlurFilter(5, 2, ShadowMapLength, ShadowMapLength);
            #region 建立小quad

            float adj = -0.5f;
            smallQuad = factory.CreateVertexBuffer(4, vtxDecl, BufferUsage.Static);
            RectVertex* vdst = (RectVertex*)smallQuad.Lock(0, 0, LockMode.None);
            vdst[0].Position = new Vector4(adj, adj, 0, 1);
            vdst[0].TexCoord = new Vector2(0, 0);
            vdst[1].Position = new Vector4(ShadowMapLength + adj, adj, 0, 1);
            vdst[1].TexCoord = new Vector2(1, 0);
            vdst[2].Position = new Vector4(adj, ShadowMapLength + adj, 0, 1);
            vdst[2].TexCoord = new Vector2(0, 1);
            vdst[3].Position = new Vector4(ShadowMapLength + adj, ShadowMapLength + adj, 0, 1);
            vdst[3].TexCoord = new Vector2(1, 1);
            smallQuad.Unlock();

            #endregion

            indexBuffer = factory.CreateIndexBuffer(IndexBufferType.Bit32, 6, BufferUsage.Static);
            int* idst = (int*)indexBuffer.Lock(0, 0, LockMode.None);

            idst[0] = 3;
            idst[1] = 1;
            idst[2] = 0;
            idst[3] = 2;
            idst[4] = 3;
            idst[5] = 0;
            indexBuffer.Unlock();

            smallQuadOp = new GeomentryData();
            smallQuadOp.BaseIndexStart = 0;
            smallQuadOp.BaseVertex = 0;
            smallQuadOp.IndexBuffer = indexBuffer;
            smallQuadOp.PrimCount = 2;
            smallQuadOp.PrimitiveType = RenderPrimitiveType.TriangleList;
            smallQuadOp.VertexBuffer = smallQuad;
            smallQuadOp.VertexCount = 4;
            smallQuadOp.VertexDeclaration = vtxDecl;
            smallQuadOp.VertexSize = RectVertex.Size;
        }

        protected override void unloadUnmanagedResources()
        {
            shadowRt.Dispose();
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            if (disposing)
            {
                //pip.Dispose();
                //DefaultSMGen.Dispose();
            }
        }

    }
}
