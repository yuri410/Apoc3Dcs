using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Threading;

namespace Apoc3D.Vfs
{
    public struct PakArchiveEntry
    {
        public string Name;
        public int Offset;
        public int Size;
        public int Flag;
    }

    public class PakArchive : Archive
    {
        static Dictionary<int, Stream> threadStreams = new Dictionary<int, Stream>();

        public const int FileId = 'P' << 16 | 'A' << 8 | 'K';

        Dictionary<string, PakArchiveEntry> entries =
            new Dictionary<string, PakArchiveEntry>(CaseInsensitiveStringComparer.Instance);

        FileLocation file;

        int fileCount;

        public PakArchive(FileLocation fl)
            : base(fl.Path, fl.Size, fl.IsInArchive)
        {
            this.file = fl;
            Stream stream = fl.GetStream;
            stream.Position = 0;

            ContentBinaryReader br = new ContentBinaryReader(stream);

            try
            {
                if (br.ReadInt32() == FileId)
                {
                    fileCount = br.ReadInt32();

                    for (int i = 0; i < fileCount; i++)
                    {
                        PakArchiveEntry ent;

                        ent.Name = br.ReadStringUnicode();
                        ent.Offset = br.ReadInt32();
                        ent.Size = br.ReadInt32();
                        ent.Flag = br.ReadInt32();

                        entries.Add(ent.Name, ent);
                    }
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

        public PakArchiveEntry[] GetEntries()
        {
            Dictionary<string, PakArchiveEntry>.ValueCollection val = entries.Values;
            PakArchiveEntry[] result = new PakArchiveEntry[val.Count];
            int index = 0;
            foreach (PakArchiveEntry e in val)
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
            PakArchiveEntry lpkEnt;

            if (entries.TryGetValue(file, out lpkEnt))
            {
                int threadId = Thread.CurrentThread.ManagedThreadId;
                Stream thStream;

                if (!threadStreams.TryGetValue(threadId, out thStream))
                {
                    thStream = this.file.GetStream;
                }


                VirtualStream res = new VirtualStream(thStream, lpkEnt.Offset, lpkEnt.Size);
                res.Position = 0;
                return res;
                //if (lpkEnt.Size > buffer.Length)
                //{
                //    System.IO.MemoryStream ms = new System.IO.MemoryStream(lpkEnt.Size + 1);
                //    decoder.Code(stream, ms, lpkEnt.CompressedSize, lpkEnt.Size, null);
                //    ms.Position = 0;
                //    return ms;
                //}
                //else
                //{
                //    fixed (byte* ptr = &buffer[0])
                //    {
                //        Apoc3D.Vfs.MemoryStream ms = new MemoryStream(ptr, lpkEnt.Size);
                //        decoder.Code(stream, ms, lpkEnt.CompressedSize, lpkEnt.Size, null);
                //        ms.Position = 0;
                //        return ms;
                //    }
                //}
            }
            return null;
        }

        public override void Dispose(bool disposing)
        {
            if (disposing)
            {
                //stream.Close();
                //stream = null;
            }
        }
    }

    class PakArchiveFactory : ArchiveFactory
    {
        public override Archive CreateInstance(string file)
        {
            return CreateInstance(new FileLocation(file));
        }

        public override Archive CreateInstance(FileLocation fl)
        {
            return new PakArchive(fl);
        }

        public override string Type
        {
            get { return ".pak"; }
        }
    }
}
