#region Using Statements
using System;
using System.Collections.Generic;
using System.Text;
using VirtualBicycle.MathLib;
using JigLibX.Math;
using JigLibX.Collision;
#endregion

namespace JigLibX.Geometry
{
    public class TriangleMesh : Primitive
    {
        private Octree octree = new Octree();

        private int maxTrianglesPerCell;
        private float minCellSize;

        public TriangleMesh()
            : base((int)PrimitiveType.TriangleMesh)
        {
        }

        public void CreateMesh(List<Vector3> vertices,
            List<TriangleVertexIndices> triangleVertexIndices,
            int maxTrianglesPerCell, float minCellSize)
        {
            int numVertices = vertices.Count;

            octree.Clear(true);
            octree.AddTriangles(vertices, triangleVertexIndices);
            octree.BuildOctree(maxTrianglesPerCell, minCellSize);

            this.maxTrianglesPerCell = maxTrianglesPerCell;
            this.minCellSize = minCellSize;
        }

        public override void GetBoundingBox(out AABox box)
        {
            box = octree.BoundingBox.Clone() as AABox;
            box.Transform = Transform;
        }

        private Matrix transformMatrix;
        private Matrix invTransform;
        public override Transform Transform
        {
            get
            {
                return base.Transform;
            }
            set
            {
                base.Transform = value;
                transformMatrix = transform.Orientation;
                transformMatrix.TranslationValue = transform.Position;
                invTransform = Matrix.Invert(transformMatrix);
            }
        }

        // use a cached version as this occurs ALOT during triangle mesh traversal
        public override Matrix TransformMatrix
        {
            get
            {
                return transformMatrix;
            }
        }
        // use a cached version as this occurs ALOT during triangle mesh traversal
        public override Matrix InverseTransformMatrix
        {
            get
            {
                return invTransform;
            }
        }

        public Octree Octree
        {
            get { return this.octree; }
        }

        public int GetNumTriangles()
        {
            return octree.NumTriangles;
        }

        public IndexedTriangle GetTriangle(int iTriangle)
        {
            return octree.GetTriangle(iTriangle);
        }

        public Vector3 GetVertex(int iVertex)
        {
            return octree.GetVertex(iVertex);
        }

        public void GetVertex(int iVertex, out Vector3 result)
        {
            result = octree.GetVertex(iVertex);
        }

        // BEN-PATCH: Someone else's patch... needs testing but the idea seems correct.
        public unsafe int GetTrianglesIntersectingtAABox(int* triangles, int maxTriangles, ref BoundingBox bb)
        {
            // rotated aabb
            BoundingBox rotBB = bb;

            Vector3 bbCorner = new Vector3();
            Vector3 bbCornerT;

            for (int a = 0; a < 2; a++)
            {
                for (int b = 0; b < 2; b++)
                {
                    for (int c = 0; c < 2; c++)
                    {
                        bbCorner.X = ((a == 0) ? bb.Minimum.X : bb.Maximum.X);
                        bbCorner.Y = ((b == 0) ? bb.Minimum.Y : bb.Maximum.Y);
                        bbCorner.Z = ((c == 0) ? bb.Minimum.Z : bb.Maximum.Z);
                        bbCornerT = Vector3.TransformSimple(bbCorner, invTransform);

                        BoundingBoxHelper.AddPoint(ref bbCornerT, ref rotBB);
                    }
                }
            }
            return octree.GetTrianglesIntersectingtAABox(triangles, maxTriangles, ref rotBB);
        }

        public override Primitive Clone()
        {
            TriangleMesh triangleMesh = new TriangleMesh();
            //            triangleMesh.CreateMesh(vertices, triangleVertexIndices, maxTrianglesPerCell, minCellSize);
            // its okay to share the octree
            triangleMesh.octree = this.octree;
            triangleMesh.Transform = Transform;
            return triangleMesh;
        }

        public override bool SegmentIntersect(out float frac, out Vector3 pos, out Vector3 normal, Segment seg)
        {
            // move segment into octree space
            seg.Origin = Vector3.TransformSimple(seg.Origin, invTransform);
            seg.Delta = Vector3.TransformNormal(seg.Delta, invTransform);


            BoundingBox segBox = BoundingBoxHelper.InitialBox;
            BoundingBoxHelper.AddSegment(seg, ref segBox);

            unsafe
            {
#if USE_STACKALLOC
                int* potentialTriangles = stackalloc int[MaxLocalStackTris];
                {
#else
                int[] potTriArray = DetectFunctor.IntStackAlloc();
                fixed (int* potentialTriangles = potTriArray)
                {
#endif
                    int numTriangles = GetTrianglesIntersectingtAABox(potentialTriangles, DetectFunctor.MaxLocalStackTris, ref segBox);

                    float tv1, tv2;

                    pos = Vector3.Zero;
                    normal = Vector3.Zero;

                    float bestFrac = float.MaxValue;
                    for (int iTriangle = 0; iTriangle < numTriangles; ++iTriangle)
                    {
                        IndexedTriangle meshTriangle = GetTriangle(potentialTriangles[iTriangle]);
                        float thisFrac;
                        Triangle tri = new Triangle(GetVertex(meshTriangle.GetVertexIndex(0)),
                          GetVertex(meshTriangle.GetVertexIndex(1)),
                          GetVertex(meshTriangle.GetVertexIndex(2)));

                        if (Intersection.SegmentTriangleIntersection(out thisFrac, out tv1, out tv2, seg, tri))
                        {
                            if (thisFrac < bestFrac)
                            {
                                bestFrac = thisFrac;
                                // re-project
                                pos = Vector3.TransformSimple(seg.GetPoint(thisFrac), transformMatrix);
                                normal = Vector3.TransformNormal(meshTriangle.Plane.Normal, transformMatrix);
                            }
                        }
                    }

                    frac = bestFrac;
                    if (bestFrac < float.MaxValue)
                    {
                        DetectFunctor.FreeStackAlloc(potTriArray);
                        return true;
                    }
                    else
                    {
                        DetectFunctor.FreeStackAlloc(potTriArray);
                        return false;
                    }
#if USE_STACKALLOC
                }
#else
                }
#endif
            }
        }

        public override float GetVolume()
        {
            return 0.0f;
        }

        public override float GetSurfaceArea()
        {
            return 0.0f;
        }

        public override void GetMassProperties(PrimitiveProperties primitiveProperties, out float mass, out Vector3 centerOfMass, out Matrix inertiaTensor)
        {
            mass = 0.0f;
            centerOfMass = Vector3.Zero;
            inertiaTensor = Matrix.Identity;
        }
    }
}
