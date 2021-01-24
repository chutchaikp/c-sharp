using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace c_sharp_socket_client
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
                        var user = FakeData();
                        var msgs = user.ToJSON();
                        byte[] tmpBytes = Encoding.ASCII.GetBytes(msgs);
                        var tmpResult = tmpSocketServerUdp.SendTo(tmpBytes, tmpBytes.Length, SocketFlags.None, tmpIep);

                        //tmpSocketServerUdp.Close();                    
                        //tmpSocketServerUdp = null;        

                        Console.WriteLine("Send data to the server 127.0.0.1 " + user.FirstName);
                        
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
                IPAddress tmpIp = IPAddress.Parse("127.0.0.1");
                IPEndPoint tmpIep = new IPEndPoint(tmpIp, 1725); 
                Socket server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                while (true)
                {
                    try
                    {
                        NetworkStream ns = new NetworkStream(server);
                        while (true)
                        {
                            var input = FakeData().ToJSON();
                            if (ns.CanWrite)
                            {
                                ns.Write(Encoding.ASCII.GetBytes(input), 0, input.Length);
                                ns.Flush();
                            }

                            System.Threading.Thread.Sleep(1000);
                        }

                        log.Debug("Disconnecting from server...");
                        
                        ns.Close();
                        server.Shutdown(SocketShutdown.Both);
                        server.Close();
                    }
                    catch (Exception err)
                    {
                        log.Error(err);
                        Console.WriteLine(err.Message);                        
                        System.Threading.Thread.Sleep(1000);
                    }
                }
            }
            catch (Exception errMain)
            {
                log.Error(errMain);
                Console.WriteLine(errMain.Message);
                Console.ReadKey();
            }
        }

        static User FakeData()
        {
            FakeData fake = new FakeData();
            return fake.Data(10);
        }
    }
}
