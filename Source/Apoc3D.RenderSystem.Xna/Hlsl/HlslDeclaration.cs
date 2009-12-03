using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Apoc3D.RenderSystem.Xna
{
    struct HlslDeclaration
    {
        public HlslType Type
        {
            get;
            set;
        }

        public AutoParamType Usage
        {
            get;
            set;
        }
        public int Index
        {
            get;
            set;
        }

        public string Name
        {
            get;
            set;
        }

        public HlslRegisterType Register
        {
            get;
            set;
        }

        public int RegisterIndex
        {
            get;
            set;
        }
    }
}