using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Apoc3D.Graphics;
using Apoc3D.MathLib;
using X = Microsoft.Xna.Framework;
using XG = Microsoft.Xna.Framework.Graphics;

namespace Apoc3D.RenderSystem.Xna
{
    class XnaSamplerState : SamplerState
    {
        XG.SamplerState xnaSS;

        public XnaSamplerState(XG.SamplerState state, int index)
            : base(index)
        {
            xnaSS = state;
        }

        public override TextureAddressMode AddressU
        {
            get
            {
                return XnaUtils.ConvertEnum(xnaSS.AddressU);
            }
            set
            {
                xnaSS.AddressU = XnaUtils.ConvertEnum(value);
            }
        }

        public override TextureAddressMode AddressV
        {
            get
            {
                return XnaUtils.ConvertEnum(xnaSS.AddressV);
            }
            set
            {
                xnaSS.AddressV = XnaUtils.ConvertEnum(value);
            }
        }

        public override TextureAddressMode AddressW
        {
            get
            {
                return XnaUtils.ConvertEnum(xnaSS.AddressW);
            }
            set
            {
                xnaSS.AddressW = XnaUtils.ConvertEnum(value);
            }
        }

        public override ColorValue BorderColor
        {
            get
            {
                return new ColorValue(xnaSS.BorderColor.PackedValue);
            }
            set
            {
                xnaSS.BorderColor = new XG.Color(value.R, value.G, value.B, value.A);
            }
        }

        public override TextureFilter MagFilter
        {
            get
            {
                return XnaUtils.ConvertEnum(xnaSS.MagFilter);
            }
            set
            {
                xnaSS.MagFilter = XnaUtils.ConvertEnum(value);
            }
        }

        public override int MaxAnisotropy
        {
            get { return xnaSS.MaxAnisotropy; }
            set { xnaSS.MaxAnisotropy = value; }
        }

        public override int MaxMipLevel
        {
            get { return xnaSS.MaxMipLevel; }
            set { xnaSS.MaxMipLevel = value; }
        }

        public override TextureFilter MinFilter
        {
            get
            {
                return XnaUtils.ConvertEnum(xnaSS.MinFilter);
            }
            set
            {
                xnaSS.MinFilter = XnaUtils.ConvertEnum(value);
            }
        }

        public override TextureFilter MipFilter
        {
            get
            {
                return XnaUtils.ConvertEnum(xnaSS.MipFilter);
            }
            set
            {
                xnaSS.MipFilter = XnaUtils.ConvertEnum(value);
            }
        }

        public override float MipMapLevelOfDetailBias
        {
            get { return xnaSS.MipMapLevelOfDetailBias; }
            set { xnaSS.MipMapLevelOfDetailBias = value; }
        }
    }
}