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
            throw new NotSupportedException("XNA????????????????????????????RenderControl");
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