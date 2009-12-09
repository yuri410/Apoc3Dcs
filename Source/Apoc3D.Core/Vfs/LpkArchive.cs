using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Apoc3D.Vfs
{
    public struct LpkArchiveEntry 
    {
        public string Name;
        public int CompressedSize;
        public int Offset;
        public int Size;
        public int Flag;
    }

    public class LpkArchive : Archive
    {
        static byte[] buffer = new byte[1048576 * 4];

        public const int FileId = 'L' << 16 | 'P' << 8 | 'K';

        SevenZip.Compression.LZMA.Decoder decoder = new SevenZip.Compression.LZMA.Decoder();

        Dictionary<string, LpkArchiveEntry> entries =
            new Dictionary<string, LpkArchiveEntry>(CaseInsensitiveStringComparer.Instance);

        Stream stream;

        int fileCount;

        public LpkArchive(FileLocation fl)
            : base(fl.Path, fl.Size, fl.IsInArchive)
        {
            stream = fl.GetStream;
            stream.Position = 0;
            ContentBinaryReader br = new ContentBinaryReader(stream);

            try
            {
                if (br.ReadInt32() == FileId)
                {
                    fileCount = br.ReadInt32();

                    for (int i = 0; i < fileCount; i++)
                    {
                        LpkArchiveEntry ent;

                        ent.Name = br.ReadStringUnicode();
                        ent.CompressedSize = br.ReadInt32();
                        ent.Offset = br.ReadInt32();
                        ent.Size = br.ReadInt32();
                        ent.Flag = br.ReadInt32();

                        entries.Add(ent.Name, ent);
                    }

                    int propLen = br.ReadInt32();
                    byte[] props = br.ReadBytes(propLen);                    
                    decoder.SetDecoderProperties(props);
                }
                else
                {
                    throw new DataFormatException();
                }
            }
            catch
            {
                throw;
            }
            finally
            {
                br.Close();
            }
        }

        public LpkArchiveEntry[] GetEntries()
        {
            Dictionary<string, LpkArchiveEntry>.ValueCollection val = entries.Values;
            LpkArchiveEntry[] result = new LpkArchiveEntry[val.Count];
            int index = 0;
            foreach (LpkArchiveEntry e in val)
            {
                result[index++] = e;
            }
            return result;
        }

        public override int FileCount
        {
            get { return fileCount; }
        }

        public unsafe override Stream GetEntryStream(string file)
        {
            LpkArchiveEntry lpkEnt;

            if (entries.TryGetValue(file, out lpkEnt))
            {
                if (lpkEnt.Size > buffer.Length)
                {
                    System.IO.MemoryStream ms = new System.IO.MemoryStream(lpkEnt.Size + 1);
                    decoder.Code(stream, ms, lpkEnt.CompressedSize, lpkEnt.Size, null);
                    return ms;
                }
                else
                {
                    fixed (byte* ptr = &buffer[0])
                    {
                        Apoc3D.Vfs.MemoryStream ms = new MemoryStream(ptr, lpkEnt.Size);
                        decoder.Code(stream, ms, lpkEnt.CompressedSize, lpkEnt.Size, null);
                        return ms;
                    }
                }
            }
            return null;
        }

        public override void Dispose(bool disposing)
        {
            if (disposing)
            {
                stream.Close();
                stream = null;
            }
        }
    }

    class LpkArchiveFactory : ArchiveFactory
    {
        public override Archive CreateInstance(string file)
        {
            return CreateInstance(new FileLocation(file));
        }

        public override Archive CreateInstance(FileLocation fl)
        {
            return new LpkArchive(fl);
        }

        public override string Type
        {
            get { return ".lpk"; }
        }
    }
}
