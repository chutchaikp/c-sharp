using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace c_sharp_queue
{
    public partial class MainForm : Form
    {
        MessagePool _messagePool;

        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            System.Threading.Thread.Sleep(1000);

            // Init queue

            log.Debug("^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^");

            MessageService MessageTask;
            _messagePool = new MessagePool();
            MessageTask = new MessageService(_messagePool);
            MessageTask.Start(); // Gen n work trread 
        }
        
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        //public void StartListening()
        //{
        //    try
        //    {
        //        PersonMessageService ClientTask;

        //        // Client Connections Pool
        //        MessagePool ConnectionPool = new MessagePool();

        //        // Client Task to handle client requests
        //        ClientTask = new PersonMessageService(ConnectionPool);

        //        ClientTask.Start();

        //        IPAddress serverAddr = IPAddress.Parse(Program.SERVER_ADDRESS);
        //        int serverPort = Program.SERVER_PORT;

        //        Socket mySocketServerUdp = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
        //        IPEndPoint localIpEndPoint = new IPEndPoint(serverAddr, serverPort);

        //        try
        //        {
        //            mySocketServerUdp.Bind(localIpEndPoint);
        //            int ClientNbr = 0;
        //            // Start listening for connections.
        //            while (Program.ServerForm.DoExit == false)
        //            {
        //                Byte[] received = null;
        //                EndPoint remoteEP = (localIpEndPoint);
        //                int bytesReceived = 0;

        //                try
        //                {
        //                    if (mySocketServerUdp.Available > 0)
        //                    {
        //                        int nbToRead = Math.Max(Program.BUFFER_SIZE, mySocketServerUdp.Available);
        //                        received = new Byte[nbToRead];
        //                        bytesReceived = mySocketServerUdp.ReceiveFrom(received, ref remoteEP);
        //                    }
        //                    else
        //                    {
        //                        Thread.Sleep(10);
        //                        continue;
        //                    }
        //                }
        //                catch (Exception exx)
        //                {
        //                    // Case device has been power off :(
        //                    log.Error("UdpServer.cs exception");
        //                    continue;
        //                }

        //                // did we received something ?
        //                if (bytesReceived > 0)
        //                {
        //                    StringBuilder dataReceived = new StringBuilder();
        //                    dataReceived.Append(System.Text.Encoding.ASCII.GetString(received));

        //                    // An incoming message needs to be processed.
        //                    ConnectionPool.Enqueue(new UDPClientHandler(remoteEP, ClientNbr, dataReceived.ToString()));

        //                    // Send ACK to the client
        //                    //byte[] cmdBytes = new byte[] { };
        //                    //cmdBytes = Encoding.ASCII.GetBytes(requiredCommand);
        //                    //var sendSackResult = mySocketServerUdp.SendTo(cmdBytes, cmdBytes.Length, SocketFlags.None, remoteEP);                            
        //                }
        //                else
        //                {
        //                    // Program.ServerForm.updateNbConnection(ConnectionPool.Count);
        //                    Thread.Sleep(100);
        //                }
        //            }
        //            // Stop client requests handling
        //            ClientTask.Stop();

        //            log.Debug("Closing Listener...");

        //            mySocketServerUdp.Shutdown(SocketShutdown.Both);
        //            //mySocketServerUdp.Disconnect(false);
        //            mySocketServerUdp.Close(2);
        //            mySocketServerUdp = null;
        //        }
        //        catch (Exception e)
        //        {
        //            log.Error(e);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        log.Error(ex);
        //    }
        //}

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                FakeData fake = new FakeData();
                var persons = fake.Data(10000);

                foreach (var item in persons)
                {
                    _messagePool.Enqueue(new PersonMessageService(item, _messagePool));
                }
                
                lblNbInQueue.Text = _messagePool.Count.ToString();

            }
            catch (Exception)
            {                
            }
        }

        private delegate void updateNbInQueueDelegate(object item);
        public void updateNbInQueue(object value)
        {
            try
            {
                if (this.lblNbInQueue.InvokeRequired)
                {
                    this.lblNbInQueue.Invoke(new updateNbInQueueDelegate(this.updateNbInQueue), value);
                }
                else
                {
                    this.lblNbInQueue.Text = Convert.ToString(value);
                }
            }
            catch { }
        }

        private delegate void incrementNbLoggedDelegate();
        public void incrementNbLogged()
        {
            try
            {
                if (this.lblNbLogged.InvokeRequired)
                {
                    this.lblNbLogged.Invoke(new incrementNbLoggedDelegate(this.incrementNbLogged));
                }
                else
                {
                    var nb = Convert.ToInt32(this.lblNbLogged.Text) + 1;
                    this.lblNbLogged.Text = nb.ToString();
                }
            }
            catch { }
        }


        private void MainForm_Leave(object sender, EventArgs e)
        {
            _messagePool = null;
        }
    }
}

