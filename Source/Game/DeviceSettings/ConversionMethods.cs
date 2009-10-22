﻿using SlimDX;
using SlimDX.Direct3D9;
using D3D10 = SlimDX.Direct3D10;
using DXGI = SlimDX.DXGI;

namespace VirtualBicycle
{
    static class ConversionMethods
    {
        public static int GetDepthBits(Format format)
        {
            switch (format)
            {
                case Format.D32SingleLockable:
                case Format.D32:
                    return 32;

                case Format.D24X8:
                case Format.D24S8:
                case Format.D24X4S4:
                case Format.D24SingleS8:
                    return 24;

                case Format.D16Lockable:
                case Format.D16:
                    return 16;

                case Format.D15S1:
                    return 15;

                default:
                    return 0;
            }
        }

        public static int GetStencilBits(Format format)
        {
            switch (format)
            {
                case Format.D15S1:
                    return 1;

                case Format.D24X4S4:
                    return 4;

                case Format.D24S8:
                case Format.D24SingleS8:
                    return 8;

                default:
                    return 0;
            }
        }

        public static int GetColorBits(Format format)
        {
            switch (format)
            {
                case Format.R8G8B8:
                case Format.A8R8G8B8:
                case Format.A8B8G8R8:
                case Format.X8R8G8B8:
                    return 8;

                case Format.R5G6B5:
                case Format.X1R5G5B5:
                case Format.A1R5G5B5:
                    return 5;

                case Format.A4R4G4B4:
                case Format.X4R4G4B4:
                    return 4;

                case Format.R3G3B2:
                case Format.A8R3G3B2:
                    return 2;

                case Format.A2R10G10B10:
                case Format.A2B10G10R10:
                    return 10;

                case Format.A16B16G16R16:
                    return 16;

                default:
                    return 0;
            }
        }

        public static int GetColorBits(DXGI.Format format)
        {
            switch (format)
            {
                case SlimDX.DXGI.Format.R32G32B32A32_Float:
                case SlimDX.DXGI.Format.R32G32B32A32_SInt:
                case SlimDX.DXGI.Format.R32G32B32A32_Typeless:
                case SlimDX.DXGI.Format.R32G32B32A32_UInt:
                case SlimDX.DXGI.Format.R32G32B32_Float:
                case SlimDX.DXGI.Format.R32G32B32_SInt:
                case SlimDX.DXGI.Format.R32G32B32_Typeless:
                case SlimDX.DXGI.Format.R32G32B32_UInt:
                    return 32;

                case SlimDX.DXGI.Format.R16G16B16A16_Float:
                case SlimDX.DXGI.Format.R16G16B16A16_SInt:
                case SlimDX.DXGI.Format.R16G16B16A16_SNorm:
                case SlimDX.DXGI.Format.R16G16B16A16_Typeless:
                case SlimDX.DXGI.Format.R16G16B16A16_UInt:
                case SlimDX.DXGI.Format.R16G16B16A16_UNorm:
                    return 16;

                case SlimDX.DXGI.Format.R10G10B10A2_Typeless:
                case SlimDX.DXGI.Format.R10G10B10A2_UInt:
                case SlimDX.DXGI.Format.R10G10B10A2_UNorm:
                    return 10;

                case SlimDX.DXGI.Format.R8G8B8A8_SInt:
                case SlimDX.DXGI.Format.R8G8B8A8_SNorm:
                case SlimDX.DXGI.Format.R8G8B8A8_Typeless:
                case SlimDX.DXGI.Format.R8G8B8A8_UInt:
                case SlimDX.DXGI.Format.R8G8B8A8_UNorm:
                case SlimDX.DXGI.Format.R8G8B8A8_UNorm_SRGB:
                    return 8;

                case SlimDX.DXGI.Format.B5G5R5A1_UNorm:
                case SlimDX.DXGI.Format.B5G6R5_UNorm:
                    return 5;

                default:
                    return 0;
            }
        }

        public static D3D10.DriverType ToDirect3D10(DeviceType type)
        {
            if (type == DeviceType.Hardware)
                return SlimDX.Direct3D10.DriverType.Hardware;
            else if (type == DeviceType.Reference)
                return SlimDX.Direct3D10.DriverType.Reference;
            else if (type == DeviceType.Software)
                return SlimDX.Direct3D10.DriverType.Software;
            else
                return SlimDX.Direct3D10.DriverType.Null;
        }

        public static DeviceType ToDirect3D9(D3D10.DriverType type)
        {
            if (type == SlimDX.Direct3D10.DriverType.Hardware)
                return DeviceType.Hardware;
            else if (type == SlimDX.Direct3D10.DriverType.Reference)
                return DeviceType.Reference;
            else if (type == SlimDX.Direct3D10.DriverType.Software)
                return DeviceType.Software;
            else
                return DeviceType.NullReference;
        }

        public static int ToDirect3D10(MultisampleType type, int quality)
        {
            if (type == MultisampleType.NonMaskable)
                return quality;
            else
                return (int)type;
        }

        public static MultisampleType ToDirect3D9(int type)
        {
            return (MultisampleType)type;
        }

        public static DXGI.Format ToDirect3D10(Format format)
        {
            switch (format)
            {
                case Format.R8G8B8:
                case Format.A8R8G8B8:
                case Format.X8R8G8B8:
                case Format.A4R4G4B4:
                case Format.R3G3B2:
                case Format.A8R3G3B2:
                case Format.X4R4G4B4:
                    return SlimDX.DXGI.Format.R8G8B8A8_UNorm;
                case Format.R5G6B5:
                    return SlimDX.DXGI.Format.B5G6R5_UNorm;
                case Format.X1R5G5B5:
                case Format.A1R5G5B5:
                    return SlimDX.DXGI.Format.B5G5R5A1_UNorm;
                case Format.A8:
                    return SlimDX.DXGI.Format.A8_UNorm;
                case Format.A2B10G10R10:
                case Format.A2R10G10B10:
                    return SlimDX.DXGI.Format.R10G10B10A2_UNorm;
                case Format.A8B8G8R8:
                case Format.X8B8G8R8:
                    return SlimDX.DXGI.Format.B8G8R8A8_UNorm;
                case Format.G16R16:
                    return SlimDX.DXGI.Format.R16G16_UNorm;
                case Format.A16B16G16R16:
                    return SlimDX.DXGI.Format.R16G16B16A16_UNorm;
                case Format.R16F:
                    return SlimDX.DXGI.Format.R16_Float;
                case Format.G16R16F:
                    return SlimDX.DXGI.Format.R16G16_Float;
                case Format.A16B16G16R16F:
                    return SlimDX.DXGI.Format.R16G16B16A16_Float;
                case Format.R32F:
                    return SlimDX.DXGI.Format.R32_Float;
                case Format.G32R32F:
                    return SlimDX.DXGI.Format.R32G32_Float;
                case Format.A32B32G32R32F:
                    return SlimDX.DXGI.Format.R32G32B32A32_Float;
                case Format.D15S1:
                case Format.D16:
                case Format.D16Lockable:
                    return SlimDX.DXGI.Format.D16_UNorm;
                case Format.D24S8:
                case Format.D24SingleS8:
                case Format.D24X4S4:
                case Format.D24X8:
                    return SlimDX.DXGI.Format.D24_UNorm_S8_UInt;
                case Format.D32:
                case Format.D32Lockable:
                case Format.D32SingleLockable:
                    return SlimDX.DXGI.Format.D32_Float;
            }

            return SlimDX.DXGI.Format.Unknown;
        }

        public static Format ToDirect3D9(DXGI.Format format)
        {
            switch (format)
            {
                case SlimDX.DXGI.Format.R8G8B8A8_UNorm:
                    return Format.A8R8G8B8;
                case SlimDX.DXGI.Format.B5G6R5_UNorm:
                    return Format.R5G6B5;
                case SlimDX.DXGI.Format.B5G5R5A1_UNorm:
                    return Format.A1R5G5B5;
                case SlimDX.DXGI.Format.A8_UNorm:
                    return Format.A8;
                case SlimDX.DXGI.Format.R10G10B10A2_UNorm:
                    return Format.A2B10G10R10;
                case SlimDX.DXGI.Format.B8G8R8A8_UNorm:
                    return Format.A8B8G8R8;
                case SlimDX.DXGI.Format.R16G16_UNorm:
                    return Format.G16R16;
                case SlimDX.DXGI.Format.R16G16B16A16_UNorm:
                    return Format.A16B16G16R16;
                case SlimDX.DXGI.Format.R16_Float:
                    return Format.R16F;
                case SlimDX.DXGI.Format.R16G16_Float:
                    return Format.G16R16F;
                case SlimDX.DXGI.Format.R16G16B16A16_Float:
                    return Format.A16B16G16R16F;
                case SlimDX.DXGI.Format.R32_Float:
                    return Format.R32F;
                case SlimDX.DXGI.Format.R32G32_Float:
                    return Format.G32R32F;
                case SlimDX.DXGI.Format.R32G32B32A32_Float:
                    return Format.A32B32G32R32F;
            }

            return Format.Unknown;
        }

        public static float ToFloat(Rational rational)
        {
            float denom = 1;
            if (rational.Denominator != 0)
                denom = rational.Denominator;
            return rational.Numerator / denom;
        }
    }
}
