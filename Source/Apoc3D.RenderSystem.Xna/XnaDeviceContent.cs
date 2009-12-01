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

        // xna��ʵ��ֻ��������Ⱦ��ϵͳ����RenderWindow������ֻ���Խ���һ��
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
                throw new NotSupportedException("XNAͼ����Ⱦ��ϵͳ���ܽ��������RenderControl");
            }
            return renWnd;
        }

        public override Apoc3D.Graphics.RenderSystem RenderSystem
        {
            get { return renderSystem; }
        }

    }
}