using System;
using System.Collections.Generic;
using System.Text;
using Apoc3D.Collections;
using Apoc3D.MathLib;
using Code2015.Effects;

namespace Apoc3D.Graphics
{
    /// <summary>
    /// Applications use the methods of the RenderSystem to perform DrawPrimitive-based rendering, create resources,
    /// work with system-level variables, adjust gamma ramp levels, work with palettes, and create shaders.
    /// </summary>
    public abstract class RenderSystem
    {
        Capabilities caps;

        int batchCount;
        int primitiveCount;
        int vertexCount;
        protected RenderMode mode;

        public RenderMode Mode
        {
            get { return mode; }
            set { mode = value; }
        }

        /// <summary>
        ///  获取渲染场景所用到的批次数量
        /// </summary>
        public int BatchCount
        {
            get { return batchCount; }
            protected set { batchCount = value; }
        }
        /// <summary>
        ///  获取渲染的场景的图元数量
        /// </summary>
        public int PrimitiveCount
        {
            get { return primitiveCount; }
            protected set { primitiveCount = value; }
        }
        /// <summary>
        ///  获取渲染的场景的顶点数量
        /// </summary>
        public int VertexCount
        {
            get { return vertexCount; }
            protected set { vertexCount = value; }
        }

        protected RenderSystem(string renderSystemName)
        {
            RenderSystemName = renderSystemName;
        }

        /// <summary>
        ///  初始化渲染系统
        /// </summary>
        public abstract void Init();


        /// <summary>
        ///  获取硬件对图形API支持的情况
        /// </summary>
        public Capabilities RenderSystemCaps
        {
            get { return caps; }
            protected set { caps = value; }
        }

        /// <summary>
        ///  获取一个<seealso cref="System.String"/>，表示渲染系统的名称
        /// </summary>
        public string RenderSystemName
        {
            get;
            private set;
        }

        /// <summary>
        ///  获取一个<see cref="ObjectFactory"/>，是用来创建图形API对象的抽象工厂
        /// </summary>
        public ObjectFactory ObjectFactory
        {
            get;
            protected set;
        }

        /// <summary>
        ///  获取一个渲染状态管理器，提供渲染状态相关的接口
        /// </summary>
        public RenderStateManager RenderStates
        {
            get;
            protected set;
        }

        /// <summary>
        ///  开始场景的渲染
        /// </summary>
        public virtual void BeginFrame()
        {
            batchCount = 0;
            primitiveCount = 0;
            vertexCount = 0;
        }

        /// <summary>
        ///  结束场景的渲染
        /// </summary>
        public virtual void EndFrame() { }

        /// <summary>
        ///  清除屏幕
        /// </summary>
        /// <param name="flags">表示清除的目标</param>
        /// <param name="color">清除颜色缓冲后的颜色</param>
        /// <param name="depth">清除深度缓冲后的深度</param>
        /// <param name="stencil">清除蒙板缓冲后的蒙板值</param>
        public abstract void Clear(ClearFlags flags, ColorValue color, float depth, int stencil);

        /// <summary>
        ///  设置RenderTarget
        /// </summary>
        /// <param name="index"></param>
        /// <param name="rt"></param>
        public abstract void SetRenderTarget(int index, RenderTarget rt);

        /// <summary>
        ///  获取RenderTarget
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public abstract RenderTarget GetRenderTarget(int index);



        /// <summary>
        ///  设置纹理
        /// </summary>
        /// <param name="index"></param>
        /// <param name="texture"></param>
        public abstract void SetTexture(int index, Texture texture);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public abstract Texture GetTexture(int index);

        /// <summary>
        ///  获取所有纹理层的Sampler状态
        /// </summary>
        /// <returns></returns>
        public abstract SamplerStateCollection GetSamplerStates();

        /// <summary>
        ///  设置顶点Shader
        /// </summary>
        /// <param name="shader"></param>
        public abstract void BindShader(VertexShader shader);

        /// <summary>
        ///  设置像素Shader
        /// </summary>
        /// <param name="shader"></param>
        public abstract void BindShader(PixelShader shader);

        /// <summary>
        ///  执行RenderOperation，渲染图形
        ///  渲染的时候必须有Pixel Shader使用，否则会引发异常
        /// </summary>
        /// <param name="op"></param>
        public virtual void Render(Material material, RenderOperation[] op)
        {
            if (op == null)
            {
                return;
            }
            for (int i = 0; i < op.Length; i++)
            {
                batchCount++;
                primitiveCount += op[i].Geomentry.PrimCount;
                vertexCount += op[i].Geomentry.VertexCount;
            }
        }

        public virtual void Render(Material material, RenderOperation[] op, int count)
        {
            if (op == null)
            {
                return;
            }
            for (int i = 0; i < count; i++)
            {
                batchCount++;
                primitiveCount += op[i].Geomentry.PrimCount;
                vertexCount += op[i].Geomentry.VertexCount;
            }
        }
        public virtual void RenderSimpleBlend(GeomentryData op)
        {
            batchCount++;
            primitiveCount += op.PrimCount;
            vertexCount += op.VertexCount;
        }
        public virtual void RenderSimple(GeomentryData op) 
        {
            batchCount++;
            primitiveCount += op.PrimCount;
            vertexCount += op.VertexCount;
        }
        public abstract Viewport Viewport { get; set; }
    }
}
