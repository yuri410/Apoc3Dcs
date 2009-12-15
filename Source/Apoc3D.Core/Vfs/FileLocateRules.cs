﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Apoc3D.Vfs
{
    public static class FileLocateRules
    {
        public static FileLocateRule Model
        {
            get;
            private set;
        }

        static FileLocateRules()
        {
            LocateCheckPoint[] pts = new LocateCheckPoint[1];
            pts[0] = new LocateCheckPoint();
            pts[0].AddPath(Paths.Models);

            Model = new FileLocateRule(pts);
        }
    }
}
