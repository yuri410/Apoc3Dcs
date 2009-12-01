using System;
using System.Collections.Generic;
using System.Text;
using Apoc3D.Graphics;

namespace Apoc3D.RenderSystem.Xna
{
    public class Plugin : IPlugin
    {

        #region IPlugin 成员

        public void Load()
        {
            GraphicsAPIManager.Instance.RegisterGraphicsAPI(new XnaGraphicsAPIFactory());
        }

        public void Unload()
        {
            GraphicsAPIManager.Instance.UnregisterGraphicsAPI(XnaGraphicsAPIFactory.APIName);
        }

        public string Name
        {
            get { return "XNA Render Subsystem"; }
        }

        #endregion
    }
}
