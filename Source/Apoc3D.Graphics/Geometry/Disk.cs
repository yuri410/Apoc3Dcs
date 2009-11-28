using System;
using System.Collections.Generic;
using System.Text;

namespace Apoc3D.Graphics.Geometry
{
    /// <summary>
    /// A disk geometry primitive constructed with PrimitiveMesh
    /// </summary>
    public class Disk : Model
    {
        /// <summary>
        /// Creates a disk or annulus on the y = 0 plane. The disk has a radius of outer, and 
        /// contains a concentric circular hole with a radius of inner. If inner is 0, 
        /// then no hole is generated. The disk is subdivided around the Y axis into radial slices 
        /// (like pizza slices)
        /// </summary>
        /// <param name="inner">Specifies the inner radius of the disk (may be 0).</param>
        /// <param name="outer">Specifies the outer radius of the disk. Must be larger than
        /// the 'inner' radius</param>
        /// <param name="slices">Specifies the number of subdivisions around the Y axis. Must be
        /// larger than 2.</param>
        /// <param name="twoSided">Specifies whether to render both front and back side</param>
        public Disk(float inner, float outer, int slices, bool twoSided)
            : base(CreateDisk(inner, outer, slices, 0, Math.PI * 2, twoSided))
        {
        }

        internal Disk(float inner, float outer, int slices, double start, double sweep, bool twoSided)
            : base(CreateDisk(inner, outer, slices, start, sweep, twoSided))
        {
        }

        private static PrimitiveMesh CreateDisk(float inner, float outer, int slices, 
            double start, double sweep, bool twoSided)
        {
            if (slices < 3)
                throw new ArgumentException("Cannot draw a disk with slices less than 3");
            if (inner < 0)
                throw new ArgumentException("Inner radius has to be greater than or equal to 0");
            if (outer <= 0)
                throw new ArgumentException("Outer radius has to be greater than 0");
            if (inner >= outer)
                throw new ArgumentException("Inner radius has to be less than outer radius");

            PrimitiveMesh mesh = new PrimitiveMesh();

            List<VertexPositionNormal> vertices = new List<VertexPositionNormal>();

            double angle = start;
            double incr = sweep / slices;
            float cos, sin;
            bool hasInner = (inner > 0);
            // Add top & bottom side vertices
            if (!hasInner)
            {
                VertexPositionNormal front = new VertexPositionNormal();
                front.Position = new Vector3(0, 0, 0);
                front.Normal = new Vector3(0, 1, 0);
                vertices.Add(front);

                if (twoSided)
                {
                    VertexPositionNormal back = new VertexPositionNormal();
                    back.Position = new Vector3(0, 0, 0);
                    back.Normal = new Vector3(0, -1, 0);
                    vertices.Add(back);
                }
            }

            // Add inner & outer vertices
            for (int i = 0; i <= slices; i++, angle += incr)
            {
                cos = (float)Math.Cos(angle);
                sin = (float)Math.Sin(angle);

                if (hasInner)
                {
                    VertexPositionNormal inside = new VertexPositionNormal();
                    inside.Position = new Vector3(cos * inner, 0, sin * inner);
                    inside.Normal = new Vector3(0, 1, 0);
                    vertices.Add(inside);

                    if (twoSided)
                        vertices.Add(new VertexPositionNormal(inside.Position, new Vector3(0, -1, 0)));
                }

                VertexPositionNormal outside = new VertexPositionNormal();
                outside.Position = new Vector3(cos * outer, 0, sin * outer);
                outside.Normal = new Vector3(0, 1, 0);
                vertices.Add(outside);

                if (twoSided)
                    vertices.Add(new VertexPositionNormal(outside.Position, new Vector3(0, -1, 0)));
            }

            mesh.VertexDeclaration = new VertexDeclaration(State.Device,
                VertexPositionNormal.VertexElements);

            mesh.VertexBuffer = new VertexBuffer(State.Device,
                VertexPositionNormal.SizeInBytes * vertices.Count, BufferUsage.None);
            mesh.VertexBuffer.SetData(vertices.ToArray());

            List<short> indices = new List<short>();

            // Create front side
            if (twoSided)
            {
                if (hasInner)
                {
                    for (int i = 0; i < vertices.Count - 2; i++)
                    {
                        indices.Add((short)(2 * i));
                        indices.Add((short)(2 * (i + 2 - (i+1)%2)));
                        indices.Add((short)(2 * (i + 2 - i%2)));

                        indices.Add((short)(2 * i - 1));
                        indices.Add((short)(2 * (i + 2 - (i + 1) % 2) - 1));
                        indices.Add((short)(2 * (i + 2 - i % 2) - 1));
                    }
                }
                else
                {
                    for (int i = 1; i < vertices.Count - 1; i++)
                    {
                        indices.Add((short)0);
                        indices.Add((short)(2 * i));
                        indices.Add((short)(2 * (i + 1)));

                        indices.Add((short)0);
                        indices.Add((short)(i + 3));
                        indices.Add((short)(i + 1));
                    }
                }
            }
            else
            {
                if (hasInner)
                {
                    for (int i = 0; i < vertices.Count - 2; i++)
                    {
                        indices.Add((short)i);
                        indices.Add((short)(i + 2 - (i+1)%2));
                        indices.Add((short)(i + 2 - i%2));
                    }
                }
                else
                {
                    for (int i = 1; i < vertices.Count - 1; i++)
                    {
                        indices.Add((short)0);
                        indices.Add((short)(i));
                        indices.Add((short)(i + 1));
                    }
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
