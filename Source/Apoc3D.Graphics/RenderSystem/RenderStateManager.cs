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
using Apoc3D.MathLib;
using Apoc3D.Media;
using Apoc3D.Graphics.Collections;

namespace Apoc3D.Graphics
{
    public abstract class ClipPlane 
    {
        public abstract bool Enabled
        {
            get;
            set;
        }
        public abstract Plane Plane
        {
            get;
            set;
        }
    }

    public abstract class RenderStateManager
    {
        public RenderStateManager(RenderSystem rdrSys)
        {
            RenderSystem = rdrSys;
        }

        public RenderSystem RenderSystem
        {
            get;
            private set;
        }

        protected TextureWrapCollection texWrapCollection;
        protected ClipPlaneCollection clipPlaneCollecion;

        #region AlphaTest
        public abstract bool AlphaTestEnable
        {
            get;
            set;
        }
        /// <summary>
        ///  获取或设置alpha测试的方式
        /// </summary>
        public abstract CompareFunction AlphaFunction
        {
            get;
            set;
        }
        public abstract int AlphaReference
        {
            get;
            set;
        }
        #endregion

        #region Alpha混合
        /// <summary>
        ///  获取或设置Alpha混合是否开启
        /// </summary>
        public abstract bool AlphaBlendEnable
        {
            get;
            set;
        }
        /// <summary>
        ///  获取或设置Alpha混合的方式
        /// </summary>
        public abstract BlendFunction BlendOperation
        {
            get;
            set;
        }
        public abstract Blend SourceBlend
        {
            get;
            set;
        }
        public abstract Blend DestinationBlend
        {
            get;
            set;
        }
        public abstract ColorValue BlendFactor
        {
            get;
            set;
        }

        /// <summary>
        ///  获取或设置对于alpha通的混合是否开启
        /// </summary>
        public abstract bool AlphaBlendSeparateEnabled
        {
            get;
            set;
        }
        public abstract BlendFunction BlendOperationAlpha
        {
            get;
            set;
        }
        public abstract Blend SourceBlendAlpha
        {
            get;
            set;
        }
        public abstract Blend DestinationBlendAlpha
        {
            get;
            set;
        }
        #endregion

        #region ColorWriteChannels
        /// <summary>
        ///  获取或设置可以写入到RT上的颜色通道
        /// </summary>
        public abstract ColorWriteChannels ColorWriteChannels
        {
            get;
            set;
        }
        /// <summary>
        ///  获取或设置可以写入到RT上的颜色通道
        /// </summary>
        public abstract ColorWriteChannels ColorWriteChannels1
        {
            get;
            set;
        }
        /// <summary>
        ///  获取或设置可以写入到RT上的颜色通道
        /// </summary>
        public abstract ColorWriteChannels ColorWriteChannels2
        {
            get;
            set;
        }
        /// <summary>
        ///  获取或设置可以写入到RT上的颜色通道
        /// </summary>
        public abstract ColorWriteChannels ColorWriteChannels3
        {
            get;
            set;
        }
        #endregion

        #region Depth
        public abstract float DepthBias
        {
            get;
            set;
        }
        public abstract bool DepthBufferEnable
        {
            get;
            set;
        }
        public abstract CompareFunction DepthBufferFunction
        {
            get;
            set;
        }
        public abstract bool DepthBufferWriteEnable
        {
            get;
            set;
        }
        public abstract float SlopeScaleDepthBias
        {
            get;
            set;
        }
        #endregion

        public abstract CullMode CullMode
        {
            get;
            set;
        }

        public abstract FillMode FillMode
        {
            get;
            set;
        }
        
        public abstract bool MultiSampleAntiAlias
        {
            get;
            set;
        }
        public abstract int MultiSampleMask
        {
            get;
            set;
        }
        

        #region Point States
        public abstract float PointSize
        {
            get;
            set;
        }
        public abstract float PointSizeMax
        {
            get;
            set;
        }
        public abstract float PointSizeMin
        {
            get;
            set;
        }
        public abstract bool PointSpriteEnable
        {
            get;
            set;
        }
        #endregion

        #region Stencil

        public abstract int ReferenceStencil
        {
            get;
            set;
        }
        public abstract StencilOperation StencilDepthBufferFail
        {
            get;
            set;
        }
        public abstract bool StencilEnable
        {
            get;
            set;
        }
        public abstract StencilOperation StencilFail
        {
            get;
            set;
        }
        public abstract CompareFunction StencilFunction
        {
            get;
            set;
        }
        public abstract int StencilMask
        {
            get;
            set;
        }
        public abstract StencilOperation StencilPass
        {
            get;
            set;
        }
        public abstract int StencilWriteMask
        {
            get;
            set;
        }
        public abstract bool TwoSidedStencilMode
        {
            get;
            set;
        }
        public abstract StencilOperation CounterClockwiseStencilDepthBufferFail
        {
            get;
            set;
        }
        public abstract StencilOperation CounterClockwiseStencilFail
        {
            get;
            set;
        }
        public abstract CompareFunction CounterClockwiseStencilFunction
        {
            get;
            set;
        }
        public abstract StencilOperation CounterClockwiseStencilPass
        {
            get;
            set;
        }
        #endregion

        #region ScissorTest
        public abstract bool ScissorTestEnable
        {
            get;
            set;
        }

        public abstract Rectangle ScissorTestRectangle
        {
            get;
            set;
        }
        #endregion

        public ClipPlaneCollection ClipPlanes 
        {
            get { return clipPlaneCollecion; }
        }
        public TextureWrapCollection TextureWraps
        {
            get { return texWrapCollection; }
        }
    }
}
