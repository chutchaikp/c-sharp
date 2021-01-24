using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace c_sharp_socket_server_with_queue
{
    public class ThreadRecord
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        int myParam;
        ArrayList Buffer;

        public ThreadRecord()
        {
            this.myParam = 0;
            this.Buffer = new ArrayList();
        }

        public ThreadRecord(ref ArrayList buffer)
        {
            this.Buffer = buffer;
            this.myParam = 0;
        }

        public void SetBuffer(ArrayList buffer)
        {
            this.Buffer = buffer;
        }

        public void ThreadLoop()
        {
            while (Thread.CurrentThread.IsAlive && (Program.ServerForm.DoExit == false))
            {
                if (Buffer != null && Buffer.Count > 0)
                {
                    int nbData = Math.Min(1000, Buffer.Count);
                    for (int i = 0; i < nbData; i++)
                    {
                        var evtMsg = Buffer[i];
                        if (evtMsg != null)
                        {
                            var msg = evtMsg.ToString().Replace("\0", "").Trim();
                            log.Info(msg);
                        }
                    }
                }
            }
        }


    }
}
