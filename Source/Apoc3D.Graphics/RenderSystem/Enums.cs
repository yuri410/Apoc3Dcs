using System;
using System.Collections.Generic;
using System.Text;

namespace Apoc3D.Graphics
{
    public enum StateBlockType
    {
        All = 0,
        VertexState = 1,
        PixelState = 2
    }
    public enum LockMode
    {
        None,
        Discard,
        ReadOnly,
        NoOverwrite
    }

    public enum CompareFunction
    {
        Always = 8,
        Equal = 3,
        Greater = 5,
        GreaterEqual = 7,
        Less = 2,
        LessEqual = 4,
        Never = 1,
        NotEqual = 6
    }

    [Flags()]
    public enum ColorWriteChannels
    {
        All = 15,
        Alpha = 8,
        Blue = 4,
        Green = 2,
        None = 0,
        Red = 1
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

    [Flags]
    public enum ClearFlags
    {
        DepthBuffer = 2,
        Stencil = 4,
        Target = 1
    }
    public enum BlendFunction
    {
        Add = 1,
        Max = 5,
        Min = 4,
        ReverseSubtract = 3,
        Subtract = 2
    }

    public enum Blend
    {
        BlendFactor = 14,
        BothInverseSourceAlpha = 13,
        BothSourceAlpha = 12,
        DestinationAlpha = 7,
        DestinationColor = 9,
        InverseBlendFactor = 15,
        InverseDestinationAlpha = 8,
        InverseDestinationColor = 10,
        InverseSourceAlpha = 6,
        InverseSourceColor = 4,
        One = 2,
        SourceAlpha = 5,
        SourceAlphaSaturation = 11,
        SourceColor = 3,
        Zero = 1
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

    public enum StencilOperation
    {
        Decrement = 8,
        DecrementSaturation = 5,
        Increment = 7,
        IncrementSaturation = 4,
        Invert = 6,
        Keep = 1,
        Replace = 3,
        Zero = 2
    }

    public enum SwapEffect
    {
        Default,
        Discard,
        Flip,
        Copy
    }



    /// <summary>
    /// Texture stage states define multi-blender texture operations. 
    /// Some sampler states set up vertex processing, and some set up pixel processing. 
    /// Texture stage states can be saved and restored using stateblocks. 
    /// </summary>
    public enum TextureStage
    {
        /// <summary>
        /// Settings for the third alpha operand for triadic operations. 
        /// Use values from TextureArgument to set this state. 
        /// The default value is TextureArgument.Current. 
        /// </summary>
        AlphaArg0 = 27,
        /// <summary>
        /// Texture-stage state is the first alpha argument for the stage. 
        /// Use values from TextureArgument to set this state. 
        /// The default value is TextureArgument.Texture. 
        /// </summary>
        AlphaArg1 = 5,
        /// <summary>
        /// Texture-stage state is the second alpha argument for the stage. Use values from TextureArgument to set this state. The default value is TextureArgument.Current.
        /// </summary>
        AlphaArg2 = 6,
        /// <summary>
        /// 
        /// </summary>
        AlphaOperation = 4,
        /// <summary>
        /// 
        /// </summary>
        BumpEnvironmentLOffset = 23,

        /// <summary>
        /// 
        /// </summary>
        BumpEnvironmentLScale = 22,
        /// <summary>
        /// 
        /// </summary>
        BumpEnvironmentMat00 = 7,
        /// <summary>
        /// 
        /// </summary>
        BumpEnvironmentMat01 = 8,
        /// <summary>
        /// 
        /// </summary>
        BumpEnvironmentMat10 = 9,
        /// <summary>
        /// 
        /// </summary>
        BumpEnvironmentMat11 = 10,
        /// <summary>
        /// 
        /// </summary>
        ColorArg0 = 26,
        /// <summary>
        /// 
        /// </summary>
        ColorArg1 = 2,
        /// <summary>
        /// 
        /// </summary>
        ColorArg2 = 3,
        /// <summary>
        /// 
        /// </summary>
        ColorOperation = 1,
        /// <summary>
        /// 
        /// </summary>
        Constant = 32,
        /// <summary>
        /// 
        /// </summary>
        ResultArg = 28,
        /// <summary>
        /// 
        /// </summary>
        TexCoordIndex = 11,
        /// <summary>
        /// 
        /// </summary>
        TextureTransformFlags = 24
    }

    public enum TextureAddressMode
    {
        Border = 4,
        Clamp = 3,
        Mirror = 2,
        MirrorOnce = 5,
        Wrap = 1
    }

    public enum TextureFilter
    {
        Anisotropic = 3,
        GaussianQuad = 7,
        Linear = 2,
        None = 0,
        Point = 1,
        PyramidalQuad = 6
    }
    public enum TextureType
    {
        Texture1D = 0,
        Texture2D,
        Texture3D,
        CubeTexture
    }
    
    /// <summary>
    /// Defines the possible shading modes.
    /// </summary>
    public enum ShadeMode
    {
        /// <summary>
        /// Flat shading.
        /// </summary>
        Flat = 1,
        /// <summary>
        ///   Gouraud shading.
        /// </summary>
        Gouraud = 2,
    }

    #region
    //[Flags]
    //public enum TextureArgument
    //{
    //    AlphaReplicate = 32,
    //    Complement = 16,
    //    Constant = 6,
    //    Current = 1,
    //    Diffuse = 0,
    //    SelectMask = 15,
    //    Specular = 4,
    //    Temp = 5,
    //    Texture = 2,
    //    TFactor = 3
    //}
    //public enum TextureOperation
    //{
    //    Add = 7,
    //    AddSigned = 8,
    //    AddSigned2X = 9,
    //    AddSmooth = 11,
    //    BlendCurrentAlpha = 16,
    //    BlendDiffuseAlpha = 12,
    //    BlendFactorAlpha = 14,
    //    BlendTextureAlpha = 13,
    //    BlendTextureAlphaPM = 15,
    //    BumpEnvironmentMap = 22,
    //    BumpEnvironmentMapLuminance = 23,
    //    Disable = 1,
    //    DotProduct3 = 24,
    //    Lerp = 26,
    //    Modulate = 4,
    //    Modulate2X = 5,
    //    Modulate4X = 6,
    //    ModulateAlphaAddColor = 18,
    //    ModulateColorAddAlpha = 19,
    //    ModulateInvAlphaAddColor = 20,
    //    ModulateInvColorAddAlpha = 21,
    //    MultiplyAdd = 25,
    //    Premodulate = 17,
    //    SelectArg1 = 2,
    //    SelectArg2 = 3,
    //    Subtract = 10
    //}
    //public enum TextureTransform
    //{
    //    Count1 = 1,
    //    Count2 = 2,
    //    Count3 = 3,
    //    Count4 = 4,
    //    Disable = 0,
    //    Projected = 256
    //}
    #endregion

    [Flags]
    public enum TextureWrapCoordinates
    {
        None = 0,
        One = 2,
        Three = 8,
        Two = 4,
        Zero = 1
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
