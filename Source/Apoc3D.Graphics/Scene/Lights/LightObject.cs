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
using Apoc3D.Vfs;

namespace Apoc3D.Scene
{
    public abstract class LightObject : SceneObject
    {
        Light light;

        protected Light Light
        {
            get { return light; }
        }

        public LightObject() 
            : base(false)
        {
            light = new Light();
        }

        public LightObject(Light light)
            : base(false)
        {
            this.light = light;
        }

        #region SceneObject 序列化
        public override bool IsSerializable
        {
            get { return true; }
        }

        public override void Serialize(BinaryDataWriter data)
        {

        }
        public override string TypeTag
        {
            get { return base.TypeTag; }
        }
        #endregion

        public override RenderOperation[] GetRenderOperation()
        {
            return null;
        }
        public override void Update(GameTime dt)
        {
            
        }
    }
}
