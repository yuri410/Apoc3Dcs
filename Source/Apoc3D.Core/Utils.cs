﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace VirtualBicycle
{
    public unsafe static class Utils
    {
        public static string[] EmptyStringArray
        {
            get;
            private set;
        }

        static Utils()
        {
            EmptyStringArray = new string[0];
        }

        public static string GetTempFileName() { throw new NotImplementedException(); }
    }
}
