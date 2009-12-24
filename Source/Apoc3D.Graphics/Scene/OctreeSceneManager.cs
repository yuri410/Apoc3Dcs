﻿using System;
using System.Collections.Generic;
using System.Text;
using Apoc3D.Collections;
using Apoc3D.Graphics;
using Apoc3D.MathLib;

namespace Apoc3D.Scene
{
    /// <summary>
    ///  实现一个八叉树场景管理器
    /// </summary>
    public class OctreeSceneManager : SceneManagerBase
    {
        protected OctreeSceneNode octRootNode;

        protected Queue<OctreeSceneNode> queue;

        protected List<DynamicObject> dynObjs;

        protected List<SceneObject> farObjects;
        protected ExistTable<SceneObject> farObjTable;

        protected internal OctreeBox range;
        protected Vector3 min;
        protected Vector3 max;

        public OctreeSceneManager(OctreeBox range, float minBVSize)
        {
            this.range = range;
            MinimumBVSize = minBVSize;

            dynObjs = new List<DynamicObject>();
            farObjects = new List<SceneObject>();
            farObjTable = new ExistTable<SceneObject>();
            queue = new Queue<OctreeSceneNode>();

            octRootNode.BoundingVolume = range;
            octRootNode.BoundingVolume.GetBoundingSphere(out octRootNode.BoundingSphere);

            min = octRootNode.BoundingVolume.Center - new Vector3(octRootNode.BoundingVolume.Length * 0.5f);
            max = octRootNode.BoundingVolume.Center + new Vector3(octRootNode.BoundingVolume.Length * 0.5f);
        }

        
        public float MinimumBVSize
        {
            get;
            protected set;
        }


        /// <summary>
        ///  把物体加入到场景管理器中，并将它附到一个合适的节点
        /// </summary>
        /// <param name="obj">加入的物体</param>
        public override void AddObjectToScene(SceneObject obj)
        {
            base.AddObjectToScene(obj);

            AddObject(obj);

            DynamicObject dynObj = obj as DynamicObject;
            if (dynObj != null)
            {
                dynObjs.Add(dynObj);
            }

            obj.OnAddedToScene(this, this);
        }

        void AddObject(SceneObject obj)
        {
            Vector3 pos = obj.BoundingSphere.Center;

            if (pos.X >= min.X && pos.X <= max.X &&
                pos.Z >= min.Z && pos.Z <= max.Z)
            {
                if (pos.Y >= min.Y && pos.Y <= max.Y)
                {
                    octRootNode.AddObject(obj);
                    if (farObjTable.Exists(obj))
                    {
                        farObjTable.Remove(obj);
                        farObjects.Remove(obj);
                    }
                }
                else
                {
                    if (!farObjTable.Exists(obj))
                    {
                        farObjects.Add(obj);
                        farObjTable.Add(obj);
                    }
                }
            }
            else
            {
                if (farObjTable.Exists(obj))
                {
                    farObjTable.Remove(obj);
                    farObjects.Remove(obj);
                }
            }
        }

        /// <summary>
        ///  把物体从场景管理器中移除
        /// </summary>
        /// <param name="obj">移除的物体</param>
        public override void RemoveObjectFromScene(SceneObject obj)
        {
            base.RemoveObjectFromScene(obj);

            DynamicObject dynObj = obj as DynamicObject;
            if (dynObj != null)
            {
                dynObjs.Remove(dynObj);
            }

            RemoveObject(obj);

            obj.OnRemovedFromScene(this, this);
        }

        void RemoveObject(SceneObject obj)
        {
            octRootNode.RemoveObject(obj);

            if (farObjTable.Exists(obj))
            {
                farObjTable.Remove(obj);
                farObjects.Remove(obj);
            }
        }


        protected override void BuildSceneManager()
        {
            octRootNode = new OctreeSceneNode(this, null);
        }

        //public override SceneNode FindNode(SceneObject obj)
        //{
        //    return base.FindNode(obj);
        //}
        public override void FindObjects(FastList<SceneObject> objects, Frustum frus)
        {
            if (queue.Count == 0)
            {
                queue.Enqueue(octRootNode);
                while (queue.Count > 0)
                {
                    OctreeSceneNode node = queue.Dequeue();

                    // if the node does't intersect the frustum we don't give a damn
                    Vector3 c1 = node.BoundingSphere.Center;

                    if (frus.IntersectsSphere(ref c1, node.BoundingSphere.Radius))
                    {
                        for (int i = 0; i < 2; i++)
                            for (int j = 0; j < 2; j++)
                                for (int k = 0; k < 2; k++)
                                {
                                    if (node[i, j, k] != null)
                                    {
                                        queue.Enqueue(node[i, j, k]);
                                    }
                                }

                        for (int i = 0; i < node.AttchedObjects.Count; i++)
                        {
                            //if (frus.IsSphereIn(ref node.BoundingSphere.Center, node.BoundingSphere.Radius))
                            SceneObject curObj = node.AttchedObjects.Elements[i];

                            Vector3 c2 = node.BoundingSphere.Center;

                            if (frus.IntersectsSphere(ref c2, curObj.BoundingSphere.Radius))
                            {
                                objects.Add(curObj);
                            }
                        }
                    }

                }
            }
            else
            {
                throw new InvalidOperationException();
            }
        }

        public override SceneObject FindObject(Ray ray)
        {
            SceneObject result = null;
            float nearest = float.MaxValue;
            if (queue.Count == 0)
            {
                queue.Enqueue(octRootNode);
                while (queue.Count > 0)
                {
                    OctreeSceneNode node = queue.Dequeue();

                    // if the node does't intersect the frustum we don't give a damn
                    if (MathEx.BoundingSphereIntersects(ref  node.BoundingSphere, ref ray))
                    {
                        for (int i = 0; i < 2; i++)
                            for (int j = 0; j < 2; j++)
                                for (int k = 0; k < 2; k++)
                                {
                                    if (node[i, j, k] != null)
                                    {
                                        queue.Enqueue(node[i, j, k]);
                                    }
                                }

                        for (int i = 0; i < node.AttchedObjects.Count; i++)
                        {
                            SceneObject curObj = node.AttchedObjects.Elements[i];

                            if (curObj.IntersectsSelectionRay(ref ray))// MathEx.BoundingSphereIntersects(ref curObj.BoundingSphere, ref ray))
                            {
                                float dist = MathEx.DistanceSquared(ref  curObj.BoundingSphere.Center, ref ray.Position);
                                if (dist < nearest)
                                {
                                    nearest = dist;
                                    result = curObj;
                                }
                            }
                        }
                    }

                }
            }
            else
            {
                throw new InvalidOperationException();
            }

            for (int i = 0; i < dynObjs.Count; i++)
            {
                if (dynObjs[i].IntersectsSelectionRay(ref ray))
                {
                    float dist = MathEx.DistanceSquared(ref  dynObjs[i].BoundingSphere.Center, ref ray.Position);
                    if (dist < nearest)
                    {
                        nearest = dist;
                        result = dynObjs[i];
                    }
                }
            }
            for (int i = 0; i < farObjects.Count; i++)
            {
                if (farObjects[i].IntersectsSelectionRay(ref ray))
                {
                    float dist = MathEx.DistanceSquared(ref  farObjects[i].BoundingSphere.Center, ref ray.Position);
                    if (dist < nearest)
                    {
                        nearest = dist;
                        result = farObjects[i];
                    }
                }
            }
            return result;
        }

        public override SceneObject FindObject(Ray ray, IObjectFilter cbk)
        {
            SceneObject result = null;
            float nearest = float.MaxValue;
            if (queue.Count == 0)
            {
                queue.Enqueue(octRootNode);
                while (queue.Count > 0)
                {
                    OctreeSceneNode node = queue.Dequeue();

                    // if the node does't intersect the frustum we don't give a damn
                    if (MathEx.BoundingSphereIntersects(ref  node.BoundingSphere, ref ray))
                    {
                        for (int i = 0; i < 2; i++)
                            for (int j = 0; j < 2; j++)
                                for (int k = 0; k < 2; k++)
                                {
                                    if (node[i, j, k] != null)
                                    {
                                        queue.Enqueue(node[i, j, k]);
                                    }
                                }

                        for (int i = 0; i < node.AttchedObjects.Count; i++)
                        {
                            SceneObject curObj = node.AttchedObjects.Elements[i];

                            if (cbk.Check(curObj) &&
                                curObj.IntersectsSelectionRay(ref ray))// MathEx.BoundingSphereIntersects(ref curObj.BoundingSphere, ref ray))
                            {
                                float dist = MathEx.DistanceSquared(ref  curObj.BoundingSphere.Center, ref ray.Position);
                                if (dist < nearest)
                                {
                                    nearest = dist;
                                    result = curObj;
                                }
                            }
                        }
                    }

                }
            }
            else
            {
                throw new InvalidOperationException();
            }

            for (int i = 0; i < dynObjs.Count; i++)
            {
                if (cbk.Check(dynObjs[i]) && 
                    dynObjs[i].IntersectsSelectionRay(ref ray))
                {
                    float dist = MathEx.DistanceSquared(ref  dynObjs[i].BoundingSphere.Center, ref ray.Position);
                    if (dist < nearest)
                    {
                        nearest = dist;
                        result = dynObjs[i];
                    }
                }
            }
            for (int i = 0; i < farObjects.Count; i++)
            {
                if (cbk.Check(farObjects[i]) &&
                  farObjects[i].IntersectsSelectionRay(ref ray))
                {
                    float dist = MathEx.DistanceSquared(ref  farObjects[i].BoundingSphere.Center, ref ray.Position);
                    if (dist < nearest)
                    {
                        nearest = dist;
                        result = farObjects[i];
                    }
                }
            }
            return result;
        }

        public override void PrepareVisibleObjects(ICamera camera, PassData batchHelper)
        {
            batchHelper.visibleObjects.FastClear();

            Frustum frus = camera.Frustum;

            // do a BFS pass here

            queue.Enqueue(octRootNode);

            while (queue.Count > 0)
            {
                OctreeSceneNode node = queue.Dequeue();

                Vector3 center = node.BoundingSphere.Center;

                // if the node does't intersect the frustum we don't give a damn
                if (frus.IntersectsSphere(ref center, node.BoundingSphere.Radius))
                {
                    for (int i = 0; i < 2; i++)
                        for (int j = 0; j < 2; j++)
                            for (int k = 0; k < 2; k++)
                            {
                                if (node[i, j, k] != null)
                                {
                                    queue.Enqueue(node[i, j, k]);
                                }
                            }
                    FastList<SceneObject> objs = node.AttchedObjects;
                    for (int i = 0; i < objs.Count; i++)
                    {
                        if (objs.Elements[i].HasSubObjects)
                        {
                            objs.Elements[i].PrepareVisibleObjects(camera, 0);
                        }
                        AddVisibleObject(objs.Elements[i], 0, batchHelper);
                    }
                }

            }

            for (int i = 0; i < farObjects.Count; i++)
            {
                AddVisibleObject(farObjects[i], 0, batchHelper);
            }
            //base.PrepareVisibleObjects(camera);
        }

        public override void Update(GameTime dt)
        {
            for (int i = 0; i < SceneObjects.Count; i++)
            {
                SceneObjects[i].Update(dt);

                if (SceneObjects[i].RequiresUpdate)
                {
                    octRootNode.RemoveObject(SceneObjects[i]);
                    AddObject(SceneObjects[i]);
                }
            }

            for (int i = 0; i < dynObjs.Count; i++)
            {
                DynamicObject obj = dynObjs[i];

                octRootNode.RemoveObject(obj);

                AddObject(obj);
            }
        }
    }
}
