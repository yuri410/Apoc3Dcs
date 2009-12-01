using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Apoc3D.Graphics;
using X = Microsoft.Xna.Framework;
using XG = Microsoft.Xna.Framework.Graphics;

namespace Apoc3D.RenderSystem.Xna
{
    class XnaDeviceContent : DeviceContent
    {
        internal XnaRenderSystem renderSystem;
        XG.GraphicsDevice device;
        X.Game game;

        // xna的实现只可以由渲染子系统建立RenderWindow，并且只可以建立一个
        protected override RenderControl create(PresentParameters pm)
        {
            RenderWindow renWnd;

            if (renderSystem == null)
            {
                game = new X.Game();

                device = game.GraphicsDevice;

                renderSystem = new XnaRenderSystem(device);

                XnaRenderTarget xnaRt = new XnaRenderTarget(renderSystem, pm.BackBufferWidth, pm.BackBufferHeight);

                renWnd = new XnaRenderWindow(game, renderSystem, pm);
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