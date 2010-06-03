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
using Apoc3D.MathLib;
using Apoc3D.Media;

namespace Apoc3D.Graphics
{
    public struct ShaderSamplerState 
    {
        public TextureAddressMode AddressU;
        public TextureAddressMode AddressV;
        public TextureAddressMode AddressW;

        public ColorValue BorderColor;
        public TextureFilter MagFilter;
        public TextureFilter MinFilter;
        public TextureFilter MipFilter;
        public int MaxAnisotropy;
        public int MaxMipLevel;
        public float MipMapLODBias;

    }
    /// <summary>
    ///  表示采样器的状态，可以获取或设置采样器的参数
    /// </summary>
    public abstract class SamplerState
    {
        protected SamplerState(int index)
        {
            Index = index;
        }

        public int Index
        {
            get;
            private set;
        }

        public abstract TextureAddressMode AddressU
        {
            get;
            set;
        }
        public abstract TextureAddressMode AddressV
        {
            get;
            set;
        }
        public abstract TextureAddressMode AddressW
        {
            get;
            set;
        }
        public abstract ColorValue BorderColor
        {
            get;
            set;
        }
        public abstract TextureFilter MagFilter
        {
            get;
            set;
        }
        public abstract int MaxAnisotropy
        {
            get;
            set;
        }
        public abstract int MaxMipLevel
        {
            get;
            set;
        }
        public abstract TextureFilter MinFilter
        {
            get;
            set;
        }
        public abstract TextureFilter MipFilter
        {
            get;
            set;
        }
        public abstract float MipMapLODBias
        {
            get;
            set;
        }
    }
}
