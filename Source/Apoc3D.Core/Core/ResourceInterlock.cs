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
    public static class ResourceInterlock
    {
        static volatile object syncHelper = new object();


        public static void EnterAtomicOp()
        {
            //Thread thread = Thread.CurrentThread;

            //object syncHelper;

            //if (!syncHelperTable.TryGetValue(thread, out syncHelper))
            //{
            //    syncHelper = new object();
            //    syncHelperTable.Add(thread, syncHelper);
            //}

            Monitor.Enter(syncHelper);
        }
        public static void ExitAtomicOp() 
        {
            //Thread thread = Thread.CurrentThread;

            //object syncHelper;

            //if (!syncHelperTable.TryGetValue(thread, out syncHelper))
            //{
            //    throw new InvalidOperationException();
            //}

            Monitor.Exit(syncHelper);
        }

        //static List<object> blocked = new List<object>();

        public static void BlockAll()
        {
            //Dictionary<Thread, object>.ValueCollection value = syncHelperTable.Values;

            //foreach (object obj in value)
            //{
            Monitor.Enter(syncHelper);
            //}
        }
        public static void UnblockAll()
        {
            //Dictionary<Thread, object>.ValueCollection value = syncHelperTable.Values;

            //foreach (object obj in value)
            //{
                Monitor.Exit(syncHelper);
            //}
        }
    }
}
