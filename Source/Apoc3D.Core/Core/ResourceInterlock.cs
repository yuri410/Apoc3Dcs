using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Apoc3D.Core
{
    public static class ResourceInterlock
    {
        static Dictionary<Thread, object> syncHelperTable = new Dictionary<Thread, object>();

        public static void EnterAtomicOp()
        {
            Thread thread = Thread.CurrentThread;

            object syncHelper;

            if (!syncHelperTable.TryGetValue(thread, out syncHelper))
            {
                syncHelper = new object();
                syncHelperTable.Add(thread, syncHelper);
            }

            Monitor.Enter(syncHelper);
        }
        public static void ExitAtomicOp() 
        {
            Thread thread = Thread.CurrentThread;

            object syncHelper;

            if (!syncHelperTable.TryGetValue(thread, out syncHelper))
            {
                throw new InvalidOperationException();
            }

            Monitor.Exit(syncHelper);
        }

        public static void BlockAll()
        {
            Dictionary<Thread, object>.ValueCollection value = syncHelperTable.Values;

            foreach (object obj in value)
            {
                Monitor.Enter(obj);
            }
        }
        public static void UnblockAll() 
        {
            Dictionary<Thread, object>.ValueCollection value = syncHelperTable.Values;

            foreach (object obj in value)
            {
                Monitor.Exit(obj);
            }
        }
    }
}
