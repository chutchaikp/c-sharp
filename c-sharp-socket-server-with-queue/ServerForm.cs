using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace c_sharp_socket_server_with_queue
{
    public partial class ServerForm : Form
    {
        public Thread recordingThread;
        public Thread listeningThread;

        public ThreadRecord myThreadRecord;
        // public TCPSynchronousSocketListener myTCPListener;
        public UDPSynchronousSocketListener myUDPListener;
        public ArrayList incomingData = new ArrayList();

        public bool DoExit = false;

        public ServerForm()
        {
            InitializeComponent();

            myThreadRecord = new ThreadRecord(ref incomingData);
            recordingThread = new Thread(new ThreadStart(myThreadRecord.ThreadLoop));

            if (Program.PROTOCOL == "UDP")
            {
                myUDPListener = new UDPSynchronousSocketListener();
                listeningThread = new Thread(new ThreadStart(myUDPListener.StartListening));
            }

            // TODO TCP

            StartListener();

        }

        void StartListener()
        {
            recordingThread.Start();
            listeningThread.Start();
        }

        //private delegate void updateNbConnectionDelegate(object item);
        //public void updateNbConnection(object value)
        //{
        //    try
        //    {
        //        if (this.lblNbConnection.InvokeRequired)
        //        {
        //            this.lblNbConnection.Invoke(new updateNbConnectionDelegate(this.updateNbConnection), value);
        //        }
        //        else
        //        {
        //            this.lblNbConnection.Text = value.ToString();
        //        }
        //    }
        //    catch { }
        //}

        private delegate void updateNbReceiveDelegate(object item);
        public void updateNbReceive(object value)
        {
            try
            {
                if (this.lblNbReceive.InvokeRequired)
                {
                    this.lblNbReceive.Invoke(new updateNbReceiveDelegate(this.updateNbReceive), value);
                }
                else
                {
                    var nb = Convert.ToInt32(this.lblNbReceive.Text) + 1;
                    this.lblNbReceive.Text = nb.ToString();
                }
            }
            catch { }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            //DoExit = true;
            
            Program.ApplicationExit(null, null);
            this.Close();
            
        }
    }
}
