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
using Apoc3D.MathLib;
using X = Microsoft.Xna.Framework;
using XG = Microsoft.Xna.Framework.Graphics;

namespace Apoc3D.RenderSystem.Xna
{
    class XnaSamplerState : SamplerState
    {
        XnaRenderSystem renderSys;

        public XnaSamplerState(XnaRenderSystem rs, int index)
            : base(index)
        {
            renderSys = rs;
        }

        public override TextureAddressMode AddressU
        {
            get
            {
                return XnaUtils.ConvertEnum(renderSys.Device.SamplerStates[Index].AddressU);
            }
            set
            {
                renderSys.Device.SamplerStates[Index].AddressU = XnaUtils.ConvertEnum(value);
            }
        }

        public override TextureAddressMode AddressV
        {
            get
            {
                return XnaUtils.ConvertEnum(renderSys.Device.SamplerStates[Index].AddressV);
            }
            set
            {
                renderSys.Device.SamplerStates[Index].AddressV = XnaUtils.ConvertEnum(value);
            }
        }

        public override TextureAddressMode AddressW
        {
            get
            {
                return XnaUtils.ConvertEnum(renderSys.Device.SamplerStates[Index].AddressW);
            }
            set
            {
                renderSys.Device.SamplerStates[Index].AddressW = XnaUtils.ConvertEnum(value);
            }
        }

        public override ColorValue BorderColor
        {
            get
            {
                return new ColorValue(renderSys.Device.SamplerStates[Index].BorderColor.PackedValue);
            }
            set
            {
                renderSys.Device.SamplerStates[Index].BorderColor = new XG.Color(value.R, value.G, value.B, value.A);
            }
        }

        public override TextureFilter MagFilter
        {
            get
            {
                return XnaUtils.ConvertEnum(renderSys.Device.SamplerStates[Index].MagFilter);
            }
            set
            {
                renderSys.Device.SamplerStates[Index].MagFilter = XnaUtils.ConvertEnum(value);
            }
        }

        public override int MaxAnisotropy
        {
            get { return renderSys.Device.SamplerStates[Index].MaxAnisotropy; }
            set { renderSys.Device.SamplerStates[Index].MaxAnisotropy = value; }
        }

        public override int MaxMipLevel
        {
            get { return renderSys.Device.SamplerStates[Index].MaxMipLevel; }
            set { renderSys.Device.SamplerStates[Index].MaxMipLevel = value; }
        }

        public override TextureFilter MinFilter
        {
            get
            {
                return XnaUtils.ConvertEnum(renderSys.Device.SamplerStates[Index].MinFilter);
            }
            set
            {
                renderSys.Device.SamplerStates[Index].MinFilter = XnaUtils.ConvertEnum(value);
            }
        }

        public override TextureFilter MipFilter
        {
            get
            {
                return XnaUtils.ConvertEnum(renderSys.Device.SamplerStates[Index].MipFilter);
            }
            set
            {
                renderSys.Device.SamplerStates[Index].MipFilter = XnaUtils.ConvertEnum(value);
            }
        }

        public override float MipMapLODBias
        {
            get { return renderSys.Device.SamplerStates[Index].MipMapLevelOfDetailBias; }
            set { renderSys.Device.SamplerStates[Index].MipMapLevelOfDetailBias = value; }
        }
    }
}