using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace c_sharp_socket_server_with_queue
{
    class Program
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public static string PROTOCOL;
        public static string SERVER_ADDRESS;
        public static int SERVER_PORT;
        public static int BUFFER_SIZE;

        static void Main(string[] args)
        {
            PROTOCOL = ConfigurationManager.AppSettings["PROTOCOL"];
            SERVER_ADDRESS = ConfigurationManager.AppSettings["SERVER_ADDRESS"];
            SERVER_PORT = Convert.ToInt32(ConfigurationManager.AppSettings["SERVER_PORT"]);
            BUFFER_SIZE = Convert.ToInt32(ConfigurationManager.AppSettings["BUFFER_SIZE"]);

            if (PROTOCOL == "UDP")
            {
                Udp();
            }
            else if (PROTOCOL == "TCP")
            {
                Tcp();
            }
        }

        static void Udp()
        {
            try
            {
                IPAddress tmpIp = IPAddress.Parse(SERVER_ADDRESS); 
                IPEndPoint tmpIep = new IPEndPoint(tmpIp, SERVER_PORT);
                Socket tmpSocketServerUdp = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

                while (true)
                {
                    try
                    {
                        var users = FakeData(100);

                        foreach (User user in users)
                        {
                            var msgs = user.ToJSON();
                            byte[] tmpBytes = Encoding.ASCII.GetBytes(msgs);
                            var tmpResult = tmpSocketServerUdp.SendTo(tmpBytes, tmpBytes.Length, SocketFlags.None, tmpIep);
                            //tmpSocketServerUdp.Close(); //tmpSocketServerUdp = null;        
                            Console.WriteLine("Send data to the server 127.0.0.1 " + user.FirstName);
                        }

                        System.Threading.Thread.Sleep(1000);
                    }
                    catch (Exception err)
                    {
                        log.Error(err);
                        Console.ReadKey();
                    }
                }
            }
            catch (Exception errMain)
            {
                log.Error(errMain);
            }
        }
        
        static void Tcp()
        {
            try
            {
                IPAddress tmpIp = IPAddress.Parse(SERVER_ADDRESS);
                IPEndPoint remoteEP = new IPEndPoint(tmpIp, SERVER_PORT);
                List<byte> responseBytes = new List<byte>();

                using (TcpClient client = new TcpClient())
                {
                    client.Connect(remoteEP);
                    using (NetworkStream stream = client.GetStream())
                    {
                        while (true)
                        {
                            Console.ReadKey();

                            var inputs = FakeData(2);

                            foreach (User input in inputs)
                            {
                                var requestBytes = Encoding.ASCII.GetBytes(input.ToJSON());
                                // Send data to server
                                stream.Write(requestBytes, 0, requestBytes.Length);
                            }
                            
                            stream.Flush();
                        }
                        
                        // Read data from 
                        //while (true)
                        //{
                        //    byte[] buffer = new byte[1024];
                        //    int bytesRec = stream.Read(buffer, 0, 1024); // wait here for receive data from the server
                        //    if (bytesRec == 0)
                        //    {
                        //        break;
                        //    }
                        //    responseBytes.AddRange(buffer.Take(bytesRec));
                        //}
                    }
                }
            }
            catch (Exception error)
            {
                log.Error(error);
            }
        }

        static void TcpV2()
        {
            TcpClient tcpClient = new TcpClient();
            try
            {
                int portNum = 1777;
                tcpClient.Connect("database.fleetlocate.asia", portNum);
                NetworkStream networkStream = tcpClient.GetStream();

                if (networkStream.CanWrite && networkStream.CanRead)
                {
                    String DataToSend = "";

                    while (DataToSend != "quit")
                    {
                        Console.WriteLine("\nType a text to be sent:");
                        DataToSend = Console.ReadLine();
                        if (DataToSend.Length == 0) break;

                        Byte[] sendBytes = Encoding.ASCII.GetBytes(DataToSend);
                        networkStream.Write(sendBytes, 0, sendBytes.Length);

                        //// Reads the NetworkStream into a byte buffer.
                        //byte[] bytes = new byte[tcpClient.ReceiveBufferSize];
                        //int BytesRead = networkStream.Read(bytes, 0, (int)tcpClient.ReceiveBufferSize);

                        //// Returns the data received from the host to the console.
                        //string returndata = Encoding.ASCII.GetString(bytes, 0, BytesRead);
                        //Console.WriteLine("This is what the host returned to you: \r\n{0}", returndata);
                    }
                    networkStream.Close();
                    tcpClient.Close();
                }
                else if (!networkStream.CanRead)
                {
                    Console.WriteLine("You can not write data to this stream");
                    tcpClient.Close();
                }
                else if (!networkStream.CanWrite)
                {
                    Console.WriteLine("You can not read data from this stream");
                    tcpClient.Close();
                }
            }
            catch (SocketException)
            {
                Console.WriteLine("Sever not available!");
            }
            catch (System.IO.IOException)
            {
                Console.WriteLine("Sever not available!");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        static List<User> FakeData(int nb)
        {
            FakeData fake = new FakeData();
            return fake.Data(nb);
        }
    }
}
