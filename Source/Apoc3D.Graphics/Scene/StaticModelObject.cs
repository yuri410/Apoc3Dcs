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
using System.ComponentModel;
using System.Text;
using Apoc3D.Design;
using Apoc3D.Graphics;
using Apoc3D.MathLib;

namespace Apoc3D.Scene
{
    /// <summary>
    ///  表示静态物体
    /// </summary>
    public abstract class StaticModelObject : Entity
    {
        #region 字段

        //bool isPhyBuilt;

        //TriangleMesh cdMesh;

        #endregion

        #region 构造函数

        protected StaticModelObject()
            : base(false)
        {
        }

        protected StaticModelObject(bool hasSubObjects)
            : base(hasSubObjects)
        {

        }

        protected StaticModelObject(Vector3 position, Quaternion orientation, bool hasSubObjects)
            : base(hasSubObjects)
        {
            base.position = position;
            base.orientation = Matrix.RotationQuaternion(orientation);

            base.UpdateTransform();
        }

        #endregion

       
        //public override bool IntersectsSelectionRay(ref Ray ray)
        //{
        //    if (RigidBody == null)
        //    {
        //        return base.IntersectsSelectionRay(ref ray);                       
        //    }

        //    BvhTriMeshResShape shape = RigidBody.CollisionShape as BvhTriMeshResShape;

        //    LineSegment line = new LineSegment(ray.Position, ray.Direction * 1000 + ray.Position);

        //    if (shape != null)
        //    {
        //        CollisionWorld.ClosestRayResultCallback cbk = new CollisionWorld.ClosestRayResultCallback(line.Start, line.End);

        //        PM.Matrix worldTocollisionObject = MathUtil.InvertMatrix(RigidBody.WorldTransform);

        //        PM.Vector3 rayFromLocal = PM.Vector3.TransformNormal(line.Start, worldTocollisionObject);
        //        PM.Vector3 rayToLocal = PM.Vector3.TransformNormal(line.End, worldTocollisionObject);

        //        BridgeTriangleRaycastCallback rcb = new BridgeTriangleRaycastCallback(rayFromLocal, rayToLocal, cbk, RigidBody, shape);
        //        rcb.HitFraction = cbk.ClosestHitFraction;

        //        PM.Vector3 rayAabbMinLocal = rayFromLocal;
        //        MathUtil.SetMin(ref rayAabbMinLocal, rayToLocal);
        //        PM.Vector3 rayAabbMaxLocal = rayFromLocal;
        //        MathUtil.SetMax(ref rayAabbMaxLocal, rayToLocal);

        //        shape.ProcessAllTriangles(rcb, rayAabbMinLocal, rayAabbMaxLocal);

        //        return (cbk.CollisionObject == RigidBody);
        //    }

        //    return false;
        //}

        #region 物理相关

        [Browsable(false)]
        public override bool HasPhysicsModel
        {
            get { return true; }
        }
        //public override bool IntersectsSelectionRay(ref Apoc3D.MathLib.Ray ray)
        //{
        //    if (cdMesh != null) 
        //    {
        //        float frac;
        //        Vector3 p1, p2;
        //        Segment seg = new Segment(ray.Position, ray.Direction * 1000);
        //        if (cdMesh.SegmentIntersect(out frac, out p1, out p2, seg)) 
        //        {
        //            return true;
        //        }
        //    }
        //    return base.IntersectsSelectionRay(ref ray);
        //}

        //public unsafe override void BuildPhysicsModel(PhysicsSystem world)
        //{
        //    if (!isPhyBuilt)
        //    {
        //        //UpdateTransform();

        //        //Matrix trans = Transformation;
        //        //trans.M41 = 0;
        //        //trans.M42 = 0;
        //        //trans.M43 = 0;

        //        //cdMesh = CollisionMeshManager.Instance.CreateInstance(ModelL0, trans);
        //        //BvhTriMeshResShape shape = new BvhTriMeshResShape(cdMesh);


        //        //motionState = new DefaultMotionState(Matrix.Translation(position));

        //        //RigidBody = new RigidBody(0, motionState, shape);
        //        //RigidBody.CollisionFlags |= CollisionOptions.StaticObject;

        //        //PM.Vector3 aabbMin;
        //        //PM.Vector3 aabbMax;
        //        //shape.GetAabb(PM.Matrix.Identity, out aabbMin, out aabbMax);

        //        //float rad = PM.Vector3.Distance(aabbMin, aabbMax) * 0.5f;

        //        //BoundingSphereOffset = 0.5f * (aabbMin + aabbMax);
        //        //BoundingSphere.Radius = rad;

        //        //if (world != null)
        //        //{
        //        //    world.AddRigidBody(RigidBody);
        //        //}
        //        isPhyBuilt = true;
        //    }
        //}

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            //if (disposing)
            //{
            //    CollisionMeshManager.Instance.DestoryInstance(cdMesh);
            //}
            //cdMesh = null;
        }
        #endregion

    }
}
