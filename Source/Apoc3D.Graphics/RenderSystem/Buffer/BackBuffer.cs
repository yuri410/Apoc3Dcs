using System;
using System.Collections.Generic;
using System.Text;
using Apoc3D.Media;

namespace Apoc3D.Graphics
{
    /// <summary>
    ///  表示纹理中的Surface，在这里通常是RenderTarget
    /// </summary>
    public abstract class BackBuffer : HardwareBuffer
    {

        public int Width
        {
            get;
            private set;
        }
        public int Height
        {
            get;
            private set;
        }

        public ImagePixelFormat ColorFormat
        {
            get;
            private set;
        }

        protected BackBuffer(int width, int height, BufferUsage usage, ImagePixelFormat format)
            : base(usage, PixelFormat.GetMemorySize(width, height, 1, format), false)
        {
            Width = width;
            Height = height;

            ColorFormat = format;
        }
    }
}
 