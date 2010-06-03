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
