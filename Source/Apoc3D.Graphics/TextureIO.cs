using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Apoc3D.MathLib;
using Apoc3D.Media;
using Apoc3D.Vfs;

namespace Apoc3D.Graphics
{
    public unsafe struct TextureLevelData
    {
        static readonly string WidthTag = "Width";
        static readonly string HeightTag = "Height";
        static readonly string DepthTag = "Depth";
        static readonly string ContentTag = "Content";
        static readonly string LevelSizeTag = "LevelSize";

        /// <summary>
        ///  纹理层的宽度
        /// </summary>
        public int Width;
        /// <summary>
        ///  纹理层的高度
        /// </summary>
        public int Height;
        /// <summary>
        ///  纹理层的深度
        /// </summary>
        public int Depth;

        /// <summary>
        ///  纹理层的大小
        /// </summary>
        public int LevelSize;

        /// <summary>
        ///  字节数据
        /// </summary>
        public byte[] Content;

        public void LoadData(BinaryDataReader data)
        {
            this.Width = data.GetDataInt32(WidthTag);
            this.Height = data.GetDataInt32(HeightTag);
            this.Depth = data.GetDataInt32(DepthTag);
            this.LevelSize = data.GetDataInt32(LevelSizeTag);

            ContentBinaryReader br1 = data.GetData(ContentTag);
            
            this.Content = br1.ReadBytes(LevelSize);
            
            br1.Close();
        }

        public void SaveData(BinaryDataWriter data)
        {
            data.AddEntry(WidthTag, Width);
            data.AddEntry(HeightTag, Height);
            data.AddEntry(DepthTag, Depth);
            data.AddEntry(LevelSizeTag, LevelSize);

            ContentBinaryWriter bw = data.AddEntry(ContentTag);
            bw.Write(Content);
            bw.Close();
        }
    }

    /// <summary>
    ///  表示纹理的数据
    /// </summary>
    public unsafe struct TextureData
    {
        /// <summary>
        ///  表示纹理的类型
        /// </summary>
        public TextureType Type;

        public TextureLevelData[] Levels;

        /// <summary>
        ///  表示纹理的像素格式
        /// </summary>
        public ImagePixelFormat Format;

        /// <summary>
        ///  表示纹理的大小，包括所有的层
        /// </summary>
        public int ContentSize;

        /// <summary>
        ///  表示纹理的层数
        /// </summary>
        public int LevelCount;

        #region 常量


        //public const int MaxBufferSize = 4 * 1048576;

        public const int ID = 'A' << 24 | 'T' << 16 | 'E' << 8 | 'X';

        static readonly string TypeTag = "Type";
        static readonly string FormatTag = "Format";
        static readonly string ContentSizeTag = "ContentSize";
        static readonly string LevelCountTag = "LevelCount";
        static readonly string LevelTag = "Level";
        //static readonly byte[] internalLoadBuffer = new byte[MaxBufferSize];

        #endregion

        public void Load(ResourceLocation rl)
        {
            ContentBinaryReader br = new ContentBinaryReader(rl);

            if (br.ReadInt32() == ID)
            {
                BinaryDataReader data = br.ReadBinaryData();

                this.Type = (TextureType)data.GetDataInt32(TypeTag);
              
                this.Format = (ImagePixelFormat)data.GetDataInt32(FormatTag);
                this.ContentSize = data.GetDataInt32(ContentSizeTag);
                this.LevelCount = data.GetDataInt32(LevelCountTag);

                Levels = new TextureLevelData[LevelCount];
                for (int i = 0; i < LevelCount; i++)
                {
                    ContentBinaryReader br2 = data.GetData(LevelTag + i.ToString());
                    BinaryDataReader data2 = br2.ReadBinaryData();
                    Levels[i].LoadData(data2);

                    data2.Close();
                    br2.Close();
                }

                data.Close();
            }

            br.Close();
        }

        public void Save(Stream stream)
        {
            ContentBinaryWriter bw = new ContentBinaryWriter(stream);

            bw.Write(ID);

            BinaryDataWriter data = new BinaryDataWriter();

            data.AddEntry(TypeTag, (int)Type);
            data.AddEntry(FormatTag, (int)Format);
            data.AddEntry(ContentSizeTag, ContentSize);
            data.AddEntry(LevelCountTag, LevelCount);

            for (int i = 0; i < LevelCount; i++)
            {
                ContentBinaryWriter bw2 = data.AddEntry(LevelTag + i.ToString());
                BinaryDataWriter data2 = new BinaryDataWriter();
                Levels[i].SaveData(data2);

                bw2.Write(data2);

                data2.Dispose();
                bw2.Close();
            }

            bw.Write(data);
            data.Dispose();

            bw.Close();
        }
    }

    public unsafe class TDMPIO
    {
        public float Xllcorner;
        public float Yllcorner;
        public float XSpan;
        public float YSpan;
        public int Width;
        public int Height;
        public float[] Data;

        public int Bits;

        public static readonly string Extension = ".tdmp";
        public static readonly string XllCornerTag = "xllcorner";
        public static readonly string YllCornerTag = "yllcorner";
        public static readonly string XSpanTag = "xspan";
        public static readonly string YSpanTag = "yspan";

        public static readonly string WidthTag = "width";
        public static readonly string HeightTag = "height";
        public static readonly string BitsTag = "bits";
        public static readonly string DataTag = "data";


        public virtual void Load(ResourceLocation rl) 
        {
            ContentBinaryReader br = new ContentBinaryReader(rl);

            BinaryDataReader data = br.ReadBinaryData();
            Xllcorner = data.GetDataSingle(TDMPIO.XllCornerTag);
            Yllcorner = data.GetDataSingle(TDMPIO.YllCornerTag);

            Width = data.GetDataInt32(TDMPIO.WidthTag);
            Height = data.GetDataInt32(TDMPIO.HeightTag);

            XSpan = data.GetDataSingle(TDMPIO.XSpanTag, 5f);
            YSpan = data.GetDataSingle(TDMPIO.YSpanTag, 5f);

            Bits = data.GetDataInt32(TDMPIO.BitsTag, 32);
            
            Data = new float[Height * Width];
            ContentBinaryReader br2 = data.GetData(TDMPIO.DataTag);

            switch (Bits) 
            {   
                case 8:
                    for (int i = 0; i < Height; i++)
                    {
                        for (int j = 0; j < Width; j++)
                        {
                            Data[i * Width + j] = br.ReadByte() / (float)byte.MaxValue;
                        }
                    }
                    break;
                case 12:
                    int len = Height * Width;
                    for (int i = 0; i < len; i += 2)
                    {
                        byte a = br.ReadByte();
                        byte b = br.ReadByte();
                        byte c = br.ReadByte();

                        ushort v1 = (ushort)(a | ((0xF & (b >> 4)) << 8));
                        ushort v2 = (ushort)(c | ((0xF & b) << 8));

                        Data[i] = v1 / (float)(2 << 12);
                        if (i + 1 < len)
                            Data[i + 1] = v1 / (float)(2 << 12);
                    }
                    break;
                case 16:
                    for (int i = 0; i < Height; i++)
                    {
                        for (int j = 0; j < Width; j++)
                        {
                            Data[i * Width + j] = Half.Convert(br.ReadUInt16());
                        }
                    }
                    break;
                case 32:
                    for (int i = 0; i < Height; i++)
                    {
                        for (int j = 0; j < Width; j++)
                        {
                            Data[i * Width + j] = br.ReadSingle();
                        }
                    }
                    break;
            }
            

            br2.Close();
            data.Close();
            br.Close();
        }

        public virtual void Save(Stream stream) 
        {
            BinaryDataWriter result = new BinaryDataWriter();

            result.AddEntry(TDMPIO.XllCornerTag, Xllcorner);
            result.AddEntry(TDMPIO.YllCornerTag, Yllcorner);
            result.AddEntry(TDMPIO.WidthTag, Width);
            result.AddEntry(TDMPIO.HeightTag, Height);
            result.AddEntry(TDMPIO.XSpanTag, XSpan);
            result.AddEntry(TDMPIO.YSpanTag, YSpan);

            result.AddEntry(TDMPIO.BitsTag, Bits);


            ContentBinaryWriter bw = result.AddEntry(TDMPIO.DataTag);

            switch (Bits)
            {
                case 8:
                    for (int i = 0; i < Data.Length; i++)
                    {
                        int val = (int)Data[i] * byte.MaxValue;
                        if (val > byte.MaxValue)
                            val = byte.MaxValue;
                        if (val < 0)
                            val = 0;
                        bw.Write((byte)(val));
                    }
                    break;
                case 12:
                    int len = Data.Length;
                    for (int i = 0; i < len; i += 2)
                    {
                        ushort v1 = (ushort)(Data[i] * (2 << 12));
                        ushort v2;

                        if (i + 1 < len)
                            v2 = (ushort)(Data[i + 1] * (2 << 12));
                        else
                            v2 = 0;

                        byte a = (byte)(v1 & 0xff);
                        byte b = (byte)(((v1 >> 8) & 0xF) << 4 | ((v2 >> 8) & 0xF));
                        byte c = (byte)(v2 & 0xff);

                        bw.Write(a);
                        bw.Write(b);
                        bw.Write(c);
                    }
                    break;
                case 16:
                    for (int i = 0; i < Data.Length; i++)
                    {
                        bw.Write(Half.Convert(Data[i]));
                    }
                    break;
                case 32:
                    for (int i = 0; i < Data.Length; i++)
                    {
                        bw.Write(Data[i]);
                    }
                    break;
            }

            bw.Close();

            bw = new ContentBinaryWriter(stream);
            bw.Write(result);
            bw.Close();

            result.Dispose();
        }        
    }
}