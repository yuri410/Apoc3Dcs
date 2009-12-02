using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Apoc3D.Collections;
using Apoc3D.Graphics;
using Apoc3D.MathLib;
using X = Microsoft.Xna.Framework;
using XG = Microsoft.Xna.Framework.Graphics;
using XGC = Microsoft.Xna.Framework.Graphics.GraphicsDeviceCapabilities;

namespace Apoc3D.RenderSystem.Xna
{
    class XnaRenderSystem : Apoc3D.Graphics.RenderSystem
    {
        internal XG.GraphicsDevice device;

        internal XG.RenderTarget2D defaultRtXna;
        XnaRenderTarget defaultRt;

        XnaRenderTarget[] cachedRenderTargets;



        public XnaRenderSystem(XG.GraphicsDevice device)
            : base(XnaGraphicsAPIFactory.APIName + "RenderSystem")
        {
            this.device = device;
        }

        public override void Init()
        {
            defaultRtXna = (XG.RenderTarget2D)device.GetRenderTarget(0);
            defaultRt = new XnaRenderTarget(this, defaultRtXna);


            Capabilities caps = new Capabilities();

            #region AlphaCompareCapabilities
            XGC.CompareCaps caps1 = device.GraphicsDeviceCapabilities.AlphaCompareCapabilities;

            if (caps1.SupportsAlways)
            {
                caps.AlphaCompareCaps |= CompareCaps.Always;
            }
            if (caps1.SupportsEqual)
            {
                caps.AlphaCompareCaps |= CompareCaps.Equal;
            }
            if (caps1.SupportsGreater)
            {
                caps.AlphaCompareCaps |= CompareCaps.Greater;
            }
            if (caps1.SupportsGreaterEqual)
            {
                caps.AlphaCompareCaps |= CompareCaps.GreaterEqual;
            }
            if (caps1.SupportsLess)
            {
                caps.AlphaCompareCaps |= CompareCaps.Less;
            }
            if (caps1.SupportsLessEqual)
            {
                caps.AlphaCompareCaps |= CompareCaps.LessEqual;
            }
            if (caps1.SupportsNever)
            {
                caps.AlphaCompareCaps |= CompareCaps.Never;
            }
            if (caps1.SupportsNotEqual)
            {
                caps.AlphaCompareCaps |= CompareCaps.NotEqual;
            }
            #endregion

            #region CubeTextureFilterCapabilities
            XGC.FilterCaps caps2 = device.GraphicsDeviceCapabilities.CubeTextureFilterCapabilities;

            if (caps2.SupportsMagnifyAnisotropic)
            {
                caps.CubeTextureFilterCaps |= FilterCaps.MagAnisotropic;
            }
            if (caps2.SupportsMagnifyGaussianQuad)
            {
                caps.CubeTextureFilterCaps |= FilterCaps.MagGaussianQuad;
            }
            if (caps2.SupportsMagnifyLinear)
            {
                caps.CubeTextureFilterCaps |= FilterCaps.MagLinear;
            }
            if (caps2.SupportsMagnifyPoint)
            {
                caps.CubeTextureFilterCaps |= FilterCaps.MagPoint;
            }
            if (caps2.SupportsMagnifyPyramidalQuad)
            {
                caps.CubeTextureFilterCaps |= FilterCaps.MagPyramidalQuad;
            }
            if (caps2.SupportsMinifyAnisotropic)
            {
                caps.CubeTextureFilterCaps |= FilterCaps.MinAnisotropic;
            }
            if (caps2.SupportsMinifyGaussianQuad)
            {
                caps.CubeTextureFilterCaps |= FilterCaps.MinGaussianQuad;
            }
            if (caps2.SupportsMinifyLinear)
            {
                caps.CubeTextureFilterCaps |= FilterCaps.MinLinear;
            }
            if (caps2.SupportsMinifyPoint)
            {
                caps.CubeTextureFilterCaps |= FilterCaps.MinPoint;
            }
            if (caps2.SupportsMinifyPyramidalQuad)
            {
                caps.CubeTextureFilterCaps |= FilterCaps.MinPyramidalQuad;
            }
            if (caps2.SupportsMipMapLinear)
            {
                caps.CubeTextureFilterCaps |= FilterCaps.MipLinear;
            }
            if (caps2.SupportsMipMapPoint)
            {
                caps.CubeTextureFilterCaps |= FilterCaps.MipPoint;
            }
            #endregion

            #region DeclarationTypeCapabilities
            XGC.DeclarationTypeCaps caps3 = device.GraphicsDeviceCapabilities.DeclarationTypeCapabilities;
            if (caps3.SupportsNormalized101010)
            {
                caps.DeclarationTypes |= DeclarationTypeCaps.UInt101010N;
            }
            if (caps3.SupportsUInt101010)
            {
                caps.DeclarationTypes |= DeclarationTypeCaps.UInt101010;
            }
            if (caps3.SupportsHalfVector2)
            {
                caps.DeclarationTypes |= DeclarationTypeCaps.HalfVector2;
            }
            if (caps3.SupportsHalfVector4)
            {
                caps.DeclarationTypes |= DeclarationTypeCaps.HalfVector4;
            }
            if (caps3.SupportsByte4)
            {
                caps.DeclarationTypes |= DeclarationTypeCaps.UByte4;
            }
            if (caps3.SupportsNormalizedShort2)
            {
                caps.DeclarationTypes |= DeclarationTypeCaps.Short2N;
            }
            if (caps3.SupportsNormalizedShort4)
            {
                caps.DeclarationTypes |= DeclarationTypeCaps.Short4N;
            }
            if (caps3.SupportsRgba32)
            {
                caps.DeclarationTypes |= DeclarationTypeCaps.UByte4N;
            }
            if (caps3.SupportsRgba64)
            {
                caps.DeclarationTypes |= DeclarationTypeCaps.UShort4N;
            }
            if (caps3.SupportsRg32)
            {
                caps.DeclarationTypes |= DeclarationTypeCaps.UShort2N;
            }

            #endregion

            #region DepthBufferCompareCapabilities
            caps1 = device.GraphicsDeviceCapabilities.DepthBufferCompareCapabilities;
            if (caps1.SupportsAlways)
            {
                caps.DepthCompareCaps |= CompareCaps.Always;
            }
            if (caps1.SupportsEqual)
            {
                caps.DepthCompareCaps |= CompareCaps.Equal;
            }
            if (caps1.SupportsGreater)
            {
                caps.DepthCompareCaps |= CompareCaps.Greater;
            }
            if (caps1.SupportsGreaterEqual)
            {
                caps.DepthCompareCaps |= CompareCaps.GreaterEqual;
            }
            if (caps1.SupportsLess)
            {
                caps.DepthCompareCaps |= CompareCaps.Less;
            }
            if (caps1.SupportsLessEqual)
            {
                caps.DepthCompareCaps |= CompareCaps.LessEqual;
            }
            if (caps1.SupportsNever)
            {
                caps.DepthCompareCaps |= CompareCaps.Never;
            }
            if (caps1.SupportsNotEqual)
            {
                caps.DepthCompareCaps |= CompareCaps.NotEqual;
            }
            #endregion

            #region DestinationBlendCapabilities
            XGC.BlendCaps caps4 = device.GraphicsDeviceCapabilities.DestinationBlendCapabilities;
            if (caps4.SupportsBlendFactor)
            {
                caps.DestinationBlendCaps |= BlendCaps.BlendFactor;
            }
            if (caps4.SupportsBothInverseSourceAlpha )
            {
                caps.DestinationBlendCaps |= BlendCaps.BothInverseSourceAlpha;
            }
            if (caps4 .SupportsDestinationAlpha)
            {
                caps.DestinationBlendCaps |= BlendCaps.DestinationAlpha;
            }
            if (caps4.SupportsDestinationColor)
            {
                caps.DestinationBlendCaps |= BlendCaps.DestinationColor;
            }
            if (caps4.SupportsInverseDestinationAlpha)
            {
                caps.DestinationBlendCaps |= BlendCaps.InverseDestinationAlpha;
            }
            if (caps4.SupportsInverseDestinationColor)
            {
                caps.DestinationBlendCaps |= BlendCaps.InverseDestinationColor;
            }
            if (caps4.SupportsInverseSourceAlpha)
            {
                caps.DestinationBlendCaps |= BlendCaps.InverseSourceAlpha;
            }
            if (caps4.SupportsInverseSourceColor)
            {
                caps.DestinationBlendCaps |= BlendCaps.InverseSourceColor;
            }
            if (caps4.SupportsOne)
            {
                caps.DestinationBlendCaps |= BlendCaps.One;
            }
            if (caps4.SupportsSourceAlpha)
            {
                caps.DestinationBlendCaps |= BlendCaps.SourceAlpha;
            }
            if (caps4.SupportsSourceAlphaSat)
            {
                caps.DestinationBlendCaps |= BlendCaps.SourceAlphaSaturated;
            }
            if (caps4 .SupportsSourceColor)
            {
                caps.DestinationBlendCaps |= BlendCaps.SourceColor;
            }
            if (caps4.SupportsZero)
            {
                caps.DestinationBlendCaps |= BlendCaps.Zero;
            }
            #endregion

            #region DeviceCapabilities
            XGC.DeviceCaps caps5 = device.GraphicsDeviceCapabilities.DeviceCapabilities;
            if (caps5.CanDrawSystemToNonLocal)
            {
                caps.DeviceCaps |= DeviceCaps.CanRenderSysToNonLocal;
            }
            if (caps5.CanRenderAfterFlip)
            {
                caps.DeviceCaps |= DeviceCaps.CanRenderAfterFlip;
            }
            if (caps5.SupportsDrawPrimitives2)
            {
                caps.DeviceCaps |= DeviceCaps.DrawPrimitives2;
            }
            if (caps5.SupportsDrawPrimitives2Ex)
            {
                caps.DeviceCaps |= DeviceCaps.DrawPrimitives2Extended;
            }
            if (caps5.SupportsDrawPrimitivesTransformedVertex)
            {
                caps.DeviceCaps |= DeviceCaps.DrawPrimTLVertex;
            }
            if (caps5.SupportsExecuteSystemMemory)
            {
                caps.DeviceCaps |= DeviceCaps.ExecuteSystemMemory;
            }
            if (caps5.SupportsExecuteVideoMemory)
            {
                caps.DeviceCaps |= DeviceCaps.ExecuteVideoMemory;
            }
            if (caps5.SupportsHardwareRasterization)
            {
                caps.DeviceCaps |= DeviceCaps.HWRasterization;
            }
            if (caps5.SupportsHardwareTransformAndLight)
            {
                caps.DeviceCaps |= DeviceCaps.HWTransformAndLight;
            }
            if (caps5.SupportsSeparateTextureMemories)
            {
                caps.DeviceCaps |= DeviceCaps.SeparateTextureMemory;
            }
            if (caps5.SupportsTextureNonLocalVideoMemory)
            {
                caps.DeviceCaps |= DeviceCaps.TextureNonLocalVideoMemory;
            }
            if (caps5.SupportsTextureSystemMemory)
            {
                caps.DeviceCaps |= DeviceCaps.TextureSystemMemory;
            }
            if (caps5.SupportsTextureVideoMemory)
            {
                caps.DeviceCaps |= DeviceCaps.TextureVideoMemory;
            }
            if (caps5.SupportsTransformedVertexSystemMemory)
            {
                caps.DeviceCaps |= DeviceCaps.TLVertexSystemMemory;
            }
            if (caps5.SupportsTransformedVertexVideoMemory)
            {
                caps.DeviceCaps |= DeviceCaps.TLVertexVideoMemory;
            }
            if (caps5.SupportsStreamOffset)
            {
                caps.DeviceCaps2 |= DeviceCaps2.StreamOffset;
            }
            if (caps5.VertexElementScanSharesStreamOffset) 
            {
                caps.DeviceCaps2 |= DeviceCaps2.VertexElementsCanShareStreamOffset;
            }
            
            #endregion

            #region LineCapabilities
            XG.GraphicsDeviceCapabilities.LineCaps lineCaps = device.GraphicsDeviceCapabilities.LineCapabilities;
            if (lineCaps.SupportsAlphaCompare )
            {
                caps.LineCaps |= LineCaps.AlphaCompare;
            }
            if (lineCaps.SupportsAntiAlias)
            {
                caps.LineCaps |= LineCaps.Antialias;
            }
            if (lineCaps.SupportsBlend)
            {
                caps.LineCaps |= LineCaps.Blend;
            }
            if (lineCaps.SupportsDepthBufferTest)
            {
                caps.LineCaps |= LineCaps.DepthTest;
            }
            if (lineCaps.SupportsFog)
            {
                caps.LineCaps |= LineCaps.Fog;
            }
            if (lineCaps.SupportsTextureMapping)
            {
                caps.LineCaps |= LineCaps.Texture;
            }
            #endregion

            #region DriverCapabilities
            XG.GraphicsDeviceCapabilities.PixelShaderCaps caps6 = device.GraphicsDeviceCapabilities.PixelShaderCapabilities;

            PixelShader20Caps pscaps = new PixelShader20Caps ();
            if (caps6.SupportsArbitrarySwizzle) 
            {                
                pscaps.Caps |= PixelShaderCaps.ArbitrarySwizzle;    
            }
            if (caps6.SupportsGradientInstructions) 
            {
                pscaps.Caps |= PixelShaderCaps.GradientInstructions;
            }
            if (caps6.SupportsNoDependentReadLimit) 
            {
                pscaps.Caps |= PixelShaderCaps.NoDependentReadLimit;
            }
            if (caps6.SupportsNoTextureInstructionLimit) 
            {
                pscaps.Caps |= PixelShaderCaps.NoTextureInstructionLimit;
            }
            if (caps6.SupportsPredication) 
            {
                pscaps.Caps |= PixelShaderCaps.Predication;
            }
            pscaps.DynamicFlowControlDepth = caps6.DynamicFlowControlDepth;
            pscaps.InstructionSlotCount = caps6.NumberInstructionSlots;
            pscaps.StaticFlowControlDepth = caps6.StaticFlowControlDepth;
            pscaps.TempCount = caps6.NumberTemps;
            
            caps.PS20Caps = pscaps;

            #endregion

            #region RasterCapabilities
            XG.GraphicsDeviceCapabilities.RasterCaps caps7 = device.GraphicsDeviceCapabilities.RasterCapabilities;

            if (caps7.SupportsAnisotropy)
            {
                caps.RasterCaps |= RasterCaps.Anisotropy;
            }
            if (caps7.SupportsColorPerspective) 
            {
                caps.RasterCaps |= RasterCaps.ColorPerspective;
            }
            if (caps7.SupportsDepthBias)
            {
                caps.RasterCaps |= RasterCaps.DepthBias;
            }
            if (caps7.SupportsDepthBufferLessHsr)
            {
                caps.RasterCaps |= RasterCaps.ZBufferLessHsr;
            } 
            if (caps7.SupportsDepthBufferTest)
            {
                caps.RasterCaps |= RasterCaps.DepthTest;
            }
            if (caps7.SupportsDepthFog)
            {
                caps.RasterCaps |= RasterCaps.ZFog;
            }
            if (caps7.SupportsFogRange)
            {
                caps.RasterCaps |= RasterCaps.FogRange;
            }
            if (caps7.SupportsFogTable)
            {
                caps.RasterCaps |= RasterCaps.FogTable;
            }
            if (caps7.SupportsFogVertex)
            {
                caps.RasterCaps |= RasterCaps.FogVertex;
            }
            if (caps7.SupportsMipMapLevelOfDetailBias)
            {
                caps.RasterCaps |= RasterCaps.MipMapLodBias;
            }
            if (caps7.SupportsMultisampleToggle)
            {
                caps.RasterCaps |= RasterCaps.MultisampleToggle;
            }
            if (caps7.SupportsScissorTest)
            {
                caps.RasterCaps |= RasterCaps.ScissorTest;
            }
            if (caps7.SupportsSlopeScaleDepthBias)
            {
                caps.RasterCaps |= RasterCaps.SlopeScaleDepthBias;
            }
            if (caps7.SupportsWFog)
            {
                caps.RasterCaps |= RasterCaps.WFog;
            }
            #endregion

            #region ShadingCapabilities
            XG.GraphicsDeviceCapabilities.ShadingCaps caps8 = device.GraphicsDeviceCapabilities.ShadingCapabilities;
            if (caps8.SupportsAlphaGouraudBlend)
            {
                caps.ShadeCaps |= ShadeCaps.AlphaGouraudBlend;
            }
            if (caps8.SupportsColorGouraudRgb)
            {
                caps.ShadeCaps |= ShadeCaps.ColorGouraudRgb;
            }
            if (caps8.SupportsFogGouraud)
            {
                caps.ShadeCaps |= ShadeCaps.FogGouraud;
            }
            if (caps8.SupportsSpecularGouraudRgb)
            {
                caps.ShadeCaps |= ShadeCaps.SpecularGouraudRgb;
            }
            #endregion


            RenderSystemCaps = caps;



            cachedRenderTargets = new XnaRenderTarget[device.GraphicsDeviceCapabilities.MaxSimultaneousRenderTargets];
            cachedRenderTargets[0] = defaultRt;
        }

        public override void Clear(ClearFlags flags, ColorValue color, float depth, int stencil)
        {
            XG.Color clr = new XG.Color(color.R, color.G, color.B, color.A);
            device.Clear(XnaUtils.ConvertEnum(flags), clr, depth, stencil);
        }

        public override void SetRenderTarget(int index, RenderTarget rt)
        {
            throw new NotImplementedException();
        }

        public override RenderTarget GetRenderTarget(int index)
        {
            throw new NotImplementedException();
        }

        public override SamplerStateCollection GetSamplerStates()
        {
            throw new NotImplementedException();
        }

        public override void BindShader(VertexShader shader)
        {
            throw new NotImplementedException();
        }

        public override void BindShader(PixelShader shader)
        {
            throw new NotImplementedException();
        }

        public override Viewport Viewport
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }
    }
}