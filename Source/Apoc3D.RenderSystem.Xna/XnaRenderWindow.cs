using System;
using System.Collections.Generic;
using System.Text;
using Apoc3D.Graphics;
using Apoc3D.MathLib;
using X = Microsoft.Xna.Framework;
using XG = Microsoft.Xna.Framework.Graphics;

namespace Apoc3D.RenderSystem.Xna
{
    class XnaRenderWindow : RenderWindow
    {
        class XGameTime : GameTime
        {
           
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
                time.SetElapsedGameTime(gameTime.ElapsedGameTime);
                time.SetElapsedRealTime((float)gameTime.ElapsedRealTime.TotalSeconds);
                time.SetFramesPerSecond(parent.FPS);
                time.SetIsRunningSlowly(gameTime.IsRunningSlowly);
                time.SetTotalGameTime(gameTime.TotalGameTime);
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
                parent.OnInitialize();

                base.Initialize();
            }
            protected override void OnExiting(object sender, EventArgs args)
            {
                parent.OnFinalize();

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
            : base(rs, pm)
        {
            this.game = new XGame(this);
            this.Tag = game;
        }

        protected override RenderTarget CreateRenderTarget(Apoc3D.Graphics.RenderSystem rs, PresentParameters pm)
        {
            throw new NotSupportedException("XNA图形渲染子系统不能建立额外的RenderControl");
        }

        public override void Run()
        {
            game.Run();
        }

        internal void SetRenderSystem(XnaRenderSystem rs)
        {
            base.RenderSystem = rs;
        }
        internal X.Game Game 
        {
            get { return game; }
        }

        public override string Title
        {
            get
            {
                return game.Window.Title;
            }
            set
            {
                game.Window.Title = value;
            }
        }

        public override Size ClientSize
        {
            get
            {
                X.Rectangle rect = game.Window.ClientBounds;
                return new Size(rect.Width, rect.Height);
            }
        }
    }
}