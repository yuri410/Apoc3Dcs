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
using System.IO;
using System.Text;
using Apoc3D.Collections;
using Apoc3D.Design;
using Apoc3D.Vfs;

namespace Apoc3D.Core
{
    /// <summary>
    ///  定义一个资源的状态
    /// </summary>
    public enum ResourceState
    {
        /// <summary>
        ///  未加载
        /// </summary>
        Unloaded,
        /// <summary>
        ///  正在加载
        /// </summary>
        Loading,
        /// <summary>
        ///  已经加载
        /// </summary>
        Loaded,
        /// <summary>
        ///  正在卸载
        /// </summary>
        Unloading
    }

    /// <summary>
    ///  表示对一个资源对象的引用
    /// </summary>
    /// <typeparam name="T">资源类型</typeparam>
    public class ResourceHandle<T> : IDisposable
        where T : Resource
    {
        T resuorce;
        object syncHelper = new object();
        bool dummy;

        public ResourceHandle(T res, bool dummy)
        {
            this.dummy = dummy;
            if (!dummy)
            {
                res.Reference();
            }
            this.resuorce = res;
        }

        public ResourceHandle(T res)
        {
            dummy = false;
            res.Reference();
            this.resuorce = res;
        }

        ~ResourceHandle()
        {
            if (!Disposed)
            {
                Dispose();
            }
        }

        /// <summary>
        ///  获取引用的资源，并激活资源
        /// </summary>
        public T Resource
        {
            get
            {
                if (!dummy)
                    resuorce.Use();
                return resuorce;
            }
        }
        public T GetWeakResource()
        {
            return resuorce;
        }

        /// <summary>
        ///  获取资源的状态。不会激活资源
        /// </summary>
        public ResourceState State 
        {
            get { return resuorce.State; }
        }
        /// <summary>
        ///  获取资源的代数。不会激活资源
        /// </summary>
        public int Generation 
        {
            get { return resuorce.Generation; }
        }

        public static implicit operator T(ResourceHandle<T> res)
        {
            if (res != null)
            {
                if (!res.dummy)
                    res.resuorce.Use();
                return res.resuorce;
            }
            return null;
        }

        public override string ToString()
        {
            return resuorce.ToString();
        }

        #region IDisposable 成员

        public bool Disposed
        {
            get;
            private set;
        }

        public void Dispose()
        {
            lock (syncHelper)
            {
                if (!Disposed)
                {
                    if (!dummy)
                        resuorce.Dereference();
                    Disposed = true;
                }
                //else
                //{
                //    throw new ObjectDisposedException(ToString());
                //}
            }
        }

        #endregion

        /// <summary>
        ///  强制激活资源，如果资源没有加载，则在后台加载
        /// </summary>
        public void Touch()
        {
            resuorce.Use();
        }
        /// <summary>
        ///  强制激活资源，如果资源没有加载，则同步加载
        /// </summary>
        public void TouchSync() 
        {
            resuorce.UseSync();
        }
    }

    /// <summary>
    ///  表示一个资源，如纹理、模型等
    /// </summary>
    public abstract class Resource : IDisposable
    {
        #region Resource Operations
        class ResourceLoader : ResourceOperation
        {
            public ResourceLoader(Resource resource)
                : base(resource)
            {
            }

            public override void Process()
            {
                if (Resource != null)
                {
                    Resource.load();
                    if (Resource.State == ResourceState.Loading)
                        Resource.State = ResourceState.Loaded;
                }
            }
        }
        class ResourceUnloader : ResourceOperation
        {
            public ResourceUnloader(Resource resource)
                : base(resource)
            {

            }

            public override void Process()
            {
                if (Resource != null)
                {
                    Resource.unload();
                    if (Resource.State == ResourceState.Unloading)
                        Resource.State = ResourceState.Unloaded;
                }
            }
        }

        #region Cache
        //class ResoueceCacheReader : ResourceOperation
        //{
        //    public ResoueceCacheReader(Resource resource)
        //        : base(resource)
        //    {

        //    }

        //    public override void Process()
        //    {
        //        if (Resource != null)
        //        {
        //            Stream stream = Resource.cacheMem.ResourceLocation.GetStream;
        //            Resource.ReadCacheData(new VirtualStream(stream));
        //            if (Resource.State == ResourceState.Loading)
        //                Resource.State = ResourceState.Loaded;
        //        }
        //    }
        //}
        //class ResoueceCacheWriter : ResourceOperation
        //{
        //    public ResoueceCacheWriter(Resource resource)
        //        : base(resource)
        //    {

        //    }

        //    public override void Process()
        //    {
        //        if (Resource != null)
        //        {
        //            Stream stream = Resource.cacheMem.ResourceLocation.GetStream;

        //            Resource.WriteCacheData(new VirtualStream(stream));
        //        }
        //    }
        //}
        #endregion
        #endregion

        /// <summary>
        ///  提供用于计算资源代数的功能
        /// </summary>
        internal class GenerationCalculator
        {
            object syncHelper = new object();

            internal volatile int generation;

            GenerationTable table;

            volatile FastQueue<float> timeQueue;

            public GenerationCalculator(GenerationTable table)
            {
                this.timeQueue = new FastQueue<float>();

                this.table = table;
                this.generation = GenerationTable.MaxGeneration - 1;
            }

            /// <summary>
            ///  获取资源的代数
            /// </summary>
            public int Generation
            {
                get
                {
                    return generation;
                }
            }

            /// <summary>
            ///  通知资源被使用
            /// </summary>
            public void Use(Resource resource)
            {
                TimeSpan time = EngineTimer.Time;

                lock (syncHelper)
                {
                    timeQueue.Enqueue((float)EngineTimer.Time.TotalSeconds);
                    while (timeQueue.Count > 5)
                        timeQueue.Dequeue();
                }
            }

            internal void UpdateGeneration()
            {
                float result = float.MinValue;

                lock (syncHelper)
                {
                    if (timeQueue.Count > 0)
                    {
                        result = 0;
                        for (int i = 0; i < timeQueue.Count; i++)
                        {
                            result += timeQueue.GetElement(i);
                        }
                        result /= timeQueue.Count;
                    }
                }
                result = (float)EngineTimer.Time.TotalSeconds - result;

                if (result > GenerationTable.GenerationLifeTime[0])
                {
                    if (result > GenerationTable.GenerationLifeTime[1])
                    {
                        if (result > GenerationTable.GenerationLifeTime[2])
                        {
                            generation = 3;
                        }
                        else
                        {
                            generation = 2;
                        }
                    }
                    else
                    {
                        generation = 1;
                    }
                }
                else
                {
                    generation = 0;
                }
            }

            public bool GenerationOutOfTime(ref TimeSpan time)
            {
                bool notEmpty;
                float topVal = 0;
                lock (syncHelper)
                {
                    notEmpty = (timeQueue.Count > 0);
                    if (notEmpty)
                        topVal = timeQueue.Tail();
                }

                if (notEmpty)
                {
                    float interval = (float)time.TotalSeconds - topVal;
                    if (generation < GenerationTable.MaxGeneration - 1 && interval > GenerationTable.GenerationLifeTime[generation])
                    {
                        return true;
                    }
                    return generation > 0 && interval <= GenerationTable.GenerationLifeTime[generation - 1];
                }
                return generation != GenerationTable.MaxGeneration - 1;
            }
        }

        internal GenerationCalculator generation;

        ResourceManager manager;

        string hashString;

        int refCount;

        ResourceState resState;

        ResourceLoader resourceLoader;
        ResourceUnloader resourceUnloader;
        object syncHelper = new object();


        /// <summary>
        /// 获取资源的引用计数
        /// </summary>
        [Browsable(false)]
        public int ReferenceCount
        {
            get { return refCount; }
        }

        internal void Reference()
        {
            if (IsManaged)
                refCount++;
        }
        internal void Dereference()
        {
            if (IsManaged)
            {
                refCount--;

                if (refCount == 0)
                {
                    if (!Disposed)
                        Dispose();
                }
            }
        }


        /// <summary>
        /// 所有资源的名称都统一用该方法计算哈希代码
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static int GetHashCode(string name)
        {
            return name.ToUpper().GetHashCode();
        }

        #region Cache
        //public bool HasCache
        //{
        //    get { return cacheMem.ResourceLocation != null; }
        //}

        //public virtual void SetCache(CacheMemory rl)
        //{
        //    Stream stm = rl.ResourceLocation.GetStream;
        //    WriteCacheData(new VirtualStream(stm));

        //    cacheMem = rl;
        //}

        //public CacheMemory GetCache()
        //{
        //    return cacheMem;
        //}

        //public virtual void ResetCache()
        //{
        //    cacheMem = new CacheMemory();
        //}

        //protected abstract void ReadCacheData(Stream stream);
        //protected abstract void WriteCacheData(Stream stream);
        #endregion

        /// <summary>
        ///  获取该资源的状态
        /// </summary>
        [BrowsableAttribute(false)]
        public ResourceState State
        {
            get
            {
                lock (syncHelper)
                    return resState;
            }
            private set
            {
                lock (syncHelper)
                    resState = value;
            }
        }

        public override int GetHashCode()
        {
            if (hashString != null)
            {
                return GetHashCode(hashString);
            }
            return base.GetHashCode();
        }

        [BrowsableAttribute(false)]
        public string HashString
        {
            get { return hashString; }
        }

        /// <summary>
        ///  创建一个不受管理的资源
        /// </summary>
        protected Resource()
        {
            resourceLoader = new ResourceLoader(this);
            resourceUnloader = new ResourceUnloader(this);
        }

        protected Resource(ResourceManager manager, string hashString)
            : this()
        {
            this.hashString = hashString;
            if (manager != null)
            {
                this.manager = manager;
                this.generation = new GenerationCalculator(manager.Table);
            }
        }


        public int Generation
        {
            get
            {
                if (generation == null)
                    return -1;
                return generation.Generation;
            }
        }

        /// <summary>
        ///  获得管理该资源的资源管理器
        /// </summary>

        [BrowsableAttribute(false)]
        public ResourceManager Manager
        {
            get { return manager; }
        }
        /// <summary>
        ///  获取该资源是否不受管理
        /// </summary>
        [BrowsableAttribute(false)]
        public bool IsManaged
        {
            get { return manager != null; }
        }


        /// <summary>
        ///  获取一个System.Boolean，表示该资源是否已经加载
        /// </summary>
        [BrowsableAttribute(false)]
        public bool IsLoaded
        {
            get { return resState == ResourceState.Loaded; }
        }


        /// <summary>
        ///  获取该资源所占内存的大小
        /// </summary>
        /// <returns></returns>
        public abstract int GetSize();

        /// <summary>
        ///  加载资源的实现
        /// </summary>
        protected abstract void load();

        /// <summary>
        ///  释放资源的实现
        /// </summary>
        protected abstract void unload();

        /// <summary>
        ///  获取一个System.Boolean，表示该资源是否可被释放
        /// </summary>
        [BrowsableAttribute(false)]
        public virtual bool IsUnloadable
        {
            get { return true; }
        }


        /// <summary>
        ///  调用该方法表示将要使用这个资源。让资源做好准备，已释放的要加载好。准备工作会在后台进行。
        /// </summary>
        public void Use()
        {
            if (IsManaged)
            {
                generation.Use(this);

                if (State == ResourceState.Unloaded)
                {
                    Load();
                }
            }
        }
        /// <summary>
        ///  Use()的同步版
        /// </summary>
        public void UseSync()
        {
            if (IsManaged)
            {
                generation.Use(this);

                if (State == ResourceState.Unloaded)
                {
                    LoadSync();
                }
            }
        }

        /// <summary>
        ///  同步加载资源
        /// </summary>
        void LoadSync() 
        {
            if (IsManaged)
            {
                generation.Use(this);

                State = ResourceState.Loading;

                load();

                manager.NotifyResourceLoaded(this);
                State = ResourceState.Loaded;
            }
        }
        /// <summary>
        ///  加载资源，在后台进行
        /// </summary>
        public void Load()
        {
            if (IsManaged)
            {
                generation.Use(this);

                State = ResourceState.Loading;

                manager.AddTask(resourceLoader);

                manager.NotifyResourceLoaded(this);
            }
        }

        /// <summary>
        ///  释放资源
        /// </summary>
        public void Unload()
        {
            if (IsManaged)
            {
                State = ResourceState.Unloading;

                manager.AddTask(resourceUnloader);

                manager.NotifyResourceUnloaded(this);
            }
        }
        public void Reload()
        {
            if (IsManaged)
            {
                if (IsLoaded)
                {
                    Unload();
                    Load();
                }
            }
        }


        /// <summary>
        ///  销毁资源
        /// </summary>
        /// <param name="disposing">是否释放</param>
        protected virtual void dispose(bool disposing)
        {
            if (disposing && IsManaged)
            {
                manager.NotifyResourceFinalizing(this);
            }
        }
        /// <summary>
        ///  获取一个System.Boolean，表示该资源是否已经销毁
        /// </summary>
        [BrowsableAttribute(false)]
        public bool Disposed
        {
            get;
            private set;
        }


        #region IDisposable 成员

        /// <summary>
        ///  销毁资源
        /// </summary>
        public void Dispose()
        {
            if (!Disposed)
            {
                dispose(true);
                Disposed = true;
            }
            else
            {
                throw new ObjectDisposedException(ToString());
            }
        }

        #endregion

        ~Resource()
        {
            if (!Disposed)
            {
                dispose(false);
                Disposed = true;
            }
        }

    }
}
