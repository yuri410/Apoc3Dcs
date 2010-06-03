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
using System.Text;
using System.Threading;

namespace Apoc3D.Core
{
    public abstract class ResourceOperation
    {
        Resource resource;

        public Resource Resource
        {
            get { return resource; }
        }

        protected ResourceOperation(Resource res)
        {
            this.resource = res;

        }

        public abstract void Process();
    }

    /// <summary>
    ///  资源异步处理器
    /// </summary>
    class AsyncProcessor : IDisposable
    {
        Queue<ResourceOperation> opQueue;

        Thread processThread;

        object syncHelper = new object();

        AutoResetEvent waiter;

        public AsyncProcessor(string name)
        {
            opQueue = new Queue<ResourceOperation>();

            waiter = new AutoResetEvent(true);
            
            processThread = new Thread(Main);
            processThread.Name = "AsyncProcessor " + name;

#if !XBOX
            processThread.SetApartmentState(ApartmentState.MTA);
#endif
            processThread.IsBackground = true;
            processThread.Start();
        }

        [MTAThread()]
        private void Main()
        {
            while (!Disposed)
            {
                ResourceOperation resOp = null;

                lock (syncHelper)
                {
                    if (opQueue.Count > 0)
                    {
                        waiter.Reset();
                        resOp = opQueue.Dequeue();
                    }
                }

                if (resOp != null)
                {
                    resOp.Process();
                }
                else
                {
                    waiter.Set();
                    Thread.Sleep(10);
                }
            }
        }

        public void AddTask(ResourceOperation op)
        {
            lock (syncHelper)
            {
                opQueue.Enqueue(op);
            }
        }

        public bool TaskCompleted
        {
            get
            {
                lock (syncHelper)
                {
                    return opQueue.Count == 0;
                }
            }
        }
        public int GetOperationCount()
        {
            lock (syncHelper)
            {
                return opQueue.Count;
            }
        }

        public void WaitForCompletion()
        {
            waiter.WaitOne();
        }

        #region IDisposable 成员

        public bool Disposed
        {
            get;
            private set;
        }

        public void Dispose()
        {
            Disposed = true;
        }

        #endregion
    }
}
