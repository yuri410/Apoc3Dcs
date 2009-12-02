using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Apoc3D.Graphics;
using X = Microsoft.Xna.Framework;
using XG = Microsoft.Xna.Framework.Graphics;

namespace Apoc3D.RenderSystem.Xna
{
    class XnaVertexDeclaration : VertexDeclaration
    {
        internal XG.VertexDeclaration vtxDecl;

        XnaRenderSystem renderSys;

        static VertexElement[] ConvertElements(XG.VertexElement[] elems)
        {
            VertexElement[] result = new VertexElement[elems.Length];
            for (int i = 0; i < result.Length; i++)
            {
                result[i] = new VertexElement(elems[i].Offset,
                    XnaUtils.ConvertEnum(elems[i].VertexElementFormat),
                    XnaUtils.ConvertEnum(elems[i].VertexElementUsage), elems[i].UsageIndex);
            }
            return result;
        }
        static XG.VertexElement[] ConvertElements(VertexElement[] elems)
        {
            XG.VertexElement[] result = new XG.VertexElement[elems.Length];
            for (int i = 0; i < result.Length; i++)
            {
                result[i] = new XG.VertexElement(0, (short)elems[i].Offset,
                    XnaUtils.ConvertEnum(elems[i].Type), XG.VertexElementMethod.Default,
                    XnaUtils.ConvertEnum(elems[i].Semantic), (byte)elems[i].Index);
            }
            return result;
        }

        public XnaVertexDeclaration(XnaRenderSystem rs, VertexElement[] elements)
            : base(elements)
        {
            this.renderSys = rs;
            this.vtxDecl = new XG.VertexDeclaration(rs.device, ConvertElements(elements));
        }

        internal XnaVertexDeclaration(XnaRenderSystem rs, XG.VertexDeclaration decl)
            : base(ConvertElements(decl.GetVertexElements()))
        {
            this.renderSys = rs;
            this.vtxDecl = decl;
        }

        public override void Dispose(bool disposing)
        {
            if (disposing)
            {
                this.vtxDecl.Dispose();
            }
            this.vtxDecl = null;
        }
    }
}