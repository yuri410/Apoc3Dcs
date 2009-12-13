using System;
using System.Collections.Generic;
using System.Text;
using Apoc3D.Graphics.Animation;
using Apoc3D.MathLib;

namespace Apoc3D.Graphics.Geometry
{
    /// <summary>
    /// A sphere geometry primitive constructed with PrimitiveMesh
    /// </summary>
    public class Sphere : Model
    {
        /// <summary>
        /// Creates a sphere of the given radius centered around the origin. The sphere 
        /// is subdivided around the Y axis into slices and along the Y axis into stacks 
        /// (similar to lines of longitude and latitude). 
        /// </summary>
        /// <param name="radius">Specifies the radius of the sphere.</param>
        /// <param name="slices">Specifies the number of subdivisions around the Y axis 
        /// (similar to lines of longitude). This has to be greater than 4 and less than 101.</param>
        /// <param name="stacks">Specifies the number of subdivisions along the Y axis 
        /// (similar to lines of latitude). This has to be greater than 4 and less than 101.</param>
        public Sphere(RenderSystem rs, float radius, int slices, int stacks)
            : base(rs, 1)
        {
            GameMesh gm = new GameMesh(rs, CreateSphere(rs, radius, slices, stacks));
            base.Entities = new GameMesh[1] { gm };
            base.CurrentAnimation = new NoAnimation();
        }

        private unsafe static MeshData CreateSphere(RenderSystem rs, float radius, int slices, int stacks)
        {
            if (slices < 5)
                throw new ArgumentException("Cannot draw a sphere with slices less than 5");
            if (slices > 100)
                throw new ArgumentException("Cannot draw a sphere with slices greater than 100");
            if (stacks < 5)
                throw new ArgumentException("Cannot draw a sphere with stacks less than 5");
            if (stacks > 100)
                throw new ArgumentException("Cannot draw a sphere with stacks greater than 100");
            if (radius <= 0)
                throw new ArgumentException("Radius has to be greater than 0");


            MeshData mesh = new MeshData(rs);

            #region 顶点数据
            List<VertexPNT1> vertices = new List<VertexPNT1>();

            double thai = 0, theta = 0;
            double thaiIncr = Math.PI / stacks;
            double thetaIncr = Math.PI * 2 / slices;
            int countB, countA;
            // Add sphere vertices
            for (countA = 0, thai = thaiIncr; thai < Math.PI - thaiIncr / 2; thai += thaiIncr, countA++)
            {
                for (countB = 0, theta = 0; countB < slices; theta += thetaIncr, countB++)
                {
                    VertexPNT1 vert = new VertexPNT1();
                    vert.pos = new Vector3((float)(radius * Math.Sin(thai) * Math.Cos(theta)),
                        (float)(radius * Math.Cos(thai)), (float)(radius * Math.Sin(thai) * Math.Sin(theta)));
                    vert.n = Vector3.Normalize(vert.pos);
                    vertices.Add(vert);
                }
            }

            VertexPNT1 temp = new VertexPNT1();
            temp.pos = new Vector3(0, radius, 0);
            temp.n = new Vector3(0, 1, 0);

            // Add north pole vertex
            vertices.Add(temp);

            temp.pos = new Vector3(0, -radius, 0);
            temp.n = new Vector3(0, -1, 0);

            // Add south pole vertex
            vertices.Add(temp);
            #endregion

            mesh.VertexElements = VertexPNT1.Elements;


            #region 索引数据
            List<MeshFace> indices = new List<MeshFace>();

            // Create the north and south pole area mesh
            for (int i = 0, j = (countA - 1) * slices; i < slices - 1; i++, j++)
            {
                indices.Add(
                    new MeshFace(vertices.Count - 2, i, i + 1));

                indices.Add(
                    new MeshFace(vertices.Count - 1, j + 1, j));
            }
            indices.Add(
                new MeshFace(vertices.Count - 2, slices - 1, 0));

            indices.Add(
                new MeshFace(vertices.Count - 1, (countA - 1) * slices, vertices.Count - 3));

            // Create side of the sphere
            for (int i = 0; i < countA - 1; i++)
            {
                for (int j = 0; j < slices - 1; j++)
                {
                    indices.Add(
                        new MeshFace(i * slices + j, (i + 1) * slices + j, (i + 1) * slices + j + 1));

                    indices.Add(
                        new MeshFace(i * slices + j, (i + 1) * slices + j + 1, i * slices + j + 1));
                }

                indices.Add(
                       new MeshFace((i + 1) * slices - 1, (i + 2) * slices - 1, (i + 1) * slices));
                indices.Add(
                       new MeshFace((i + 1) * slices - 1, (i + 1) * slices, i * slices));

            }
            #endregion

            mesh.Faces = indices.ToArray();
            mesh.VertexSize = VertexPNT1.Size;
            mesh.VertexCount = vertices.Count;

            VertexPNT1[] vexArray = vertices.ToArray();

            byte[] buffer = new byte[mesh.VertexCount * mesh.VertexSize];

            fixed (byte* dst = &buffer[0])
            {
                fixed (VertexPNT1* src = &vexArray[0])
                {
                    Memory.Copy(src, dst, buffer.Length);
                }
                mesh.SetData(dst, buffer.Length);
            }
            mesh.Materials = new Material[1][] { new Material[1] { Material.DefaultMaterial } };

            return mesh;
        }
    }
}
