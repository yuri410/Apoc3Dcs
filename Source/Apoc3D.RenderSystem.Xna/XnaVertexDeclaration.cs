/*
-----------------------------------------------------------------------------
This source file is part of Apoc3D Engine

Copyright (c) 2009+ Tao Games

This program is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with this program.  if not, write to the Free Software Foundation, 
Inc., 59 Temple Place - Suite 330, Boston, MA 02111-1307, USA, or go to
http://www.gnu.org/copyleft/gpl.txt.

-----------------------------------------------------------------------------
*/
using System;
using System.Collections.Generic;
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
            this.vtxDecl = new XG.VertexDeclaration(rs.Device, ConvertElements(elements));
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