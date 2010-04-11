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

namespace Apoc3D.Graphics
{
    /// <summary>
    ///  定义 Shadow Mapping 所需要的阴影贴图
    /// </summary>
    public class ShadowMap : UnmanagedResource, IDisposable
    {
        public const int ShadowMapLength = 512;

        RenderTarget shadowRt;

        RenderTarget stdRenderTarget;
        Viewport stdVp;

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

            LoadUnmanagedResources();

            smVp.MinZ = 0;
            smVp.MaxZ = 1;
            smVp.Height = ShadowMapLength;
            smVp.Width = ShadowMapLength;
            smVp.X = 0;
            smVp.Y = 0;
        }

        public void End()
        {
            renderSys.Viewport = stdVp;
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

            RtsCamera rtsCamera = (RtsCamera)cam;
            float height = rtsCamera.Height;

            Matrix.OrthoRH((float)ShadowMapLength * height * 0.1f, 
                (float)ShadowMapLength * height * 0.1f, 
                cam.NearPlane, cam.FarPlane, out LightProjection);


            ViewTransform = GetLightView(rtsCamera.Longitude, rtsCamera.Latitude, height);
            //LightProjection = cam.ProjectionMatrix;

            //Vector3 camPos = cam.Position;
            //Vector3 up = cam.Front;


            //Vector3 lightTarget = camPos;
            //Vector3 offset = up;
            //Vector3.Multiply(ref offset, 0.5f * zFar, out offset);

            //Vector3.Add(ref lightTarget, ref offset, out lightTarget);


            //Vector3 lightPos = lightTarget;
            //offset = lightDir;
            //Vector3.Multiply(ref offset, 0.5f * zFar, out offset);
            //Vector3.Subtract(ref lightPos, ref offset, out lightPos);


            //Vector3 v1;
            //Vector3.Subtract(ref camPos, ref lPos, out v1);


            //Vector3.Cross(ref lightDir, ref v1, out up);
            //Vector3.Cross(ref lightDir, ref up, out up);
            //up.Y = 0;
            //if (up.LengthSquared() == 0)
            //{
            //    up = new Vector3(0.707f, 0, 0.707f);
            //}

            //up = Vector3.UnitY;


            //lightTarget = camPos + (cam.Front - lightDir) * 40;
            //lightTarget.Y = camPos.Y;

            //lightPos = lightTarget - lightDir * 50;

            //Matrix.LookAtRH(ref lightPos, ref lightTarget, ref up, out ViewTransform);




            ViewProj = ViewTransform * LightProjection;
            EffectParams.DepthViewProj = ViewProj;
            //Matrix proj = cam.Frustum.proj;
            //Matrix view = cam.Frustum.view;

            //device.SetTransform(TransformState.Projection, proj);
            //device.SetTransform(TransformState.View, view);


            //ShadowTransform = view * proj * TexTransform;
        }

        public Texture ShadowColorMap
        {
            get { return shadowRt.GetColorBufferTexture(); }
        }

        protected override void loadUnmanagedResources()
        {
            shadowRt = factory.CreateRenderTarget(ShadowMapLength, ShadowMapLength, ImagePixelFormat.R32F, DepthFormat.Depth24X8);
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
