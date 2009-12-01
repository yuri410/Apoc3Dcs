using System;
using System.Collections.Generic;
using System.Text;
using Apoc3D.Core;
using Apoc3D.Graphics;

namespace Apoc3D.Graphics
{
    /// <summary>
    ///  表示渲染目标所在的控件
    /// </summary>
    public abstract class RenderControl : IDisposable
    {
        RenderTarget renderTarget;

        PresentParameters presentParams;
        RenderSystem renderSystem;

        FpsCounter fpsCounter;

        /// <summary>
        ///  创建第一个渲染目标/渲染控件时使用这个构造函数
        /// </summary>
        /// <param name="rs"></param>
        /// <param name="pm"></param>
        /// <param name="rt">现有的RenderTarget</param>
        protected RenderControl(RenderSystem rs, PresentParameters pm, RenderTarget rt)
            : this(rs, pm)
        {
            this.renderTarget = rt;
        }

        /// <summary>
        ///  在以多渲染目标，多渲染控件情况下建立RenderControl。
        ///  会根据PresentParameters中的设定建立新的RenderTarget。
        /// </summary>
        /// <param name="rs"></param>
        /// <param name="pm"></param>
        protected RenderControl(RenderSystem rs, PresentParameters pm)
        {
            this.presentParams = pm;
            this.renderSystem = rs;
            this.fpsCounter = new FpsCounter();
        }


        public RenderTarget RenderTarget
        {
            get { return renderTarget; }
        }

        /// <summary>
        ///  为这个RenderControl建立API所对应的RenderTarget
        /// </summary>
        /// <param name="rs"></param>
        /// <param name="pm"></param>
        /// <returns></returns>
        protected abstract RenderTarget CreateRenderTarget(RenderSystem rs, PresentParameters pm);

        public virtual void Present()
        {
            fpsCounter.Update();
        }

        public float FPS
        {
            get { return fpsCounter.FPS; }
        }

        /// <summary>
        ///  ？？？
        /// </summary>
        [Obsolete()]
        public void NotifyBind()
        {
            if (renderTarget == null)
            {
                renderTarget = CreateRenderTarget(renderSystem, presentParams);
            }
        }

        #region IDisposable 成员

        public bool Disposed
        {
            get;
            private set;
        }


        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {

            }
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

        ~RenderControl()
        {
            if (!Disposed)
            {
                Dispose(false);
                Disposed = false;
            }
        }

        #endregion
    }

    public delegate void UpdateEventHandler(object sender, GameTime time);

    /// <summary>
    ///  表示渲染窗口
    /// </summary>
    /// <remarks>在游戏机上即屏幕</remarks>
    public abstract class RenderWindow : RenderControl
    {
        protected RenderWindow(RenderSystem rs, PresentParameters pm, RenderTarget rt)
            : base(rs, pm, rt)
        {
        }
        protected RenderWindow(RenderSystem rs, PresentParameters pm)
            : base(rs, pm)
        {
        }

        /// <summary>
        ///  以这个渲染窗口开始主循环
        /// </summary>
        public abstract void Run();

        public event EventHandler Begin;
        public event UpdateEventHandler Update;
        public event DrawEventHandler Draw;
        public event EventHandler End;

        protected void OnBegin() 
        {
            if (Begin != null)
                Begin(this, EventArgs.Empty);
        }

        protected void OnEnd() 
        {
            if (End != null)
                End(this, EventArgs.Empty);
        }

        protected void OnUpdate(GameTime time) 
        {
            if (Update != null) 
            {
                Update(this, time);
            }
        }

        protected void OnDraw() 
        {
            if (Draw != null)
            {
                Draw();
            }
        }
    }
}
