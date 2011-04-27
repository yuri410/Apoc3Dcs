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
using Apoc3D.Collections;
using Apoc3D.Core;
using Apoc3D.Graphics;
using Apoc3D.Graphics.Effects;
using Apoc3D.MathLib;
using X = Microsoft.Xna.Framework;
using XG = Microsoft.Xna.Framework.Graphics;
using XGC = Microsoft.Xna.Framework.Graphics.GraphicsDeviceCapabilities;

namespace Apoc3D.RenderSystem.Xna
{
    class XnaRenderSystem : Apoc3D.Graphics.RenderSystem
    {
        const int MaxVertexTex = 4;

        const int MaxTexLayers = Material.MaxTexLayers;


   
        X.GraphicsDeviceManager manager;

        XnaRenderTarget[] cachedRenderTargets;

        XnaRenderTarget defaultRT;

        XnaRenderStateManager renderStates;
        XnaObjectFactory objectFactory;


        XnaTexture[] cachedTextures = new XnaTexture[MaxTexLayers];

        SamplerStateCollection samStateColl;

        internal XG.GraphicsDevice Device
        {
            get { return manager.GraphicsDevice; }
        }

        public XnaRenderSystem(X.GraphicsDeviceManager manager)
            : base(XnaGraphicsAPIFactory.APIName + " RenderSystem")
        {
            this.manager = manager;
            this.mode = RenderMode.Final;

        }

        public override void Init()
        {
            this.renderStates = new XnaRenderStateManager(this);
            base.RenderStates = renderStates;

            this.objectFactory = new XnaObjectFactory(this);
            base.ObjectFactory = objectFactory;

            #region ªÒ»°Capabilities
            Capabilities caps = new Capabilities();

            #region AlphaCompareCapabilities
            XGC.CompareCaps caps1 = Device.GraphicsDeviceCapabilities.AlphaCompareCapabilities;

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
            XGC.FilterCaps caps2 = Device.GraphicsDeviceCapabilities.CubeTextureFilterCapabilities;
            
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
            XGC.DeclarationTypeCaps caps3 = Device.GraphicsDeviceCapabilities.DeclarationTypeCapabilities;
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
            caps1 = Device.GraphicsDeviceCapabilities.DepthBufferCompareCapabilities;
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
            XGC.BlendCaps caps4 = Device.GraphicsDeviceCapabilities.DestinationBlendCapabilities;
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
            XGC.DeviceCaps caps5 = Device.GraphicsDeviceCapabilities.DeviceCapabilities;
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
            XGC.PixelShaderCaps caps6 = Device.GraphicsDeviceCapabilities.PixelShaderCapabilities;

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
            XGC.LineCaps lineCaps = Device.GraphicsDeviceCapabilities.LineCapabilities;
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
            XGC.PrimitiveCaps caps10 = Device.GraphicsDeviceCapabilities.PrimitiveCapabilities;
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
            XGC.RasterCaps caps7 = Device.GraphicsDeviceCapabilities.RasterCapabilities;

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
            XGC.ShadingCaps caps8 = Device.GraphicsDeviceCapabilities.ShadingCapabilities;
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
            caps4 = Device.GraphicsDeviceCapabilities.SourceBlendCapabilities;
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
            XGC.StencilCaps caps9 = Device.GraphicsDeviceCapabilities.StencilCapabilities;
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
            XGC.AddressCaps caps11 = Device.GraphicsDeviceCapabilities.TextureAddressCapabilities;

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
            XGC.TextureCaps caps12 = Device.GraphicsDeviceCapabilities.TextureCapabilities;
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
            caps2 = Device.GraphicsDeviceCapabilities.TextureFilterCapabilities;
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
            XGC.VertexProcessingCaps caps13 = Device.GraphicsDeviceCapabilities.VertexProcessingCapabilities;

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
            XGC.VertexShaderCaps caps14 = Device.GraphicsDeviceCapabilities.VertexShaderCapabilities;

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
            caps11 = Device.GraphicsDeviceCapabilities.VolumeTextureAddressCapabilities;

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
            caps2 = Device.GraphicsDeviceCapabilities.VolumeTextureFilterCapabilities;
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
            XGC.VertexFormatCaps caps15 = Device.GraphicsDeviceCapabilities.VertexFormatCapabilities;
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

            caps.AlphaFullScreenFlipOrDiscard = Device.GraphicsDeviceCapabilities.DriverCapabilities.SupportsAlphaFullScreenFlipOrDiscard;
            caps.CanAutoGenerateMipmap = Device.GraphicsDeviceCapabilities.DriverCapabilities.CanAutoGenerateMipMap;
            caps.CanCalibrateGamma = Device.GraphicsDeviceCapabilities.DriverCapabilities.CanCalibrateGamma;
            caps.CanManageResource = Device.GraphicsDeviceCapabilities.DriverCapabilities.CanManageResource;
            caps.CopyToSystemMemory = Device.GraphicsDeviceCapabilities.DriverCapabilities.SupportsCopyToSystemMemory;
            caps.CopyToVideoMemory = Device.GraphicsDeviceCapabilities.DriverCapabilities.SupportsCopyToVideoMemory;
            caps.DynamicTextures = Device.GraphicsDeviceCapabilities.DriverCapabilities.SupportsDynamicTextures;
            caps.FullScreenGamma = Device.GraphicsDeviceCapabilities.DriverCapabilities.SupportsFullScreenGamma;
            caps.LinearToSrgbPresentation = Device.GraphicsDeviceCapabilities.DriverCapabilities.SupportsLinearToSrgbPresentation;


            caps.ExtentsAdjust = Device.GraphicsDeviceCapabilities.ExtentsAdjust;
            caps.GuardBandBottom = Device.GraphicsDeviceCapabilities.GuardBandBottom;
            caps.GuardBandLeft = Device.GraphicsDeviceCapabilities.GuardBandLeft;
            caps.GuardBandRight = Device.GraphicsDeviceCapabilities.GuardBandRight;
            caps.GuardBandTop = Device.GraphicsDeviceCapabilities.GuardBandTop;

            caps.AdapterOrdinalInGroup = Device.GraphicsDeviceCapabilities.AdapterOrdinalInGroup;
            caps.MasterAdapterOrdinal = Device.GraphicsDeviceCapabilities.MasterAdapterOrdinal;
            caps.MaxAnisotropy = Device.GraphicsDeviceCapabilities.MaxAnisotropy;
            caps.MaxPixelShader30InstructionSlots = Device.GraphicsDeviceCapabilities.MaxPixelShader30InstructionSlots;
            caps.MaxPointSize = Device.GraphicsDeviceCapabilities.MaxPointSize;
            caps.MaxPrimitiveCount = Device.GraphicsDeviceCapabilities.MaxPrimitiveCount;
            caps.MaxSimultaneousTextures = Device.GraphicsDeviceCapabilities.MaxSimultaneousTextures;
            caps.MaxSimultaneousRT = Device.GraphicsDeviceCapabilities.MaxSimultaneousRenderTargets;
            caps.MaxStreams = Device.GraphicsDeviceCapabilities.MaxStreams;
            caps.MaxStreamStride = Device.GraphicsDeviceCapabilities.MaxStreamStride;
            caps.MaxTextureAspectRatio = Device.GraphicsDeviceCapabilities.MaxTextureAspectRatio;
            caps.MaxTextureHeight = Device.GraphicsDeviceCapabilities.MaxTextureHeight;
            caps.MaxTextureRepeat = Device.GraphicsDeviceCapabilities.MaxTextureRepeat;
            caps.MaxTextureWidth = Device.GraphicsDeviceCapabilities.MaxTextureWidth;
            caps.MaxUserClipPlanes = Device.GraphicsDeviceCapabilities.MaxUserClipPlanes;
            caps.MaxVertexIndex = Device.GraphicsDeviceCapabilities.MaxVertexIndex;
            caps.MaxVertexShader30InstructionSlots = Device.GraphicsDeviceCapabilities.MaxVertexShader30InstructionSlots;
            caps.MaxVertexShaderConstants = Device.GraphicsDeviceCapabilities.MaxVertexShaderConstants;
            caps.MaxVertexW = Device.GraphicsDeviceCapabilities.MaxVertexW;
            caps.MaxVolumeExtent = Device.GraphicsDeviceCapabilities.MaxVolumeExtent;
            caps.NumberOfAdaptersInGroup = Device.GraphicsDeviceCapabilities.NumberOfAdaptersInGroup;
            caps.PixelShader1xMaxValue = Device.GraphicsDeviceCapabilities.PixelShader1xMaxValue;
            caps.PixelShaderVersion = Device.GraphicsDeviceCapabilities.PixelShaderVersion;
            
            #region PresentInterval
            XG.PresentInterval caps16 = Device.GraphicsDeviceCapabilities.PresentInterval;

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

            caps.VertexShaderVersion = Device.GraphicsDeviceCapabilities.VertexShaderVersion;


            #endregion

            
            cachedRenderTargets = new XnaRenderTarget[Device.GraphicsDeviceCapabilities.MaxSimultaneousRenderTargets];

            XnaSamplerState[] states = new XnaSamplerState[Device.GraphicsDeviceCapabilities.MaxSimultaneousTextures];
            for (int i = 0; i < states.Length; i++) 
            {
                states[i] = new XnaSamplerState(this, i);
            }

            samStateColl = new SamplerStateCollection(states);

        }

        public override void Clear(ClearFlags flags, ColorValue color, float depth, int stencil)
        {
            XG.Color clr = new XG.Color(color.R, color.G, color.B, color.A);
            Device.Clear(XnaUtils.ConvertEnum(flags), clr, depth, stencil);
        }

        public override void SetRenderTarget(int index, RenderTarget rt)
        {
            if (rt == null)
            {
                throw new ArgumentNullException("rt");
                //Device.SetRenderTarget(index, null);
                //cachedRenderTargets[index] = null;
                //Device.DepthStencilBuffer = defaultDSBuffer;
            }
            //else
            //{
            XnaRenderTarget xrt = (XnaRenderTarget)rt;

            Device.SetRenderTarget(index, xrt.colorBufXna);

            if (xrt.depthBufXna != null)
            {
                Device.DepthStencilBuffer = xrt.depthBufXna;
            }

            cachedRenderTargets[index] = xrt;
            //}
        }

        public override RenderTarget GetRenderTarget(int index)
        {
            if (defaultRT == null)
            {
                defaultRT = new XnaRenderTarget(this, Device.DepthStencilBuffer);
                cachedRenderTargets[0] = defaultRT;
            }
            return cachedRenderTargets[index];
        }

        public override void BindShader(VertexShader shader)
        {
            if (shader == null)
            {
                Device.VertexShader = null;
            }
            else
            {
                XnaVertexShader vs = (XnaVertexShader)shader;
                Device.VertexShader = vs.vsXna;
            }
        }

        public override void BindShader(PixelShader shader)
        {
            if (shader == null)
            {
                Device.PixelShader = null;
            }
            else
            {
                XnaPixelShader ps = (XnaPixelShader)shader;
                Device.PixelShader = ps.psXna;
            }
        }

        public override Viewport Viewport
        {
            get
            {
                Viewport result;
                XG.Viewport xv = Device.Viewport;
                result.Height = xv.Height;
                result.MaxZ = xv.MaxDepth;
                result.MinZ = xv.MinDepth;
                result.Width = xv.Width;
                result.X = xv.X;
                result.Y = xv.Y;
                return result;
            }
            set
            {
                XG.Viewport result = new XG.Viewport();
                result.Height = value.Height;
                result.MaxDepth = value.MaxZ;
                result.MinDepth = value.MinZ;
                result.Width = value.Width;
                result.X = value.X;
                result.Y = value.Y;
                Device.Viewport = result;
            }
        }
        public override void RenderSimpleBlend(GeomentryData op)
        {
            base.RenderSimpleBlend(op);

            renderStates.AlphaBlendEnable = true;
            renderStates.SourceBlend = Blend.SourceAlpha;
            renderStates.DestinationBlend = Blend.InverseSourceAlpha;
            renderStates.BlendOperation = BlendFunction.Add;

            renderStates.CullMode = CullMode.Clockwise;

            renderStates.AlphaTestEnable = false;

            renderStates.DepthBufferEnable = false;
            renderStates.DepthBufferWriteEnable = false;

            if (op.VertexCount == 0)
                return;

            XnaVertexBuffer xnavb = (XnaVertexBuffer)op.VertexBuffer;

            if (xnavb.vertexBuffer != null)
            {
                Device.Vertices[0].SetSource(xnavb.vertexBuffer, 0, op.VertexSize);
            }
            else
            {
                Device.Vertices[0].SetSource(xnavb.dynVb, 0, op.VertexSize);
            }

            Device.VertexDeclaration = ((XnaVertexDeclaration)op.VertexDeclaration).vtxDecl;

            if (op.UseIndices)
            {
                XnaIndexBuffer xnaib = (XnaIndexBuffer)op.IndexBuffer;
                if (xnaib.indexBuffer != null)
                {
                    Device.Indices = xnaib.indexBuffer;
                }
                else
                {
                    Device.Indices = xnaib.dynIb;
                }

                Device.DrawIndexedPrimitives(XnaUtils.ConvertEnum(op.PrimitiveType),
                    op.BaseVertex, 0,
                    op.VertexCount, op.BaseIndexStart,
                    op.PrimCount);
            }
            else
            {
                Device.DrawPrimitives(XnaUtils.ConvertEnum(op.PrimitiveType), 0, op.PrimCount);
            }
        }
        public override void RenderSimple(GeomentryData op)
        {
            base.RenderSimple(op);


            renderStates.AlphaBlendEnable = false;
            renderStates.CullMode = CullMode.Clockwise;

            renderStates.AlphaTestEnable = false;

            renderStates.DepthBufferEnable = false;
            renderStates.DepthBufferWriteEnable = false;

            if (op.VertexCount == 0)
                return;

            XnaVertexBuffer xnavb = (XnaVertexBuffer)op.VertexBuffer;

            if (xnavb.vertexBuffer != null)
            {
                Device.Vertices[0].SetSource(xnavb.vertexBuffer, 0, op.VertexSize);
            }
            else
            {
                Device.Vertices[0].SetSource(xnavb.dynVb, 0, op.VertexSize);
            }

            Device.VertexDeclaration = ((XnaVertexDeclaration)op.VertexDeclaration).vtxDecl;

            if (op.UseIndices)
            {
                XnaIndexBuffer xnaib = (XnaIndexBuffer)op.IndexBuffer;
                if (xnaib.indexBuffer != null)
                {
                    Device.Indices = xnaib.indexBuffer;
                }
                else
                {
                    Device.Indices = xnaib.dynIb;
                }

                Device.DrawIndexedPrimitives(XnaUtils.ConvertEnum(op.PrimitiveType),
                    op.BaseVertex, 0,
                    op.VertexCount, op.BaseIndexStart,
                    op.PrimCount);
            }
            else
            {
                Device.DrawPrimitives(XnaUtils.ConvertEnum(op.PrimitiveType), 0, op.PrimCount);
            }
        }
        public override void Render(Material material, RenderOperation[] op)
        {
            Render(material, op, op.Length);
        }


        MaterialFlags currentBlendFlags;

        public override void Render(Material material, RenderOperation[] opList, int count)
        {
            if (opList == null)
                return;

            Device.PixelShader = null;
            Device.VertexShader = null;
            
            Effect effect = material.Effect;

            if (effect == null)
            {
                return;
            }
            if (!effect.SupportsMode(mode))
            {
                return;
            }

            //if (currentBlendFlags != material.Flags)
            {
                switch (material.Flags)
                {
                    case MaterialFlags.None:
                        renderStates.SourceBlend = Blend.SourceAlpha;
                        renderStates.DestinationBlend = Blend.InverseSourceAlpha;
                        renderStates.BlendOperation = BlendFunction.Add;
                        break;

                    case MaterialFlags.BlendBright_Color:
                        renderStates.SourceBlend = Blend.SourceColor;
                        renderStates.DestinationBlend = Blend.One;
                        renderStates.BlendOperation = BlendFunction.Add;
                        break;

                    case MaterialFlags.BlendBright:

                        renderStates.SourceBlend = Blend.SourceAlpha;
                        renderStates.DestinationBlend = Blend.One;
                        renderStates.BlendOperation = BlendFunction.Add;
                        break;
                }
                currentBlendFlags = material.Flags;
            }

            renderStates.AlphaBlendEnable = material.IsTransparent;
            renderStates.CullMode = material.CullMode;
            if (material.AlphaRef > 0)
            {
                renderStates.AlphaTestEnable = true;
                renderStates.AlphaReference = (int)(material.AlphaRef * byte.MaxValue);
                renderStates.AlphaFunction = CompareFunction.GreaterEqual;
            }
            else
            {
                renderStates.AlphaTestEnable = false;
            }
            renderStates.DepthBufferEnable = material.ZEnabled;
            renderStates.DepthBufferWriteEnable = material.ZWriteEnabled;

            int passCount = effect.Begin(mode);
            for (int p = 0; p < passCount; p++)
            {
                effect.BeginPass(p);

                for (int j = 0; j < count; j++)
                {
                    RenderOperation op = opList[j];
                    GeomentryData gm = op.Geomentry;

                    if (gm.VertexCount == 0 || gm.PrimCount == 0)
                        continue;

                    BatchCount++;
                    PrimitiveCount += gm.PrimCount;
                    VertexCount += gm.VertexCount;

                    effect.Setup(material, ref op);

                    XnaVertexBuffer xnavb = (XnaVertexBuffer)gm.VertexBuffer;

                    if (xnavb.vertexBuffer != null)
                    {
                        Device.Vertices[0].SetSource(xnavb.vertexBuffer, 0, gm.VertexSize);
                    }
                    else
                    {
                        Device.Vertices[0].SetSource(xnavb.dynVb, 0, gm.VertexSize);
                    }

                    Device.VertexDeclaration = ((XnaVertexDeclaration)gm.VertexDeclaration).vtxDecl;

                    if (gm.UseIndices)
                    {
                        XnaIndexBuffer xnaib = (XnaIndexBuffer)gm.IndexBuffer;
                        if (xnaib.indexBuffer != null)
                        {
                            Device.Indices = xnaib.indexBuffer;
                        }
                        else
                        {
                            Device.Indices = xnaib.dynIb;
                        }

                        try
                        {
                            Device.DrawIndexedPrimitives(XnaUtils.ConvertEnum(gm.PrimitiveType),
                                gm.BaseVertex, 0,
                                gm.VertexCount, gm.BaseIndexStart,
                                gm.PrimCount);
                        }
                        catch (Exception) { }
                    }
                    else
                    {
                        try
                        {
                            Device.DrawPrimitives(XnaUtils.ConvertEnum(gm.PrimitiveType), 0, gm.PrimCount);
                        }
                        catch (Exception) { }
                    }
                } // for (int j = 0; j < opList.Count; j++)
                effect.EndPass();
            }
            effect.End();
        }

        public override void BeginFrame()
        {    
            base.BeginFrame();

            //this.defaultDSBuffer = Device.DepthStencilBuffer;


            //ResourceInterlock.BlockAll();
        }
        public override void EndFrame()
        {
            XG.TextureCollection coll = Device.Textures;

            for (int i = 0; i < MaxTexLayers; i++) 
            {
                coll[i] = null;
            }

            coll = Device.VertexTextures;
            for (int i = 0; i < MaxVertexTex; i++) 
            {
                coll[i] = null;
            }

            XG.VertexStreamCollection coll2 = Device.Vertices;
            for (int i = 0; i < RenderSystemCaps.MaxStreams; i++)
            {
                Device.Vertices[0].SetSource(null, 0, 0);
            }
            

            base.EndFrame();

            //ResourceInterlock.UnblockAll();
        }

        public override void SetTexture(int index, Texture texture)
        {
            XnaTexture xt = (XnaTexture)texture;
            if (xt != null)
            {
                if (xt.State == ResourceState.Loaded)
                {
                    if (xt.cube != null)
                        Device.Textures[index] = xt.cube;
                    else if (xt.tex2D != null)
                        Device.Textures[index] = xt.tex2D;
                    else
                        Device.Textures[index] = xt.tex3D;
                    cachedTextures[index] = xt;
                }
                else 
                {
                    if (xt.cube != null)
                        Device.Textures[index] = null;
                    else if (xt.tex2D != null)
                        Device.Textures[index] = null;
                    else
                        Device.Textures[index] = null;
                    cachedTextures[index] = null;
                }
            }
        }
        public override Texture GetTexture(int index)
        {
            return cachedTextures[index];
        }

        public override SamplerStateCollection GetSamplerStates()
        {
            return samStateColl;
        }

    }
}