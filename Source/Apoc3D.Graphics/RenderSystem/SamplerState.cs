﻿using System;
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
