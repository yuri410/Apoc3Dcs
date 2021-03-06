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
using System.Text;
using System.IO;
using System.Runtime.InteropServices;
using Apoc3D.Vfs;

namespace Apoc3D
{
    public class StringTable : Dictionary<string, KeyValuePair<string, string>>
    {

        protected StringTable(IEqualityComparer<string> comparer)
            : base(comparer)
        { }

        public StringTable()
            : base(CaseInsensitiveStringComparer.Instance)
        { }


        public new virtual string this[string a]
        {
            get
            {
                KeyValuePair<string, string> value;
                return base.TryGetValue(a, out value) ? value.Value : "[MISSING]" + a;
            }
        }

        public void AddEntries(Dictionary<string, KeyValuePair<string, string>> ent)
        {
            foreach (KeyValuePair<string, KeyValuePair<string, string>> e in ent)
            {
                Add(e.Key, e.Value);
            }
        }
    }

    public abstract class StringTableFormat
    {
        public StringTable Load(ResourceLocation rl)
        {
            StringTable st = new StringTable();
            Read(st, rl.GetStream);
            return st;
        }
        public StringTable Load(string file)
        {
            return Load(new FileLocation(file));
        }

        public abstract void Read(Dictionary<string, KeyValuePair<string, string>> data, Stream stm);
        public abstract void Write(Dictionary<string, KeyValuePair<string, string>> data, Stream stm);

        public abstract string[] Filers { get; }
    }
    public class StringTableCsfFormat : StringTableFormat
    {
        public const int CsfID = ((byte)'C' << 24) | ((byte)'S' << 16) | ((byte)'F' << 8) | (byte)' ';

        [StructLayout(LayoutKind.Sequential)]
        protected struct CsfHeader
        {
            public const int LabelID = ((byte)'L' << 24) | ((byte)'B' << 16) | ((byte)'L' << 8) | (byte)' ';
            public const int StringID = ((byte)'S' << 24) | ((byte)'T' << 16 | (byte)'R' << 8 | (byte)' ');
            public const int WStringID = ((byte)'S' << 24) | ((byte)'T' << 16 | (byte)'R' << 8 | (byte)'W');

            public int id;
            public int flag1;
            public int count1;
            public int count2;
            public int zero;
            public int flag2;
        }

        public override string[] Filers
        {
            get { return new string[] { ".csf" }; }
        }

        public override void Read(Dictionary<string, KeyValuePair<string, string>> data, Stream stm)
        {
            ContentBinaryReader br = new ContentBinaryReader(stm, Encoding.ASCII);

            CsfHeader header;

            header.id = br.ReadInt32();
            if (header.id == CsfID)
            {
                header.flag1 = br.ReadInt32();
                header.count1 = br.ReadInt32();
                header.count2 = br.ReadInt32();
                header.zero = br.ReadInt32();
                header.flag2 = br.ReadInt32();

                for (int i = 0; i < header.count1; i++)
                {
                    int id = br.ReadInt32();

                    if (id == CsfHeader.LabelID)
                    {
                        int flag = br.ReadInt32();

                        int len = br.ReadInt32();
                        char[] chs = br.ReadChars(len);
                        string key = new string(chs);

                        if ((flag & 1) != 0)
                        {
                            id = br.ReadInt32(); // label内容类型

                            // 读字符串（内容）
                            len = br.ReadInt32(); //长度

                            string val = string.Empty;
                            if (len > 0)
                            {
                                char[] chars = new char[len];
                                for (int j = 0; j < len; j++)
                                    chars[j] = (char)(ushort)((byte)~br.ReadByte() | ((byte)~br.ReadByte() << 8));

                                val = new string(chars);
                            }
                            string ext = null;
                            // 读附加数据
                            if (id == CsfHeader.WStringID)
                            {
                                len = br.ReadInt32();
                                ext = new string(br.ReadChars(len));
                            }

                            data.Add(key, new KeyValuePair<string, string>(ext, val));
                        }
                        else
                            data.Add(key, new KeyValuePair<string, string>(string.Empty, string.Empty));
                    }
                }
            }
            else
                throw new DataFormatException(stm.ToString());

            br.Close();
        }
        public override void Write(Dictionary<string, KeyValuePair<string, string>> data, Stream stm)
        {
            BinaryWriter bw = new BinaryWriter(stm);

            bw.Write(CsfID);
            bw.Write((int)3);
            bw.Write(data.Count);
            bw.Write(data.Count);
            bw.Write((int)0);
            bw.Write((int)0);

            foreach (KeyValuePair<string, KeyValuePair<string, string>> entry in data)
            {
                string key = entry.Key;
                string text = entry.Value.Value;
                string ext = entry.Value.Key;

                bw.Write((int)CsfHeader.LabelID);

                if (text != string.Empty | ext != string.Empty)
                {
                    bw.Write((int)1);

                    bw.Write(key.Length);
                    bw.Write(key.ToCharArray());

                    bool hasExtra = !string.IsNullOrEmpty(ext);

                    if (hasExtra)
                        bw.Write((int)CsfHeader.WStringID);
                    else
                        bw.Write((int)CsfHeader.StringID);

                    bw.Write(text.Length);

                    for (int j = 0; j < text.Length; j++)
                    {
                        ushort v = (ushort)text[j];

                        bw.Write((byte)~(v & 255));
                        bw.Write((byte)~(v >> 8));
                    }

                    if (hasExtra)
                    {
                        bw.Write(ext.Length);
                        bw.Write(ext.ToCharArray());
                    }
                }
                else
                {
                    bw.Write((int)0);
                    bw.Write(key.Length);
                    bw.Write(key.ToCharArray());
                }
            }

            bw.Close();
        }
    }
}
