﻿using System;
using System.Collections.Generic;

namespace Apoc3D.Graphics.Geometry
{
    /// <summary>
    /// A chamfer cylinder geometry primitive constructed with PrimitiveMesh
    /// </summary>
    public class ChamferCylinder : Model
    {
        /// <summary>
        /// Creates a chamfer cylinder (a cylinder with spherical side) oriented along the Y axis. 
        /// </summary>
        /// <param name="radius">The radius of the cylinder at the base.</param>
        /// <param name="height">The height of the cylinder.</param>
        /// <param name="slices">The number of subdivisions around the Y axis (similar to lines 
        /// of longitude). This has to be greater than 4 and less than 101.</param>
        public ChamferCylinder(float radius, float height, int slices) :
            base(CreateChamferCylinder(radius, height, slices))
        {
        }

        private static PrimitiveMesh CreateChamferCylinder(float radius, float height, int slices)
        {
            if(slices < 5)
                throw new ArgumentException("Cannot draw a capsule with slices less than 5");
            if (slices > 100)
                throw new ArgumentException("Cannot draw a capsule with slices greater than 100");
            if (radius <= 0)
                throw new ArgumentException("radius should be greater than zero");
            if (height <= 0)
                throw new ArgumentException("height should be greater than zero");

            PrimitiveMesh mesh = new PrimitiveMesh();

            List<VertexPositionNormal> vertices = new List<VertexPositionNormal>();

            float halfH = height / 2;
            Vector3 topCenter = new Vector3(0, halfH, 0);
            Vector3 bottomCenter = new Vector3(0, -halfH, 0);

            double thai = 0, theta = 0;
            double thaiIncr = Math.PI / slices;
            double thetaIncr = Math.PI * 2 / slices;
            int countB, countA;

            // Add side vertices
            for (countA = 0, thai = 0; thai <= Math.PI; thai += thaiIncr, countA++)
            {
                for (countB = 0, theta = 0; countB < slices; theta += thetaIncr, countB++)
                {
                    VertexPositionNormal vert = new VertexPositionNormal();
                    vert.Position = new Vector3((float)(halfH * Math.Sin(thai) * Math.Cos(theta) + radius * Math.Cos(theta)),
                        (float)(halfH * Math.Cos(thai)), 
                        (float)(halfH * Math.Sin(thai) * Math.Sin(theta) + radius * Math.Sin(theta)));
                    vert.Normal = Vector3.Normalize(vert.Position);
                    vertices.Add(vert);
                }
            }

            // Add north pole vertex
            vertices.Add(new VertexPositionNormal(topCenter, new Vector3(0, 1, 0)));
            // Add south pole vertex
            vertices.Add(new VertexPositionNormal(bottomCenter, new Vector3(0, -1, 0)));

            mesh.VertexDeclaration = new VertexDeclaration(State.Device,
                VertexPositionNormal.VertexElements);

            mesh.VertexBuffer = new VertexBuffer(State.Device,
                VertexPositionNormal.SizeInBytes * vertices.Count, BufferUsage.None);
            mesh.VertexBuffer.SetData(vertices.ToArray());

            List<short> indices = new List<short>();

            // Create side of the hemispheres
            for (int i = 0; i < countA - 1; i++)
            {
                for (int j = 0; j < slices - 1; j++)
                {
                    indices.Add((short)(i * slices + j));
                    indices.Add((short)((i + 1) * slices + j));
                    indices.Add((short)((i + 1) * slices + j + 1));

                    indices.Add((short)(i * slices + j));
                    indices.Add((short)((i + 1) * slices + j + 1));
                    indices.Add((short)(i * slices + j + 1));
                }

                indices.Add((short)((i + 1) * slices - 1));
                indices.Add((short)((i + 2) * slices - 1));
                indices.Add((short)((i + 1) * slices));

                indices.Add((short)((i + 1) * slices - 1));
                indices.Add((short)((i + 1) * slices));
                indices.Add((short)(i * slices ));
            }

            // Create the north and south pole area mesh
            for (int i = 0, j = (countA - 1) * slices; i < slices - 1; i++, j++)
            {
                indices.Add((short)(vertices.Count - 2));
                indices.Add((short)i);
                indices.Add((short)(i + 1));

                indices.Add((short)(vertices.Count - 1));
                indices.Add((short)(j + 1));
                indices.Add((short)j);
            }
            indices.Add((short)(vertices.Count - 2));
            indices.Add((short)(slices - 1));
            indices.Add(0);

            indices.Add((short)(vertices.Count - 1));
            indices.Add((short)((countA - 1) * slices));
            indices.Add((short)(vertices.Count - 3));

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
