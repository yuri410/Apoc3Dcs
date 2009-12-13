using System;
using System.Collections.Generic;
using System.Text;
using Apoc3D.MathLib;

namespace Apoc3D.Graphics.Geometry
{
    public unsafe class Line : Model
    {
        public Line(RenderSystem rs, VertexElement[] elements, Vector3[] points, float width)
            : base(rs, 1)
        {
            MeshData data = new MeshData(rs);

            int size = MeshData.ComputeVertexSize(elements);

            int vbSize = points.Length * size;
            byte[] buffer = new byte[vbSize * 2];

            fixed (byte* dst = &buffer[0]) 
            {
                Vector3* vptr = (Vector3*)dst;


            }

            GameMesh mesh = new GameMesh(rs, data);

            base.Entities = new GameMesh[1] { mesh };
        }

    }
}
