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
using Apoc3D.Collections;
using Apoc3D.Design;
using Apoc3D.Graphics;
using Apoc3D.MathLib;
using Apoc3D.Vfs;

namespace Apoc3D.Scene
{
    /// <summary>
    /// 场景中的对象
    /// </summary>
    public abstract class SceneObject : IRenderable, IUpdatable, IDisposable
    {
        #region 物理相关

        //Body rigidBody;

        ///// <summary>
        /////  获取该物体在物理引擎中的的刚体对象
        ///// </summary>
        //[Browsable(false)]
        //public Body RigidBody
        //{
        //    get { return rigidBody; }
        //    protected set { rigidBody = value; }
        //}

        /// <summary>
        ///  获取一个布尔值，表示该物体是否有物理方面的资源需要创建。
        ///  当值为真时，引擎会调用BuildPhysicsModel方法
        /// </summary>
        [Browsable(false)]
        public virtual bool HasPhysicsModel
        {
            get { return false; }
        }

        ///// <summary>
        /////  加载该物体所有与物体有关的资源，当场景中所有物体均被加载后，引擎才会开始调用该方法。
        ///// </summary>
        ///// <param name="world"></param>
        //public virtual void BuildPhysicsModel(PhysicsSystem world)
        //{
        //    throw new NotSupportedException();
        //}

        //public CollisionSkin CollisionShape 
        //{
        //    get 
        //    {
        //        if (rigidBody == null) 
        //        {
        //            return null;
        //        }
        //        return rigidBody.CollisionSkin;
        //    }
        //}

        #endregion

        #region 字段

        /// <summary>
        ///  该物体的包围球
        /// </summary>
        public BoundingSphere BoundingSphere;

        /// <summary>
        ///  该物体在世界坐标系中的变换矩阵
        /// </summary>
        public Matrix Transformation = Matrix.Identity;

        #endregion

        #region 属性


        /// <summary>
        ///  获取或设置一个布尔值，表示该物体是否需要更新
        /// </summary>
        /// <remarks>当变换矩阵变化以后为true</remarks>
        [Browsable(false)]
        public bool RequiresUpdate
        {
            get;
            set;
        }

        /// <summary>
        ///  获取场景物体所在的场景管理器
        /// </summary>
        [Browsable(false)]
        public SceneManagerBase SceneManager
        {
            get;
            protected set;
        }

        /// <summary>
        ///  获取场景物体所在的Scene Node
        /// </summary>
        [Browsable(false)]
        public SceneNodeBase ParentSceneNode
        {
            get;
            protected internal set;
        }

        /// <summary>
        /// 获取该SceneObject是否包含子物体。当它为真时，渲染时会引擎回调用
        /// </summary>
        [Browsable(false)]
        public bool HasSubObjects
        {
            get;
            protected set;
        }

        #endregion

        #region 构造函数

        /// <summary>
        ///  SceneObject的构造函数。
        /// </summary>
        /// <param name="hasSubObject">表示该物体是否含有子物体，当为真时，引擎在渲染时会调用PrepareVisibleObjects</param>
        protected SceneObject(bool hasSubObject)
        {
            HasSubObjects = hasSubObject;
        }

        #endregion


        /// <summary>
        ///  如果该SceneObject包含子物体，准备摄像机的可见物体
        /// </summary>
        /// <param name="cam">
        ///  渲染到的摄像机
        /// </param>
        /// <param name="level">LOD级别</param>
        public virtual void PrepareVisibleObjects(ICamera cam, int level)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        ///  当物体加入到场景管理器时被调用
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="sceneMgr">加入到的场景管理器</param>
        public virtual void OnAddedToScene(object sender, SceneManagerBase sceneMgr) { }

        /// <summary>
        ///  当物体从场景管理器中移除时被调用
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="sceneMgr">源场景管理器</param>
        public virtual void OnRemovedFromScene(object sender, SceneManagerBase sceneMgr) { }


        /// <summary>
        ///  计算物体是否与拾取射线相交
        /// </summary>
        /// <param name="ray"></param>
        /// <returns></returns>
        public virtual bool IntersectsSelectionRay(ref Ray ray)
        {
            return MathEx.BoundingSphereIntersects(ref BoundingSphere, ref ray);
        }


        #region IRenderable 成员

        /// <summary>
        ///  获取该物体的所有渲染操作
        /// </summary>
        /// <returns></returns>
        public abstract RenderOperation[] GetRenderOperation();

        /// <summary>
        ///  获取该物体特定LOD级别的所有渲染操作
        /// </summary>
        /// <returns></returns>
        public virtual RenderOperation[] GetRenderOperation(int level) 
        {
            return GetRenderOperation();
        }
        #endregion

        #region IUpdatable 成员

        /// <summary>
        ///  更新该物体的状态，每一帧如果可见，则被引擎调用
        /// </summary>
        /// <param name="dt">帧时间间隔，以秒为单位</param>
        public abstract void Update(GameTime dt);

        #endregion

        #region SceneObject 序列化

        /// <summary>
        ///  获取一个布尔值，表示该物体是否可序列化为二进制数据
        /// </summary>
        [Browsable(false)]
        public abstract bool IsSerializable
        {
            get;
        }

        /// <summary>
        ///  序列化这个物体，并将数据存入 二进制层次块data。
        /// </summary>
        /// <param name="data"></param>
        public virtual void Serialize(BinaryDataWriter data)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        ///  获取一个字符串，表示该物体的类型。
        /// </summary>
        [Browsable(false)]
        public virtual string TypeTag
        {
            get { return "Unknown"; }
        }
        #endregion

        #region IDisposable 成员

        /// <summary>
        ///  释放场景物体所使用的所有资源。应在派生类中重写。
        /// </summary>
        /// <param name="disposing">表示是否需要释放该物体所引用的其他资源，当为真时，调用那些资源的Dispose方法</param>
        protected virtual void Dispose(bool disposing)
        {

        }

        /// <summary>
        ///  获取一个布尔值，表示该场景物体是否已经释放了资源。
        /// </summary>
        [Browsable(false)]
        public bool Disposed
        {
            get;
            private set;
        }

        /// <summary>
        ///  释放场景物体所使用的所有资源。
        /// </summary>
        public void Dispose()
        {
            if (!Disposed)
            {
                Dispose(true);

                Disposed = true;
            }
            else
            {
                throw new ObjectDisposedException(ToString());
            }
        }

        #endregion
    }
}
