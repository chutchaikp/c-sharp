using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace c_sharp_queue
{

    class MessagePool
    {
        // Creates a synchronized wrapper around the Queue.
        private Queue SyncdQ = Queue.Synchronized(new Queue());

        public void Enqueue(MessageHandler client)
        {
            SyncdQ.Enqueue(client);
        }

        public MessageHandler Dequeue()
        {
            return (MessageHandler)(SyncdQ.Dequeue());
        }

        public int Count {
            get { return SyncdQ.Count; }
        }

        public object SyncRoot {
            get { return SyncdQ.SyncRoot; }
        }

    } // class ClientConnectionPool

    class MessageService
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        const int NUM_OF_THREAD = 10; // 

        private MessagePool messagePool;
        private bool ContinueProcess = false;
        private Thread[] ThreadTask = new Thread[NUM_OF_THREAD];

        public MessageService(MessagePool messagePool)
        {
            this.messagePool = messagePool;
        }

        public void Start()
        {
            ContinueProcess = true;
            // Start threads to handle Client Task
            for (int i = 0; i < ThreadTask.Length; i++)
            {
                ThreadTask[i] = new Thread(new ThreadStart(this.Process));
                ThreadTask[i].Start();
            }
        }

        private void Process()
        {
            while (ContinueProcess)
            {
                try
                {
                    MessageHandler message = null;
                    lock (messagePool.SyncRoot)
                    {
                        if (messagePool.Count > 0)
                        {
                            message = (MessageHandler)messagePool.Dequeue();
                        }
                    }
                    // Program.mainForm.updateNbInQueue(messagePool.Count);
                    // log.Info("Message in Queue = " + messagePool.Count );

                    if (message != null)
                    {
                        message.Process(); // Provoke client
                                           // if client still connect, schedul for later processingle it 
                        if (message.IsAlive())
                        {
                            messagePool.Enqueue(message);
                        }
                    }
                    // Cause App running slowly // Thread.Sleep(5000);
                    Thread.Sleep(100);
                }
                catch (Exception error)
                {
                    log.Error(error);
                }
            }
        }

        public void Stop()
        {
            try
            {
                ContinueProcess = false;
                for (int i = 0; i < ThreadTask.Length; i++)
                {
                    if (ThreadTask[i] != null && ThreadTask[i].IsAlive)
                        ThreadTask[i].Abort();
                }

                // Close all client connections
                while (messagePool.Count > 0)
                {
                    MessageHandler client = null;
                    client = (MessageHandler)messagePool.Dequeue();
                    client.Close();
                }
            }
            catch (Exception err)
            {
                Console.WriteLine(err);
            }
        }
    } // class ClientService


    abstract class MessageHandler
    {
        abstract public void Process();
        abstract protected void ProcessDataReceived();

        abstract public void Close();

        abstract public bool IsAlive();

    } // class ClientHandler 


}
