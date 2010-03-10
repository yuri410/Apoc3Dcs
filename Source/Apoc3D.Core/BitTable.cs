using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Apoc3D.Vfs;

namespace Apoc3D
{
    public class BitTable
    {
        int[] data;
        int size;

        public BitTable(int size)
        {
            size = size / 32 + 1;

            this.size = size;
            data = new int[size];
        }

        public bool GetBit(int x)
        {
            int xx = x % 32;

            x = x / 32;

            int v = data[x];

            return ((v >> xx) & 1) != 0;
        }

        public void SetBit(int x, bool v)
        {
            int xx = x % 32;

            x = x / 32;

            //int vv = v ? 1 : 0;
            if (v)
            {
                data[x] |= (1 << xx);
            }
            else
            {
                data[x] &= ~(1 << xx);
            }
        }


        public void Load(FileLocation fl)
        {
            ContentBinaryReader br = new ContentBinaryReader(fl);
            size = br.ReadInt32();
            data = new int[size];
            for (int i = 0; i < size; i++)
            {
                data[i] = br.ReadInt32();
            }
            br.Close();
        }

        public void Save(Stream stm)
        {
            ContentBinaryWriter bw = new ContentBinaryWriter(stm);

            bw.Write(size);
            for (int i = 0; i < size; i++)
            {
                bw.Write(data[i]);
            }

            bw.Close();
        }
    }

}
