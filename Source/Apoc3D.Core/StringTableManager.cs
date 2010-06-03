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
using Apoc3D.Core;
using Apoc3D.Vfs;

namespace Apoc3D
{
    public class StringTableManager
    {
        static StringTable strTbl;
        static StringTableManager singleton;

        public static StringTableManager Instance
        {
            get
            {
                return singleton;
            }
        }
        public static void Initialize()
        {
            if (singleton == null)
            {
                singleton = new StringTableManager();
            }
            EngineConsole.Instance.Write("字符串对照表管理器初始化完毕。", ConsoleMessageType.Information);
        }

        public static StringTable StringTable
        {
            get { return strTbl; }
        }

        Dictionary<string, StringTableFormat> formats = new Dictionary<string, StringTableFormat>(CaseInsensitiveStringComparer.Instance);

        public void Register(StringTableFormat fmt)
        {
            string[] exts = fmt.Filers;
            for (int i = 0; i < exts.Length; i++)
            {
                formats.Add(exts[i], fmt);
            }
        }

        public void LoadStringTable(FileLocation fl)
        {
            if (strTbl == null)
            {
                string ext = Path.GetExtension(fl.Path);

                StringTableFormat fmt;
                if (formats.TryGetValue(ext, out fmt))
                {
                    strTbl = fmt.Load(fl);
                }
                else
                    throw new NotSupportedException(ext);
            }
        }



    }
}
