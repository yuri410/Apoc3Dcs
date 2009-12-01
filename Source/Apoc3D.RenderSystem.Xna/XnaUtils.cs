using System;
using System.Collections.Generic;
using System.Text;
using Apoc3D.Media;
using X = Microsoft.Xna.Framework;
using XG = Microsoft.Xna.Framework.Graphics;

namespace Apoc3D.Graphics.D3D9
{
    internal struct D3D9Utils
    {
        const int MaxFmtCount = 100;

        static XG.SurfaceFormat[] cfmtConv;
        static XG.DepthFormat[] dfmtConv;
        static ImagePixelFormat[] cfmtConv2;
        static DepthFormat[] dfmtConv2;

        static void BuildColorFormatConv()
        {
            #region AL2XNA
            cfmtConv = new XG.SurfaceFormat[(int)ImagePixelFormat.Count];

            for (int i = 0; i < cfmtConv.Length; i++)
            {
                cfmtConv[i] = XG.SurfaceFormat.Unknown;
            }

            cfmtConv[(int)ImagePixelFormat.A16B16G16R16] = XG.SurfaceFormat.Unknown;
            cfmtConv[(int)ImagePixelFormat.A16B16G16R16F] = XG.SurfaceFormat.HalfVector4;
            cfmtConv[(int)ImagePixelFormat.A1R5G5B5] = XG.SurfaceFormat.Bgra5551;
            cfmtConv[(int)ImagePixelFormat.A2B10G10R10] = XG.SurfaceFormat.Unknown;
            cfmtConv[(int)ImagePixelFormat.A2R10G10B10] = XG.SurfaceFormat.Rgba1010102;
            cfmtConv[(int)ImagePixelFormat.A32B32G32R32F] = XG.SurfaceFormat.Vector4;
            cfmtConv[(int)ImagePixelFormat.A4L4] = XG.SurfaceFormat.LuminanceAlpha8;
            cfmtConv[(int)ImagePixelFormat.A4R4G4B4] = XG.SurfaceFormat.Bgra4444;
            cfmtConv[(int)ImagePixelFormat.Alpha8] = XG.SurfaceFormat.Alpha8;
            cfmtConv[(int)ImagePixelFormat.A8B8G8R8] = XG.SurfaceFormat.Unknown;
            cfmtConv[(int)ImagePixelFormat.A8L8] = XG.SurfaceFormat.LuminanceAlpha16;
            cfmtConv[(int)ImagePixelFormat.A8R8G8B8] = XG.SurfaceFormat.Color;
            cfmtConv[(int)ImagePixelFormat.DXT1] = XG.SurfaceFormat.Dxt1;
            cfmtConv[(int)ImagePixelFormat.DXT2] = XG.SurfaceFormat.Dxt2;
            cfmtConv[(int)ImagePixelFormat.DXT3] = XG.SurfaceFormat.Dxt3;
            cfmtConv[(int)ImagePixelFormat.DXT4] = XG.SurfaceFormat.Dxt4;
            cfmtConv[(int)ImagePixelFormat.DXT5] = XG.SurfaceFormat.Dxt5;
            cfmtConv[(int)ImagePixelFormat.G16R16] = XG.SurfaceFormat.Rg32;
            cfmtConv[(int)ImagePixelFormat.G16R16F] = XG.SurfaceFormat.HalfVector2;
            cfmtConv[(int)ImagePixelFormat.G32R32F] = XG.SurfaceFormat.Vector2;
            cfmtConv[(int)ImagePixelFormat.Luminance16] = XG.SurfaceFormat.Luminance16;

            cfmtConv[(int)ImagePixelFormat.Luminance8] = XG.SurfaceFormat.Luminance8;
            cfmtConv[(int)ImagePixelFormat.R16F] = XG.SurfaceFormat.HalfSingle;
            cfmtConv[(int)ImagePixelFormat.R32F] = XG.SurfaceFormat.Single;
            cfmtConv[(int)ImagePixelFormat.R3G3B2] = XG.SurfaceFormat.Bgr233;
            cfmtConv[(int)ImagePixelFormat.R5G6B5] = XG.SurfaceFormat.Bgr565;
            cfmtConv[(int)ImagePixelFormat.R8G8B8] = XG.SurfaceFormat.Bgr24;
            cfmtConv[(int)ImagePixelFormat.R8G8B8A8] = XG.SurfaceFormat.Rgba32;
            cfmtConv[(int)ImagePixelFormat.X8B8G8R8] = XG.SurfaceFormat.Rgb32;
            cfmtConv[(int)ImagePixelFormat.X8R8G8B8] = XG.SurfaceFormat.Bgr32;
            #endregion

            #region XNA2AL
            cfmtConv2 = new ImagePixelFormat[MaxFmtCount];
            for (int i = 0; i < MaxFmtCount; i++)
            {
                cfmtConv2[i] = ImagePixelFormat.Unknown;
            }
            cfmtConv2[(int)XG.SurfaceFormat.Alpha8 + 1] = ImagePixelFormat.Alpha8;
            cfmtConv2[(int)XG.SurfaceFormat.Bgr233 + 1] = ImagePixelFormat.R3G3B2;
            cfmtConv2[(int)XG.SurfaceFormat.Bgr24 + 1] = ImagePixelFormat.R8G8B8;
            cfmtConv2[(int)XG.SurfaceFormat.Bgr32 + 1] = ImagePixelFormat.X8R8G8B8;
            cfmtConv2[(int)XG.SurfaceFormat.Bgr444 + 1] = ImagePixelFormat.Unknown;
            cfmtConv2[(int)XG.SurfaceFormat.Bgr555 + 1] = ImagePixelFormat.Unknown;
            cfmtConv2[(int)XG.SurfaceFormat.Bgr565 + 1] = ImagePixelFormat.R5G6B5;
            cfmtConv2[(int)XG.SurfaceFormat.Bgra1010102 + 1] = ImagePixelFormat.Unknown;
            cfmtConv2[(int)XG.SurfaceFormat.Bgra2338 + 1] = ImagePixelFormat.Unknown;
            cfmtConv2[(int)XG.SurfaceFormat.Bgra4444 + 1] = ImagePixelFormat.A4R4G4B4;
            cfmtConv2[(int)XG.SurfaceFormat.Bgra5551 + 1] = ImagePixelFormat.A1R5G5B5;
            cfmtConv2[(int)XG.SurfaceFormat.Color + 1] = ImagePixelFormat.A8R8G8B8;
            cfmtConv2[(int)XG.SurfaceFormat.Depth15Stencil1 + 1] = ImagePixelFormat.Depth;
            cfmtConv2[(int)XG.SurfaceFormat.Depth16 + 1] = ImagePixelFormat.Depth;
            cfmtConv2[(int)XG.SurfaceFormat.Depth24 + 1] = ImagePixelFormat.Depth;
            cfmtConv2[(int)XG.SurfaceFormat.Depth24Stencil4 + 1] = ImagePixelFormat.Depth;
            cfmtConv2[(int)XG.SurfaceFormat.Depth24Stencil8 + 1] = ImagePixelFormat.Depth;
            cfmtConv2[(int)XG.SurfaceFormat.Depth24Stencil8Single + 1] = ImagePixelFormat.Depth;
            cfmtConv2[(int)XG.SurfaceFormat.Depth32 + 1] = ImagePixelFormat.Depth;
            cfmtConv2[(int)XG.SurfaceFormat.Dxt1 + 1] = ImagePixelFormat.DXT1;
            cfmtConv2[(int)XG.SurfaceFormat.Dxt2 + 1] = ImagePixelFormat.DXT2;
            cfmtConv2[(int)XG.SurfaceFormat.Dxt3 + 1] = ImagePixelFormat.DXT3;
            cfmtConv2[(int)XG.SurfaceFormat.Dxt4 + 1] = ImagePixelFormat.DXT4;
            cfmtConv2[(int)XG.SurfaceFormat.Dxt5 + 1] = ImagePixelFormat.DXT5;
            cfmtConv2[(int)XG.SurfaceFormat.HalfSingle + 1] = ImagePixelFormat.R16F;
            cfmtConv2[(int)XG.SurfaceFormat.HalfVector2 + 1] = ImagePixelFormat.G16R16F;
            cfmtConv2[(int)XG.SurfaceFormat.HalfVector4 + 1] = ImagePixelFormat.A16B16G16R16F;
            cfmtConv2[(int)XG.SurfaceFormat.Luminance16 + 1] = ImagePixelFormat.Luminance16;
            cfmtConv2[(int)XG.SurfaceFormat.Luminance8 + 1] = ImagePixelFormat.Luminance8;
            cfmtConv2[(int)XG.SurfaceFormat.LuminanceAlpha16 + 1] = ImagePixelFormat.A8L8;
            cfmtConv2[(int)XG.SurfaceFormat.LuminanceAlpha8 + 1] = ImagePixelFormat.A4L4;
            cfmtConv2[(int)XG.SurfaceFormat.Multi2Bgra32 + 1] = ImagePixelFormat.Unknown;
            cfmtConv2[(int)XG.SurfaceFormat.NormalizedAlpha1010102 + 1] = ImagePixelFormat.Unknown;
            cfmtConv2[(int)XG.SurfaceFormat.NormalizedByte2 + 1] = ImagePixelFormat.Unknown;
            cfmtConv2[(int)XG.SurfaceFormat.NormalizedByte2Computed + 1] = ImagePixelFormat.Unknown;
            cfmtConv2[(int)XG.SurfaceFormat.NormalizedByte4 + 1] = ImagePixelFormat.Unknown;
            cfmtConv2[(int)XG.SurfaceFormat.NormalizedLuminance16 + 1] = ImagePixelFormat.Unknown;
            cfmtConv2[(int)XG.SurfaceFormat.NormalizedLuminance32 + 1] = ImagePixelFormat.Unknown;
            cfmtConv2[(int)XG.SurfaceFormat.NormalizedShort2 + 1] = ImagePixelFormat.Unknown;
            cfmtConv2[(int)XG.SurfaceFormat.NormalizedShort4 + 1] = ImagePixelFormat.Unknown;
            cfmtConv2[(int)XG.SurfaceFormat.Palette8 + 1] = ImagePixelFormat.Unknown;
            cfmtConv2[(int)XG.SurfaceFormat.PaletteAlpha16 + 1] = ImagePixelFormat.Unknown;
            cfmtConv2[(int)XG.SurfaceFormat.Rg32 + 1] = ImagePixelFormat.G16R16;
            cfmtConv2[(int)XG.SurfaceFormat.Rgb32 + 1] = ImagePixelFormat.B8G8R8;
            cfmtConv2[(int)XG.SurfaceFormat.Rgba1010102 + 1] = ImagePixelFormat.Unknown;
            cfmtConv2[(int)XG.SurfaceFormat.Rgba32 + 1] = ImagePixelFormat.R8G8B8A8;
            cfmtConv2[(int)XG.SurfaceFormat.Rgba64 + 1] = ImagePixelFormat.Unknown;
            cfmtConv2[(int)XG.SurfaceFormat.Single + 1] = ImagePixelFormat.R32F;
            cfmtConv2[(int)XG.SurfaceFormat.Unknown + 1] = ImagePixelFormat.Unknown;
            cfmtConv2[(int)XG.SurfaceFormat.Vector2 + 1] = ImagePixelFormat.G32R32F;
            cfmtConv2[(int)XG.SurfaceFormat.Vector4 + 1] = ImagePixelFormat.A32B32G32R32F;
            #endregion
        }
        static void BuildDepthFormatConv()
        {
            dfmtConv = new XG.DepthFormat[(int)DepthFormat.Count];

            for (int i = 0; i < dfmtConv.Length; i++)
            {
                dfmtConv[i] = XG.DepthFormat.Unknown;
            }

            dfmtConv[(int)DepthFormat.Depth15Stencil1] = XG.DepthFormat.Depth15Stencil1;
            dfmtConv[(int)DepthFormat.Depth16] = XG.DepthFormat.Depth16;
            dfmtConv[(int)DepthFormat.Depth16Lockable] = XG.DepthFormat.Unknown;
            dfmtConv[(int)DepthFormat.Depth24X8] = XG.DepthFormat.Depth24;
            dfmtConv[(int)DepthFormat.Depth24Stencil4] = XG.DepthFormat.Depth24Stencil4;
            dfmtConv[(int)DepthFormat.Depth24Stencil8] = XG.DepthFormat.Depth24Stencil8;
            dfmtConv[(int)DepthFormat.Depth24Stencil8Single] = XG.DepthFormat.Depth24Stencil8Single;
            dfmtConv[(int)DepthFormat.Depth32] = XG.DepthFormat.Depth32;
            dfmtConv[(int)DepthFormat.Depth32Lockable] = XG.DepthFormat.Unknown;
            dfmtConv[(int)DepthFormat.Depth32Single] = XG.DepthFormat.Unknown;

            dfmtConv2 = new DepthFormat[MaxFmtCount];
            for (int i = 0; i < MaxFmtCount; i++)
            {
                dfmtConv2[i] = DepthFormat.Count;
            }
            dfmtConv2[(int)XG.DepthFormat.Unknown + 1] = DepthFormat.Count;
            dfmtConv2[(int)XG.DepthFormat.Depth15Stencil1 + 1] = DepthFormat.Depth15Stencil1;
            dfmtConv2[(int)XG.DepthFormat.Depth16 + 1] = DepthFormat.Depth16;
            dfmtConv2[(int)XG.DepthFormat.Depth24 + 1] = DepthFormat.Depth24X8;
            dfmtConv2[(int)XG.DepthFormat.Depth24Stencil4 + 1] = DepthFormat.Depth24Stencil4;
            dfmtConv2[(int)XG.DepthFormat.Depth24Stencil8 + 1] = DepthFormat.Depth24Stencil8;
            dfmtConv2[(int)XG.DepthFormat.Depth24Stencil8Single + 1] = DepthFormat.Depth24Stencil8Single;
            dfmtConv2[(int)XG.DepthFormat.Depth32 + 1] = DepthFormat.Depth32;
        }

        static D3D9Utils()
        {
            BuildColorFormatConv();
            BuildDepthFormatConv();
        }


        public static XG.ClearOptions ConvertEnum(ClearFlags flags)
        {           
            return (XG.ClearOptions)flags;
        }

        public static XG.BufferUsage ConvertEnum(BufferUsage usage)
        {
            XG.BufferUsage ret = 0;

            if ((usage & BufferUsage.Dynamic) == BufferUsage.Dynamic)
            {
                // Only add the dynamic flag for the default pool, and
                // we use default pool when buffer is discardable
                if ((usage & BufferUsage.Discardable) != BufferUsage.Discardable)
                    ret |= XG.BufferUsage.None;
            }
            if ((usage & BufferUsage.WriteOnly) == BufferUsage.WriteOnly)
            {
                ret |= XG.BufferUsage.WriteOnly;
            }
            return ret;
        }

        #region VertexElementUsage
        public static XG.VertexElementUsage ConvertEnum(VertexElementUsage semantic)
        {
            return (XG.VertexElementUsage)semantic;
        }
        public static XG.VertexElementFormat ConvertEnum(VertexElementFormat type)
        {
            return (XG.VertexElementFormat)type;
        }
        #endregion

        #region Blend
        public static XG.Blend ConvertEnum(Blend dv)
        {
            return (XG.Blend)dv;
        }
        public static Blend ConvertEnum(XG.Blend dv)
        {
            return (Blend)dv;
        }
        #endregion

        #region CompareFunction
        public static XG.CompareFunction ConvertEnum(CompareFunction dv)
        {
            return (XG.CompareFunction)dv;
        }
        public static CompareFunction ConvertEnum(XG.CompareFunction dv)
        {
            return (CompareFunction)dv;
        }
        #endregion CompareFunction

        #region BlendFunction
        public static XG.BlendFunction ConvertEnum(BlendFunction dv)
        {
            return (XG.BlendFunction)dv;
        }
        public static BlendFunction ConvertEnum(XG.BlendFunction dv)
        {
            return (BlendFunction)dv;
        }
        #endregion

        #region StencilOperation
        public static XG.StencilOperation ConvertEnum(StencilOperation dv)
        {
            return (XG.StencilOperation)dv;
        }
        public static StencilOperation ConvertEnum(XG.StencilOperation dv)
        {
            return (StencilOperation)dv;
        }
        #endregion

        #region ColorWriteChannels
        public static XG.ColorWriteChannels ConvertEnum(ColorWriteChannels dv)
        {
            return (XG.ColorWriteChannels)dv;
        }
        public static ColorWriteChannels ConvertEnum(XG.ColorWriteChannels dv)
        {
            return (ColorWriteChannels)dv;
        }
        #endregion

        #region TextureAddressMode
        public static XG.TextureAddressMode ConvertEnum(TextureAddressMode type)
        {
            return (XG.TextureAddressMode)type;
        }
        public static TextureAddressMode ConvertEnum(XG.TextureAddressMode ta) 
        {
            return (TextureAddressMode)ta;
        }
        #endregion

        #region TextureWrapCoordinates
        public static XG.TextureWrapCoordinates ConvertEnum(TextureWrapCoordinates dv)
        {
            return (XG.TextureWrapCoordinates)dv;
        }
        public static TextureWrapCoordinates ConvertEnum(XG.TextureWrapCoordinates dv)
        {
            return (TextureWrapCoordinates)dv;
        }
        #endregion

        #region ImagePixelFormat
        public static XG.SurfaceFormat ConvertEnum(ImagePixelFormat format)
        {
            return cfmtConv[(int)format];
        }
        public static ImagePixelFormat ConvertEnum(XG.SurfaceFormat format) 
        {
            return cfmtConv2[(int)format + 1];
        }
        #endregion

        public static XG.TextureUsage ConvertEnum(TextureUsage usage)
        {
            XG.TextureUsage result = XG.TextureUsage.None;
            if ((usage & TextureUsage.AutoMipMap) == TextureUsage.AutoMipMap)
            {
                result |= XG.TextureUsage.AutoGenerateMipMap;
            }
            //if ((usage & TextureUsage.Dynamic) == TextureUsage.Dynamic)
            //{
            //    result |= XG.TextureUsage.None;
            //}
            //if ((usage & TextureUsage.WriteOnly) == TextureUsage.WriteOnly)
            //{
            //    result |= XG.TextureUsage.WriteOnly;
            //}
            //if ((usage & TextureUsage.RenderTarget) == TextureUsage.RenderTarget)
            //{
            //    result |= XG.TextureUsage.RenderTarget;
            //}

            return result;
        }

        #region DepthFormat
        public static XG.DepthFormat ConvertEnum(DepthFormat format) 
        {
            return dfmtConv[(int)format];
        }
        public static DepthFormat ConvertEnum(XG.DepthFormat format)
        {
            return dfmtConv2[(int)format + 1];
        }
        #endregion

        #region Depth-Color Inter
        public static XG.SurfaceFormat ConvertEnumInter(DepthFormat format)
        {
            return (XG.SurfaceFormat)dfmtConv[(int)format];
        }
        public static DepthFormat ConvertEnumDepthInter(XG.SurfaceFormat format)
        {
            return dfmtConv2[(int)format + 1];
        }
        #endregion

        #region TextureFilter
        public static XG.TextureFilter ConvertEnum(TextureFilter filter)
        {
            return (XG.TextureFilter)filter;
        }
        public static TextureFilter ConvertEnum(XG.TextureFilter ft)
        {
            return (TextureFilter)ft;
        }
        #endregion
    }
}
