using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Apoc3D.Collections;

namespace Apoc3D.Core
{
    /// <summary>
    ///  资源分代管理，代数越小，资源使用越频繁
    /// </summary>
    class GenerationTable : IDisposable
    {
        class RefEqualityComparer : IEqualityComparer<Resource>
        {
            #region IEqualityComparer<Resource> 成员

            public bool Equals(Resource x, Resource y)
            {
                return object.ReferenceEquals(x, y);
            }

            public int GetHashCode(Resource obj)
            {
                return obj.GetHashCode();
            }

            #endregion
        }

        public const int MaxGeneration = 4;

        internal static readonly float[] GenerationLifeTime = new float[MaxGeneration] { 3, 6, 10, 30 };

        class STMethod : IDisposable
        {
            GenerationTable table;
            Thread guThread;
            ResourceManager manager;

            public STMethod(GenerationTable table, ResourceManager manager) 
            {
                this.table = table;

                this.manager = manager;

                this.guThread = new Thread(GenerationUpdate_Main);
                this.guThread.Name = "Generation Update Thread for" + manager.ToString();
#if !XBOX
                this.guThread.SetApartmentState(ApartmentState.MTA);
#endif
                this.guThread.IsBackground = true;
                this.guThread.Start();

            }


            void SubTask_GenUpdate()
            {
                const int passTimeLimit = 4000;

                TimeSpan timeStart = EngineTimer.Time;
                TimeSpan time = EngineTimer.Time;

                int count;
                lock (table.syncHelper2)
                {
                    count = table.genList.Count;
                }
                if (count > 0)
                {
                    int loopCount = 0;

                    int remainingTime = passTimeLimit;
                    int perObjTime = passTimeLimit / count;
                    int actlObjTime = Math.Max(1, Math.Min(perObjTime, 10));

                    for (int j = 0; j < count; j++)
                    {
                        Resource res;

                        lock (table.syncHelper2)
                        {
                            count = table.genList.Count;
                            if (j < count)
                                res = table.genList[j];
                            else break;
                        }

                        if (res.generation.GenerationOutOfTime(ref timeStart))
                        {
                            int og = res.generation.generation;

                            res.generation.UpdateGeneration();
                            int ng = res.generation.generation;

                            if (ng != og)
                            {
                                table.UpdateGeneration(og, ng, res);
                                //Console.WriteLine("{0}由{1}改变至{2}", res.HashString, og.ToString(), ng.ToString());
                                //EngineConsole.Instance.Write("资源{0}由{1}改变至{2}", res.HashString, og.ToString(), ng.ToString());
                            }
                        }

                        if (loopCount++ % 10 == 0)
                        {
                            time = EngineTimer.Time;
                            remainingTime -= (int)(time - timeStart).TotalMilliseconds;

                            loopCount = 0;
                        }

                        if (perObjTime >= 1 && remainingTime > 0)
                        {
                            Thread.Sleep(actlObjTime);
                        }
                    }
                }

            }
            void SubTask_Manage()
            {
                int predictCSize = manager.UsedCacheSize;

                if (predictCSize > manager.TotalCacheSize)
                {
                    for (int i = 3; i >= 1 && predictCSize > manager.TotalCacheSize; i--)
                    {
                        foreach (Resource r in table.gen[i])
                        {
                            if (r != null && r.State == ResourceState.Loaded && r.IsUnloadable)
                            {
                                predictCSize -= r.GetSize();
                                r.Unload();

                                if (predictCSize <= manager.TotalCacheSize)
                                {
                                    break;
                                }
                            }
                        }
                    }
                }
            }

            /// <summary>
            ///  更新代数
            ///  会根据代的时间长度，对于不同代的资源，隔一段时间检查代数，更新
            /// </summary>
            [MTAThread()]
            private void GenerationUpdate_Main()
            {
                const int ManageInterval = 10;

                int times = 0;
                while (!Disposed)
                {
                    SubTask_GenUpdate();

                    if (times++ % ManageInterval == 0)
                    {
                        SubTask_Manage();
                    }

                    Thread.Sleep(100);
                }
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
                    Disposed = true;

#if !XBOX
                    const int MaxWait = 10;
                    for (int i = 0; i < MaxWait; i++) 
                    {
                        if (guThread.IsAlive) 
                        {
                            Thread.Sleep(10);
                        }
                    }
                    if (guThread.IsAlive)
#endif
                        guThread.Abort();
                }
            }

            #endregion

        }
        class MTMethod : IDisposable
        {
            enum ManageState
            {
                Off,
                RequiresSynchronize,
                Ready,
            }
            ManageState manageState;
            GenerationTable table;
            ResourceManager manager;

            Thread guThread;
            Thread mgrThread;

            /// <summary>
            ///  代数计算线程与管理线程同步，锁定manageState用
            /// </summary>
            object syncHelper3 = new object();

            ManageState State
            {
                get
                {
                    lock (syncHelper3)
                        return manageState;
                }
                set
                {
                    lock (syncHelper3)
                        manageState = value;
                }
            }

            public MTMethod(GenerationTable table, ResourceManager manager)
            {
                this.table = table;
                this.manager = manager;

                this.guThread = new Thread(GenerationUpdate_Main);
                guThread.Name = "Generation Update Thread for" + manager.ToString();
#if !XBOX
                guThread.SetApartmentState(ApartmentState.MTA);
#endif
                guThread.IsBackground = true;
                guThread.Start();

                this.mgrThread = new Thread(Manage_Main);
                mgrThread.Name = "Resource Management Thread for" + manager.ToString();
#if !XBOX
                mgrThread.IsBackground = true;
                mgrThread.SetApartmentState(ApartmentState.MTA);
#endif
                mgrThread.Start();
            }

            /// <summary>
            ///  更新代数
            ///  会根据代的时间长度，对于不同代的资源，隔一段时间检查代数，更新
            /// </summary>
            [MTAThread()]
            private void GenerationUpdate_Main()
            {
                const int passTimeLimit = 4000;

                while (!Disposed)
                {
                    bool isSyncing;

                    lock (syncHelper3)
                    {
                        isSyncing = manageState == ManageState.RequiresSynchronize;
                    }

                    TimeSpan timeStart = EngineTimer.Time;
                    TimeSpan time = EngineTimer.Time;

                    int count;
                    lock (table.syncHelper2)
                    {
                        count = table.genList.Count;
                    }

                    if (count > 0)
                    {
                        int loopCount = 0;

                        int remainingTime = passTimeLimit;
                        int perObjTime = passTimeLimit / count;
                        int actlObjTime = Math.Max(1, Math.Min(perObjTime, 10));

                        for (int j = 0; j < count; j++)
                        {
                            Resource res;

                            lock (table.syncHelper2)
                            {
                                res = table.genList[j];
                            }

                            if (res.generation.GenerationOutOfTime(ref timeStart))
                            {
                                int og = res.generation.generation;

                                res.generation.UpdateGeneration();
                                int ng = res.generation.generation;

                                if (ng != og)
                                {
                                    table.UpdateGeneration(og, ng, res);
                                    //Console.WriteLine("{0}由{1}改变至{2}", res.HashString, og.ToString(), ng.ToString());
                                    //EngineConsole.Instance.Write("资源{0}由{1}改变至{2}", res.HashString, og.ToString(), ng.ToString());
                                }
                            }

                            if (loopCount++ % 10 == 0)
                            {
                                time = EngineTimer.Time;
                                remainingTime -= (int)(time - timeStart).TotalMilliseconds;

                                loopCount = 0;
                            }

                            if (perObjTime >= 1 && remainingTime > 0)
                            {
                                Thread.Sleep(actlObjTime);
                            }
                        }
                    }

                    if (isSyncing)
                    {
                        lock (syncHelper3)
                        {
                            manageState = ManageState.Ready;
                        }
                    }

                    Thread.Sleep(100);
                }
            }

            /// <summary>
            ///  负责释放资源
            /// </summary>
            [MTAThread()]
            private void Manage_Main()
            {
                while (!Disposed)
                {
                    bool flag;

                    lock (syncHelper3)
                    {
                        flag = manageState == ManageState.Ready;

                        if (flag)
                        {
                            manageState = ManageState.Off;
                        }
                    }

                    if (flag)
                    {
                        int predictCSize = manager.UsedCacheSize;

                        if (predictCSize > manager.TotalCacheSize)
                        {
                            for (int i = 3; i >= 1 && predictCSize > manager.TotalCacheSize; i--)
                            {
                                foreach (Resource r in table.gen[i])
                                {
                                    if (r != null && r.State == ResourceState.Loaded && r.IsUnloadable)
                                    {
                                        predictCSize -= r.GetSize();
                                        r.Unload();

                                        if (predictCSize <= manager.TotalCacheSize)
                                        {
                                            break;
                                        }
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        if (manager.UsedCacheSize > manager.TotalCacheSize)
                        {
                            State = ManageState.RequiresSynchronize;
                        }
                    }

                    Thread.Sleep(1000);
                }
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
                    Disposed = true;
#if !XBOX

                    const int MaxWait = 10;
                    for (int i = 0; i < MaxWait; i++)
                    {
                        if (guThread.IsAlive)
                        {
                            Thread.Sleep(10);
                        }
                    }
                    if (guThread.IsAlive)
#endif
                        guThread.Abort();
#if !XBOX
                    for (int i = 0; i < MaxWait; i++)
                    {
                        if (mgrThread.IsAlive)
                        {
                            Thread.Sleep(10);
                        }
                    }
                    if (mgrThread.IsAlive)
#endif
                        mgrThread.Abort();
                }
            }

            #endregion
        }
        

        /// <summary>
        ///  对gen的线程锁
        /// </summary>
        object syncHelper = new object();
        /// <summary>
        ///  对genList的线程锁
        /// </summary>
        object syncHelper2 = new object();


        volatile ExistTable<Resource>[] gen;
        volatile List<Resource> genList;

        IDisposable manageThread;

        public GenerationTable(ResourceManager mgr)
        {
            gen = new ExistTable<Resource>[MaxGeneration];
            genList = new List<Resource>();
            for (int i = 0; i < MaxGeneration; i++)
            {
                gen[i] = new ExistTable<Resource>();
            }

            manageThread = new STMethod(this, mgr);
        }

        public ExistTable<Resource> this[int index]
        {
            get
            {
                return gen[index];
            }
        }

        public void AddResource(Resource res)
        {
            int g = res.Generation;
            if (g != -1)
            {
                lock (syncHelper)
                {
                    gen[g].Add(res);
                }
                lock (syncHelper2)
                {
                    genList.Add(res);
                }
            }
        }
        public void RemoveResource(Resource res)
        {
            int g = res.Generation;

            if (g != -1)
            {
                lock (syncHelper)
                {
                    gen[g].Remove(res);
                }
                lock (syncHelper2)
                {
                    genList.Remove(res);
                }
            }
        }

        internal void UpdateGeneration(int oldGeneration, int newGeneration, Resource resource)
        {
            lock (syncHelper)
            {
                if (oldGeneration != -1 && gen[oldGeneration].Exists(resource))
                    gen[oldGeneration].Remove(resource);

                if (!gen[newGeneration].Exists(resource))
                    gen[newGeneration].Add(resource);
            }
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
                manageThread.Dispose();
                Disposed = true;
            }
        }

        #endregion
    }

}
