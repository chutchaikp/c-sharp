using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace c_sharp_socket_server_with_queue
{
    public class TCPSynchronousSocketListener
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);


        public void StartListening()
        {

            ClientService ClientTask;
            ClientConnectionPool ConnectionPool = new ClientConnectionPool();
            ClientTask = new ClientService(ConnectionPool);
            ClientTask.Start();

            IPAddress localAddr = IPAddress.Parse(Program.SERVER_ADDRESS);
            int serverPort = Program.SERVER_PORT;

            TcpClient client = new TcpClient();
            TcpListener listener = new TcpListener(localAddr, serverPort);
            try
            {
                listener.Start();

                int ClientNbr = 0;

                // Start listening for connections.
                log.Debug("Waiting for a connection...");

                while (Program.ServerForm.DoExit == false)
                {
                    try
                    {
                        client = listener.AcceptTcpClient();
                        if (client != null)
                        {
                            ClientNbr++;
                            ConnectionPool.Enqueue(new TCPClientHandler(client, ClientNbr));
                            Program.ServerForm.updateNbConnection(+1);
                        }
                        else
                        {
                            break;
                        }
                    }
                    catch (Exception errX)
                    {
                        log.Error(errX);
                    }
                }

                log.Debug("+------------- closing Listener---------------+");

                listener.Stop();

                // Stop client requests handling
                ClientTask.Stop();
            }
            catch (Exception e)
            {
                log.Error(e.ToString());
            }
        }
    }

    class TCPClientHandler : ClientHandler
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private TcpClient ClientSocket;
        private NetworkStream networkStream;
        bool ContinueProcess = false;
        private byte[] bytes;       // Data buffer for incoming data.
        private StringBuilder receivedData = null; // Received data string.
        private string data = null; // Incoming data from the client.
        private int ClientNumber = 0; // Client Index.
        private DateTime socketLastConnectionTime = DateTime.UtcNow;

        public TCPClientHandler(TcpClient ClientSocket, int clientNumber)
        {
            receivedData = new StringBuilder();
            ClientNumber = clientNumber;
            ClientSocket.ReceiveTimeout = 100; // 2 * 1000; 
            this.ClientSocket = ClientSocket;
            networkStream = ClientSocket.GetStream();
            bytes = new byte[ClientSocket.ReceiveBufferSize];
            ContinueProcess = true;
            socketLastConnectionTime = DateTime.UtcNow;
        }

        override public void Process()
        {
            try
            {
                int BytesRead = networkStream.Read(bytes, 0, (int)bytes.Length);
                if (BytesRead > 0)
                {
                    // Clear 
                    // receivedData = new StringBuilder();

                    var list = new List<byte>(bytes);
                    list.RemoveRange(BytesRead, (int)bytes.Length - BytesRead);
                    var hexArray = ByteArrayToString(list.ToArray());

                    if (list.Count == 18)
                    {
                        // login message
                        receivedData.Append(hexArray);
                        socketLastConnectionTime = DateTime.UtcNow;
                        hexArray = hexArray.Replace("\0", "").Trim();
                        AckMessage(hexArray);

                        ProcessDataReceived();
                    }
                    else
                    {
                        receivedData.Append(hexArray);
                        socketLastConnectionTime = DateTime.UtcNow;
                        hexArray = hexArray.Replace("\0", "").Trim();
                        AckMessage(hexArray);
                    }
                }
                else
                {
                    // All the data has arrived; put it in response.
                    ProcessDataReceived();

                    #region
                    TimeSpan span = (DateTime.UtcNow - socketLastConnectionTime);
                    if (span.Minutes >= 5)
                    {
                        networkStream.Close();
                        ClientSocket.Close();
                        ContinueProcess = false;

                        Program.serverForm.updateNbConnection(-1);
                    }
                    #endregion
                }
            }
            catch (IOException err1)
            {
                // log.Error("Error.IOException " + err1.Message);
                ProcessDataReceived();
            }
            catch (SocketException sex)
            {
                networkStream.Close();
                ClientSocket.Close();
                ContinueProcess = false;
                log.Error("Conection is broken!");
            }

        }  // Process()


        override protected void ProcessDataReceived()
        {
            // Program.ServerForm.updateNbConnections(_pool.Count);

            if (receivedData.Length > 0)
            {
            }
        }


        override public void Close()
        {
            networkStream.Close();
            ClientSocket.Close();
        }

        override public bool IsAlive()
        {
            return ContinueProcess;
        }

        public static string ByteArrayToString(byte[] ba)
        {
            StringBuilder hex = new StringBuilder(ba.Length * 2);
            foreach (byte b in ba)
            {
                //hex.AppendFormat(" {0:X2}", b);
                hex.AppendFormat(" {0:x2}", b);
            }
            return hex.ToString();
        }

        static void AckMessage(string hex)
        {
            // 

        }

    } // class ClientHandler 

}