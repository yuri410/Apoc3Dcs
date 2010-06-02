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
using System.Text;
using Apoc3D.Graphics;
using X = Microsoft.Xna.Framework;
using XG = Microsoft.Xna.Framework.Graphics;

namespace Apoc3D.RenderSystem.Xna
{
    class XnaDeviceContent : DeviceContent
    {
        internal XnaRenderSystem renderSystem;
        //XG.GraphicsDevice device;
        X.GraphicsDeviceManager manager;
        X.Game game;

        public XnaDeviceContent()
            : base(false)
        {

        }

        // xna的实现只可以由渲染子系统建立RenderWindow，并且只可以建立一个
        protected override RenderControl create(PresentParameters pm)
        {
            XnaRenderWindow renWnd;

            if (renderSystem == null)
            {
                renWnd = new XnaRenderWindow(null, pm);

                game = renWnd.Game;

                
                manager = new X.GraphicsDeviceManager(game);

                manager.MinimumPixelShaderProfile = XG.ShaderProfile.PS_2_0;
                manager.MinimumVertexShaderProfile = XG.ShaderProfile.VS_2_0;
                manager.PreferMultiSampling = false;
                manager.PreferredDepthStencilFormat = XnaUtils.ConvertEnum(pm.DepthFormat);
                manager.PreferredBackBufferFormat = XnaUtils.ConvertEnum(pm.BackBufferFormat);
                manager.PreferredBackBufferHeight = pm.BackBufferHeight;
                manager.PreferredBackBufferWidth = pm.BackBufferWidth;
                manager.SynchronizeWithVerticalRetrace = pm.PresentInterval == PresentInterval.Default;
                manager.IsFullScreen = !pm.IsWindowed;


                manager.ApplyChanges();


                renderSystem = new XnaRenderSystem(manager);
                renderSystem.Init();
                renWnd.SetRenderSystem(renderSystem);
            }
            else
            {
                throw new NotSupportedException("XNA图形渲染子系统不能建立额外的RenderControl");
            }
            return renWnd;
        }

        public override Apoc3D.Graphics.RenderSystem RenderSystem
        {
            get { return renderSystem; }
        }

    }
}