using System;
using System.Collections.Generic;
using System.Text;
using SevenZip;
using System.IO;

namespace LZMATest
{
    class Program
    {
        const UInt32 kAdditionalSize = (6 << 20);
        const UInt32 kCompressedAdditionalSize = (1 << 10);
        const UInt32 kMaxLzmaPropSize = 10;

        static void Main(string[] args)
        {
            SevenZip.Compression.LZMA.Encoder encoder = new SevenZip.Compression.LZMA.Encoder();
            SevenZip.Compression.LZMA.Decoder decoder = new SevenZip.Compression.LZMA.Decoder();

            UInt32 dictionarySize = 4048576;

            CoderPropID[] propIDs = 
			{ 
				CoderPropID.DictionarySize,
                CoderPropID.NumThreads,
			};
            object[] properties = 
			{
				1048576,
                2,
			};

            UInt32 kBufferSize = dictionarySize + kAdditionalSize;
            UInt32 kCompressedBufferSize = (kBufferSize / 2) + kCompressedAdditionalSize;

            encoder.SetCoderProperties(propIDs, properties);
            //System.IO.MemoryStream propStream = new System.IO.MemoryStream();
            //encoder.WriteCoderProperties(propStream);

            //FileStream compressedStream = new FileStream(@"E:\Desktop\sss\lzma.bin", FileMode.OpenOrCreate, FileAccess.Write);

            //FileStream inStream = new FileStream(@"E:\Desktop\srtm513_16xx.rar", FileMode.Open, FileAccess.Read);
            //encoder.Code(inStream, compressedStream, -1, -1, null);
            //inStream.Close();
            //compressedStream.Close();

            string[] dems = Directory.GetFiles(@"E:\Desktop\srtm513_16", "*.tdmp");

            for (int i = 0; i < dems.Length; i++)
            {
                string fileName = Path.GetFileNameWithoutExtension(dems[i]);
                FileStream compressedStream = new FileStream(@"E:\Desktop\sss\" + fileName + ".bin", FileMode.OpenOrCreate, FileAccess.Write);

                FileStream inStream = new FileStream(dems[i], FileMode.Open, FileAccess.Read);
                encoder.Code(inStream, compressedStream, -1, -1, null);

                Console.Write(i.ToString());
                Console.Write(" : ");
                Console.WriteLine(dems[i]);
                inStream.Close();
                compressedStream.Close();
            }
        }
    }
}
