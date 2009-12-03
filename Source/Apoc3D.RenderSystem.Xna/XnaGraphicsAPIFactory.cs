using System;
using System.Collections.Generic;
using System.Text;
using Apoc3D.Graphics;
using Apoc3D.Collections;
using XG = Microsoft.Xna.Framework;

namespace Apoc3D.RenderSystem.Xna
{
    class XnaGraphicsAPIFactory : GraphicsAPIFactory
    {
        internal static readonly string APIName = "XNA3.0";

        static GraphicsAPIDescription desc;

        static XnaGraphicsAPIFactory()
        {
            PlatformAPISupport[] platforms = new PlatformAPISupport[1];
            platforms[0] = new PlatformAPISupport(100, "XBox");
            desc = new GraphicsAPIDescription(APIName, new PlatformCollection(platforms));
        }

        public XnaGraphicsAPIFactory()
            : base(desc)
        {

        }
        public override DeviceContent CreateDeviceContent()
        {
            return new XnaDeviceContent();
        }
   }
}