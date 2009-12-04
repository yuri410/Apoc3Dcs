using System;
using System.Collections.Generic;
using System.Text;

namespace Apoc3D.Graphics.Collections
{
    public abstract class TextureWrapCollection
    {
        public abstract TextureWrapCoordinates this[int index]
        {
            get;
            set;
        }
    }
}