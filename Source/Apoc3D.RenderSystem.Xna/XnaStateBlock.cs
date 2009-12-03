using System;
using System.Collections.Generic;
using System.Text;
using Apoc3D.Graphics;
using X = Microsoft.Xna.Framework;
using XG = Microsoft.Xna.Framework.Graphics;

namespace Apoc3D.RenderSystem.Xna
{
    class XnaStateBlock : StateBlock
    {        
        internal XG.StateBlock stateBlockXna;

        public XnaStateBlock(XnaRenderSystem rs) 
            : base(rs)
        {
            stateBlockXna = new XG.StateBlock(rs.device);
        }

        internal XnaStateBlock(XnaRenderSystem rs, XG.StateBlock stateBlock)
            : base(rs)
        {
            stateBlockXna = stateBlock;
        }

        public override void Capture()
        {
            stateBlockXna.Capture();
        }

        public override void Apply()
        {
            stateBlockXna.Apply();
        }
    }
}