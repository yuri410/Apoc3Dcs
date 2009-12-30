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