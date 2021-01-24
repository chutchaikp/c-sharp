using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace c_sharp_socket_server_with_queue
{
    public class UDPSynchronousSocketListener
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public void StartListening()
        {
            try
            {
                ClientService ClientTask;

                // Client Connections Pool
                ClientConnectionPool ConnectionPool = new ClientConnectionPool();

                // Client Task to handle client requests
                ClientTask = new ClientService(ConnectionPool);

                ClientTask.Start();

                IPAddress serverAddr = IPAddress.Parse(Program.SERVER_ADDRESS);
                int serverPort = Program.SERVER_PORT;

                Socket mySocketServerUdp = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
                IPEndPoint localIpEndPoint = new IPEndPoint(serverAddr, serverPort);

                try
                {                 
                    mySocketServerUdp.Bind(localIpEndPoint);                 
                    int ClientNbr = 0;                    
                    // Start listening for connections.
                    while (Program.ServerForm.DoExit == false)
                    {
                        Byte[] received = null;
                        EndPoint remoteEP = (localIpEndPoint);
                        int bytesReceived = 0;

                        try
                        {
                            if (mySocketServerUdp.Available > 0)
                            {
                                int nbToRead = Math.Max(Program.BUFFER_SIZE, mySocketServerUdp.Available);
                                received = new Byte[nbToRead];                              
                                bytesReceived = mySocketServerUdp.ReceiveFrom(received, ref remoteEP);
                            }
                            else
                            {
                                Thread.Sleep(10);
                                continue;
                            }
                        }
                        catch (Exception exx)
                        {
                            // Case device has been power off :(
                            log.Error("UdpServer.cs exception"); 
                            continue;
                        }

                        // did we received something ?
                        if (bytesReceived > 0)
                        {   
                            StringBuilder dataReceived = new StringBuilder();                           
                            dataReceived.Append(System.Text.Encoding.ASCII.GetString(received));
                            
                            // An incoming message needs to be processed.
                            ConnectionPool.Enqueue(new UDPClientHandler(remoteEP, ClientNbr, dataReceived.ToString()));

                            // Send ACK to the client
                            //byte[] cmdBytes = new byte[] { };
                            //cmdBytes = Encoding.ASCII.GetBytes(requiredCommand);
                            //var sendSackResult = mySocketServerUdp.SendTo(cmdBytes, cmdBytes.Length, SocketFlags.None, remoteEP);                            
                        }
                        else
                        {
                            // Program.ServerForm.updateNbConnection(ConnectionPool.Count);
                            Thread.Sleep(100);
                        }
                    }
                    // Stop client requests handling
                    ClientTask.Stop();
                    
                    log.Debug("Closing Listener...");

                    mySocketServerUdp.Shutdown(SocketShutdown.Both);
                    //mySocketServerUdp.Disconnect(false);
                    mySocketServerUdp.Close(2);
                    mySocketServerUdp = null;
                }
                catch (Exception e)
                {
                    log.Error(e);
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
        }

    }

    class UDPClientHandler : ClientHandler
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private EndPoint ClientEndPoint;
        bool ContinueProcess = false;
        private string data = null; // Incoming data from the client.
        private int ClientNumber = 0; // Client Index.
        private DateTime socketLastConnectionTime = DateTime.UtcNow;

        public UDPClientHandler(EndPoint clientEndPoint, int clientNumber, string message)
        {
            ClientNumber = clientNumber;
            ContinueProcess = true;
            data = message;
            ClientEndPoint = clientEndPoint;
        }

        override public void Process()
        {
            try
            {
                // All the data has arrived; put it in response.
                ProcessDataReceived();
            }
            catch (Exception)
            {

            }

        }  // Process()

        override protected void ProcessDataReceived()
        {
            try
            {
                if (data.Length > 0)
                {
                    bool bQuit = true;
                    string dataReceived = data.Trim();                    
                    if (dataReceived.Length > 0)
                    {
                        Program.ServerForm.incomingData.Add(dataReceived);
                        Program.ServerForm.updateNbReceive(1);
                    }
                    
                    // Client stop processing  
                    if (bQuit)
                    {
                        ContinueProcess = false;
                        // log.Debug("Socket Closed"); 
                    }
                }
            }
            catch (Exception ex)
            {
                ContinueProcess = false;
                log.Error("Socket Exception"); 
            }
        }

        override public void Close()
        {
            //networkStream.Close();
            //ClientSocket.Close();
        }

        override public bool IsAlive()
        {
            return ContinueProcess;
        }
        
    } // class UDPClientHandler 
}
