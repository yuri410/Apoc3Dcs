using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Apoc3D.Graphics;
using XG = Microsoft.Xna.Framework;

namespace Apoc3D.RenderSystem.Xna
{
    class XnaDeviceContent : DeviceContent
    {
        internal XnaRenderSystem renderSystem;

        protected override RenderControl create(PresentParameters pm)
        {
            throw new NotImplementedException();
        }

        public override Apoc3D.Graphics.RenderSystem RenderSystem
        {
            get { return renderSystem; }
        }

    }
}