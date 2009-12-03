using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Apoc3D.Media;
using Apoc3D.Vfs;

namespace Apoc3D.Graphics
{
    public unsafe struct TextureData
    {
        public TextureType Type;

        public int Width;

        public int Height;

        public int Depth;

        public ImagePixelFormat Format;

        public int ContentSize;

        public int LevelCount;

        public fixed int LevelSize[Material.MaxTexLayers];

        public byte[] Content;

        #region ³£Á¿


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
                for (int i = 0; i < Material.MaxTexLayers; i++)
                {
                    this.LevelSize[i] = br.ReadInt32();
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

}