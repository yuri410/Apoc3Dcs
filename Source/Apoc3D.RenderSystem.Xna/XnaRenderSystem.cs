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

            #region ªÒ»°Capabilities
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

            #region PrimitiveCapabilities
            XG.GraphicsDeviceCapabilities.PrimitiveCaps caps10 = device.GraphicsDeviceCapabilities.PrimitiveCapabilities;
            if (caps10.SupportsBlendOperation)
            {
                caps.PrimitiveMiscCaps |= PrimitiveMiscCaps.BlendOperation;
            }
            if (caps10.SupportsClipTransformedVertices)
            {
                caps.PrimitiveMiscCaps |= PrimitiveMiscCaps.ClipTLVertices;
            }
            if (caps10.SupportsColorWrite)
            {
                caps.PrimitiveMiscCaps |= PrimitiveMiscCaps.ColorWriteEnable;
            }
            if (caps10.SupportsCullClockwiseFace)
            {
                caps.PrimitiveMiscCaps |= PrimitiveMiscCaps.CullCW;
            }
            if (caps10.SupportsCullCounterClockwiseFace)
            {
                caps.PrimitiveMiscCaps |= PrimitiveMiscCaps.CullCCW;
            }
            if (caps10.SupportsCullNone)
            {
                caps.PrimitiveMiscCaps |= PrimitiveMiscCaps.CullNone;
            }
            if (caps10.SupportsFogAndSpecularAlpha)
            {
                caps.PrimitiveMiscCaps |= PrimitiveMiscCaps.FogAndSpecularAlpha;
            }
            if (caps10.SupportsIndependentWriteMasks)
            {
                caps.PrimitiveMiscCaps |= PrimitiveMiscCaps.IndependentWriteMasks;
            }
            if (caps10.SupportsMaskZ)
            {
                caps.PrimitiveMiscCaps |= PrimitiveMiscCaps.MaskZ;
            }
            if (caps10.SupportsMultipleRenderTargetsIndependentBitDepths)
            {
                caps.PrimitiveMiscCaps |= PrimitiveMiscCaps.MrtIndependentBitDepths;
            }
            if (caps10.SupportsMultipleRenderTargetsPostPixelShaderBlending)
            {
                caps.PrimitiveMiscCaps |= PrimitiveMiscCaps.MrtPostPixelShaderBlending;
            }
            if (caps10.SupportsSeparateAlphaBlend)
            {
                caps.PrimitiveMiscCaps |= PrimitiveMiscCaps.SeparateAlphaBlend;
            }
            if (caps10.HasFogVertexClamped)
            {
                caps.PrimitiveMiscCaps |= PrimitiveMiscCaps.FogVertexClamped;
            }
            if (caps10.IsNullReference)
            {
                caps.PrimitiveMiscCaps |= PrimitiveMiscCaps.NullReference;
            }
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

            #region SourceBlendCapabilities
            caps4 = device.GraphicsDeviceCapabilities.SourceBlendCapabilities;
            if (caps4.SupportsBlendFactor)
            {
                caps.SourceBlendCaps |= BlendCaps.BlendFactor;
            }
            if (caps4.SupportsBothInverseSourceAlpha)
            {
                caps.SourceBlendCaps |= BlendCaps.BothInverseSourceAlpha;
            }
            if (caps4.SupportsDestinationAlpha)
            {
                caps.SourceBlendCaps |= BlendCaps.DestinationAlpha;
            }
            if (caps4.SupportsDestinationColor)
            {
                caps.SourceBlendCaps |= BlendCaps.DestinationColor;
            }
            if (caps4.SupportsInverseDestinationAlpha)
            {
                caps.SourceBlendCaps |= BlendCaps.InverseDestinationAlpha;
            }
            if (caps4.SupportsInverseDestinationColor)
            {
                caps.SourceBlendCaps |= BlendCaps.InverseDestinationColor;
            }
            if (caps4.SupportsInverseSourceAlpha)
            {
                caps.SourceBlendCaps |= BlendCaps.InverseSourceAlpha;
            }
            if (caps4.SupportsInverseSourceColor)
            {
                caps.SourceBlendCaps |= BlendCaps.InverseSourceColor;
            }
            if (caps4.SupportsOne)
            {
                caps.SourceBlendCaps |= BlendCaps.One;
            }
            if (caps4.SupportsSourceAlpha)
            {
                caps.SourceBlendCaps |= BlendCaps.SourceAlpha;
            }
            if (caps4.SupportsSourceAlphaSat)
            {
                caps.SourceBlendCaps |= BlendCaps.SourceAlphaSaturated;
            }
            if (caps4.SupportsSourceColor)
            {
                caps.SourceBlendCaps |= BlendCaps.SourceColor;
            }
            if (caps4.SupportsZero)
            {
                caps.SourceBlendCaps |= BlendCaps.Zero;
            }
            #endregion

            #region StencilCapabilities
            XG.GraphicsDeviceCapabilities.StencilCaps caps9 = device.GraphicsDeviceCapabilities.StencilCapabilities;
            if (caps9.SupportsDecrement)
            {
                caps.StencilCaps |= StencilCaps.Decrement;
            }
            if (caps9.SupportsDecrementSaturation)
            {
                caps.StencilCaps |= StencilCaps.DecrementClamp;
            }
            if (caps9.SupportsIncrement)
            {
                caps.StencilCaps |= StencilCaps.Increment;
            }
            if (caps9.SupportsIncrementSaturation)
            {
                caps.StencilCaps |= StencilCaps.IncrementClamp;
            }
            if (caps9.SupportsInvert)
            {
                caps.StencilCaps |= StencilCaps.Invert;
            }
            if (caps9.SupportsKeep)
            {
                caps.StencilCaps |= StencilCaps.Keep;
            }
            if (caps9.SupportsReplace)
            {
                caps.StencilCaps |= StencilCaps.Replace;
            }
            if (caps9.SupportsTwoSided)
            {
                caps.StencilCaps |= StencilCaps.TwoSided;
            }
            if (caps9.SupportsZero)
            {
                caps.StencilCaps |= StencilCaps.Zero;
            }
            
            RenderSystemCaps = caps;
            #endregion

            #region TextureAddressCapabilities
            XG.GraphicsDeviceCapabilities.AddressCaps caps11 = device.GraphicsDeviceCapabilities.TextureAddressCapabilities;

            if (caps11.SupportsBorder)
            {
                caps.TextureAddressCaps |= TextureAddressCaps.Border;
            }
            if (caps11.SupportsClamp)
            {
                caps.TextureAddressCaps |= TextureAddressCaps.Clamp;
            }
            if (caps11.SupportsIndependentUV)
            {
                caps.TextureAddressCaps |= TextureAddressCaps.IndependentUV;
            }
            if (caps11.SupportsMirror)
            {
                caps.TextureAddressCaps |= TextureAddressCaps.Mirror;
            }
            if (caps11.SupportsMirrorOnce)
            {
                caps.TextureAddressCaps |= TextureAddressCaps.MirrorOnce;
            }
            if (caps11.SupportsWrap)
            {
                caps.TextureAddressCaps |= TextureAddressCaps.Wrap;
            }
            #endregion

            #region TextureCapabilities
            XG.GraphicsDeviceCapabilities.TextureCaps caps12 = device.GraphicsDeviceCapabilities.TextureCapabilities;
            if (caps12.SupportsAlpha)
            {
                caps.TextureCaps |= TextureCaps.Alpha;
            }
            if (caps12.SupportsCubeMap)
            {
                caps.TextureCaps |= TextureCaps.CubeMap;
            }
            if (caps12.SupportsMipCubeMap)
            {
                caps.TextureCaps |= TextureCaps.MipCubeMap;
            }
            if (caps12.SupportsMipMap)
            {
                caps.TextureCaps |= TextureCaps.MipMap;
            }
            if (caps12.SupportsMipVolumeMap)
            {
                caps.TextureCaps |= TextureCaps.MipVolumeMap;
            }
            if (caps12.SupportsNonPower2Conditional)
            {
                caps.TextureCaps |= TextureCaps.NonPow2Conditional;
            }
            if (caps12.SupportsNoProjectedBumpEnvironment)
            {
                caps.TextureCaps |= TextureCaps.NoProjectedBumpEnvironment;
            } 
            if (caps12.SupportsPerspective)
            {
                caps.TextureCaps |= TextureCaps.Perspective;
            }
            if (caps12.SupportsProjected)
            {
                caps.TextureCaps |= TextureCaps.Projected;
            }
            if (caps12.SupportsTextureRepeatNotScaledBySize)
            {
                caps.TextureCaps |= TextureCaps.TextureRepeatNotScaledBySize;
            }
            if (caps12.SupportsVolumeMap)
            {
                caps.TextureCaps |= TextureCaps.VolumeMap;
            }
            if (caps12.RequiresCubeMapPower2)
            {
                caps.TextureCaps |= TextureCaps.CubeMapPow2;
            }
            if (caps12.RequiresPower2)
            {
                caps.TextureCaps |= TextureCaps.Pow2;
            }
            if (caps12.RequiresSquareOnly)
            {
                caps.TextureCaps |= TextureCaps.SquareOnly;
            }
            if (caps12.RequiresVolumeMapPower2)
            {
                caps.TextureCaps |= TextureCaps.VolumeMapPow2;
            }
            #endregion

            #region TextureFilterCapabilities
            caps2 = device.GraphicsDeviceCapabilities.TextureFilterCapabilities;
            if (caps2.SupportsMagnifyAnisotropic)
            {
                caps.TextureFilterCaps |= FilterCaps.MagAnisotropic;
            }
            if (caps2.SupportsMagnifyGaussianQuad)
            {
                caps.TextureFilterCaps |= FilterCaps.MagGaussianQuad;
            }
            if (caps2.SupportsMagnifyLinear)
            {
                caps.TextureFilterCaps |= FilterCaps.MagLinear;
            }
            if (caps2.SupportsMagnifyPoint)
            {
                caps.TextureFilterCaps |= FilterCaps.MagPoint;
            }
            if (caps2.SupportsMagnifyPyramidalQuad)
            {
                caps.TextureFilterCaps |= FilterCaps.MagPyramidalQuad;
            }
            if (caps2.SupportsMinifyAnisotropic)
            {
                caps.TextureFilterCaps |= FilterCaps.MinAnisotropic;
            }
            if (caps2.SupportsMinifyGaussianQuad)
            {
                caps.TextureFilterCaps |= FilterCaps.MinGaussianQuad;
            }
            if (caps2.SupportsMinifyLinear)
            {
                caps.TextureFilterCaps |= FilterCaps.MinLinear;
            }
            if (caps2.SupportsMinifyPoint)
            {
                caps.TextureFilterCaps |= FilterCaps.MinPoint;
            }
            if (caps2.SupportsMinifyPyramidalQuad)
            {
                caps.TextureFilterCaps |= FilterCaps.MinPyramidalQuad;
            }
            if (caps2.SupportsMipMapLinear)
            {
                caps.TextureFilterCaps |= FilterCaps.MipLinear;
            }
            if (caps2.SupportsMipMapPoint)
            {
                caps.TextureFilterCaps |= FilterCaps.MipPoint;
            }
            #endregion

            #region VertexProcessingCapabilities
            XG.GraphicsDeviceCapabilities.VertexProcessingCaps caps13 = device.GraphicsDeviceCapabilities.VertexProcessingCapabilities;

            if (caps13.SupportsLocalViewer) 
            {
                caps.VertexProcessingCaps |= VertexProcessingCaps.LocalViewer;
            }
            if (caps13.SupportsNoTextureGenerationNonLocalViewer) 
            {
                caps.VertexProcessingCaps |= VertexProcessingCaps.NoTexGenNonLocalViewer;
            }
            if (caps13.SupportsTextureGeneration) 
            {
                caps.VertexProcessingCaps |= VertexProcessingCaps.TextureGen;
            }
            if (caps13.SupportsTextureGenerationSphereMap) 
            {
                caps.VertexProcessingCaps |= VertexProcessingCaps.TexGenSphereMap;
            }
            #endregion

            #region VertexShaderCapabilities
            XG.GraphicsDeviceCapabilities.VertexShaderCaps caps14 = device.GraphicsDeviceCapabilities.VertexShaderCapabilities;

            VertexShader20Caps vscaps = new VertexShader20Caps();

            if (caps14.SupportsPredication) 
            {
                vscaps.Caps |= VertexShaderCaps.Predication;
            }
            vscaps.DynamicFlowControlDepth = caps14.DynamicFlowControlDepth;
            vscaps.StaticFlowControlDepth = caps14.StaticFlowControlDepth;
            vscaps.TempCount = caps14.NumberTemps;

            caps.VS20Caps = vscaps;
            #endregion

            #region VolumeTextureAddressCapabilities
            caps11 = device.GraphicsDeviceCapabilities.VolumeTextureAddressCapabilities;

            if (caps11.SupportsBorder)
            {
                caps.VolumeTextureAddressCaps |= TextureAddressCaps.Border;
            }
            if (caps11.SupportsClamp)
            {
                caps.VolumeTextureAddressCaps |= TextureAddressCaps.Clamp;
            }
            if (caps11.SupportsIndependentUV)
            {
                caps.VolumeTextureAddressCaps |= TextureAddressCaps.IndependentUV;
            }
            if (caps11.SupportsMirror)
            {
                caps.VolumeTextureAddressCaps |= TextureAddressCaps.Mirror;
            }
            if (caps11.SupportsMirrorOnce)
            {
                caps.VolumeTextureAddressCaps |= TextureAddressCaps.MirrorOnce;
            }
            if (caps11.SupportsWrap)
            {
                caps.VolumeTextureAddressCaps |= TextureAddressCaps.Wrap;
            }
            #endregion

            #region VolumeTextureFilterCapabilities
            caps2 = device.GraphicsDeviceCapabilities.VolumeTextureFilterCapabilities;
            if (caps2.SupportsMagnifyAnisotropic)
            {
                caps.VolumeTextureFilterCaps |= FilterCaps.MagAnisotropic;
            }
            if (caps2.SupportsMagnifyGaussianQuad)
            {
                caps.VolumeTextureFilterCaps |= FilterCaps.MagGaussianQuad;
            }
            if (caps2.SupportsMagnifyLinear)
            {
                caps.VolumeTextureFilterCaps |= FilterCaps.MagLinear;
            }
            if (caps2.SupportsMagnifyPoint)
            {
                caps.VolumeTextureFilterCaps |= FilterCaps.MagPoint;
            }
            if (caps2.SupportsMagnifyPyramidalQuad)
            {
                caps.VolumeTextureFilterCaps |= FilterCaps.MagPyramidalQuad;
            }
            if (caps2.SupportsMinifyAnisotropic)
            {
                caps.VolumeTextureFilterCaps |= FilterCaps.MinAnisotropic;
            }
            if (caps2.SupportsMinifyGaussianQuad)
            {
                caps.VolumeTextureFilterCaps |= FilterCaps.MinGaussianQuad;
            }
            if (caps2.SupportsMinifyLinear)
            {
                caps.VolumeTextureFilterCaps |= FilterCaps.MinLinear;
            }
            if (caps2.SupportsMinifyPoint)
            {
                caps.VolumeTextureFilterCaps |= FilterCaps.MinPoint;
            }
            if (caps2.SupportsMinifyPyramidalQuad)
            {
                caps.VolumeTextureFilterCaps |= FilterCaps.MinPyramidalQuad;
            }
            if (caps2.SupportsMipMapLinear)
            {
                caps.VolumeTextureFilterCaps |= FilterCaps.MipLinear;
            }
            if (caps2.SupportsMipMapPoint)
            {
                caps.VolumeTextureFilterCaps |= FilterCaps.MipPoint;
            }
            #endregion

            #region VertexFormatCapabilities
            XG.GraphicsDeviceCapabilities.VertexFormatCaps caps15 = device.GraphicsDeviceCapabilities.VertexFormatCapabilities;
            if (caps15.SupportsDoNotStripElements)
            {
                caps.FVFCaps |= VertexFormatCaps.DoNotStripElements;
            }
            if (caps15.SupportsPointSize) 
            {
                caps.FVFCaps |= VertexFormatCaps.PointSize;
            }
            caps.MaxSimultaneousTextures = caps15.NumberSimultaneousTextureCoordinates;

            #endregion

            caps.AlphaFullScreenFlipOrDiscard = device.GraphicsDeviceCapabilities.DriverCapabilities.SupportsAlphaFullScreenFlipOrDiscard;
            caps.CanAutoGenerateMipmap = device.GraphicsDeviceCapabilities.DriverCapabilities.CanAutoGenerateMipMap;
            caps.CanCalibrateGamma = device.GraphicsDeviceCapabilities.DriverCapabilities.CanCalibrateGamma;
            caps.CanManageResource = device.GraphicsDeviceCapabilities.DriverCapabilities.CanManageResource;
            caps.CopyToSystemMemory = device.GraphicsDeviceCapabilities.DriverCapabilities.SupportsCopyToSystemMemory;
            caps.CopyToVideoMemory = device.GraphicsDeviceCapabilities.DriverCapabilities.SupportsCopyToVideoMemory;
            caps.DynamicTextures = device.GraphicsDeviceCapabilities.DriverCapabilities.SupportsDynamicTextures;
            caps.FullScreenGamma = device.GraphicsDeviceCapabilities.DriverCapabilities.SupportsFullScreenGamma;
            caps.LinearToSrgbPresentation = device.GraphicsDeviceCapabilities.DriverCapabilities.SupportsLinearToSrgbPresentation;


            caps.ExtentsAdjust = device.GraphicsDeviceCapabilities.ExtentsAdjust;
            caps.GuardBandBottom = device.GraphicsDeviceCapabilities.GuardBandBottom;
            caps.GuardBandLeft = device.GraphicsDeviceCapabilities.GuardBandLeft;
            caps.GuardBandRight = device.GraphicsDeviceCapabilities.GuardBandRight;
            caps.GuardBandTop = device.GraphicsDeviceCapabilities.GuardBandTop;

            caps.AdapterOrdinalInGroup = device.GraphicsDeviceCapabilities.AdapterOrdinalInGroup;
            caps.MasterAdapterOrdinal = device.GraphicsDeviceCapabilities.MasterAdapterOrdinal;
            caps.MaxAnisotropy = device.GraphicsDeviceCapabilities.MaxAnisotropy;
            caps.MaxPixelShader30InstructionSlots = device.GraphicsDeviceCapabilities.MaxPixelShader30InstructionSlots;
            caps.MaxPointSize = device.GraphicsDeviceCapabilities.MaxPointSize;
            caps.MaxPrimitiveCount = device.GraphicsDeviceCapabilities.MaxPrimitiveCount;
            caps.MaxSimultaneousTextures = device.GraphicsDeviceCapabilities.MaxSimultaneousTextures;
            caps.MaxStreams = device.GraphicsDeviceCapabilities.MaxStreams;
            caps.MaxStreamStride = device.GraphicsDeviceCapabilities.MaxStreamStride;
            caps.MaxTextureAspectRatio = device.GraphicsDeviceCapabilities.MaxTextureAspectRatio;
            caps.MaxTextureHeight = device.GraphicsDeviceCapabilities.MaxTextureHeight;
            caps.MaxTextureRepeat = device.GraphicsDeviceCapabilities.MaxTextureRepeat;
            caps.MaxTextureWidth = device.GraphicsDeviceCapabilities.MaxTextureWidth;
            caps.MaxUserClipPlanes = device.GraphicsDeviceCapabilities.MaxUserClipPlanes;
            caps.MaxVertexIndex = device.GraphicsDeviceCapabilities.MaxVertexIndex;
            caps.MaxVertexShader30InstructionSlots = device.GraphicsDeviceCapabilities.MaxVertexShader30InstructionSlots;
            caps.MaxVertexShaderConstants = device.GraphicsDeviceCapabilities.MaxVertexShaderConstants;
            caps.MaxVertexW = device.GraphicsDeviceCapabilities.MaxVertexW;
            caps.MaxVolumeExtent = device.GraphicsDeviceCapabilities.MaxVolumeExtent;
            caps.NumberOfAdaptersInGroup = device.GraphicsDeviceCapabilities.NumberOfAdaptersInGroup;
            caps.PixelShader1xMaxValue = device.GraphicsDeviceCapabilities.PixelShader1xMaxValue;
            caps.PixelShaderVersion = device.GraphicsDeviceCapabilities.PixelShaderVersion;

            #region PresentInterval
            XG.PresentInterval caps16 = device.GraphicsDeviceCapabilities.PresentInterval;

            if ((caps16 & XG.PresentInterval.Immediate) == XG.PresentInterval.Immediate)
            {
                caps.PresentationIntervals |= PresentInterval.Immediate;
            }
            if ((caps16 & XG.PresentInterval.One) == XG.PresentInterval.One)
            {
                caps.PresentationIntervals |= PresentInterval.One;
            }
            if ((caps16 & XG.PresentInterval.Two) == XG.PresentInterval.Two)
            {
                caps.PresentationIntervals |= PresentInterval.Two;
            }
            if ((caps16 & XG.PresentInterval.Three) == XG.PresentInterval.Three)
            {
                caps.PresentationIntervals |= PresentInterval.Three;
            }
            if ((caps16 & XG.PresentInterval.Four) == XG.PresentInterval.Four)
            {
                caps.PresentationIntervals |= PresentInterval.Immediate;
            }
            if ((caps16 & XG.PresentInterval.Default) == XG.PresentInterval.Default)
            {
                caps.PresentationIntervals |= PresentInterval.Default;
            }
            #endregion

            caps.SimultaneousRTCount = device.GraphicsDeviceCapabilities.MaxSimultaneousRenderTargets;
            caps.VertexShaderVersion = device.GraphicsDeviceCapabilities.VertexShaderVersion;


            #endregion

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