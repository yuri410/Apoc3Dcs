using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Apoc3D.MathLib;
using Apoc3D.Media;
using Apoc3D.Vfs;

namespace Apoc3D.Graphics
{
    /// <summary>
    ///  ��ʾ���������
    /// </summary>
    public unsafe struct TextureData
    {
        /// <summary>
        ///  ��ʾ���������
        /// </summary>
        public TextureType Type;

        /// <summary>
        ///  ��ʾ����Ŀ��
        /// </summary>
        public int Width;

        /// <summary>
        ///  ����ĸ߶�
        /// </summary>
        public int Height;

        /// <summary>
        ///  ��������
        /// </summary>
        public int Depth;

        /// <summary>
        ///  ��ʾ��������ظ�ʽ
        /// </summary>
        public ImagePixelFormat Format;

        /// <summary>
        ///  ��ʾ����Ĵ�С���������еĲ�
        /// </summary>
        public int ContentSize;

        /// <summary>
        ///  ��ʾ����Ĳ���
        /// </summary>
        public int LevelCount;

        /// <summary>
        ///  ��ʾ����ÿһ��Ĵ�С
        /// </summary>
        public fixed int LevelSize[Material.MaxTexLayers];

        /// <summary>
        ///  ������������ݣ��ӵ�һ�㿪ʼһ��һ������
        /// </summary>
        public byte[] Content;

        #region ����


        public const int MaxBufferSize = 4 * 1048576;

        public const int ID = 'A' << 24 | 'T' << 16 | 'E' << 8 | 'X';

        static readonly string TypeTag = "Type";
        static readonly string WidthTag = "Width";
        static readonly string HeightTag = "Height";
        static readonly string DepthTag = "Depth";
        static readonly string FormatTag = "Format";
        static readonly string ContentTag = "Content";
        static readonly string ContentSizeTag = "ContentSize";
        static readonly string LevelCountTag = "LevelCount";
        static readonly string LevelSizeTag = "LevelSize";

        static readonly byte[] internalLoadBuffer = new byte[MaxBufferSize];

        #endregion

        public void Load(ResourceLocation rl)
        {
            ContentBinaryReader br = new ContentBinaryReader(rl);

            if (br.ReadInt32() == ID)
            {
                BinaryDataReader data = br.ReadBinaryData();

                this.Type = (TextureType)data.GetDataInt32(TypeTag);
                this.Width = data.GetDataInt32(WidthTag);
                this.Height = data.GetDataInt32(HeightTag);
                this.Depth = data.GetDataInt32(DepthTag);
                this.Format = (ImagePixelFormat)data.GetDataInt32(FormatTag);
                this.ContentSize = data.GetDataInt32(ContentSizeTag);
                this.LevelCount = data.GetDataInt32(LevelCountTag);

                ContentBinaryReader br1 = data.GetData(LevelSizeTag);
                fixed (int* dst = LevelSize)
                {
                    for (int i = 0; i < Material.MaxTexLayers; i++)
                    {
                        dst[i] = br.ReadInt32();
                    }
                }
                br1.Close();

                br1 = data.GetData(ContentTag);
                if (ContentSize > MaxBufferSize)
                {
                    this.Content = br1.ReadBytes(ContentSize);
                }
                else
                {
                    this.Content = internalLoadBuffer;

                    br1.Read(Content, 0, ContentSize);
                }
                br1.Close();

                data.Close();
            }

            br.Close();
        }

        public void Save(Stream stream)
        {
            throw new NotImplementedException();
        }
    }

    public unsafe struct TDMP16IO
    {
        static readonly string XllCornerTag = "xllcorner";
        static readonly string YllCornerTag = "yllcorner";
        static readonly string XSpanTag = "xspan";
        static readonly string YSpanTag = "yspan";

        static readonly string WidthTag = "width";
        static readonly string HeightTag = "height";
        static readonly string BitsTag = "bits";
        static readonly string DataTag = "data";

        public float Xllcorner;
        public float Yllcorner;
        public float XSpan;
        public float YSpan;
        public int Width;
        public int Height;

        public Half[] Data;

        public int Bits { get { return 16; } }

        public void Load(ResourceLocation rl)
        {
            ContentBinaryReader br = new ContentBinaryReader(rl);

            BinaryDataReader data = br.ReadBinaryData();
            Xllcorner = data.GetDataSingle(XllCornerTag);
            Yllcorner = data.GetDataSingle(YllCornerTag);

            Width = data.GetDataInt32(WidthTag);
            Height = data.GetDataInt32(HeightTag);

            XSpan = data.GetDataSingle(XSpanTag, 5f);
            YSpan = data.GetDataSingle(YSpanTag, 5f);

            int bits = data.GetDataInt32(BitsTag);
            if (bits != Bits)
                throw new DataFormatException();

            Data = new Half[Height * Width];
            ContentBinaryReader br2 = data.GetData(DataTag);

            for (int i = 0; i < Height; i++)
            {
                for (int j = 0; j < Width; j++)
                {
                    Data[i * Width + j] = new Half(br.ReadUInt16());
                }
            }

            br2.Close();

            br.Close();
        }

        public void Save(Stream stream)
        {
            BinaryDataWriter result = new BinaryDataWriter();

            result.AddEntry(XllCornerTag, Xllcorner);
            result.AddEntry(YllCornerTag, Yllcorner);
            result.AddEntry(WidthTag, Width);
            result.AddEntry(HeightTag, Height);
            result.AddEntry(XSpanTag, XSpan);
            result.AddEntry(YSpanTag, YSpan);

            result.AddEntry(BitsTag, 16);

            Stream dataStream = result.AddEntryStream(DataTag);

            ContentBinaryWriter bw = new ContentBinaryWriter(dataStream);
            for (int i = 0; i < Data.Length; i++)
            {
                bw.Write(Data[i].InternalValue);
            }

            bw.Close();

            bw = new ContentBinaryWriter(stream);
            bw.Write(result);
            bw.Close();
        }

    }
}