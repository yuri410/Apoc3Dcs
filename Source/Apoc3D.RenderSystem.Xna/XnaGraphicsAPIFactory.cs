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
using Apoc3D.Collections;
using XG = Microsoft.Xna.Framework;

namespace Apoc3D.RenderSystem.Xna
{
    public class XnaGraphicsAPIFactory : GraphicsAPIFactory
    {
        internal static readonly string APIName = "XNA3.0";

        static GraphicsAPIDescription desc;

        static XnaGraphicsAPIFactory()
        {
            PlatformAPISupport[] platforms = new PlatformAPISupport[2];
            platforms[0] = new PlatformAPISupport(100, "XBox");
            platforms[1] = new PlatformAPISupport(50, "Windows");
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