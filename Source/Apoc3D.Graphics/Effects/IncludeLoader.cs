﻿using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Apoc3D.Vfs;

namespace Apoc3D.Graphics.Effects
{
    public class IncludeHandler : Include
    {
        static IncludeHandler singleton;

        public static IncludeHandler Instance
        {
            get
            {
                if (singleton == null)
                {
                    singleton = new IncludeHandler();
                }
                return singleton;
            }
        }

        private IncludeHandler()
        {

        }

        #region Include 成员

        public void Close(Stream stream)
        {
            stream.Close();
        }

        public void Open(IncludeType includeType, string fileName, out Stream stream)
        {
            FileLocation fl = FileSystem.Instance.Locate(FileSystem.CombinePath(Paths.Effects, fileName), FileLocateRules.Default);
            stream = fl.GetStream;
        }

        #endregion
    }
}
