using System;
using System.Collections.Generic;
using System.Text;

namespace Apoc3D.Graphics
{
    //public enum StateBlockType
    //{
    //    All = 0,
    //    VertexState = 1,
    //    PixelState = 2
    //}
    public enum LockMode
    {
        None,
        Discard,
        ReadOnly,
        NoOverwrite
    }

    /// <summary>
    ///  定义用于alpha，蒙板，以及深度测试所用到的比较方式。
    /// </summary>
    /// <remarks>可以直接转换的API：XNA</remarks>
    public enum CompareFunction
    {
        /// <summary>
        ///  总是无法通过测试
        /// </summary>
        Never = 1,

        /// <summary>
        ///  如果预定值比当前像素的值小，则通过
        /// </summary>
        Less = 2,

        /// <summary>
        ///  如果预定值与当前像素的值相同，则通过
        /// </summary>
        Equal = 3,

        /// <summary>
        ///  如果预定值小于等于当前像素的值，则通过
        /// </summary>
        LessEqual = 4,
        
        /// <summary>
        ///  如果预定值当前像素的值大，则通过
        /// </summary>
        Greater = 5,

        /// <summary>
        ///  如果预定值与当前像素的值不相同，则通过
        /// </summary>
        NotEqual = 6,

        /// <summary>
        ///  如果预定值大于等于当前像素的值，则通过
        /// </summary>
        GreaterEqual = 7,

        /// <summary>
        ///  总是通过测试
        /// </summary>
        Always = 8,
    }

    /// <summary>
    ///  Defines the color channels that can be chosen for a per-channel write to
    ///  a render target color buffer.
    /// </summary>
    /// <remarks>可以直接转换的API：XNA</remarks>
    [Flags()]
    public enum ColorWriteChannels
    {
        /// <summary>
        ///  No channel selected.
        /// </summary>
        None = 0,
        /// <summary>
        ///  Red channel of a buffer.
        /// </summary>
        Red = 1,
        /// <summary>
        ///  Green channel of a buffer.
        /// </summary>
        Green = 2,
        /// <summary>
        ///  Blue channel of a buffer.
        /// </summary>
        Blue = 4,
        /// <summary>
        ///  Alpha channel of a buffer.
        /// </summary>
        Alpha = 8,        
        /// <summary>
        ///  All buffer channels.
        /// </summary>
        All = 15,
    }

    [Flags()]
    public enum BufferUsage
    {
        Static = 1,
        Dynamic = 2,
        WriteOnly = 4,
        Discardable = 8
    }


    public enum IndexBufferType    
    {
        Bit16,
        Bit32
    }

    public enum CullMode
    {
        Clockwise = 2,
        CounterClockwise = 3,
        None = 1
    }

    public enum CubeMapFace
    {
        PositiveX,
        NegativeX,
        PositiveY,
        NegativeY,
        PositiveZ,
        NegativeZ
    }


    public enum FillMode
    {
        Point = 1,
        Solid = 3,
        WireFrame = 2
    }

    public enum MultiSampleType
    {
        None,
        NonMaskable,
        TwoSamples,
        ThreeSamples,
        FourSamples,
        FiveSamples,
        SixSamples,
        SevenSamples,
        EightSamples,
        NineSamples,
        TenSamples,
        ElevenSamples,
        TwelveSamples,
        ThirteenSamples,
        FourteenSamples,
        FifteenSamples,
        SixteenSamples
    }

    [Flags]
    public enum PresentOptions
    {
        DeviceClip = 4,
        DiscardDepthStencil = 2,
        None = 0,
        Video = 16
    }

    /// <summary>
    ///  
    /// </summary>
    /// <remarks>对xna可以直接转换</remarks>
    [Flags]
    public enum ClearFlags
    {
        Target = 1,
        DepthBuffer = 2,
        Stencil = 4
    }

    /// <summary>
    /// 
    /// </summary>
    /// <remarks>可以直接转换的API：XNA</remarks>
    public enum BlendFunction
    {
        /// <summary>
        ///  The result is the destination added to the source.Result = (Source Color
        ///  * Source Blend) + (Destination Color * Destination Blend)
        /// </summary>
        Add = 1,

        /// <summary>
        ///  The result is the destination subtracted from the source.Result = (Source
        ///  Color * Source Blend) − (Destination Color * Destination Blend)
        /// </summary>
        Subtract = 2,
   
        /// <summary>
        ///  The result is the source subtracted from the destination.Result = (Destination
        ///  Color * Destination Blend) −(Source Color * Source Blend)
        /// </summary>
        ReverseSubtract = 3,

        /// <summary>
        ///  The result is the minimum of the source and destination.Result = min( (Source
        ///     Color * Source Blend), (Destination Color * Destination Blend) )
        /// </summary>
        Min = 4,

        /// <summary>
        ///  The result is the maximum of the source and destination.Result = max( (Source
        ///     Color * Source Blend), (Destination Color * Destination Blend) )
        /// </summary>
        Max = 5,
    }

    /// <summary>
    ///  定义颜色混合因子
    /// </summary>
    /// <remarks>可以直接转换的API：XNA</remarks>
    public enum Blend
    {
        /// <summary>
        ///  每个颜色通道都会乘以0
        /// </summary>
        Zero = 1,

        /// <summary>
        ///  每个颜色通道都会乘以1
        /// </summary>
        One = 2,
        
        /// <summary>
        ///  每个颜色通道都会和混合源的颜色中的相应通道相乘
        /// </summary>
        SourceColor = 3,

        /// <summary>
        ///  Each component of the color is multiplied by the inverse of the source color.
        ///  This can be represented as (1 − Rs, 1 − Gs, 1 − Bs, 1 − As) where R, G, B,
        ///  and A respectively stand for the red, green, blue, and alpha destination
        ///  values.
        /// </summary>
        InverseSourceColor = 4,

        /// <summary>
        ///  Each component of the color is multiplied by the alpha value of the source.
        ///  This can be represented as (As, As, As, As), where As is the alpha source
        ///  value.
        /// </summary>
        SourceAlpha = 5,
  
        /// <summary>
        ///  Each component of the color is multiplied by the inverse of the alpha value
        ///  of the source. This can be represented as (1 − As, 1 − As, 1 − As, 1 − As),
        ///  where As is the alpha destination value.
        /// </summary>
        InverseSourceAlpha = 6,
  
        /// <summary>
        ///  Each component of the color is multiplied by the alpha value of the destination.
        ///  This can be represented as (Ad, Ad, Ad, Ad), where Ad is the destination
        ///  alpha value.
        /// </summary>
        DestinationAlpha = 7,
        
        /// <summary>
        ///  Each component of the color is multiplied by the inverse of the alpha value
        ///  of the destination. This can be represented as (1 − Ad, 1 − Ad, 1 − Ad, 1
        ///  − Ad), where Ad is the alpha destination value.
        /// </summary>
        InverseDestinationAlpha = 8,
        
        /// <summary>
        ///  Each component color is multiplied by the destination color. This can be
        ///  represented as (Rd, Gd, Bd, Ad), where R, G, B, and A respectively stand
        ///  for red, green, blue, and alpha destination values.
        /// </summary>
        DestinationColor = 9,
          
        /// <summary>
        ///  Each component of the color is multiplied by the inverse of the destination
        ///  color. This can be represented as (1 − Rd, 1 − Gd, 1 − Bd, 1 − Ad), where
        ///  Rd, Gd, Bd, and Ad respectively stand for the red, green, blue, and alpha
        ///  destination values.
        /// </summary>
        InverseDestinationColor = 10,
           
        /// <summary>
        ///  Each component of the color is multiplied by either the alpha of the source
        ///  color, or the inverse of the alpha of the source color, whichever is greater.
        ///  This can be represented as (f, f, f, 1), where f = min(A, 1 − Ad).
        /// </summary>
        SourceAlphaSaturation = 11,
            
        /// <summary>
        ///  This mode is obsolete. The same effect can be achieved by setting the source
        ///  and destination blend factors to SourceAlpha and InverseSourceAlpha in separate
        ///  calls.
        /// </summary>
        BothSourceAlpha = 12,
        
        /// <summary>
        ///  Each component of the source color is multiplied by the inverse
        ///  of the alpha of the source color, and each component of the destination color
        ///  is multiplied by the alpha of the source color. This can be represented as
        ///  (1 − As, 1 − As, 1 − As, 1 − As), with a destination blend factor of (As,
        ///  As, As, As); the destination blend selection is overridden. This blend mode
        ///  is supported only for the RenderState.SourceBlend render state.
        /// </summary>
        [Obsolete("Win32 only")]
        BothInverseSourceAlpha = 13,
        
        /// <summary>
        ///  Each component of the color is multiplied by a constant set in RenderStateManager.BlendFactor.
        /// </summary>
        BlendFactor = 14,

        /// <summary>
        ///  Each component of the color is multiplied by the inverse of a constant set
        ///  in RenderState.BlendFactor. This blend mode is supported only if GraphicsDeviceCapabilities.BlendCaps.SupportsBlendFactor
        ///  is true in the GraphicsDeviceCapabilities.SourceBlendCapabilities or GraphicsDeviceCapabilities.DestinationBlendCapabilities
        ///  properties.
        /// </summary>
        InverseBlendFactor = 15,
    }

    [Flags]
    public enum QueryUsages
    {
        Filter = 131072,
        None = 0,
        PostPixelShaderBlending = 524288,
        SrgbRead = 65536,
        SrgbWrite = 262144,
        VertexTexture = 1048576,
        WrapAndMip = 2097152
    }

    /// <summary>
    ///  Defines stencil buffer operations.
    /// </summary>
    /// <remarks>可以直接转换的API：XNA</remarks>
    public enum StencilOperation
    {
        /// <summary>
        ///  Does not update the stencil-buffer entry. This is the default value.
        /// </summary>
        Keep = 1,
        /// <summary>
        ///  Sets the stencil-buffer entry to 0.
        /// </summary>
        Zero = 2,
        /// <summary>
        ///  Replaces the stencil-buffer entry with a reference value.
        /// </summary>
        Replace = 3,
        /// <summary>
        ///  Increments the stencil-buffer entry, clamping to the maximum value.
        /// </summary>
        IncrementSaturation = 4,
        /// <summary>
        ///  Decrements the stencil-buffer entry, clamping to 0.
        /// </summary>
        DecrementSaturation = 5,
        /// <summary>
        ///  Inverts the bits in the stencil-buffer entry.
        /// </summary>
        Invert = 6,
        /// <summary>
        ///  Increments the stencil-buffer entry, wrapping to 0 if the new value exceeds
        ///     the maximum value.
        /// </summary>
        Increment = 7,
        /// <summary>
        ///  Decrements the stencil-buffer entry, wrapping to the maximum value if the
        ///     new value is less than 0.
        /// </summary>
        Decrement = 8,
    }

    public enum SwapEffect
    {
        Default,
        Discard,
        Flip,
        Copy
    }

    /// <summary>
    ///  Defines constants that describe supported texture-addressing modes. Reference
    ///  page contains links to related conceptual articles.
    /// </summary>
    /// <remarks>可以直接转换的API：XNA</remarks>
    public enum TextureAddressMode
    {
        /// <summary>
        ///  Tile the texture at every integer junction. For example, for u values between
        ///  0 and 3, the texture is repeated three times; no mirroring is performed.
        /// </summary>
        Wrap = 1,
        /// <summary>
        ///   Similar to Wrap, except that the texture is flipped at every integer junction.
        ///   For u values between 0 and 1, for example, the texture is addressed normally;
        ///   between 1 and 2, the texture is flipped (mirrored); between 2 and 3, the
        ///   texture is normal again, and so on.
        /// </summary>
        Mirror = 2,        
        /// <summary>
        ///   Texture coordinates outside the range [0.0, 1.0] are set to the texture color
        ///   at 0.0 or 1.0, respectively.
        /// </summary>
        Clamp = 3,        
        /// <summary>
        ///  Texture coordinates outside the range [0.0, 1.0] are set to the border color.
        /// </summary>
        Border = 4,
        /// <summary>
        ///  Similar to Mirror and Clamp. Takes the absolute value of the texture coordinate
        ///  (thus, mirroring around 0), and then clamps to the maximum value. The most
        ///  common usage is for volume textures, where support for the full MirrorOnce
        ///  texture-addressing mode is not necessary, but the data is symmetrical around
        ///  the one axis.
        /// </summary>
        MirrorOnce = 5,
    }

    /// <summary>
    ///  Defines how a texture will be filtered as it is minified for each mipmap
    ///   level.
    /// </summary>
    /// <remarks>可以直接转换的API：XNA</remarks>
    public enum TextureFilter
    {
        /// <summary>
        ///  Mipmapping disabled. The rasterizer uses the magnification filter instead.
        /// </summary>
        None = 0,
        /// <summary>
        ///   Point filtering used as a texture magnification or minification filter. The
        ///   texel with coordinates nearest to the desired pixel value is used. The texture
        ///   filter used between mipmap levels is based on the nearest point; that is,
        ///   the rasterizer uses the color from the texel of the nearest mipmap texture.
        /// </summary>
        Point = 1,
        /// <summary>
        ///  Bilinear interpolation filtering used as a texture magnification or minification
        ///  filter. A weighted average of a 2×2 area of texels surrounding the desired
        ///  pixel is used. The texture filter used between mipmap levels is trilinear
        ///  mipmap interpolation, in which the rasterizer performs linear interpolation
        ///  on pixel color, using the texels of the two nearest mipmap textures.
        /// </summary>
        Linear = 2,
        /// <summary>
        ///  Anisotropic texture filtering used as a texture magnification or minification
        ///  filter. This type of filtering compensates for distortion caused by the difference
        ///  in angle between the texture polygon and the plane of the screen.
        /// </summary>
        Anisotropic = 3,
        /// <summary>
        ///  A 4-sample tent filter used as a texture magnification or minification filter.
        /// </summary>
        PyramidalQuad = 6,
        /// <summary>
        ///  A 4-sample Gaussian filter used as a texture magnification or minification filter.
        /// </summary>
        GaussianQuad = 7,
    }
    public enum TextureType
    {
        Texture1D = 0,
        Texture2D,
        Texture3D,
        CubeTexture
    }
    
    /// <summary>
    ///  Defines supported wrap coordinates. Reference page contains links to related
    ///  conceptual articles.
    /// </summary>
    /// <remarks>可以直接转换的API：XNA</remarks>
    [Flags]
    public enum TextureWrapCoordinates
    {
        /// <summary>
        ///  No texture wrap coordinates specified.
        /// </summary>
        None = 0,
        /// <summary>
        ///  U texture wrapping (wrapping in the direction of the first dimension).
        /// </summary>
        Zero = 1,
        /// <summary>
        ///  V texture wrapping (wrapping in the direction of the second dimension).
        /// </summary>
        One = 2,
        /// <summary>
        ///  W texture wrapping (wrapping in the direction of the third dimension).
        /// </summary>
        Two = 4,
        /// <summary>
        ///  Texture wrapping in the direction of the fourth dimension.
        /// </summary>
        Three = 8,
    }

    public enum RenderPrimitiveType
    {
        /// <summary>
        ///		Render the vertices as individual points.
        /// </summary>
        PointList = 1,
        /// <summary>
        ///		Render the vertices as a series of individual lines.
        /// </summary>
        LineList,
        /// <summary>
        ///		Render the vertices as a continuous line.
        /// </summary>
        LineStrip,
        /// <summary>
        ///		Render the vertices as a series of individual triangles.
        /// </summary>
        TriangleList,
        /// <summary>
        ///		Render the vertices as a continous set of triangles in a zigzag type fashion.
        /// </summary>
        TriangleStrip,
        /// <summary>
        ///		Render the vertices as a set of trinagles in a fan like formation.
        /// </summary>
        TriangleFan
    }

    /// <summary>
    /// 
    /// </summary>
    /// <remarks>可以和xna的直接转换，源自xnafx</remarks>
    public enum VertexElementFormat : byte
    {
        Byte4 = 5,
        Color = 4,
        HalfVector2 = 15,
        HalfVector4 = 16,
        Normalized101010 = 14,
        NormalizedShort2 = 9,
        NormalizedShort4 = 10,
        Rg32 = 11,
        Rgba32 = 8,
        Rgba64 = 12,
        Short2 = 6,
        Short4 = 7,
        Single = 0,
        UInt101010 = 13,
        Unused = 17,
        Vector2 = 1,
        Vector3 = 2,
        Vector4 = 3
    }

    /// <summary>
    /// 
    /// </summary>
    /// <remarks>可以和xna的直接转换，源自xnafx</remarks>
    public enum VertexElementUsage : byte
    {
        Binormal = 7,
        BlendIndices = 2,
        BlendWeight = 1,
        Color = 10,
        Depth = 12,
        Fog = 11,
        Normal = 3,
        PointSize = 4,
        Position = 0,
        PositionTransformed = 9,
        Sample = 13,
        Tangent = 6,
        TessellateFactor = 8,
        TextureCoordinate = 5
    }

    /// <summary>
    ///		Specifies how a texture is to be used in the engine.
    /// </summary>
    [Flags]
    public enum TextureUsage
    {
        Static = BufferUsage.Static,
        Dynamic = BufferUsage.Dynamic,
        WriteOnly = BufferUsage.WriteOnly,
        StaticWriteOnly = BufferUsage.Static | BufferUsage.WriteOnly,
        DynamicWriteOnly = BufferUsage.Dynamic | BufferUsage.WriteOnly,
        Discardable = BufferUsage.Discardable,
        /// <summary>
        ///    Mipmaps will be automatically generated for this texture
        ///	 </summary>
        AutoMipMap = 0x100,
        /// <summary>
        ///    This texture will be a render target, ie. used as a target for render to texture
        ///    setting this flag will ignore all other texture usages except AutoMipMap
        ///	 </summary>
        RenderTarget = 0x200,
        /// <summary>
        ///    Default to automatic mipmap generation static textures
        ///	</summary>
        Default = AutoMipMap | StaticWriteOnly
    }

    public enum TransformState
    {
        View = 2,
        Projection = 3,

        Texture0 = 16,
        Texture1 = 17,
        Texture2 = 18,
        Texture3 = 19,
        Texture4 = 20,
        Texture5 = 21,
        Texture6 = 22,
        Texture7 = 23,

        World = 256,
        World1 = 257,
        World2 = 258,
        World3 = 259
    }


    public enum PresentInterval
    {
        /// <summary>
        ///  The device will present immediately without waiting for the refresh.
        /// </summary>
        Immediate = -2147483648,
        /// <summary>
        ///  The device will wait for the vertical retrace period.
        /// </summary>
        Default = 0,        
        /// <summary>
        ///  The device will wait for the vertical retrace period.
        /// </summary>
        One = 1,
        /// <summary>
        ///  Present operations will not be affected more than twice every screen refresh.
        /// </summary>
        Two = 2,
        /// <summary>
        ///  Present operations will not be affected more than three times every screen refresh
        /// </summary>
        Three = 4,
        /// <summary>
        ///  Present operations will not be affected more than four times every screen refresh.
        /// </summary>
        Four = 8,
    }
}
