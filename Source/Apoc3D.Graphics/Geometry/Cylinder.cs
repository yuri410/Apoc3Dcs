using System;
using System.Collections.Generic;
using System.Text;
using Apoc3D.MathLib;

namespace Apoc3D.Graphics.Geometry
{
    /// <summary>
    /// A cylinder geometry primitive constructed with PrimitiveMesh
    /// </summary>
    public class Cylinder : Model
    {
        /// <summary>
        /// Creates a cylinder (actually a truncated cone) oriented along the Y axis. The base of the cylinder 
        /// is placed at Y = -height/2, and the top at height/2 = height. A cylinder is subdivided around 
        /// the Y axis into slices.
        /// </summary>
        /// <param name="bottom">Specifies the radius of the cylinder at y = -height/2.</param>
        /// <param name="top">Specifies the radius of the cylinder at y = height/2.</param>
        /// <param name="height">Specifies the height of the cylinder.</param>
        /// <param name="slices">
        /// Specifies the number of subdivisions around the Y axis. Should be greater than or equal to 3.
        /// </param>
        public Cylinder(float bottom, float top, float height, int slices)
            : base(CreateCylinder(bottom, top, height, slices))
        {
        }

        private static PrimitiveMesh CreateCylinder(float bottom, float top, float height, int slices)
        {
            if(slices < 3)
                throw new ArgumentException("Cannot draw a cylinder with slices less than 3");
            if (top < 0)
                throw new ArgumentException("Top has to be a positive natural number");
            if (bottom <= 0)
                throw new ArgumentException("Bottom has to be greater than zero");
            if (height <= 0)
                throw new ArgumentException("Height should be greater than zero");

            PrimitiveMesh mesh = new PrimitiveMesh();

            List<VertexPN> vertices = new List<VertexPN>();

            // Add top center vertex
            VertexPN topCenter;
            topCenter.pos = new Vector3(0, height / 2, 0);
            topCenter.n = new Vector3(0, 1, 0);

            vertices.Add(topCenter);

            // Add bottom center vertex
            VertexPN bottomCenter;
            bottomCenter.pos = new Vector3(0, -height / 2, 0);
            bottomCenter.n = new Vector3(0, -1, 0);

            vertices.Add(bottomCenter);

            double angle = 0;
            double incr = Math.PI * 2 / slices;
            float cos, sin;
            bool hasTop = (top > 0);
            Vector3 u, v;
            Matrix mat;
            bool tilted = (top != bottom);
            float rotAngle = (float)Math.Atan(bottom / height);
            Vector3 down = -Vector3.UnitY;
            if (hasTop)
            {
                // Add top & bottom side vertices
                for (int i = 0; i <= slices; i++, angle += incr)
                {
                    cos = (float)Math.Cos(angle);
                    sin = (float)Math.Sin(angle);

                    VertexPN topSide;
                    topSide.pos = new Vector3(cos * top, height / 2, sin * top);
                    topSide.n = Vector3.Normalize(topSide.pos - topCenter.pos);

                    VertexPN topSide2;
                    topSide2.pos = new Vector3(cos * top, height / 2, sin * top);
                    topSide2.n = topCenter.n;

                    // Add bottom side vertices
                    VertexPN bottomSide;
                    bottomSide.pos = new Vector3(cos * bottom, -height / 2, sin * bottom);
                    bottomSide.n = Vector3.Normalize(bottomSide.pos - bottomCenter.pos);

                    VertexPN bottomSide2;
                    bottomSide2.pos = new Vector3(cos * bottom, -height / 2, sin * bottom);
                    bottomSide2.n = bottomCenter.n;

                    if (tilted)
                    {
                        v = topSide.n;
                        u = Vector3.Cross(v, down);
                        mat = Matrix.Translation(v) * Matrix.RotationAxis(u, -rotAngle);
                        topSide.n = bottomSide.n = Vector3.Normalize(mat.TranslationValue);
                    }

                    vertices.Add(topSide);
                    vertices.Add(topSide2);
                    vertices.Add(bottomSide);
                    vertices.Add(bottomSide2);
                }
            }
            else
            {
                // Add top & bottom side vertices
                for (int i = 0; i <= slices; i++, angle += incr)
                {
                    cos = (float)Math.Cos(angle);
                    sin = (float)Math.Sin(angle);

                    VertexPN topSide;
                    topSide.pos = topCenter.pos;

                    // Add bottom side vertices
                    VertexPN bottomSide;
                    bottomSide.pos = new Vector3(cos * bottom, -height / 2, sin * bottom);
                    bottomSide.n = Vector3.Normalize(bottomSide.pos - bottomCenter.pos);

                    VertexPN bottomSide2;
                    bottomSide2.pos = new Vector3(cos * bottom, -height / 2, sin * bottom);
                    bottomSide2.n = bottomCenter.n;

                    v = bottomSide.n;
                    u = Vector3.Cross(v, down);
                    mat = Matrix.Translation(v) * Matrix.RotationAxis(u, rotAngle);
                    topSide.n = bottomSide.n = Vector3.Normalize(mat.TranslationValue);

                    vertices.Add(topSide);
                    vertices.Add(bottomSide);
                    vertices.Add(bottomSide2);
                }
            }

            mesh.VertexDeclaration = new VertexDeclaration(State.Device,
                VertexPN.Elements);

            mesh.VertexBuffer = new VertexBuffer(State.Device,
                VertexPositionNormal.SizeInBytes * vertices.Count, BufferUsage.None);
            mesh.VertexBuffer.SetData(vertices.ToArray());

            List<short> indices = new List<short>();

            if (hasTop)
            {
                // Create top & bottom circle 
                for (int i = 2; i < vertices.Count - 7; i += 4)
                {
                    indices.Add(0);
                    indices.Add((short)(i + 1));
                    indices.Add((short)(i + 5));

                    indices.Add(1);
                    indices.Add((short)(i + 7));
                    indices.Add((short)(i + 3));
                }

                // Create side
                for (int i = 2; i < vertices.Count - 7; i += 4)
                {
                    indices.Add((short)i);
                    indices.Add((short)(i + 2));
                    indices.Add((short)(i + 4));

                    indices.Add((short)(i + 4));
                    indices.Add((short)(i + 2));
                    indices.Add((short)(i + 6));
                }
            }
            else
            {
                // Create bottom circle 
                for (int i = 2; i < vertices.Count - 5; i += 3)
                {
                    indices.Add(1);
                    indices.Add((short)(i + 5));
                    indices.Add((short)(i + 2));
                }

                // Create side
                for (int i = 2; i < vertices.Count - 5; i += 3)
                {
                    indices.Add((short)i);
                    indices.Add((short)(i + 1));
                    indices.Add((short)(i + 4));
                }
            }

            mesh.IndexBuffer = new IndexBuffer(State.Device, typeof(short), indices.Count,
                BufferUsage.None);
            mesh.IndexBuffer.SetData(indices.ToArray());

            mesh.SizeInBytes = VertexPositionNormal.SizeInBytes;
            mesh.NumberOfVertices = vertices.Count;
            mesh.NumberOfPrimitives = indices.Count / 3;

            return mesh;
        }
    }
}
