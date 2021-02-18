using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace c_sharp_socket_server_with_queue
{
    class ClientConnectionPool
    {
        // Creates a synchronized wrapper around the Queue.
        private Queue SyncdQ = Queue.Synchronized(new Queue());

        public void Enqueue(ClientHandler client)
        {
            SyncdQ.Enqueue(client);
        }

        public ClientHandler Dequeue()
        {
            return (ClientHandler)(SyncdQ.Dequeue());
        }

        public int Count {
            get { return SyncdQ.Count; }
        }

        public object SyncRoot {
            get { return SyncdQ.SyncRoot; }
        }

    } // class ClientConnectionPool

    class ClientService
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        const int NUM_OF_THREAD = 1; // 

        private ClientConnectionPool ConnectionPool;
        private bool ContinueProcess = false;
        private Thread[] ThreadTask = new Thread[NUM_OF_THREAD];

        public ClientService(ClientConnectionPool ConnectionPool)
        {
            this.ConnectionPool = ConnectionPool;
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
            try
            {
                while (ContinueProcess)
                {

                    ClientHandler client = null;
                    lock (ConnectionPool.SyncRoot)
                    {
                        if (ConnectionPool.Count > 0)
                        {
                            if (Program.PROTOCOL == "TCP")
                            {
                                client = (TCPClientHandler)ConnectionPool.Dequeue();
                            }
                            if (Program.PROTOCOL == "UDP")
                            {
                                client = (UDPClientHandler)ConnectionPool.Dequeue();
                            }
                        }
                    }

                    if (client != null)
                    {
                        client.Process(); // Provoke client
                                          // if client still connect, schedufor later processingle it 
                        if (client.IsAlive())
                        {
                            ConnectionPool.Enqueue(client);
                        }
                    }
                    
                    Thread.Sleep(100);                    
                }
            }
            catch (Exception _error)
            {
                log.Error(_error);
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
                while (ConnectionPool.Count > 0)
                {
                    ClientHandler client = null;
                    if (Program.PROTOCOL == "TCP")
                    {
                        client = (TCPClientHandler)ConnectionPool.Dequeue();
                    }
                    if (Program.PROTOCOL == "UDP")
                    {
                        client = (UDPClientHandler)ConnectionPool.Dequeue();
                    }
                    client.Close();
                    log.Debug("Client connection is closed!");
                }
            }
            catch (Exception err)
            {
                Console.WriteLine(err);
            }
        }
    } // class ClientService


    abstract class ClientHandler
    {
        abstract public void Process();
        abstract protected void ProcessDataReceived();

        abstract public void Close();

        abstract public bool IsAlive();

    } // class ClientHandler 


}
