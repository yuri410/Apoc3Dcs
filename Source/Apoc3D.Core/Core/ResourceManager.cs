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
using Apoc3D.Vfs;

namespace Apoc3D.Core
{
    /// <summary>
    ///  定义一个资源管理器，可以动态的进行资源加载与释放
    /// </summary>
    public abstract class ResourceManager : IDisposable
    {
        /// <summary>
        ///  受管理的资源哈希表
        /// </summary>
        Dictionary<string, Resource> hashTable;

        /// <summary>
        ///  受管理的资源的列表
        /// </summary>
        GenerationTable genTable;

        /// <summary>
        ///  最大允许的缓存大小
        /// </summary>
        int totalCacheSize;

        /// <summary>
        ///  已经使用的缓存大小
        /// </summary>
        int curUsedCache;

        
        AsyncProcessor asyncProc;

        /// <summary>
        ///  以默认缓存大小创建一个资源管理器
        /// </summary>
        protected ResourceManager()
            : this(8 * 1048576)
        {
        }

        /// <summary>
        ///  以特定缓存大小创建一个资源管理器
        /// </summary>
        /// <param name="cacheSize">缓存大小</param>
        protected ResourceManager(int cacheSize)
        {
            asyncProc = new AsyncProcessor(this.GetType().Name);
            hashTable = new Dictionary<string, Resource>();
            totalCacheSize = cacheSize;
            genTable = new GenerationTable(this);
        }

        internal GenerationTable Table
        {
            get { return genTable; }
        }



        /// <summary>
        ///  获取或设置最大允许的缓存大小
        /// </summary>
        public int TotalCacheSize
        {
            get { return totalCacheSize; }
            set
            {
                if (value < totalCacheSize)
                {
                    totalCacheSize = value;
                }
                else
                {
                    totalCacheSize = value;
                }
            }
        }

        /// <summary>
        ///  获取已经使用的缓存大小
        /// </summary>
        public int UsedCacheSize
        {
            get { return curUsedCache; }
        }

        /// <summary>
        ///  提示资源管理器一个资源已经加载
        /// </summary>
        /// <param name="res">资源</param>
        public void NotifyResourceLoaded(Resource res)
        {
            curUsedCache += res.GetSize();
        }

        /// <summary>
        ///  提示资源管理器一个资源已经释放
        /// </summary>
        /// <param name="res">资源</param>
        public void NotifyResourceUnloaded(Resource res)
        {
            curUsedCache -= res.GetSize();
        }

        /// <summary>
        ///  提示资源管理器一个资源已经销毁
        /// </summary>
        /// <param name="res">资源</param>
        public void NotifyResourceFinalizing(Resource res)
        {
            if (!Disposed)
            {
                hashTable.Remove(res.HashString);
                genTable.RemoveResource(res);
            }
        }


        internal void AddTask(ResourceOperation op) 
        {
            asyncProc.AddTask(op);
        }
        public bool IsIdle
        {
            get
            {
                return asyncProc.TaskCompleted;
            }
        }
        public void WaitForIdle()
        {
            asyncProc.WaitForCompletion();
        }
        public int GetCurrentOperationCount()
        {
            return asyncProc.GetOperationCount();
        }

        /// <summary>
        ///  提示资源管理器已经创建了一个新资源，将它放入管理范围
        /// </summary>
        /// <param name="res"></param>
        protected void NotifyResourceNew(Resource res)
        {
            if (!Disposed)
            {
                hashTable.Add(res.HashString, res);
                genTable.AddResource(res);
            }
        }

        /// <summary>
        ///  检查一个资源
        /// </summary>
        /// <param name="hashcode">资源的哈希代码</param>
        /// <returns>如果找到，则返回该资源。否则返回null</returns>
        public Resource Exists(string hashString)
        {
            Resource res;
            if (hashTable.TryGetValue(hashString, out res))
            {
                return res;
            }
            return null;
        }

        #region IDisposable 成员

        public bool Disposed
        {
            get;
            private set;
        }

        public void Dispose()
        {
            if (!Disposed)
            {
                if (asyncProc != null &&
                    !asyncProc.Disposed)
                {
                    asyncProc.Dispose();
                    asyncProc = null;
                }

                if (genTable != null &&
                    !genTable.Disposed)
                {
                    genTable.Dispose();
                    genTable = null;
                }
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
