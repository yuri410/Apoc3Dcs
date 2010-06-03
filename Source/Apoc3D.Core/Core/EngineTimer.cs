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
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Diagnostics;

namespace Apoc3D.Core
{
    public static class EngineTimer
    {
        class Clock
        {
            long baseRealTime;
            long lastRealTime;
            bool lastRealTimeValid;
            int suspendCount;
            long suspendStartTime;
            long timeLostToSuspension;
            TimeSpan currentTimeBase;
            TimeSpan currentTimeOffset;

            public TimeSpan CurrentTime
            {
                get { return currentTimeBase + currentTimeOffset; }
            }

            public TimeSpan ElapsedTime
            {
                get;
                private set;
            }

            public TimeSpan ElapsedAdjustedTime
            {
                get;
                private set;
            }

            public static long Frequency
            {
                get { return Stopwatch.Frequency; }
            }

            public Clock()
            {
                Reset();
            }

            public void Reset()
            {
                currentTimeBase = TimeSpan.Zero;
                currentTimeOffset = TimeSpan.Zero;
                baseRealTime = Stopwatch.GetTimestamp();
                lastRealTimeValid = false;
            }

            public void Suspend()
            {
                suspendCount++;
                if (suspendCount == 1)
                    suspendStartTime = Stopwatch.GetTimestamp();
            }

            /// <summary>
            /// Resumes a previously suspended clock.
            /// </summary>
            public void Resume()
            {
                suspendCount--;
                if (suspendCount <= 0)
                {
                    timeLostToSuspension += Stopwatch.GetTimestamp() - suspendStartTime;
                    suspendStartTime = 0;
                }
            }

            public void Step()
            {
                long counter = Stopwatch.GetTimestamp();

                if (!lastRealTimeValid)
                {
                    lastRealTime = counter;
                    lastRealTimeValid = true;
                }

                try
                {
                    currentTimeOffset = CounterToTimeSpan(counter - baseRealTime);
                }
                catch (OverflowException)
                {
                    // update the base value and try again to adjust for overflow
                    currentTimeBase += currentTimeOffset;
                    baseRealTime = lastRealTime;

                    try
                    {
                        // get the current offset
                        currentTimeOffset = CounterToTimeSpan(counter - baseRealTime);
                    }
                    catch (OverflowException)
                    {
                        // account for overflow
                        baseRealTime = counter;
                        currentTimeOffset = TimeSpan.Zero;
                    }
                }

                try
                {
                    ElapsedTime = CounterToTimeSpan(counter - lastRealTime);
                }
                catch (OverflowException)
                {
                    ElapsedTime = TimeSpan.Zero;
                }

                try
                {
                    ElapsedAdjustedTime = CounterToTimeSpan(counter - (lastRealTime + timeLostToSuspension));
                    timeLostToSuspension = 0;
                }
                catch (OverflowException)
                {
                    ElapsedAdjustedTime = TimeSpan.Zero;
                }

                lastRealTime = counter;
            }

            static TimeSpan CounterToTimeSpan(long delta)
            {
                return TimeSpan.FromTicks((delta * 10000000) / Frequency);
            }
        }

        static Clock clock;

        static TimeSpan startTime;
        static TimeSpan curTime;

        static object syncHelper = new object();
        static Thread thread;

        static EngineTimer()
        {
            clock = new Clock();
            startTime = clock.CurrentTime;

            thread = new Thread(Update);
            thread.Name = "Timer Auto Update";
            thread.IsBackground = true;
#if !XBOX
            thread.SetApartmentState(ApartmentState.MTA);
#endif
            //thread.Priority = ThreadPriority.AboveNormal;
            thread.Start();
        }

        #region 属性


        public static TimeSpan Time
        {
            get
            {
                lock (syncHelper)
                {
                    return curTime;
                }
            }
        }

        #endregion

        #region 方法
        private static void Update()
        {
            while (!Disposed)
            {
                clock.Step();

                lock (syncHelper)
                {
                    curTime = clock.CurrentTime;
                }
                Thread.Sleep(15);
            }
        }

        #endregion

        public static bool Disposed
        {
            get;
            private set;
        }

        public static void Dispose() 
        {
            if (!Disposed)
            {
                Disposed = true;
                const int MaxWait = 10;

                for (int i = 0; i < MaxWait; i++)
                {
                    if (thread.IsAlive)
                        Thread.Sleep(10);
                }
                thread.Abort();

            }
        }
    }
}
