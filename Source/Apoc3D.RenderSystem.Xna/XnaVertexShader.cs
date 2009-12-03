using System;
using System.Collections.Generic;
using System.Text;
using Apoc3D.Graphics;
using X = Microsoft.Xna.Framework;
using XG = Microsoft.Xna.Framework.Graphics;

namespace Apoc3D.RenderSystem.Xna
{
    class XnaVertexShader : VertexShader
    {
        XG.GraphicsDevice device;
        public XnaVertexShader(XnaRenderSystem rs)
            : base(rs)
        {
            device = rs.device;
        }

        public override void SetValue<T>(string paramName, T value)
        {
            
            throw new NotImplementedException();
        }

        public override void SetTexture(string paramName, Texture tex)
        {
            throw new NotImplementedException();
        }

        public override void AutoSetParameters()
        {
            throw new NotImplementedException();
        }
    }
}