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
        X.GraphicsDeviceManager manager;
        X.Game game;

        public XnaDeviceContent()
            : base(false)
        {

        }

        // xna��ʵ��ֻ��������Ⱦ��ϵͳ����RenderWindow������ֻ���Խ���һ��
        protected override RenderControl create(PresentParameters pm)
        {
            XnaRenderWindow renWnd;

            if (renderSystem == null)
            {
                renWnd = new XnaRenderWindow(null, pm);

                game = renWnd.Game;

                device = game.GraphicsDevice;

                manager = new X.GraphicsDeviceManager(game);

                manager.MinimumPixelShaderProfile = XG.ShaderProfile.PS_2_0;
                manager.MinimumVertexShaderProfile = XG.ShaderProfile.VS_2_0;
                manager.PreferMultiSampling = true;
                manager.PreferredDepthStencilFormat = XnaUtils.ConvertEnum(pm.DepthFormat);
                manager.PreferredBackBufferFormat = XnaUtils.ConvertEnum(pm.BackBufferFormat);
                manager.PreferredBackBufferHeight = pm.BackBufferHeight;
                manager.PreferredBackBufferWidth = pm.BackBufferWidth;
                manager.SynchronizeWithVerticalRetrace = pm.PresentInterval == PresentInterval.Default;
                manager.IsFullScreen = !pm.IsWindowed;

                manager.ApplyChanges();

                renderSystem = new XnaRenderSystem(device);
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