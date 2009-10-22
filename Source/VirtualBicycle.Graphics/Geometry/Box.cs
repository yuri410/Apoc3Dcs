using System;
using System.Collections.Generic;
using System.Text;

namespace VirtualBicycle.Graphics.Geometry
{
    /// <summary>
    /// A box geometry primitive constructed with PrimitiveMesh
    /// </summary>
    public class Box : Model
    {
        /// <summary>
        /// Create a box with the given dimensions.
        /// </summary>
        /// <param name="xdim">X dimension</param>
        /// <param name="ydim">Y dimension</param>
        /// <param name="zdim">Z dimension</param>
        public Box(float xdim, float ydim, float zdim)
            : this(new Vector3(xdim, ydim, zdim))
        {
        }

        /// <summary>
        /// Creates a box whose edges are the given length.
        /// </summary>
        /// <param name="length">Length of each side of the cube</param>
        public Box(float length)
            : this(new Vector3(length, length, length))
        {
        }

        /// <summary>
        /// Create a box with the given dimensions.
        /// </summary>
        /// <param name="dimension">Dimension of the box geometry</param>
        public Box(Vector3 dimension)
            : base(CreateBox(dimension))
        {
        }

        private static PrimitiveMesh CreateBox(Vector3 dimension)
        {
            PrimitiveMesh mesh = new PrimitiveMesh();

            VertexPositionNormal[] vertices = new VertexPositionNormal[24];
            Vector3 halfExtent = dimension / 2;

            Vector3 v0 = new Vector3(-halfExtent.X, -halfExtent.Y, -halfExtent.Z);
            Vector3 v1 = new Vector3(halfExtent.X, -halfExtent.Y, -halfExtent.Z);
            Vector3 v2 = new Vector3(-halfExtent.X, halfExtent.Y, -halfExtent.Z);
            Vector3 v3 = new Vector3(halfExtent.X, halfExtent.Y, -halfExtent.Z);
            Vector3 v4 = new Vector3(halfExtent.X, halfExtent.Y, halfExtent.Z);
            Vector3 v5 = new Vector3(-halfExtent.X, halfExtent.Y, halfExtent.Z);
            Vector3 v6 = new Vector3(halfExtent.X, -halfExtent.Y, halfExtent.Z);
            Vector3 v7 = new Vector3(-halfExtent.X, -halfExtent.Y, halfExtent.Z);

            Vector3 nZ = new Vector3(0, 0, -1);
            Vector3 pZ = new Vector3(0, 0, 1);
            Vector3 nX = new Vector3(-1, 0, 0);
            Vector3 pX = new Vector3(1, 0, 0);
            Vector3 nY = new Vector3(0, -1, 0);
            Vector3 pY = new Vector3(0, 1, 0);

            vertices[0].Position = v0; vertices[1].Position = v1; 
            vertices[2].Position = v2; vertices[3].Position = v3; 
            
            vertices[4].Position = v0; vertices[5].Position = v7;
            vertices[6].Position = v2; vertices[7].Position = v5; 
            
            vertices[8].Position = v4; vertices[9].Position = v5; 
            vertices[10].Position = v7; vertices[11].Position = v6;

            vertices[12].Position = v4; vertices[13].Position = v3; 
            vertices[14].Position = v1; vertices[15].Position = v6; 
            
            vertices[16].Position = v2; vertices[17].Position = v4;
            vertices[18].Position = v5; vertices[19].Position = v3; 
            
            vertices[20].Position = v0; vertices[21].Position = v1; 
            vertices[22].Position = v6; vertices[23].Position = v7;

            for (int i = 0; i < 4; i++)
                vertices[i].Normal = nZ;
            for (int i = 4; i < 8; i++)
                vertices[i].Normal = nX;
            for (int i = 8; i < 12; i++)
                vertices[i].Normal = pZ;
            for (int i = 12; i < 16; i++)
                vertices[i].Normal = pX;
            for (int i = 16; i < 20; i++)
                vertices[i].Normal = pY;
            for (int i = 20; i < 24; i++)
                vertices[i].Normal = nY;

            mesh.VertexDeclaration = new VertexDeclaration(State.Device,
                VertexPositionNormal.VertexElements);

            mesh.VertexBuffer = new VertexBuffer(State.Device,
                VertexPositionNormal.SizeInBytes * 24, BufferUsage.None);
            mesh.VertexBuffer.SetData(vertices);

            short [] indices = new short[36];

            indices[0] = 0; indices[1] = 1; indices[2] = 2;
            indices[3] = 2; indices[4] = 1; indices[5] = 3;
            indices[6] = 4; indices[7] = 6; indices[8] = 5;
            indices[9] = 6; indices[10] = 7; indices[11] = 5;
            indices[12] = 11; indices[13] = 10; indices[14] = 9;
            indices[15] = 11; indices[16] = 9; indices[17] = 8;
            indices[18] = 14; indices[19] = 15; indices[20] = 13;
            indices[21] = 15; indices[22] = 12; indices[23] = 13;
            indices[24] = 19; indices[25] = 17; indices[26] = 18;
            indices[27] = 19; indices[28] = 18; indices[29] = 16;
            indices[30] = 21; indices[31] = 20; indices[32] = 23;
            indices[33] = 21; indices[34] = 23; indices[35] = 22;

            mesh.IndexBuffer = new IndexBuffer(State.Device, typeof(short), 36,
                BufferUsage.None);
            mesh.IndexBuffer.SetData(indices);

            mesh.SizeInBytes = VertexPositionNormal.SizeInBytes;
            mesh.NumberOfVertices = 24;
            mesh.NumberOfPrimitives = 12;

            return mesh;
        }
    }
}
