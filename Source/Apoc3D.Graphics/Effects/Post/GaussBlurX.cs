using System;
using System.Collections.Generic;
using System.Text;
using Apoc3D.Vfs;

namespace Apoc3D.Graphics.Effects
{
    class GaussBlurX : PostEffect
    {
        public GaussBlurX(RenderSystem rs)
            : base(rs)
        {
            string filePath = "gaussBlurX.ps";
            FileLocation fl = FileSystem.Instance.Locate(filePath, FileLocateRule.Effects);

            LoadPixelShader(rs, fl);
        }
    }
}
