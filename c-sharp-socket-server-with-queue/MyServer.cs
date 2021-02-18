// using c_sharp_socket_client;
using c_sharp_socket_server_with_queue;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace c_sharp_socket_server_with_queue
{
    public class MYSynchronousSocketListener
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public void StartListening()
        {

            ClientService ClientTask;

            // Client Connections Pool
            ClientConnectionPool ConnectionPool = new ClientConnectionPool();

            // Client Task to handle client requests
            ClientTask = new ClientService(ConnectionPool);

            ClientTask.Start();

            // return;
           
            // while (Program.ServerForm.DoExit == false)
            {
                var users = FakeData(1);
                foreach (var u in users)
                {
                    ConnectionPool.Enqueue(new MyClientHandler(u));
                    Program.ServerForm.incrementNbReceive(); // +1 
                }
            }
                
             ClientTask.Stop();           
        }

        static List<User> FakeData(int nb)
        {
            FakeData fake = new FakeData();
            return fake.Data(nb);
        }

    }

    class MyClientHandler : ClientHandler
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        
        bool ContinueProcess = false;
        User receivedData;

        public MyClientHandler(User usr)
        {
            receivedData = usr;
            ContinueProcess = true;
        }

        override public void Process()
        {
            ProcessDataReceived();
        }  // Process()

        override protected void ProcessDataReceived()
        {
            ContinueProcess = true;

            log.Info(receivedData);

            // System.Threading.Thread.Sleep(3000);

            ContinueProcess = false;
        }

        override public void Close()
        {
           
        }

        override public bool IsAlive()
        {
            return ContinueProcess;
        }

    } // class ClientHandler 

}