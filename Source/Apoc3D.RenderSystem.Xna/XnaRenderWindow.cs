using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Apoc3D.Graphics;
using X = Microsoft.Xna.Framework;
using XG = Microsoft.Xna.Framework.Graphics;

namespace Apoc3D.RenderSystem.Xna
{
    class XnaRenderWindow : RenderWindow
    {
        class XGameTime : GameTime
        {
            public void SetElapsedGameTime(float v)
            {
                ElapsedGameTime = v;
            }
            public void SetElapsedRealTime(float v)
            {
                ElapsedRealTime = v;
            }

            public void SetFramesPerSecond(float v)
            {
                FramesPerSecond = v;
            }
            public void SetIsRunningSlowly(bool v)
            {
                IsRunningSlowly = v;
            }
            public void SetTotalGameTime(float v)
            {
                TotalGameTime = v;
            }
            public void SetTotalRealTime(float v)
            {
                TotalRealTime = v;
            }
        }

        public class XGame : X.Game
        {
            XnaRenderWindow parent;
            XGameTime time;

            public XGame(XnaRenderWindow wnd)
            {
                this.parent = wnd;
                this.time = new XGameTime();
            }

            protected override void Update(X.GameTime gameTime)
            {
                base.Update(gameTime);
                time.SetElapsedGameTime((float)gameTime.ElapsedGameTime.TotalSeconds);
                time.SetElapsedRealTime((float)gameTime.ElapsedRealTime.TotalSeconds);
                time.SetFramesPerSecond(parent.FPS);
                time.SetIsRunningSlowly(gameTime.IsRunningSlowly);
                time.SetTotalGameTime((float)gameTime.TotalGameTime.TotalSeconds);
                time.SetTotalRealTime((float)gameTime.TotalRealTime.TotalSeconds);

                parent.OnUpdate(time);
            }

            protected override void Draw(X.GameTime gameTime)
            {
                base.Draw(gameTime);

                parent.OnDraw();
            }
            protected override void Initialize()
            {
                base.Initialize();

                parent.OnInitialize();
            }
            protected override void OnExiting(object sender, EventArgs args)
            {
                base.OnExiting(sender, args);
            }
            protected override void UnloadContent()
            {
                base.UnloadContent();

                parent.OnUnload();
            }
            protected override void LoadContent()
            {
                base.LoadContent();

                parent.OnLoad();
            }
        }

        XGame game;       

        public XnaRenderWindow(XnaRenderSystem rs, PresentParameters pm)
            : base(null, pm)
        {
            this.game = new XGame(this);
        }

        protected override RenderTarget CreateRenderTarget(Apoc3D.Graphics.RenderSystem rs, PresentParameters pm)
        {
            throw new NotSupportedException("XNAͼ����Ⱦ��ϵͳ���ܽ��������RenderControl");
        }

        public override void Run()
        {
            game.Run();
        }

        public X.Game Game 
        {
            get { return game; }
        }
    }
}