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
using Apoc3D.Graphics;
using Apoc3D.Graphics.Collections;
using Apoc3D.MathLib;
using X = Microsoft.Xna.Framework;
using XG = Microsoft.Xna.Framework.Graphics;

namespace Apoc3D.RenderSystem.Xna
{
    class XnaClipPlane : ClipPlane
    {
        XG.ClipPlane xplane;
        public XnaClipPlane(XG.ClipPlane xnaPlane)
        {
            this.xplane = xnaPlane;
        }

        public override bool Enabled
        {
            get { return xplane.IsEnabled; }
            set { xplane.IsEnabled = value; }
        }

        public override Plane Plane
        {
            get
            {
                X.Plane plane = xplane.Plane;
                return new Plane(plane.Normal.X, plane.Normal.Y, plane.Normal.Z, plane.D);
            }
            set
            {
                xplane.Plane = new X.Plane(value.Normal.X, value.Normal.Y, value.Normal.Z, value.D);
            }
        }
    }

    class XnaRenderStateManager : RenderStateManager
    {
        XnaRenderSystem renderSys;

        XG.RenderState xnaState
        {
            get { return device.RenderState; }
        }
        XG.GraphicsDevice device
        {
            get { return renderSys.Device; }
        }

        public XnaRenderStateManager(XnaRenderSystem rs)
            : base(rs)
        {
            this.renderSys = rs;

            XnaClipPlane[] planes = new XnaClipPlane[32];
            for (int i = 0; i < planes.Length; i++)
            {
                planes[i] = new XnaClipPlane(device.ClipPlanes[i]);
            }

            this.clipPlaneCollecion = new ClipPlaneCollection(planes);
            this.texWrapCollection = new XnaTextureWrapCollection(device);
        }

        #region Alpha混合
        /// <summary>
        ///  获取或设置Alpha混合是否开启
        /// </summary>
        public override bool AlphaBlendEnable
        {
            get { return xnaState.AlphaBlendEnable; }
            set { xnaState.AlphaBlendEnable = value; }
        }

        /// <summary>
        ///  获取或设置Alpha混合的方式
        /// </summary>
        public override BlendFunction BlendOperation
        {
            get
            {
                return XnaUtils.ConvertEnum(xnaState.BlendFunction);
            }
            set
            {
                xnaState.BlendFunction = XnaUtils.ConvertEnum(value);
            }
        }

        public override ColorValue BlendFactor
        {
            get
            {
                XG.Color v = xnaState.BlendFactor;
                return new ColorValue(v.PackedValue);
            }
            set
            {
                xnaState.BlendFactor = new XG.Color(value.R, value.G, value.B, value.A);
            }
        }

        public override Blend SourceBlend
        {
            get
            {
                return XnaUtils.ConvertEnum(xnaState.SourceBlend);
            }
            set
            {
                xnaState.SourceBlend = XnaUtils.ConvertEnum(value);
            }
        }
        public override Blend DestinationBlend
        {
            get
            {
                return XnaUtils.ConvertEnum(xnaState.DestinationBlend);
            }
            set
            {
                xnaState.DestinationBlend = XnaUtils.ConvertEnum(value);
            }
        }



        /// <summary>
        ///  获取或设置对于alpha通的混合是否开启
        /// </summary>
        public override bool AlphaBlendSeparateEnabled
        {
            get
            {
                return xnaState.SeparateAlphaBlendEnabled;
            }
            set
            {
                xnaState.SeparateAlphaBlendEnabled = value;
            }
        }
        public override BlendFunction BlendOperationAlpha
        {
            get
            {
                return XnaUtils.ConvertEnum(xnaState.AlphaBlendOperation);
            }
            set
            {
                xnaState.AlphaBlendOperation = XnaUtils.ConvertEnum(value);
            }
        }

        public override Blend SourceBlendAlpha
        {
            get
            {
                return XnaUtils.ConvertEnum(xnaState.AlphaSourceBlend);
            }
            set
            {
                xnaState.AlphaSourceBlend = XnaUtils.ConvertEnum(value);
            }
        }
        public override Blend DestinationBlendAlpha
        {
            get
            {
                return XnaUtils.ConvertEnum(xnaState.AlphaDestinationBlend);
            }
            set
            {
                xnaState.AlphaDestinationBlend = XnaUtils.ConvertEnum(value);
            }
        }
        #endregion

        #region AlphaTest
        public override bool AlphaTestEnable
        {
            get { return xnaState.AlphaTestEnable; }
            set { xnaState.AlphaTestEnable = value; }
        }
        /// <summary>
        ///  获取或设置alpha测试的方式
        /// </summary>
        public override CompareFunction AlphaFunction
        {
            get
            {
                return XnaUtils.ConvertEnum(xnaState.AlphaFunction);
            }
            set
            {
                xnaState.AlphaFunction = XnaUtils.ConvertEnum(value);
            }
        }
        public override int AlphaReference
        {
            get
            {
                return xnaState.ReferenceAlpha;
            }
            set
            {
                xnaState.ReferenceAlpha = value;
            }
        }
        #endregion

        #region ColorWriteChannels
        /// <summary>
        ///  获取或设置可以写入到RT上的颜色通道
        /// </summary>
        public override ColorWriteChannels ColorWriteChannels
        {
            get
            {
                return XnaUtils.ConvertEnum(xnaState.ColorWriteChannels);
            }
            set
            {
                xnaState.ColorWriteChannels = XnaUtils.ConvertEnum(value);
            }
        }
        /// <summary>
        ///  获取或设置可以写入到RT上的颜色通道
        /// </summary>
        public override ColorWriteChannels ColorWriteChannels1
        {
            get
            {
                return XnaUtils.ConvertEnum(xnaState.ColorWriteChannels1);
            }
            set
            {
                xnaState.ColorWriteChannels1 = XnaUtils.ConvertEnum(value);
            }
        }
        /// <summary>
        ///  获取或设置可以写入到RT上的颜色通道
        /// </summary>
        public override ColorWriteChannels ColorWriteChannels2
        {
            get
            {
                return XnaUtils.ConvertEnum(xnaState.ColorWriteChannels2);
            }
            set
            {
                xnaState.ColorWriteChannels2 = XnaUtils.ConvertEnum(value);
            }
        }
        /// <summary>
        ///  获取或设置可以写入到RT上的颜色通道
        /// </summary>
        public override ColorWriteChannels ColorWriteChannels3
        {
            get
            {
                return XnaUtils.ConvertEnum(xnaState.ColorWriteChannels3);
            }
            set
            {
                xnaState.ColorWriteChannels3 = XnaUtils.ConvertEnum(value);
            }
        }

        #endregion

        public override CullMode CullMode
        {
            get
            {
                return XnaUtils.ConvertEnum(xnaState.CullMode);
            }
            set
            {
                xnaState.CullMode = XnaUtils.ConvertEnum(value);
            }
        }

        #region Depth
        public override float DepthBias
        {
            get
            {
                return xnaState.DepthBias;
            }
            set
            {
                xnaState.DepthBias = value;
            }
        }

        public override bool DepthBufferEnable
        {
            get
            {
                return xnaState.DepthBufferEnable;
            }
            set
            {
                xnaState.DepthBufferEnable = value;
            }
        }

        public override CompareFunction DepthBufferFunction
        {
            get
            {
                return XnaUtils.ConvertEnum(xnaState.DepthBufferFunction);
            }
            set
            {
                xnaState.DepthBufferFunction = XnaUtils.ConvertEnum(value);
            }
        }

        public override bool DepthBufferWriteEnable
        {
            get
            {
                return xnaState.DepthBufferWriteEnable;
            }
            set
            {
                xnaState.DepthBufferWriteEnable = value;
            }
        }

        public override float SlopeScaleDepthBias
        {
            get
            {
                return xnaState.SlopeScaleDepthBias;
            }
            set
            {
                xnaState.SlopeScaleDepthBias = value;
            }
        }
        #endregion


        public override FillMode FillMode
        {
            get
            {
                return XnaUtils.ConvertEnum(xnaState.FillMode);
            }
            set
            {
                xnaState.FillMode = XnaUtils.ConvertEnum(value);
            }
        }

        public override bool MultiSampleAntiAlias
        {
            get
            {
                return xnaState.MultiSampleAntiAlias;
            }
            set
            {
                xnaState.MultiSampleAntiAlias = value;
            }
        }

        public override int MultiSampleMask
        {
            get
            {
                return xnaState.MultiSampleMask;
            }
            set
            {
                xnaState.MultiSampleMask = value;
            }
        }

        #region Point States
        public override float PointSize
        {
            get
            {
                return xnaState.PointSize;
            }
            set
            {
                xnaState.PointSize = value;
            }
        }

        public override float PointSizeMax
        {
            get
            {
                return xnaState.PointSizeMax;
            }
            set
            {
                xnaState.PointSizeMax = value;
            }
        }

        public override float PointSizeMin
        {
            get
            {
                return xnaState.PointSizeMin;
            }
            set
            {
                xnaState.PointSizeMin = value;
            }
        }

        public override bool PointSpriteEnable
        {
            get
            {
                return xnaState.PointSpriteEnable;
            }
            set
            {
                xnaState.PointSpriteEnable = value;
            }
        }
        #endregion

        #region ScissorTest
        public override bool ScissorTestEnable
        {
            get
            {
                return xnaState.ScissorTestEnable;
            }
            set
            {
                xnaState.ScissorTestEnable = value;
            }
        }
        public override Rectangle ScissorTestRectangle
        {
            get
            {
                X.Rectangle rect = device.ScissorRectangle;
                return new Rectangle(rect.X, rect.Y, rect.Width, rect.Height);
            }
            set
            {
                device.ScissorRectangle = new X.Rectangle(value.X, value.Y, value.Width, value.Height);
            }
        }
        #endregion

        #region Stencil
        public override int ReferenceStencil
        {
            get
            {
                return xnaState.ReferenceStencil;
            }
            set
            {
                xnaState.ReferenceStencil = value;
            }
        }
        public override StencilOperation StencilDepthBufferFail
        {
            get
            {
                return XnaUtils.ConvertEnum(xnaState.StencilDepthBufferFail);
            }
            set
            {
                xnaState.StencilDepthBufferFail = XnaUtils.ConvertEnum(value);
            }
        }
        public override bool StencilEnable
        {
            get
            {
                return xnaState.StencilEnable;
            }
            set
            {
                xnaState.StencilEnable = value;
            }
        }
        public override StencilOperation StencilFail
        {
            get
            {
                return XnaUtils.ConvertEnum(xnaState.StencilFail);
            }
            set
            {
                xnaState.StencilFail = XnaUtils.ConvertEnum(value);
            }
        }
        public override CompareFunction StencilFunction
        {
            get
            {
                return XnaUtils.ConvertEnum(xnaState.StencilFunction);
            }
            set
            {
                xnaState.StencilFunction = XnaUtils.ConvertEnum(value);
            }
        }
        public override int StencilMask
        {
            get
            {
                return xnaState.StencilMask;
            }
            set
            {
                xnaState.StencilMask = value;
            }
        }
        public override StencilOperation StencilPass
        {
            get
            {
                return XnaUtils.ConvertEnum(xnaState.StencilPass);
            }
            set
            {
                xnaState.StencilPass = XnaUtils.ConvertEnum(value);
            }
        }
        public override int StencilWriteMask
        {
            get
            {
                return xnaState.StencilWriteMask;
            }
            set
            {
                xnaState.StencilWriteMask = value;
            }
        }
        public override StencilOperation CounterClockwiseStencilDepthBufferFail
        {
            get
            {
                return XnaUtils.ConvertEnum(xnaState.CounterClockwiseStencilDepthBufferFail);
            }
            set
            {
                xnaState.CounterClockwiseStencilDepthBufferFail = XnaUtils.ConvertEnum(value);
            }
        }
        public override StencilOperation CounterClockwiseStencilFail
        {
            get
            {
                return XnaUtils.ConvertEnum(xnaState.CounterClockwiseStencilFail);
            }
            set
            {
                xnaState.CounterClockwiseStencilFail = XnaUtils.ConvertEnum(value);
            }
        }
        public override CompareFunction CounterClockwiseStencilFunction
        {
            get
            {
                return XnaUtils.ConvertEnum(xnaState.CounterClockwiseStencilFunction);
            }
            set
            {
                xnaState.CounterClockwiseStencilFunction = XnaUtils.ConvertEnum(value);
            }
        }
        public override StencilOperation CounterClockwiseStencilPass
        {
            get
            {
                return XnaUtils.ConvertEnum(xnaState.CounterClockwiseStencilPass);
            }
            set
            {
                xnaState.CounterClockwiseStencilPass = XnaUtils.ConvertEnum(value);
            }
        }
        public override bool TwoSidedStencilMode
        {
            get
            {
                return xnaState.TwoSidedStencilMode;
            }
            set
            {
                xnaState.TwoSidedStencilMode = value;
            }
        }
        #endregion

        //#region Wraps
        //public override TextureWrapCoordinates Wrap0
        //{
        //    get
        //    {
        //        return XnaUtils.ConvertEnum(xnaState.Wrap0);
        //    }
        //    set
        //    {
        //        xnaState.Wrap0 = XnaUtils.ConvertEnum(value);
        //    }
        //}
        //public override TextureWrapCoordinates Wrap1
        //{
        //    get
        //    {
        //        return XnaUtils.ConvertEnum(xnaState.Wrap1);
        //    }
        //    set
        //    {
        //        xnaState.Wrap1 = XnaUtils.ConvertEnum(value);
        //    }
        //}
        //public override TextureWrapCoordinates Wrap10
        //{
        //    get
        //    {
        //        return XnaUtils.ConvertEnum(xnaState.Wrap10);
        //    }
        //    set
        //    {
        //        xnaState.Wrap10 = XnaUtils.ConvertEnum(value);
        //    }
        //}
        //public override TextureWrapCoordinates Wrap11
        //{
        //    get
        //    {
        //        return XnaUtils.ConvertEnum(xnaState.Wrap11);
        //    }
        //    set
        //    {
        //        xnaState.Wrap11 = XnaUtils.ConvertEnum(value);
        //    }
        //}
        //public override TextureWrapCoordinates Wrap12
        //{
        //    get
        //    {
        //        return XnaUtils.ConvertEnum(xnaState.Wrap12);
        //    }
        //    set
        //    {
        //        xnaState.Wrap12 = XnaUtils.ConvertEnum(value);
        //    }
        //}
        //public override TextureWrapCoordinates Wrap13
        //{
        //    get
        //    {
        //        return XnaUtils.ConvertEnum(xnaState.Wrap13);
        //    }
        //    set
        //    {
        //        xnaState.Wrap13 = XnaUtils.ConvertEnum(value);
        //    }
        //}
        //public override TextureWrapCoordinates Wrap14
        //{
        //    get
        //    {
        //        return XnaUtils.ConvertEnum(xnaState.Wrap14);
        //    }
        //    set
        //    {
        //        xnaState.Wrap14 = XnaUtils.ConvertEnum(value);
        //    }
        //}
        //public override TextureWrapCoordinates Wrap15
        //{
        //    get
        //    {
        //        return XnaUtils.ConvertEnum(xnaState.Wrap15);
        //    }
        //    set
        //    {
        //        xnaState.Wrap15 = XnaUtils.ConvertEnum(value);
        //    }
        //}
        //public override TextureWrapCoordinates Wrap2
        //{
        //    get
        //    {
        //        return XnaUtils.ConvertEnum(xnaState.Wrap2);
        //    }
        //    set
        //    {
        //        xnaState.Wrap2 = XnaUtils.ConvertEnum(value);
        //    }
        //}
        //public override TextureWrapCoordinates Wrap3
        //{
        //    get
        //    {
        //        return XnaUtils.ConvertEnum(xnaState.Wrap3);
        //    }
        //    set
        //    {
        //        xnaState.Wrap3 = XnaUtils.ConvertEnum(value);
        //    }
        //}
        //public override TextureWrapCoordinates Wrap4
        //{
        //    get
        //    {
        //        return XnaUtils.ConvertEnum(xnaState.Wrap4);
        //    }
        //    set
        //    {
        //        xnaState.Wrap4 = XnaUtils.ConvertEnum(value);
        //    }
        //}
        //public override TextureWrapCoordinates Wrap5
        //{
        //    get
        //    {
        //        return XnaUtils.ConvertEnum(xnaState.Wrap5);
        //    }
        //    set
        //    {
        //        xnaState.Wrap5 = XnaUtils.ConvertEnum(value);
        //    }
        //}
        //public override TextureWrapCoordinates Wrap6
        //{
        //    get
        //    {
        //        return XnaUtils.ConvertEnum(xnaState.Wrap6);
        //    }
        //    set
        //    {
        //        xnaState.Wrap6 = XnaUtils.ConvertEnum(value);
        //    }
        //}
        //public override TextureWrapCoordinates Wrap7
        //{
        //    get
        //    {
        //        return XnaUtils.ConvertEnum(xnaState.Wrap7);
        //    }
        //    set
        //    {
        //        xnaState.Wrap7 = XnaUtils.ConvertEnum(value);
        //    }
        //}
        //public override TextureWrapCoordinates Wrap8
        //{
        //    get
        //    {
        //        return XnaUtils.ConvertEnum(xnaState.Wrap8);
        //    }
        //    set
        //    {
        //        xnaState.Wrap8 = XnaUtils.ConvertEnum(value);
        //    }
        //}
        //public override TextureWrapCoordinates Wrap9
        //{
        //    get
        //    {
        //        return XnaUtils.ConvertEnum(xnaState.Wrap9);
        //    }
        //    set
        //    {
        //        xnaState.Wrap9 = XnaUtils.ConvertEnum(value);
        //    }
        //}
        //#endregion
    }
}